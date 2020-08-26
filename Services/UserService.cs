using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using feeddcity.Common;
using feeddcity.Data;
using feeddcity.Interfaces;
using feeddcity.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace feeddcity.Services
{
    public class UserService: IUser
    {
        private readonly ICommon _common;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DbConnection _dbConnection;
        public UserService(ICommon common, IHttpContextAccessor contextAccessor, DbConnection dbConnection)
        {
            _common = common;
            _contextAccessor = contextAccessor;
            _dbConnection = dbConnection;
        }
        public int CreateUser(CreateUserModel user)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var rfcBytes = new Rfc2898DeriveBytes(user.Password, salt, 10000);
            byte[] hash = rfcBytes.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string hashedPassword = Convert.ToBase64String(hashBytes);
            
            const string sql = "INSERT INTO Users (FirstName, LastName, Email, UserName, HashedPassword) VALUES (@FirstName, @LastName, @Email, @UserName, @HashedPassword);";
            var connection = _dbConnection.Connection;
            int affectedRows = connection.Execute(sql, new
            {
                user.FirstName,
                user.LastName,
                Email = user.EmailAddress,
                UserName = user.EmailAddress,
                hashedPassword
            });
            return affectedRows;
        }

        public User GetUser(string emailAddress) => 
            _dbConnection.Connection.QueryFirstOrDefault<User>("SELECT * from Users WHERE Email = @EmailAddress;", new { EmailAddress = emailAddress });
        

        public User AuthenticateUser(string emailAddress, string password)
        {
            User user = GetUser(emailAddress);
            if (user == null)
            {
                return null;
            }
            byte[] hashBytes = Convert.FromBase64String(user.HashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var rfcBytes = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = rfcBytes.GetBytes(20);
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return null;
                }
            }
            return user;
        }

        public int ResetPassword(string oldPassword, string newPassword)
        {
            AuthenticatedUserClaimsModel userClaims = GetUserClaims();
            if (userClaims == null)
            {
                throw new UnauthorizedAccessException("Incorrect username or password. Unexpected request!");
            }
            
            User user = AuthenticateUser(userClaims.UserName, oldPassword);
            if (user == null)
            {
                throw new AuthenticationException("Incorrect username or password");
            }
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            var rfcBytes = new Rfc2898DeriveBytes(newPassword, salt, 10000);
            byte[] hash = rfcBytes.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string hashedPassword = Convert.ToBase64String(hashBytes);
            const string sql = "UPDATE Users SET HashedPassword = @HashedPassword WHERE Id = @UserId;";
            return _dbConnection.Connection.Execute(sql, new {HashedPassword = hashedPassword, UserId = user.Id});
        }

        public void LogLastSignIn(int userId) => 
            _dbConnection.Connection.Execute("UPDATE Users SET LastSignIn = @LastSignIn WHERE Id = @Id;", new { LastSignIn = DateTime.Now, Id = userId });

        public string GenerateAuthToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] jwtSecret = Encoding.ASCII.GetBytes(_common.GetAuthSecretKey());
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor();
            Dictionary<string, object> claims = new Dictionary<string, object>
            {
                ["UserId"] = user.Id,
                ["UserName"] = user.Username,
            };
            tokenDescriptor.Claims = claims;
            tokenDescriptor.Expires = DateTime.UtcNow.AddDays(7);
            tokenDescriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtSecret), SecurityAlgorithms.HmacSha256Signature);
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public AuthenticatedUserClaimsModel GetUserClaims()
        {
            AuthenticatedUserClaimsModel userClaims = new AuthenticatedUserClaimsModel();
            IEnumerable<Claim> claims = _contextAccessor.HttpContext.User.Claims;
            Claim[] enumerable = claims as Claim[] ?? claims.ToArray();
            string userId = enumerable?.SingleOrDefault(x => x.Type == "UserId")?.Value;
            userClaims.UserName = enumerable?.SingleOrDefault(x => x.Type == "UserName")?.Value;
            if (userId != null && !string.IsNullOrEmpty(userId))
            {
                userClaims.UserId = int.Parse(userId);
            }
            else
            {
                return null;
            }
            return userClaims;
        }
    }
}
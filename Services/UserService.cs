using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using feeddcity.Common;
using feeddcity.Data;
using feeddcity.Interfaces;
using feeddcity.Models.User;
using Microsoft.IdentityModel.Tokens;

namespace feeddcity.Services
{
    public class UserService: IUser
    {
        private readonly ICommon _common;
        public UserService(ICommon common)
        {
            _common = common;
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
            var connection = _common.GetConnection();
            connection.Open();
            int affectedRows = connection.Execute(sql, new
            {
                user.FirstName,
                user.LastName,
                Email = user.EmailAddress,
                UserName = user.EmailAddress,
                hashedPassword
            });
            connection.Close();
            return affectedRows;
        }

        public User GetUser(string emailAddress)
        {
            var connection = _common.GetConnection();
            connection.Open();
            string sql = "SELECT * from Users WHERE Email = @EmailAddress;";
            User user = connection.QueryFirstOrDefault<User>(sql, new { EmailAddress = emailAddress });
            connection.Close();
            return user;
        }

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
            throw new System.NotImplementedException();
        }
    }
}
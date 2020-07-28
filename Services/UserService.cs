using System;
using System.Security.Cryptography;
using Dapper;
using feeddcity.Common;
using feeddcity.Data;
using feeddcity.Interfaces;
using feeddcity.Models.User;

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
            throw new System.NotImplementedException();
        }

        public User AuthenticateUser(string emailAddress, string password)
        {
            throw new System.NotImplementedException();
        }

        public string GenerateAuthToken(User user)
        {
            throw new System.NotImplementedException();
        }

        public AuthenticatedUserClaimsModel GetUserClaims()
        {
            throw new System.NotImplementedException();
        }
    }
}
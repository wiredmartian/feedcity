using System;
using System.Security.Authentication;
using Dapper;
using feeddcity.Data;
using feeddcity.Interfaces;
using feeddcity.Models.DropOff;

namespace feeddcity.Services
{
    public class DropOffService : IDropOff
    {
        private readonly DbConnection _dbConnection;
        private readonly IUser _userSvc;
        public DropOffService(DbConnection dbConnection, IUser userSvc)
        {
            _dbConnection = dbConnection;
            _userSvc = userSvc;
        }

        public int CreateDropOff(DropOffZoneModel model)
        {
            var userClaims = _userSvc.GetUserClaims();
            if (userClaims == null)
            {
                throw new InvalidCredentialException("User claims not found against token");
            }
            const string sql = "INSERT INTO DropOffZones (PhysicalAddress, Latitude, Longitude, Province, City, StreetName, ContactName, ContactNumber, EmailAddress) VALUES (@PhysicalAddress, @Latitude, @Longitude, @Province, @City, @StreetName, @ContactName, @ContactNumber, @EmailAddress);";
            int rows = _dbConnection.Connection.Execute(sql, new
            {
                model.PhysicalAddress,
                model.Latitude,
                model.Longitude,
                model.Province,
                model.City,
                model.StreetName,
                model.ContactName,
                model.ContactNumber,
                model.EmailAddress
            });
            return rows;
        }
    }
}
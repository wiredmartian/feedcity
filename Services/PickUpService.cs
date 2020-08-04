using Dapper;
using feeddcity.Common;
using feeddcity.Data;
using feeddcity.Interfaces;
using feeddcity.Models.PickUp;
using feeddcity.Models.User;

namespace feeddcity.Services
{
    public class PickUpService : IPickUp
    {
        
        private readonly ICommon _common;
        private readonly IUser _userSvc;
        private readonly DbConnection _dbConnection;
        public PickUpService(ICommon common, IUser userSvc, DbConnection dbConnection)
        {
            _common = common;
            _userSvc = userSvc;
            _dbConnection = dbConnection;
        }

        public int RequestPickUp(PickUpRequestModel model)
        {
            AuthenticatedUserClaimsModel userClaims = _userSvc.GetUserClaims();
            const string sql = "INSERT INTO PickupRequests (UserId, Location, Latitude, Longitude, ContactName, ContactNumber, Status, Notes) VALUES (@UserId, @Location, @Latitude, @Longitude, @ContactName, @ContactNumber, @Status, @Notes);";
            var connection = _dbConnection.Connection;
            int rows = connection.Execute(sql, new
            {
                userClaims.UserId,
                model.Location,
                model.Latitude,
                model.Longitude,
                model.ContactName,
                model.ContactNumber, 
                Status = PickUpStatus.Pending,
                model.Notes
            });
            return rows;
        }
    }
}
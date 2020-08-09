using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using feeddcity.Data;
using feeddcity.Interfaces;
using feeddcity.Models.PickUp;
using feeddcity.Models.User;

namespace feeddcity.Services
{
    public class PickUpService : IPickUp
    {
        
        private readonly IUser _userSvc;
        private readonly DbConnection _dbConnection;
        public PickUpService(IUser userSvc, DbConnection dbConnection)
        {
            _userSvc = userSvc;
            _dbConnection = dbConnection;
        }

        public int RequestPickUp(PickUpRequestModel model)
        {
            AuthenticatedUserClaimsModel userClaims = _userSvc.GetUserClaims();
            const string sql = "INSERT INTO PickupRequests (UserId, Location, Latitude, Longitude, ContactName, ContactNumber, Status, Notes) VALUES (@UserId, @Location, @Latitude, @Longitude, @ContactName, @ContactNumber, @Status, @Notes);";
            int rows = _dbConnection.Connection.Execute(sql, new
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

        public int UpdatePickUpStatus(PickUpStatus status, int pickUpId)
        {
            const string sql = "UPDATE PickupRequests SET Status = @PickUpStatus WHERE Id = @PickUpId;";
            return  _dbConnection.Connection.Execute(sql, new { PickUpStatus = status, PickUpId = pickUpId });
        }

        public List<PickUpRequest> GetPickUpRequests(PickUpStatus status)
        {
            const string query = "SELECT * FROM PickupRequests WHERE Status = @PickUpStatus;";
            var requests = _dbConnection.Connection.Query<PickUpRequest>(query, new {PickUpStatus = status});
            return requests.ToList();
        }

        public int AcceptPickUpRequest(int pickUpId)
        {
            var request = GetSinglePickupRequest(pickUpId);
            if (request == null)
            {
                return 0;
            }
            if (request.AcceptedBy != 0)
            {
                throw new Exception("Pick up has already been accepted by someone else");
            }
            int currentUser = _userSvc.GetUserClaims().UserId;
            const string sql = "UPDATE PickupRequests SET Status = 2, AcceptedBy = @AcceptedBy WHERE Id = @Id;";
            return _dbConnection.Connection.Execute(sql, new {AcceptedBy = currentUser, Id = pickUpId});
        }

        public PickUpRequest GetSinglePickupRequest(int pickUpId)
        {
            const string sql = "SELECT * FROM PickupRequests WHERE Id = @PickUpId AND ClosedOn IS NULL;";
            return _dbConnection.Connection.QuerySingleOrDefault<PickUpRequest>(sql, new { PickUpId = pickUpId });
        }
        public List<PickUpRequest> GetUserPickUpRequests(int userId)
        {
            const string sql = "SELECT * FROM PickupRequests WHERE UserId = @UserId AND ClosedOn IS NULL;";
            return _dbConnection.Connection.Query<PickUpRequest>(sql, new { UserId = userId }).ToList();
        }

        public List<PickUpRequest> GetUserPickUpRequests()
        {
            AuthenticatedUserClaimsModel userClaims = _userSvc.GetUserClaims();
            const string sql = "SELECT * FROM PickupRequests WHERE UserId = @UserId AND ClosedOn IS NULL;";
            return _dbConnection.Connection.Query<PickUpRequest>(sql, new { UserId = userClaims.UserId }).ToList();
        }
        public int CompletePickUpRequest(int pickUpId)
        {
            DateTime completedOn = DateTime.UtcNow;
            const string sql = "UPDATE PickupRequests SET Status = @PickUpStatus, ClosedOn = @CompleteDate WHERE Id = @PickUpId;";
            return _dbConnection.Connection.Execute(sql, new { PickUpStatus = 3, CompleteDate = completedOn, PickUpId = pickUpId });
        }
        public int CancelPickUpRequest(int pickUpId)
        {
            AuthenticatedUserClaimsModel userClaims = _userSvc.GetUserClaims();
            DateTime completedOn = DateTime.UtcNow;
            const string sql = "UPDATE PickupRequests SET Status = @PickUpStatus, ClosedOn = @CompleteDate WHERE Id = @PickUpId AND UserId = @UserId;";
            return _dbConnection.Connection.Execute(sql, new { PickUpStatus = 3, CompleteDate = completedOn, PickUpId = pickUpId, UserId = userClaims.UserId });

        }

        public List<PickUpRequest> GetActiveRequests()
        {
            const string sql = "SELECT * FROM PickupRequests WHERE ClosedOn IS NULL;";
            return _dbConnection.Connection.Query<PickUpRequest>(sql).ToList();

        }
        
    }
}
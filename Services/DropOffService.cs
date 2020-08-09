using System;
using feeddcity.Data;

namespace feeddcity.Services
{
    public class DropOffService
    {
        private readonly DbConnection _dbConnection;
        public DropOffService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public int CreateDropOff()
        {
            throw new NotImplementedException();
        }
    }
}
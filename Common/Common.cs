using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace feeddcity.Common
{
    public interface ICommon
    {
        IDbConnection GetConnection();
        string GetAuthSecretKey();

    }
    public class Common : ICommon
    {
        private readonly IConfiguration _config;

        public Common(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }
        public string GetAuthSecretKey()
        {
            return _config.GetValue<string>("AppSettings:JWTSecret");
        }
    }
}
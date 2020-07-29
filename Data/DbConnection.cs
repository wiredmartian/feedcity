using System;
using MySql.Data.MySqlClient;

namespace feeddcity.Data
{
    public class DbConnection : IDisposable
    {
        public MySqlConnection Connection;

        public DbConnection(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
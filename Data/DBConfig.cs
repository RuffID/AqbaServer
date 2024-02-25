using AqbaServer.Helper;
using MySql.Data.MySqlClient;

namespace AqbaServer.Data
{
    public class DBConfig
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "localhost";
            int port = 3306;
            string database = "aqba";
            string username = "root";
            string password = "";

            return GetDBConnection(host, port, database, username, password);
        }
        public static MySqlConnection GetDBConnection(string host, int port, string database, string username, string password)
        {
            // Connection String.
            if (string.IsNullOrEmpty(Config.ConnectionString))
            {
                return new MySqlConnection
                    ("Server=" + host + ";Database=" + database + ";port=" + port + ";User Id=" + username + ";password=" + password);
            }
            else
            {
                return new MySqlConnection(Config.ConnectionString);
            }
        }
    }
}
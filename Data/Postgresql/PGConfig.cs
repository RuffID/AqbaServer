using AqbaServer.Helper;
using Npgsql;

namespace AqbaServer.Data.Postgresql
{
    public class PGConfig
    {
        static readonly string host;
        static readonly int port;
        static readonly string database;
        static readonly string username;
        static readonly string password;

        static PGConfig() 
        {
            host = "localhost";
            port = 5432;
            database = "postgres";
            username = "user";
            password = "postgres";
        }

        public static NpgsqlConnection GetPsqlConnection()
        {
            return GetPsqlConnection(host, port, database, username, password);
        }

        static NpgsqlConnection GetPsqlConnection(string host, int port, string database, string username, string password)
        {
            // Connection String.
            if (string.IsNullOrEmpty(Config.ODBCConnectionString))
            {
                return new NpgsqlConnection
                    ("host=" + host + ";database=" + database + ";port=" + port + ";username=" + username + ";password=" + password);
            }
            else return new NpgsqlConnection(Config.ODBCConnectionString);
        }
    }
}


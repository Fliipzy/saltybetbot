using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaltyLibrary.Data
{
    public class DBConnection
    {
        private DBConnection() { }

        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string  Password { get; set; }

        private MySqlConnection connection;
        public MySqlConnection Connection { get { return connection; } }

        private static DBConnection instance = null;

        public void SetDBCredentials(DBCredentials credentials)
        {
            this.DatabaseName = credentials.DBName;
            this.Username = credentials.Username;
            this.Password = credentials.Password;
        }

        public static DBConnection Instance()
        {
            if (instance == null)
            {
                instance = new DBConnection();
            }
            return instance;
        }

        public bool IsConnected()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                {
                    return false;
                }
                string connection_str = String.Format("Server=localhost; database={0}; UID={1}; password={2}", DatabaseName, Username, Password);
                connection = new MySqlConnection(connection_str);
                connection.Open();
            }
            return true;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}

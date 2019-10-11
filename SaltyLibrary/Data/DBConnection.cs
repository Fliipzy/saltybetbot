using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SaltyLibrary.Data
{
    public class DBConnection
    {
        private DBConnection() { }

        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string  Password { get; set; }
        public MySqlConnection Connection { get; private set; }

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
                if (string.IsNullOrEmpty(DatabaseName))
                {
                    return false;
                }
                string connection_str = string.Format("server=localhost;database={0};userid={1};password={2}", DatabaseName, Username, Password);
                Connection = new MySqlConnection(connection_str);
                Connection.Open();
            }
            else if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
            return true;
        }

        public void Close()
        {
            Connection.Close();
        }
    }
}

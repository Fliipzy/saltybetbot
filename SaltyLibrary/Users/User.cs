using System;
using System.Collections.Generic;
using System.Text;
using SaltyLibrary.Saltybet.Enums;
using SaltyLibrary.Users.Interfaces;

namespace SaltyLibrary.Users
{
    public class User : IUser
    {
        public User()
        {
            
        }

        public string Username { get; set; }

        public int Balance { get; set; }

        public int TotalBets { get; set; }

        public int WorldRanking { get; set; }

        public DateTime JoinDate { get; set; }

        public SaltyRank SaltyRank { get; set; }

        public IUserCredentials UserCredentials { get; set; }
    }
}

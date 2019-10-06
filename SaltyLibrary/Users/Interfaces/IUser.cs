using SaltyLibrary.Saltybet.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaltyLibrary.Users.Interfaces
{
    public interface IUser
    {
        string Username { get; }
        int Balance { get; }
        int TotalBets { get; }
        int WorldRanking { get; }
        DateTime JoinDate { get; }
        SaltyRank SaltyRank { get; }
        IUserCredentials UserCredentials { get; }
    }
}

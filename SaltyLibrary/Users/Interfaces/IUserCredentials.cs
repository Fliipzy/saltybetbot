using System;
using System.Collections.Generic;
using System.Text;

namespace SaltyLibrary.Users.Interfaces
{
    public interface IUserCredentials
    {
        public string Email { get; }
        public string Password { get; }
    }
}

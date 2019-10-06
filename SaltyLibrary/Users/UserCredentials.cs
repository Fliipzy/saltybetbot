using SaltyLibrary.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SaltyLibrary.Users
{
    public class UserCredentials : IUserCredentials, IEquatable<UserCredentials>
    {
        public UserCredentials(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public bool Equals(UserCredentials other)
        {
            if (other == null )
            {
                return false;
            }
            return this.Email.Equals(other.Email) && this.Password.Equals(other.Password);
        }

        public override string ToString()
        {
            return $"{Email}";
        }
    }
}

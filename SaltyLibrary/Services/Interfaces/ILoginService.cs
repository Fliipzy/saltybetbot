using SaltyLibrary.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SaltyLibrary.Services.Interfaces
{
    public interface ILoginService
    {
        event EventHandler<EventArgs> LoggedIn;

        CookieCollection Login(IUserCredentials userCredentials);
    }
}

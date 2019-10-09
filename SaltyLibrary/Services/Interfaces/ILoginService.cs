using SaltyLibrary.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLibrary.Services.Interfaces
{
    public interface ILoginService
    {
        event EventHandler<EventArgs> LoggedIn;

        CookieContainer Login(IUserCredentials userCredentials);
    }
}

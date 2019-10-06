using SaltyLibrary.Services.Interfaces;
using SaltyLibrary.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SaltyLibrary.Services
{
    public class LoginService : ILoginService
    {
        private HttpWebRequest webRequest;

        public event EventHandler<EventArgs> LoggedIn;

        public LoginService()
        {
            webRequest = HttpWebRequest.CreateHttp("https://www.saltybet.com/authenticate?signin=1");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
        }

        public CookieCollection Login(IUserCredentials userCredentials)
        {
            string postdata = $"email={userCredentials.Email}&pword={userCredentials.Password}&authenticate=signin";
            byte[] data = Encoding.ASCII.GetBytes(postdata);

            using (Stream requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(data, 0, data.Length);
            }

            OnLoggedIn(new EventArgs());
            return null;
        }

        protected virtual void OnLoggedIn(EventArgs e)
        {
            LoggedIn?.Invoke(this, e);
        }
    }
}

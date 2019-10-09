using SaltyLibrary.Services.Interfaces;
using SaltyLibrary.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLibrary.Services
{
    public class LoginService : ILoginService
    {
        public event EventHandler<EventArgs> LoggedIn;

        public LoginService()
        {
        }

        public CookieContainer Login(IUserCredentials userCredentials)
        {
            var cookieContainer = new CookieContainer();
            var data_str = $"email={userCredentials.Email}&pword={userCredentials.Password}&authenticate=signin";
            var data_bytes = Encoding.ASCII.GetBytes(data_str);

            var request = HttpWebRequest.CreateHttp("https://www.saltybet.com/authenticate?signin=1");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data_bytes.Length;
            request.AllowAutoRedirect = true;
            request.CookieContainer = cookieContainer;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data_bytes, 0, data_bytes.Length);
                stream.Close();
            }

            WebResponse response;
            try
            {
                response = request.GetResponse();
            }

            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    //Expected (HTTP 302 Temporarily moved).
                }
                else
                {
                    Console.WriteLine("Error occurred while logging in...");
                    return null;
                }

            }

            OnLoggedIn(new EventArgs());
            return cookieContainer;
        }

        protected virtual void OnLoggedIn(EventArgs e)
        {
            LoggedIn?.Invoke(this, e);
        }
    }
}

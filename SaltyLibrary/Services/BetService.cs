using SaltyLibrary.Saltybet.Enums;
using SaltyLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SaltyLibrary.Services
{
    public class BetService : IBetService
    {
        private HttpWebRequest request;

        public event EventHandler<EventArgs> BetPlaced;

        public BetService(CookieContainer cookieContainer)
        {
            request = WebRequest.CreateHttp("https://www.saltybet.com/ajax_place_bet.php");
            request.CookieContainer = cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
        }

        public void PlaceBet(TeamColor team, int wager)
        {
            var player_num = (team == TeamColor.RED) ? "1" : "2"; 
            var data_str = $"selectedplayer=player{ player_num }&wager={ wager }";
            var data_bytes = Encoding.ASCII.GetBytes(data_str);

            using (Stream stream = request.GetRequestStream())
            {
                stream.WriteAsync(data_bytes, 0, data_bytes.Length);
            }

            var response = request.GetResponse();

            OnBetPlaced(new EventArgs());
        }

        protected virtual void OnBetPlaced(EventArgs e)
        {
            BetPlaced?.Invoke(this, e);
        }
    }
}

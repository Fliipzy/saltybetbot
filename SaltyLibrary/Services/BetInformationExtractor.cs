using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using SaltyLibrary.Saltybet;

namespace SaltyLibrary.Services
{
    public class BetInformationExtractor
    {
        HttpWebRequest request;

        public BetInformationExtractor()
        {

        }

        public Tuple<int, int> GetBetInformation()
        {
            request = WebRequest.CreateHttp("https://www.saltybet.com/zdata.json");
            request.Method = "GET";
            request.ContentType = "application/json; charset=UTF-8";
            request.PreAuthenticate = true;

            string json = null;

            using (var reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                json = reader.ReadToEnd();
                reader.Close();
            }

            int firstIndex = json.IndexOf("\"remaining\":");
            json = json.Substring(firstIndex);
            int secondIndex = json.IndexOf(",\"") + 1;
            json = "{" + json.Substring(secondIndex);

            var bettorDictionary = JsonConvert.DeserializeObject<Dictionary<string, BettorInformation>>(json);

            int p1tb = 0; 
            int p2tb = 0;

            foreach (BettorInformation betInfo in bettorDictionary.Values)
            {
                if (betInfo.Player == "1")
                {
                    p1tb++;
                    continue;
                }
                p2tb++;
            }

            return Tuple.Create<int, int>(p1tb, p2tb);
        }
    }
}

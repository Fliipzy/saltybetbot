﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using SaltyLibrary.Saltybet;
using Newtonsoft.Json;
using System.IO;

namespace SaltyLibrary.Services
{
    public class StateExtractor
    {
        HttpWebRequest request;
        public StateExtractor()
        {
        }
        public State GetCurrentState()
        {
            request = WebRequest.CreateHttp("https://www.saltybet.com/state.json");
            request.Method = "GET";
            request.ContentType = "application/json; charset=UTF-8";
            request.PreAuthenticate = true;

            string jsonResponse = null;

            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                using (var reader = new StreamReader(responseStream))
                {
                    jsonResponse = reader.ReadToEnd();
                }
            }

            var state = JsonConvert.DeserializeObject<State>(jsonResponse);
            return state;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SaltyLibrary.Data
{
    public class DBCredentials
    {
        [JsonProperty("dbname")]
        [JsonRequired]
        public string DBName { get; set; }

        [JsonProperty("username")]
        [JsonRequired]
        public string Username { get; set; }

        [JsonProperty("password")]
        [JsonRequired]
        public string Password { get; set; }

        public static DBCredentials ParseFromJsonFile(string path)
        {
            try
            {
                return JsonConvert.DeserializeObject<DBCredentials>(File.ReadAllText(path));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SaltyLibrary.Saltybet
{
    public class BettorInformation
    {
        [JsonProperty("n")]
        public string Name { get; set; }

        [JsonProperty("b")]
        public string Balance { get; set; }

        [JsonProperty("p")]
        public string Player { get; set; }

        [JsonProperty("w")]
        public string Wager { get; set; }

        [JsonProperty("r")]
        public string Rank { get; set; }

        [JsonProperty("g")]
        public string GoldMembership { get; set; }
    }
}

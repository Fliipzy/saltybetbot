using SaltyLibrary.Saltybet.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SaltyLibrary.Saltybet
{
    public class Match 
    {
        public string P1Tier { get; set; }

        public string P2Tier { get; set; }

        [JsonProperty("p1name")]
        public string P1Name { get; set; }

        [JsonProperty("p2name")]
        public string P2Name { get; set; }

        [JsonProperty("p1total")]
        public string P1Total { get; set; }

        [JsonProperty("p2total")]
        public string P2Total { get; set; }

        public int BlueTotalBetters { get; set; }

        public int RedTotalBetters { get; set; }

        public int Duration { get; set; }

        public TeamColor Winner { get; set; } = TeamColor.NONE;

        public bool Tournament { get; set; }

    }
}

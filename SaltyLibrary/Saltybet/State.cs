using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SaltyLibrary.Saltybet
{
    public class State
    {
        public State() {}

        public State(string p1name, string p2name, string p1total, string p2total, string status, string alert, int x, string remaining)
        {
            this.P1Name = p1name;
            this.P2Name = p2name;
            this.P1Total = p1total;
            this.P2Total = p2total;
            this.Status = status;
            this.Alert = alert;
            this.X = x;
            this.Remaining = remaining;
        }

        [JsonProperty("p1name")]
        public string P1Name { get; set; }

        [JsonProperty("p2name")]
        public string P2Name { get; set; }

        [JsonProperty("p1total")]
        public string P1Total { get; set; }

        [JsonProperty("p2total")]
        public string P2Total { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("alert")]
        public string Alert { get; set; }

        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("remaining")]
        public string Remaining { get; set; }
    }
}

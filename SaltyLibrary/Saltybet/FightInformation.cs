using SaltyLibrary.Saltybet.Enums;
using SaltyLibrary.Saltybet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaltyLibrary.Saltybet
{
    public class FightInformation : IFightInformation
    {
        public FightInformation() {}

        public FightInformation(string redTeamName, string blueTeamName)
        {
            this.RedTeamName = redTeamName;
            this.BlueTeamName = blueTeamName;
        }

        public string RedTeamName { get; set; }

        public string BlueTeamName { get; set; }

        public string StageName { get; set; }

        public int TotalWagerRed { get; set; }

        public int TotalWagerBlue { get; set; }

        public int BlueTotalBetters { get; set; }

        public int RedTotalBetters { get; set; }

        public TeamColor Winner { get; set; }

        public bool Tournament { get; set; }

        public override string ToString()
        {
            return $"Red: {RedTeamName}, Blue: {BlueTeamName}";
        }
    }
}

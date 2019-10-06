using SaltyLibrary.Saltybet.Enums;

namespace SaltyLibrary.Saltybet.Interfaces
{
    public interface IFightInformation
    {
        public string RedTeamName { get; set; }
        public string BlueTeamName { get; set; }
        public string StageName { get; set; }
        public bool Tournament { get; set; }
        public int TotalWagerRed { get; set; }
        public int TotalWagerBlue { get; set; }
        public int BlueTotalBetters { get; set; }
        public int RedTotalBetters { get; set; }

        public TeamColor Winner { get; set; }
    }
}

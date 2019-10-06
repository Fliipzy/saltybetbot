using SaltyLibrary.Saltybet.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaltyLibrary.Services.Interfaces
{
    public interface IBetService
    {
        event EventHandler<EventArgs> BetPlaced;

        void PlaceBet(TeamColor team, int wager);
    }
}

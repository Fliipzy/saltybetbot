using SaltyLibrary.Saltybet.Enums;
using SaltyLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaltyLibrary.Services
{
    public class BetService : IBetService
    {
        public event EventHandler<EventArgs> BetPlaced;

        public void PlaceBet(TeamColor team, int wager)
        {
            OnBetPlaced(new EventArgs());
        }

        protected virtual void OnBetPlaced(EventArgs e)
        {
            BetPlaced?.Invoke(this, e);
        }
    }
}

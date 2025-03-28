using LotteryGame.Configuration;
using System;

namespace LotteryGame.Utilities
{
    public static class ConfigurationValidator
    {
        public static void Validate(LotteryConfig config)
        {
            if (config.TicketCost <= 0)
                throw new ArgumentException("Ticket cost must be greater than 0.");
            if (config.InitialBalance <= 0)
                throw new ArgumentException("Initial balance must be greater than $0.00.");
            if (config.MinPlayers <= 0 || config.MaxPlayers < config.MinPlayers)
                throw new ArgumentException("Invalid player count configuration.");
            if (config.MinTicketsPerPlayer <= 0 || config.MaxTicketsPerPlayer < config.MinTicketsPerPlayer)
                throw new ArgumentException("Invalid ticket per player configuration.");
            if (config.GrandPrizePercentage < 0 || config.GrandPrizePercentage > 1)
                throw new ArgumentException("Invalid grand prize percentage.");
        }
    }
}

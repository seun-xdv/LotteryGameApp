using LotteryGame.Models;
using System.Collections.Generic;

namespace LotteryGame.Services
{
    public class ConsoleResultPresenter : IResultPresenter
    {
        public void PresentResults(List<Player> players, PrizeDistributionResult prizeResult, PresentationSummary summary, IConsoleService console)
        {
            console.WriteLine("\n--- Players and Ticket Purchases ---");
            console.WriteLine($"{summary.TotalPlayers} players bought {summary.TotalTickets} tickets worth ${summary.TotalTickets:F2}.");

            console.WriteLine("\n--- Winning Tickets ---");
            foreach (var win in prizeResult.WinningTickets)
            {
                console.WriteLine($"{win.Ticket.Player.Name} won {win.PrizeTier} and received ${win.PrizeAmount:F2}");
            }

            console.WriteLine($"\nHouse Profit: ${prizeResult.HouseProfit:F2}");
        }
    }
}

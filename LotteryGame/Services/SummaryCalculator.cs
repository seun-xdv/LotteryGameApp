using System.Linq;
using LotteryGame;
using LotteryGame.Models;

namespace LotteryGame.Services
{
    /// <summary>
    /// Compute player, ticket and revenue values.
    /// </summary>
    public class SummaryCalculator : ISummaryCalculator
    {
        public PresentationSummary CalculateSummary(List<Player> players, decimal ticketCost)
        {
            return new PresentationSummary
            {
                TotalPlayers = players.Count,
                TotalTickets = players.Sum(p => p.TicketCount),
                TotalRevenue = players.Sum(p => p.TicketCount) * ticketCost
            };
        }
    }
}

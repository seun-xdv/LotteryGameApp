using LotteryGame.Configuration;
using LotteryGame.Models;

namespace LotteryGame.Services
{
    /// <summary>
    /// Result of prize distribution including winning tickets and house profit.
    /// </summary>
    public class PrizeDistributionResult
    {
        public List<WinningTicket> WinningTickets { get; set; } = new List<WinningTicket>();
        public decimal HouseProfit { get; set; }
    }
    public interface IPrizeDistributor
    {
        PrizeDistributionResult DistributePrizes(List<Ticket> tickets, LotteryConfig config, decimal totalRevenue, Random random);
    }
}
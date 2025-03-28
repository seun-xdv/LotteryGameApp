using System.Collections.Generic;
using LotteryGame.Models;

namespace LotteryGame.Services
{
    /// <summary>
    /// Defines the contract for calculating game summary data.
    /// </summary>
    public interface ISummaryCalculator
    {
        PresentationSummary CalculateSummary(List<Player> players, decimal ticketCost);
    }
}

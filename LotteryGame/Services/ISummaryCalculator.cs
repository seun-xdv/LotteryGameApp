using System.Collections.Generic;
using LotteryGame.Models;

namespace LotteryGame.Services
{
    public interface ISummaryCalculator
    {
        PresentationSummary CalculateSummary(List<Player> players, decimal ticketCost);
    }
}

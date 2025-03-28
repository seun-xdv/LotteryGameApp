using LotteryGame.Models;
using System.Collections.Generic;

namespace LotteryGame.Services
{
    /// <summary>
    /// Defines the contract for presenting game results.
    /// </summary>
    public interface IResultPresenter
    {
        void PresentResults(List<Player> players, PrizeDistributionResult prizeResult, PresentationSummary summary, IConsoleService console);
    }
}

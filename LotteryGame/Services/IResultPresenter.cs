using LotteryGame.Models;
using System.Collections.Generic;

namespace LotteryGame.Services
{
    public interface IResultPresenter
    {
        void PresentResults(List<Player> players, PrizeDistributionResult prizeResult, PresentationSummary summary, IConsoleService console);
    }
}

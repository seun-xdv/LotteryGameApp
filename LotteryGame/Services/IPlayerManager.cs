using LotteryGame.Configuration;
using LotteryGame.Models;
using System.Collections.Generic;

namespace LotteryGame.Services
{
    public interface IPlayerManager
    {
        /// <summary>
        /// Creates and sets up players for the lottery game.
        /// </summary>
        List<Player> CreatePlayers(LotteryConfig config, IConsoleService console, IRandomProvider randomProvider);
    }
}
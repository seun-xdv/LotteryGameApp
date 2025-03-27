using LotteryGame.Configuration;
using LotteryGame.Models;

namespace LotteryGame.Services
{
    public interface IPlayerManager
    {
        /// <summary>
        /// Creates and sets up the players for the lottery game.
        /// </summary>
        List<Player> CreatePlayers(LotteryConfig config, IConsoleService console, Random random);
    }
}
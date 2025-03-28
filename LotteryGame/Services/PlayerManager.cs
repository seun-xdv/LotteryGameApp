using LotteryGame.Configuration;
using LotteryGame.Models;
using LotteryGame.Utilities;

namespace LotteryGame.Services
{
    /// <summary>
    /// Creates and sets up the players for the lottery game.
    /// </summary>
    public class PlayerManager : IPlayerManager
    {
        public List<Player> CreatePlayers(LotteryConfig config, IConsoleService console, IRandomProvider randomProvider)
        {
            var players = new List<Player>();

            // Create human player.
            var humanPlayer = new Player("Player 1", true, config.InitialBalance);
            players.Add(humanPlayer);
            console.WriteLine($"Welcome, Player 1! You have ${config.InitialBalance:F2}. Each ticket costs ${config.TicketCost:F2}.");
            console.Write($"How many tickets do you want to purchase ({config.MinTicketsPerPlayer}-{config.MaxTicketsPerPlayer})? ");
            
            int tickets = InputValidator.ReadValidInteger(console, config.MinTicketsPerPlayer, config.MaxTicketsPerPlayer);
            humanPlayer.PurchaseTickets(tickets, config.TicketCost);

            // Determine total players (including human) between MinPlayers and MaxPlayers.
            int totalPlayers = randomProvider.Next(config.MinPlayers, config.MaxPlayers + 1);

            // Create CPU players.
            for (int i = 2; i <= totalPlayers; i++)
            {
                var cpuPlayer = new Player($"Player {i}", false, config.InitialBalance);
                int maxTickets = Math.Min(config.MaxTicketsPerPlayer, (int)(cpuPlayer.Balance / config.TicketCost));
                int cpuTickets = randomProvider.Next(config.MinTicketsPerPlayer, maxTickets + 1);
                cpuPlayer.PurchaseTickets(cpuTickets, config.TicketCost);
                players.Add(cpuPlayer);
            }

            return players;
        }
    }
}

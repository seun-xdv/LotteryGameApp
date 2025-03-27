using LotteryGame.Configuration;
using LotteryGame.Models;

namespace LotteryGame.Services
{
    /// <summary>
    /// Creates and sets up the players for the lottery game.
    /// </summary>
    public class PlayerManager : IPlayerManager
    {
        public List<Player> CreatePlayers(LotteryConfig config, IConsoleService console, Random random)
        {
            var players = new List<Player>();

            // Create human player.
            var humanPlayer = new Player("Player 1", true, config.InitialBalance);
            players.Add(humanPlayer);
            console.WriteLine($"Welcome, Player 1! You have ${config.InitialBalance:F2}. Each ticket costs ${config.TicketCost:F2}.");
            console.Write($"How many tickets do you want to purchase ({config.MinTicketsPerPlayer}-{config.MaxTicketsPerPlayer})? ");
            int tickets = ReadValidTicketCount(console, config);
            humanPlayer.PurchaseTickets(tickets, config.TicketCost);

            // Determine total players (including human) between MinPlayers and MaxPlayers.
            int totalPlayers = random.Next(config.MinPlayers, config.MaxPlayers + 1);

            // Create CPU players.
            for (int i = 2; i <= totalPlayers; i++)
            {
                var cpuPlayer = new Player($"Player {i}", false, config.InitialBalance);
                int maxTickets = Math.Min(config.MaxTicketsPerPlayer, (int)(cpuPlayer.Balance / config.TicketCost));
                int cpuTickets = random.Next(config.MinTicketsPerPlayer, maxTickets + 1);
                cpuPlayer.PurchaseTickets(cpuTickets, config.TicketCost);
                players.Add(cpuPlayer);
            }

            return players;
        }

        private int ReadValidTicketCount(IConsoleService console, LotteryConfig config)
        {
            int count;
            while (!int.TryParse(console.ReadLine(), out count) ||
                   count < config.MinTicketsPerPlayer || count > config.MaxTicketsPerPlayer)
            {
                console.WriteLine($"Invalid input. Please enter a number between {config.MinTicketsPerPlayer} and {config.MaxTicketsPerPlayer}: ");
            }
            return count;
        }
    }
}

using LotteryGame.Configuration;
using LotteryGame.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LotteryGame.Services
{
    /// <summary>
    /// Orchestrates the lottery game flow by leveraging modular services.
    /// </summary>
    public class LotteryService : ILotteryService
    {
        private readonly LotteryConfig _config;
        private readonly IConsoleService _console;
        private readonly IPlayerManager _playerManager;
        private readonly ITicketManager _ticketManager;
        private readonly IPrizeDistributor _prizeDistributor;
        private readonly Random _random = new Random();

        public LotteryService(IOptions<LotteryConfig> config, IConsoleService console, IPlayerManager playerManager, ITicketManager ticketManager, IPrizeDistributor prizeDistributor)
        {
            _config = config.Value;
            _console = console;
            _playerManager = playerManager;
            _ticketManager = ticketManager;
            _prizeDistributor = prizeDistributor;
        }

        public void RunGame()
        {
            // 1. Create players and process ticket purchases.
            List<Models.Player> players = _playerManager.CreatePlayers(_config, _console, _random);

            // 2. Generate tickets.
            List<Ticket> tickets = _ticketManager.GenerateTickets(players);
            decimal totalRevenue = tickets.Count * _config.TicketCost;

            // 3. Distribute prizes.
            PrizeDistributionResult prizeResult = _prizeDistributor.DistributePrizes(tickets, _config, totalRevenue, _random);

            // 4. Output the results.
            _console.WriteLine("\n--- Players and Ticket Purchases ---");
            _console.WriteLine($"{players.Count} players bought {tickets.Count} tickets worth ${totalRevenue:F2}.");

            _console.WriteLine("\n--- Winning Tickets ---");
            foreach (var win in prizeResult.WinningTickets)
            {
                _console.WriteLine($"{win.Ticket.Player.Name} won {win.PrizeTier} and received ${win.PrizeAmount:F2}");
            }

            _console.WriteLine($"\nHouse Profit: ${prizeResult.HouseProfit}");
        }
    }
}
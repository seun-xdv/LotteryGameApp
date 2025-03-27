using LotteryGame.Configuration;
using LotteryGame.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LotteryGame.Services
{
    /// <summary>
    /// Lottery game logic separated from I/O.
    /// </summary>
    public class LotteryService : ILotteryService
    {
        private readonly LotteryConfig _config;
        private readonly Random _random = new Random();
        private readonly IConsoleService _console;

        public LotteryService(IOptions<LotteryConfig> config, IConsoleService console)
        {
            _config = config.Value;
            _console = console;
        }

        /// <summary>
        /// Runs the game.
        /// </summary>
        public void RunGame()
        {
            // Determine a random number of players between MinPlayers and MaxPlayers.
            int totalPlayers = _random.Next(_config.MinPlayers, _config.MaxPlayers + 1);
            var players = new List<Player>();

            // Create human player (Player 1)
            var humanPlayer = new Player("Player 1", true, _config.InitialBalance);
            players.Add(humanPlayer);

            // Create CPU players
            for (int i = 2; i <= totalPlayers; i++)
            {
                players.Add(new Player($"Player {i}", false, _config.InitialBalance));
            }

            // Human ticket purchase
            _console.WriteLine($"Welcome, Player 1! You have ${_config.InitialBalance:F2}. Each ticket costs ${_config.TicketCost:F2}.");
            _console.Write($"How many tickets do you want to purchase ({_config.MinTicketsPerPlayer}-{_config.MaxTicketsPerPlayer})? ");
            int ticketsToPurchase = GetValidTicketInput();
            humanPlayer.PurchaseTickets(ticketsToPurchase, _config.TicketCost);

            // CPU players ticket purchase (randomly determined within allowed limits)
            foreach (var player in players.Skip(1))
            {
                int cpuTickets = _random.Next(_config.MinTicketsPerPlayer,
                    Math.Min(_config.MaxTicketsPerPlayer, (int)(player.Balance / _config.TicketCost)) + 1);
                player.PurchaseTickets(cpuTickets, _config.TicketCost);
            }

            // Generate tickets for all players.
            var allTickets = new List<Ticket>();
            foreach (var player in players)
            {
                for (int i = 0; i < player.TicketCount; i++)
                {
                    allTickets.Add(new Ticket(player));
                }
            }

            // Total revenue is the number of tickets sold * price per ticket
            int totalTicketsSold = allTickets.Count;
            decimal totalRevenue = totalTicketsSold * _config.TicketCost;

            // Calculate prize pools.
            decimal grandPrizePool = totalRevenue * (decimal)_config.GrandPrizePercentage;
            decimal secondTierPool = totalRevenue * (decimal)_config.SecondTierPrizePercentage;
            decimal thirdTierPool = totalRevenue * (decimal)_config.ThirdTierPrizePercentage;

            // Determine number of winners for second and third tiers.
            int secondTierWinnersCount = (int)Math.Round(allTickets.Count * _config.SecondTierWinnerPercentage, MidpointRounding.AwayFromZero);
            int thirdTierWinnersCount = (int)Math.Round(allTickets.Count * _config.ThirdTierWinnerPercentage, MidpointRounding.AwayFromZero);

            // Shuffle the tickets.
            allTickets = allTickets.OrderBy(x => _random.Next()).ToList();

            var winningTickets = new List<WinningTicket>();

            // --- Grand Prize (one winning ticket) ---
            if (totalTicketsSold > 0)
            {
                var grandPrizeTicket = allTickets.First();
                winningTickets.Add(new WinningTicket(grandPrizeTicket, "Grand Prize", grandPrizePool));
                allTickets.RemoveAt(0);
            }

            // --- Second Tier Prize ---
            int winnersForSecondTier = Math.Min(secondTierWinnersCount, allTickets.Count);
            decimal secondTierDistributed = 0m;

            if (winnersForSecondTier > 0)
            {
                // Select the winning tickets for the second tier.
                var secondTierTickets = allTickets.Take(winnersForSecondTier).ToList();
                allTickets.RemoveRange(0, winnersForSecondTier);

                // Convert the prize pool to cents to avoid fractional rounding issues.
                int totalPrizeCents = (int)(secondTierPool * 100);
                int centsPerWinner = totalPrizeCents / winnersForSecondTier;
                decimal prizePerWinner = centsPerWinner / 100m;
                
                // Calculate the total amount distributed to second tier winners.
                secondTierDistributed = prizePerWinner * winnersForSecondTier;

                // Assign the calculated prize to each winning ticket.
                foreach (var ticket in secondTierTickets)
                {
                    winningTickets.Add(new WinningTicket(ticket, "Second Tier", prizePerWinner));
                }
            }

            // --- Third Tier Prize ---
            int winnersForThirdTier = Math.Min(thirdTierWinnersCount, allTickets.Count);
            decimal thirdTierDistributed = 0m;

            if (winnersForThirdTier > 0)
            {
                // Select the winning tickets for the third tier.
                var thirdTierTickets = allTickets.Take(winnersForThirdTier).ToList();
                allTickets.RemoveRange(0, winnersForThirdTier);

                // Convert the third tier prize pool to cents to avoid fractional rounding issues.
                int totalThirdPrizeCents = (int)(thirdTierPool * 100);
                int centsPerThirdWinner = totalThirdPrizeCents / winnersForThirdTier;
                decimal prizePerThirdWinner = centsPerThirdWinner / 100m;
                
                // Calculate the total amount distributed to third tier winners.
                thirdTierDistributed = prizePerThirdWinner * winnersForThirdTier;

                // Assign the calculated prize to each winning ticket.
                foreach (var ticket in thirdTierTickets)
                {
                    winningTickets.Add(new WinningTicket(ticket, "Third Tier", prizePerThirdWinner));
                }
            }


            // Calculate distributed prize amounts and house profit.
            decimal prizesDistributed = grandPrizePool + secondTierDistributed + thirdTierDistributed;
            decimal houseProfit = totalRevenue - prizesDistributed;

            // --- Output the results ---
            _console.WriteLine("\n--- Players and Ticket Purchases ---");
            _console.WriteLine($"{totalPlayers} players bought {totalTicketsSold} tickets worth ${totalRevenue:F2}.");
            
            _console.WriteLine("\n--- Winning Tickets ---");
            foreach (var win in winningTickets)
            {
                _console.WriteLine($"{win.Ticket.Player.Name} won {win.PrizeTier} and received ${win.PrizeAmount}");
            }
            
            _console.WriteLine($"\nHouse Profit: ${houseProfit}");
        }

        /// <summary>
        /// Reads and validates the human player's input for ticket purchase.
        /// </summary>
        /// <returns>A valid number of tickets.</returns>
        private int GetValidTicketInput()
        {
            int tickets;
            while (!int.TryParse(_console.ReadLine(), out tickets) ||
                   tickets < _config.MinTicketsPerPlayer || tickets > _config.MaxTicketsPerPlayer)
            {
                _console.Write($"Invalid input. Please enter a number between {_config.MinTicketsPerPlayer} and {_config.MaxTicketsPerPlayer}: ");
            }
            return tickets;
        }
    }
}
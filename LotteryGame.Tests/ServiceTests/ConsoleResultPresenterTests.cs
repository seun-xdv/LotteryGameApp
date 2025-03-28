using LotteryGame.Models;
using LotteryGame.Services;
using LotteryGame.Tests.ServiceTests;
using System.Collections.Generic;
using Xunit;

namespace LotteryGame.Tests.ServiceTests
{
    public class ConsoleResultPresenterTests
    {
        [Fact]
        public void PresentResults_DisplaysCorrectOutput()
        {
            // Arrange: Create sample players.
            var player1 = new Player("Player 1", true, 10);
            player1.PurchaseTickets(3, 1.0m);

            var player2 = new Player("Player 2", false, 10);
            player2.PurchaseTickets(5, 1.0m);

            var players = new List<Player> { player1, player2 };

            // Create sample winning tickets.
            var winningTickets = new List<WinningTicket>
            {
                new WinningTicket(new Ticket(player1), "Grand Prize", 5m),
                new WinningTicket(new Ticket(player2), "Second Tier", 3m)
            };

            var prizeResult = new PrizeDistributionResult
            {
                WinningTickets = winningTickets,
                HouseProfit = 2.0m
            };

            // Compute a pre-built summary to avoid logic bleed
            var summary = new PresentationSummary
            {
                TotalPlayers = players.Count,
                TotalTickets = players.Sum(p => p.TicketCount),
                TotalRevenue = players.Sum(p => p.TicketCount) * 1.0m
            };

            // Set up FakeConsoleService to capture output.
            var fakeConsole = new FakeConsoleService(new string[] { });

            // Instantiate the presenter.
            IResultPresenter presenter = new ConsoleResultPresenter();

            // Act: Present the results.
            presenter.PresentResults(players, prizeResult, summary, fakeConsole);

            string output = string.Join("\n", fakeConsole.OutputMessages);
            Assert.Contains("2 players bought 8 tickets worth $8.00", output);
            Assert.Contains("Player 1 won Grand Prize and received $5.00", output);
            Assert.Contains("Player 2 won Second Tier and received $3.00", output);
            Assert.Contains("House Profit: $2.00", output);
        }
    }
}

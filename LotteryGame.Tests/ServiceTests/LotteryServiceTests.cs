using LotteryGame.Configuration;
using LotteryGame.Services;
using LotteryGame.Models;
using Microsoft.Extensions.Options;

namespace LotteryGame.Tests.ServiceTests
{
    public class LotteryServiceTests
    {   
        // Helper method to create a LotteryService with provided configuration and fake console.
        private ILotteryService CreateLotteryService(LotteryConfig config, IConsoleService console)
        {
            var options = Options.Create(config);
            IPlayerManager playerManager = new PlayerManager();
            ITicketManager ticketManager = new TicketManager();
            IPrizeDistributor prizeDistributor = new PrizeDistributor();
            IResultPresenter resultPresenter = new ConsoleResultPresenter();
            IRandomProvider randomProvider = new FakeRandomProvider();
            ISummaryCalculator summaryCalculator = new SummaryCalculator();
            return new LotteryService(options, console, playerManager, ticketManager, prizeDistributor, resultPresenter, summaryCalculator, randomProvider);
        }
        
        [Fact]
        public void RunGame_ExecutesWithoutExceptions()
        {
            // Arrange: Fixed configuration for testing.
            var config = new LotteryConfig
            {
                TicketCost = 1.0m,
                InitialBalance = 10.0m,
                MinPlayers = 10,
                MaxPlayers = 10,
                MinTicketsPerPlayer = 1,
                MaxTicketsPerPlayer = 10,
                GrandPrizePercentage = 0.50,
                SecondTierPrizePercentage = 0.30,
                ThirdTierPrizePercentage = 0.10,
                SecondTierWinnerPercentage = 0.10,
                ThirdTierWinnerPercentage = 0.20
            };

            // Simulate valid user input ("3" tickets) via the fake console.
            var fakeConsole = new FakeConsoleService(new[] { "3" });
            var lotteryService = CreateLotteryService(config, fakeConsole);            

            // Act & Assert: Ensure the game runs without exceptions.
            var exception = Record.Exception(() => lotteryService.RunGame());
            Assert.Null(exception);

            // Verify that output contains expected sections.
            string output = string.Join("\n", fakeConsole.OutputMessages);
            Assert.Contains("Players and Ticket Purchases", output);
            Assert.Contains("Winning Tickets", output);
            Assert.Contains("House Profit", output);
        }
        
        [Fact]
        public void RunGame_InvalidInputHandledGracefully()
        {
            // Arrange: Configuration as before.
            var config = new LotteryConfig
            {
                TicketCost = 1.0m,
                InitialBalance = 10.0m,
                MinPlayers = 10,
                MaxPlayers = 10,
                MinTicketsPerPlayer = 1,
                MaxTicketsPerPlayer = 10,
                GrandPrizePercentage = 0.50,
                SecondTierPrizePercentage = 0.30,
                ThirdTierPrizePercentage = 0.10,
                SecondTierWinnerPercentage = 0.10,
                ThirdTierWinnerPercentage = 0.20
            };

            // Simulate invalid input ("abc") followed by valid input ("4").
            var fakeConsole = new FakeConsoleService(new[] { "abc", "4" });
            var lotteryService = CreateLotteryService(config, fakeConsole);

            // Act
            lotteryService.RunGame();

            // Assert: Verify that the output shows an invalid input message.
            string output = string.Join("\n", fakeConsole.OutputMessages);
            Assert.Contains("Invalid input. Please enter a number between", output);
            Assert.Contains("Players and Ticket Purchases", output);
        }
    }
}

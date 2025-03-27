using LotteryGame.Configuration;
using LotteryGame.Services;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using Xunit;

public class LotteryServiceTests
{
    // Helper to create a LotteryService with a given configuration.
    private ILotteryService CreateLotteryService(LotteryConfig config)
    {
        var options = Options.Create(config);
        return new LotteryService(options);
    }

    [Fact]
    public void RunGame_ExecutesWithoutExceptions()
    {
        // Arrange: Use a fixed configuration for predictability.
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
        var lotteryService = CreateLotteryService(config);

        // Simulate valid user input ("3" tickets) via the console.
        var input = new StringReader("3\n");
        Console.SetIn(input);

        // Capture console output.
        var output = new StringWriter();
        Console.SetOut(output);

        // Act: Run the game.
        var exception = Record.Exception(() => lotteryService.RunGame());

        // Assert: No exceptions are thrown and output includes key sections.
        Assert.Null(exception);
        var consoleOutput = output.ToString();
        Assert.Contains("Players and Ticket Purchases", consoleOutput);
        Assert.Contains("Winning Tickets", consoleOutput);
        Assert.Contains("House Profit", consoleOutput);
    }

    [Fact]
    public void RunGame_InvalidInputHandledGracefully()
    {
        // Arrange: Set up configuration.
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
        var lotteryService = CreateLotteryService(config);

        // Simulate invalid input ("abc") followed by a valid input ("4").
        var input = new StringReader("abc\n4\n");
        Console.SetIn(input);

        // Capture console output.
        var output = new StringWriter();
        Console.SetOut(output);

        // Act: Run the game.
        lotteryService.RunGame();

        // Assert: Output should show the invalid input prompt and then the normal game output.
        var consoleOutput = output.ToString();
        Assert.Contains("Invalid input. Please enter a number between", consoleOutput);
        Assert.Contains("Players and Ticket Purchases", consoleOutput);
    }
}

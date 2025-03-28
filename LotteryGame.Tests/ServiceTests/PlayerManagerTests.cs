using LotteryGame.Configuration;
using LotteryGame.Models;
using LotteryGame.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace LotteryGame.Tests.ServiceTests
{
    public class PlayerManagerTests
    {
        [Fact]
        public void CreatePlayers_HumanInputValid_CreatesCorrectHumanPlayerTickets()
        {
            // Arrange: Force a fixed number of players by setting MinPlayers == MaxPlayers.
            var config = new LotteryConfig
            {
                InitialBalance = 10,
                TicketCost = 1,
                MinPlayers = 5,
                MaxPlayers = 5,
                MinTicketsPerPlayer = 1,
                MaxTicketsPerPlayer = 10
            };

            // Fake console input: valid input "3" for the human player.
            var fakeConsole = new FakeConsoleService(new[] { "3" });
            IPlayerManager playerManager = new PlayerManager();
            var fakeRandom = new FakeRandomProvider();

            // Act
            List<Player> players = playerManager.CreatePlayers(config, fakeConsole, fakeRandom);

            // Assert
            Assert.NotEmpty(players);
            var humanPlayer = players[0];
            Assert.True(humanPlayer.IsHuman);
            Assert.Equal(3, humanPlayer.TicketCount);
            Assert.Equal(5, players.Count); // Fixed to 5 players
        }

        [Fact]
        public void CreatePlayers_InvalidInputRetriesUntilValid()
        {
            // Arrange: Fixed player count.
            var config = new LotteryConfig
            {
                InitialBalance = 10,
                TicketCost = 1,
                MinPlayers = 5,
                MaxPlayers = 5,
                MinTicketsPerPlayer = 1,
                MaxTicketsPerPlayer = 10
            };

            // Simulate invalid input ("abc") then valid input ("4").
            var fakeConsole = new FakeConsoleService(new[] { "abc", "4" });
            IPlayerManager playerManager = new PlayerManager();
            var fakeRandom = new FakeRandomProvider();

            // Act
            List<Player> players = playerManager.CreatePlayers(config, fakeConsole, fakeRandom);

            // Assert
            var humanPlayer = players[0];
            Assert.Equal(4, humanPlayer.TicketCount);
        }
    }
}

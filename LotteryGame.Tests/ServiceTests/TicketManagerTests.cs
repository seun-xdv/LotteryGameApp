using LotteryGame.Models;
using LotteryGame.Services;
using System.Collections.Generic;
using Xunit;

namespace LotteryGame.Tests.ServiceTests
{
    public class TicketManagerTests
    {
        [Fact]
        public void GenerateTickets_ReturnsCorrectNumberOfTickets()
        {
            // Arrange: Create players with predetermined ticket counts.
            var players = new List<Player>
            {
                new Player("Player 1", true, 10),
                new Player("Player 2", false, 10),
                new Player("Player 3", false, 10)
            };

            // Set ticket counts.
            players[0].PurchaseTickets(3, 1);
            players[1].PurchaseTickets(5, 1);
            players[2].PurchaseTickets(2, 1);
            int expectedTicketCount = 3 + 5 + 2; // 10 tickets

            ITicketManager ticketManager = new TicketManager();

            // Act: Generate tickets
            List<Ticket> tickets = ticketManager.GenerateTickets(players);

            // Assert: Check that tickets match expected amount (10)
            Assert.Equal(expectedTicketCount, tickets.Count);
        }
    }
}

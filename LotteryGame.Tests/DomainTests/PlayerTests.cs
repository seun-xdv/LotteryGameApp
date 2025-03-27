using LotteryGame.Models;
using Xunit;

public class PlayerTests
{
    [Fact]
    public void PurchaseTickets_WithinBalance_PurchasesCorrectly()
    {
        // Arrange: Create a player with a $10 balance.
        var player = new Player("Test Player", isHuman: false, initialBalance: 10);
        int ticketsToBuy = 5;
        decimal ticketCost = 1m;

        // Act: Buy 5 tickets at $1.00 each
        player.PurchaseTickets(ticketsToBuy, ticketCost);

        // Assert: The player should have purchased 5 tickets and have $5 remaining.
        Assert.Equal(5, player.TicketCount);
        Assert.Equal(5, player.Balance);
    }

    [Fact]
    public void PurchaseTickets_ExceedingBalance_PurchasesMaxPossible()
    {
        // Arrange: Create a player with a $10 balance.
        var player = new Player("Test Player", isHuman: false, initialBalance: 10);
        int ticketsToBuy = 15;
        decimal ticketCost = 1m;

        // Act
        player.PurchaseTickets(ticketsToBuy, ticketCost);

        // Assert: The player can only purchase 10 tickets (exhausting the balance).
        Assert.Equal(10, player.TicketCount);
        Assert.Equal(0, player.Balance);
    }
}

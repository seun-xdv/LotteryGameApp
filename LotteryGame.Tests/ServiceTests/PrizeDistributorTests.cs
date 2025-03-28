using LotteryGame.Configuration;
using LotteryGame.Models;
using LotteryGame.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace LotteryGame.Tests.ServiceTests
{
    public class PrizeDistributorTests
    {
        [Fact]
        public void DistributePrizes_CorrectlyCalculatesWinnersAndHouseProfit()
        {
            // Arrange: Create 10 tickets for a single player.
            var player = new Player("Test Player", false, 10);
            player.PurchaseTickets(10, 1); // 10 tickets
            var tickets = new List<Ticket>();
            for (int i = 0; i < player.TicketCount; i++)
            {
                tickets.Add(new Ticket(player));
            }
            // Total revenue: 10

            // Configure percentages such that:
            // - Grand Prize: 50% -> $5
            // - Second Tier: 30% -> $3, with 10% of tickets as winners => round(10*0.1)=1 winner.
            // - Third Tier: 10% -> $1, with 20% of tickets as winners => round(10*0.2)=2 winners.
            // Expected: Grand winner gets $5, second tier winner gets $3, third tier winners get $0.50 each,
            // House profit = 10 - 9 = 1.
            var config = new LotteryConfig
            {
                TicketCost = 1,
                GrandPrizePercentage = 0.50,
                SecondTierPrizePercentage = 0.30,
                ThirdTierPrizePercentage = 0.10,
                SecondTierWinnerPercentage = 0.10,
                ThirdTierWinnerPercentage = 0.20
            };
            decimal totalRevenue = tickets.Count * config.TicketCost;
            IPrizeDistributor prizeDistributor = new PrizeDistributor();
            var fakeRandom = new FakeRandomProvider();

            // Act
            PrizeDistributionResult result = prizeDistributor.DistributePrizes(new List<Ticket>(tickets), config, totalRevenue, fakeRandom);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.WinningTickets.Count);

            decimal distributed = result.WinningTickets.Sum(w => w.PrizeAmount);
            Assert.Equal(totalRevenue - result.HouseProfit, distributed);

            var grandWinner = result.WinningTickets.Find(w => w.PrizeTier == "Grand Prize");
            Assert.NotNull(grandWinner);
            Assert.Equal(5.0m, grandWinner.PrizeAmount);

            var secondTierWinner = result.WinningTickets.Find(w => w.PrizeTier == "Second Tier");
            Assert.NotNull(secondTierWinner);
            Assert.Equal(3.0m, secondTierWinner.PrizeAmount);

            var thirdTierWinners = result.WinningTickets.FindAll(w => w.PrizeTier == "Third Tier");
            Assert.Equal(2, thirdTierWinners.Count);
            foreach (var winner in thirdTierWinners)
            {
                Assert.Equal(0.5m, winner.PrizeAmount);
            }
        }
    }
}

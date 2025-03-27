using LotteryGame.Configuration;
using LotteryGame.Models;

namespace LotteryGame.Services
{
    /// <summary>
    /// Result of prize distribution including winning tickets and house profit.
    /// </summary>
    public class PrizeDistributor : IPrizeDistributor
    {
        // refactored section
        public PrizeDistributionResult DistributePrizes(List<Ticket> tickets, LotteryConfig config, decimal totalRevenue, Random random)
        {
            var result = new PrizeDistributionResult();

            // 1. Calculate prize pools.
            var pools = CalculatePrizePools(totalRevenue, config);

            // 2. Shuffle tickets.
            tickets = tickets.OrderBy(x => random.Next()).ToList();
            var winningTickets = new List<WinningTicket>();

            // 3. Grand Prize.
            if (tickets.Count > 0)
            {
                var grandPrizeWinner = SelectGrandPrize(ref tickets, pools.grandPrizePool);
                winningTickets.Add(grandPrizeWinner);
            }

            // 4. Determine winners count for second and third tiers.
            int secondTierWinnersCount = (int)Math.Round(tickets.Count * config.SecondTierWinnerPercentage, MidpointRounding.AwayFromZero);
            int thirdTierWinnersCount = (int)Math.Round(tickets.Count * config.ThirdTierWinnerPercentage, MidpointRounding.AwayFromZero);

            // 5. Second Tier Winners.
            var secondTierWinners = SelectWinners(ref tickets, secondTierWinnersCount, pools.secondTierPool, "Second Tier");
            winningTickets.AddRange(secondTierWinners);

            // 6. Third Tier Winners.
            var thirdTierWinners = SelectWinners(ref tickets, thirdTierWinnersCount, pools.thirdTierPool, "Third Tier");
            winningTickets.AddRange(thirdTierWinners);

            // 7. Compute house profit.
            decimal prizesDistributed = winningTickets.Sum(w => w.PrizeAmount);
            result.HouseProfit = totalRevenue - prizesDistributed;
            result.WinningTickets = winningTickets;

            return result;
        }

        /// <summary>
        /// Calculates the prize pools for each tier.
        /// </summary>
        private (decimal grandPrizePool, decimal secondTierPool, decimal thirdTierPool) CalculatePrizePools(decimal totalRevenue, LotteryConfig config)
        {
            decimal grandPrizePool = totalRevenue * (decimal)config.GrandPrizePercentage;
            decimal secondTierPool = totalRevenue * (decimal)config.SecondTierPrizePercentage;
            decimal thirdTierPool = totalRevenue * (decimal)config.ThirdTierPrizePercentage;
            return (grandPrizePool, secondTierPool, thirdTierPool);
        }

        /// <summary>
        /// Selects the grand prize winner.
        /// </summary>
        private WinningTicket SelectGrandPrize(ref List<Ticket> tickets, decimal grandPrizePool)
        {
            var selectedTicket = tickets.First();
            tickets.RemoveAt(0);
            return new WinningTicket(selectedTicket, "Grand Prize", grandPrizePool);
        }
        
        /// <summary>
        /// Selects winners for a given prize tier.
        /// </summary>
        private List<WinningTicket> SelectWinners(ref List<Ticket> tickets, int winnersCount, decimal prizePool, string tierName)
        {
            var selectedTickets = tickets.Take(Math.Min(winnersCount, tickets.Count)).ToList();
            var selectedTicketsCount = selectedTickets.Count;
            tickets.RemoveRange(0, selectedTickets.Count);
            var winners = new List<WinningTicket>();
            
            if (selectedTicketsCount > 0)
            {
                // Convert the prize pool to cents to avoid fractional rounding issues.
                int totalPrizeCents = (int)(prizePool * 100);
                int centsPerWinner = totalPrizeCents / selectedTicketsCount;
                decimal prizePerWinner = centsPerWinner / 100m;
                
                foreach (var ticket in selectedTickets)
                {
                    winners.Add(new WinningTicket(ticket, tierName, prizePerWinner));
                }
            }
            return winners;
        }
    }
}

using LotteryGame.Configuration;
using LotteryGame.Models;

namespace LotteryGame.Services
{
    /// <summary>
    /// Result of prize distribution including winning tickets and house profit.
    /// </summary>
    public class PrizeDistributionResult
    {
        public List<WinningTicket> WinningTickets { get; set; } = new List<WinningTicket>();
        public decimal HouseProfit { get; set; }
    }

    public class PrizeDistributor : IPrizeDistributor
    {
        public PrizeDistributionResult DistributePrizes(List<Ticket> tickets, LotteryConfig config, decimal totalRevenue, Random random)
        {
            var result = new PrizeDistributionResult();

            // Calculate prize pools.
            decimal grandPrizePool = totalRevenue * (decimal)config.GrandPrizePercentage;
            decimal secondTierPool = totalRevenue * (decimal)config.SecondTierPrizePercentage;
            decimal thirdTierPool = totalRevenue * (decimal)config.ThirdTierPrizePercentage;

            // Determine number of winners.
            int secondTierWinnersCount = (int)Math.Round(tickets.Count * config.SecondTierWinnerPercentage, MidpointRounding.AwayFromZero);
            int thirdTierWinnersCount = (int)Math.Round(tickets.Count * config.ThirdTierWinnerPercentage, MidpointRounding.AwayFromZero);

            // Shuffle tickets.
            tickets = tickets.OrderBy(x => random.Next()).ToList();
            var winningTickets = new List<WinningTicket>();

            // Total revenue is the number of tickets sold
            int totalTicketsSold = tickets.Count;

            // --- Grand Prize (one winning ticket) ---
            if (totalTicketsSold > 0)
            {
                var grandPrizeTicket = tickets.First();
                winningTickets.Add(new WinningTicket(grandPrizeTicket, "Grand Prize", grandPrizePool));
                tickets.RemoveAt(0);
            }

            // --- Second Tier Prize ---
            int winnersForSecondTier = Math.Min(secondTierWinnersCount, tickets.Count);
            decimal secondTierDistributed = 0m;

            if (winnersForSecondTier > 0)
            {
                // Select the winning tickets for the second tier.
                var secondTierTickets = tickets.Take(winnersForSecondTier).ToList();
                tickets.RemoveRange(0, winnersForSecondTier);

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
            int winnersForThirdTier = Math.Min(thirdTierWinnersCount, tickets.Count);
            decimal thirdTierDistributed = 0m;

            if (winnersForThirdTier > 0)
            {
                // Select the winning tickets for the third tier.
                var thirdTierTickets = tickets.Take(winnersForThirdTier).ToList();
                tickets.RemoveRange(0, winnersForThirdTier);

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
            result.HouseProfit = totalRevenue - prizesDistributed;
            result.WinningTickets = winningTickets;

            return result;
        }
    }
}

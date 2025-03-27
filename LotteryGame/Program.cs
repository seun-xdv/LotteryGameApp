using System;
using System.Collections.Generic;
using System.Linq;

namespace LotteryGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize random generator
            Random random = new Random();

            // Determine a random total number of players (between 10 and 15)
            int totalPlayers = random.Next(10, 16);
            var players = new List<Player>();

            // Create Player 1 (human)
            var player1 = new Player("Player 1", isHuman: true);
            players.Add(player1);

            // Create CPU players (Player 2, Player 3, ...)
            for (int i = 2; i <= totalPlayers; i++)
            {
                players.Add(new Player($"Player {i}", isHuman: false));
            }

            // Ticket purchase for Player 1 (human)
            Console.WriteLine("Welcome, Player 1! You have $10.00. Each ticket costs $1.00.");
            Console.Write("How many tickets do you want to purchase (1-10)? ");
            int ticketsToPurchase = GetValidTicketInput();
            
            // Ensure they do not exceed their balance
            ticketsToPurchase = Math.Min(ticketsToPurchase, player1.Balance);
            player1.PurchaseTickets(ticketsToPurchase);

            // CPU players purchase a random number of tickets (between 1 and 10, capped by $10)
            foreach (var player in players.Skip(1))
            {
                int cpuTickets = random.Next(1, Math.Min(10, player.Balance) + 1);
                player.PurchaseTickets(cpuTickets);
            }

            // Create a list of tickets from all players
            var allTickets = new List<Ticket>();
            foreach (var player in players)
            {
                for (int i = 0; i < player.TicketCount; i++)
                {
                    allTickets.Add(new Ticket(player));
                }
            }

            // Total revenue is the number of tickets sold (each ticket costs $1)
            int totalTicketsSold = allTickets.Count;
            decimal totalRevenue = totalTicketsSold;

            // Prize pools based on revenue
            decimal grandPrizePool = totalRevenue * 0.50m;
            decimal secondTierPool = totalRevenue * 0.30m;
            decimal thirdTierPool = totalRevenue * 0.10m;

            // Determine number of winners for second and third tiers (rounded to the nearest whole number)
            int secondTierWinnersCount = (int)Math.Round(totalTicketsSold * 0.10, MidpointRounding.AwayFromZero);
            int thirdTierWinnersCount = (int)Math.Round(totalTicketsSold * 0.20, MidpointRounding.AwayFromZero);

            // Shuffle tickets
            allTickets = allTickets.OrderBy(x => random.Next()).ToList();

            // List to hold winning tickets and prizes
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

            Console.WriteLine("\n--- Players and Ticket Purchases ---");
            Console.WriteLine($"{totalPlayers} players bought {totalTicketsSold} tickets worth ${totalRevenue:F2}.");

            Console.WriteLine("\n--- Winning Tickets ---");
            foreach (var win in winningTickets)
            {
                Console.WriteLine($"{win.Ticket.Player.Name} won {win.PrizeTier} and received ${win.PrizeAmount:F2}");
            }

            Console.WriteLine($"\nHouse Profit: ${houseProfit:F2}");
        }

        // Reads and validates user input ensuring a number between 1 and 10
        static int GetValidTicketInput()
        {
            int tickets;
            while (!int.TryParse(Console.ReadLine(), out tickets) || tickets < 1 || tickets > 10)
            {
                Console.Write("Invalid input. Please enter a number between 1 and 10: ");
            }
            return tickets;
        }
    }

    // Represents a player in the lottery
    class Player
    {
        public string Name { get; }
        public int Balance { get; private set; } = 10;
        public int TicketCount { get; private set; }
        public bool IsHuman { get; }

        public Player(string name, bool isHuman)
        {
            Name = name;
            IsHuman = isHuman;
        }

        // Purchase tickets while deducting from balance
        public void PurchaseTickets(int count)
        {
            int purchasable = Math.Min(count, Balance);
            TicketCount += purchasable;
            Balance -= purchasable;
        }
    }

    // Represents a ticket purchased by a player
    class Ticket
    {
        public Player Player { get; }
        public Ticket(Player player)
        {
            Player = player;
        }
    }

    // Represents a winning ticket along with its prize tier and amount
    class WinningTicket
    {
        public Ticket Ticket { get; }
        public string PrizeTier { get; }
        public decimal PrizeAmount { get; }
        public WinningTicket(Ticket ticket, string prizeTier, decimal prizeAmount)
        {
            Ticket = ticket;
            PrizeTier = prizeTier;
            PrizeAmount = prizeAmount;
        }
    }
}
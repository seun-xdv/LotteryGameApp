namespace LotteryGame.Models
{
    /// <summary>
    /// A player in the lottery game.
    /// </summary>
    public class Player
    {
        public string Name { get; }
        public bool IsHuman { get; }
        public decimal Balance { get; private set; }
        public int TicketCount { get; private set; }

        public Player(string name, bool isHuman, decimal initialBalance)
        {
            Name = name;
            IsHuman = isHuman;
            Balance = initialBalance;
        }

        /// <summary>
        /// Purchase tickets and update balance.
        /// </summary>
        /// <param name="count">Number of tickets to purchase.</param>
        /// <param name="ticketCost">Cost per ticket.</param>
        public void PurchaseTickets(int count, decimal ticketCost)
        {
            // Ensure the player does not purchase more tickets than they can afford.
            int purchasable = (int)Math.Min(count, Balance / ticketCost);
            TicketCount += purchasable;
            Balance -= purchasable * ticketCost;
        }
    }
}
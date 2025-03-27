namespace LotteryGame.Models
{
    /// <summary>
    /// A winning ticket with its prize tier and prize amount.
    /// </summary>
    public class WinningTicket
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
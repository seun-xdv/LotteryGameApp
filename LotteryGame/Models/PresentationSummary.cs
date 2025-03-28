namespace LotteryGame.Models
{
    /// <summary>
    /// Contains precomputed summary data for result presentation.
    /// </summary>
    public class PresentationSummary
    {
        public int TotalPlayers { get; set; }
        public int TotalTickets { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
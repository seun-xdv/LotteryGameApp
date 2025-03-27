namespace LotteryGame.Configuration
{
    /// <summary>
    /// Configuration settings for the lottery game.
    /// </summary>
    public class LotteryConfig
    {
        public decimal TicketCost { get; set; }
        public decimal InitialBalance { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int MinTicketsPerPlayer { get; set; }
        public int MaxTicketsPerPlayer { get; set; }
        public double GrandPrizePercentage { get; set; }
        public double SecondTierPrizePercentage { get; set; }
        public double ThirdTierPrizePercentage { get; set; }
        public double SecondTierWinnerPercentage { get; set; }
        public double ThirdTierWinnerPercentage { get; set; }
    }
}
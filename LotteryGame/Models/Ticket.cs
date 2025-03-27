namespace LotteryGame.Models
{
    /// <summary>
    /// A ticket purchased by a player.
    /// </summary>
    public class Ticket
    {
        public Player Player { get; }
        public Ticket(Player player)
        {
            Player = player;
        }
    }
}
using LotteryGame.Models;

namespace LotteryGame.Services
{
    /// <summary>
    /// Generates individual tickets from players' ticket counts.
    /// </summary>
    public class TicketManager : ITicketManager
    {
        public List<Ticket> GenerateTickets(List<Player> players)
        {
            var tickets = new List<Ticket>();
            foreach (var player in players)
            {
                for (int i = 0; i < player.TicketCount; i++)
                {
                    tickets.Add(new Ticket(player));
                }
            }
            return tickets;
        }
    }
}

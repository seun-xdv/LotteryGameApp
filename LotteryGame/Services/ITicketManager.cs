using LotteryGame.Models;

namespace LotteryGame.Services
{
    public interface ITicketManager
    {
        /// <summary>
        /// Generates a list of tickets based on the players’ ticket counts.
        /// </summary>
        List<Ticket> GenerateTickets(List<Player> players);
    }
}
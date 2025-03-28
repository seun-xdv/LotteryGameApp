using LotteryGame.Configuration;
using LotteryGame.Models;
using Microsoft.Extensions.Options;
using LotteryGame.Utilities;
using System.Collections.Generic;

namespace LotteryGame.Services
{
    /// <summary>
    /// Orchestrates the lottery game flow by leveraging modular services.
    /// </summary>
    public class LotteryService : ILotteryService
    {
        private readonly LotteryConfig _config;
        private readonly IConsoleService _console;
        private readonly IPlayerManager _playerManager;
        private readonly ITicketManager _ticketManager;
        private readonly IPrizeDistributor _prizeDistributor;
        private readonly IResultPresenter _resultPresenter;
        private readonly ISummaryCalculator _summaryCalculator;
        private readonly IRandomProvider _randomProvider;

        public LotteryService(IOptions<LotteryConfig> config, IConsoleService console,
                              IPlayerManager playerManager, ITicketManager ticketManager,
                              IPrizeDistributor prizeDistributor, IResultPresenter resultPresenter,
                              ISummaryCalculator summaryCalculator, IRandomProvider randomProvider)
        {
            _config = config.Value;
            _console = console;
            _playerManager = playerManager;
            _ticketManager = ticketManager;
            _prizeDistributor = prizeDistributor;
            _resultPresenter = resultPresenter;
            _summaryCalculator = summaryCalculator;
            _randomProvider = randomProvider;
        }

        public void RunGame()
        {
            // Validate configuration.
            ConfigurationValidator.Validate(_config);

            // 1. Create players and process ticket purchases.
            List<Player> players = _playerManager.CreatePlayers(_config, _console, _randomProvider);

            // 2. Generate tickets.
            List<Ticket> tickets = _ticketManager.GenerateTickets(players);
            decimal totalRevenue = tickets.Count * _config.TicketCost;

            // 3. Distribute prizes.
            PrizeDistributionResult prizeResult = _prizeDistributor.DistributePrizes(tickets, _config, totalRevenue, _randomProvider);

            // 4. Calculate summary via the dedicated service.
            PresentationSummary summary = _summaryCalculator.CalculateSummary(players, _config.TicketCost);

            // 5. Present the results.
            _resultPresenter.PresentResults(players, prizeResult, summary, _console);
        }
    }
}
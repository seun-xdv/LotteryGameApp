using LotteryGame.Configuration;
using LotteryGame.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

var services = new ServiceCollection();

// Build configuration from appsettings.json.
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();
services.AddSingleton<IConfiguration>(configuration);

// Bind LotteryConfig from the configuration.
services.Configure<LotteryConfig>(configuration.GetSection("LotteryConfig"));

// Register the console service and lottery service.
services.AddSingleton<IConsoleService, ConsoleService>();
services.AddSingleton<ILotteryService, LotteryService>();

// Build service provider.
var serviceProvider = services.BuildServiceProvider();

// Resolve lottery service.
var lotteryService = serviceProvider.GetRequiredService<ILotteryService>();

// Execute app logic.
lotteryService.RunGame();
# Lottery Game Application

A lottery game written in C# (.NET 8). This project demonstrates best practices such as separation of concerns, dependency injection, unit testing and clear modular structure.

## Project Overview

The Lottery Game simulates a simple lottery where players purchase tickets and winners are randomly selected across multiple prize tiers. The architecture emphasises modularisation, testability and clean separation between business logic and input/output operations.

## Project Structure

```
LotteryGameApp/
├── LotteryGame/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Configuration/
│   │   └── LotteryConfig.cs
│   ├── Models/
│   │   ├── Player.cs
│   │   ├── PresentationSummary.cs
│   │   ├── Ticket.cs
│   │   └── WinningTicket.cs
│   ├── Services/
│   │   ├── ConsoleResultPresenter.cs
│   │   ├── ConsoleService.cs
│   │   ├── IConsoleService.cs
│   │   ├── IPlayerManager.cs
│   │   ├── IPrizeDistributor.cs
│   │   ├── IRandomProvider.cs
│   │   ├── IResultPresenter.cs
│   │   ├── ISummaryCalculator.cs
│   │   ├── ITicketManager.cs
│   │   ├── LotteryService.cs
│   │   ├── PlayerManager.cs
│   │   ├── PrizeDistributor.cs
│   │   ├── RandomProvider.cs
│   │   └── SummaryCalculator.cs
│   └── Utilities/
│       ├── ConfigurationValidator.cs
│       └── InputValidator.cs
└── LotteryGame.Tests/
    ├── DomainTests/
    │   └── PlayerTests.cs
    └── ServiceTests/
        ├── ConsoleResultPresenterTests.cs
        ├── FakeConsoleService.cs
        ├── FakeRandomProvider.cs
        ├── LotteryServiceTests.cs
        ├── PlayerManagerTests.cs
        ├── PrizeDistributorTests.cs
        └── TicketManagerTests.cs
```

## Features

- **Modularity**: Clear separation of responsibilities into dedicated services:
  - Player management (`PlayerManager`)
  - Ticket management (`TicketManager`)
  - Prize distribution (`PrizeDistributor`)
- **Configurability**: External configuration via `appsettings.json`.
- **Dependency Injection**: Managed through Microsoft's dependency injection (`ServiceCollection`).
- **Testing**: Comprehensive unit tests using xUnit and fake implementations (`FakeConsoleService`, `FakeRandomProvider`) to ensure predictable, reliable tests.

## Prerequisites

- .NET 8 SDK installed ([Download](https://dotnet.microsoft.com/download))

## Setup & Run

1. **Clone Repository:**

```bash
git clone https://github.com/your-username/LotteryGameApp.git
cd LotteryGameApp
```

2. **Restore Dependencies:**

```bash
dotnet restore
```

3. **Build Project:**

```bash
dotnet build
```

4. **Run Application:**

```bash
dotnet run --project LotteryGame/LotteryGame.csproj
```

> **Note:** The above commands work cross-platform (Windows, Linux, macOS). For Windows Command Prompt or PowerShell, the commands remain identical.

## Run Tests

Execute unit tests via:

```bash
dotnet test
```

## Configuration

Game settings can be customised in `LotteryGame/appsettings.json`:

```json
{
  "LotteryConfig": {
    "TicketCost": 1.0,
    "InitialBalance": 10.0,
    "MinPlayers": 10,
    "MaxPlayers": 100,
    "MinTicketsPerPlayer": 1,
    "MaxTicketsPerPlayer": 10,
    "GrandPrizePercentage": 0.50,
    "SecondTierPrizePercentage": 0.30,
    "ThirdTierPrizePercentage": 0.10,
    "SecondTierWinnerPercentage": 0.10,
    "ThirdTierWinnerPercentage": 0.20
  }
}
```

## Contributing

Contributions are welcome! Please create an issue or open a pull request to discuss your proposed changes.

## License

Distributed under the MIT License. See `LICENSE` for more information.

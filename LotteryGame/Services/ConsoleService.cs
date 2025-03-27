using System;

namespace LotteryGame.Services
{
    /// <summary>
    /// Production implementation of IConsoleService that wraps System.Console.
    /// </summary>
    public class ConsoleService : IConsoleService
    {
        public string? ReadLine() => Console.ReadLine();
        public void WriteLine(string message) => Console.WriteLine(message);
        public void Write(string message) => Console.Write(message);
    }
}
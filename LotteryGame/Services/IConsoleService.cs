namespace LotteryGame.Services
{
    /// <summary>
    /// Abstraction for I/O operations.
    /// </summary>
    public interface IConsoleService
    {
        string? ReadLine();
        void WriteLine(string message);
        void Write(string message);
    }
}
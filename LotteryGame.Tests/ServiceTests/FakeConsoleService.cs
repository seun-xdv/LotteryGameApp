using LotteryGame.Services;

namespace LotteryGame.Tests.ServiceTests
{
    /// <summary>
    /// Implement IConsoleService for test builds.
    /// </summary>
    public class FakeConsoleService : IConsoleService
    {
        private readonly Queue<string> _inputQueue;
        public List<string> OutputMessages { get; } = new List<string>();

        public FakeConsoleService(IEnumerable<string> inputs)
        {
            _inputQueue = new Queue<string>(inputs);
        }

        public string ReadLine() => _inputQueue.Count > 0 ? _inputQueue.Dequeue() : "1";
        public void WriteLine(string message) => OutputMessages.Add(message);
        public void Write(string message) => OutputMessages.Add(message);
    }
}

using LotteryGame.Services;

namespace LotteryGame.Tests.ServiceTests
{
    /// <summary>
    /// Fake randomizer for predictable behaviour in tests.
    /// </summary>
    public class FakeRandomProvider : IRandomProvider
    {
        public int Next() => 0;
        public int Next(int maxValue) => 0;
        public int Next(int minValue, int maxValue) => minValue;
    }
}

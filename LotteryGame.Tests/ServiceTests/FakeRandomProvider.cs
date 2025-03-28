using LotteryGame.Services;

namespace LotteryGame.Tests.ServiceTests
{
    /// <summary>
    /// Implements IRandomProvider for test builds.
    /// </summary>
    public class FakeRandomProvider : IRandomProvider
    {
        public int Next() => 0;
        public int Next(int maxValue) => 0;
        public int Next(int minValue, int maxValue) => minValue;
    }
}

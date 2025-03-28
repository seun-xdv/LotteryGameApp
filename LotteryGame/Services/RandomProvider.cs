using System;

namespace LotteryGame.Services
{
    public class RandomProvider : IRandomProvider
    {
        private readonly Random _random;
        public RandomProvider() => _random = new Random();
        public int Next() => _random.Next();
        public int Next(int maxValue) => _random.Next(maxValue);
        public int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);
    }
}

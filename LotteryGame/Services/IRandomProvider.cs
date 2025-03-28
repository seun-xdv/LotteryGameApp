namespace LotteryGame.Services
{
    /// <summary>
    /// Abstracts random number generation for test/production scenarios.
    /// </summary>
    public interface IRandomProvider
    {
        int Next();
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
    }
}

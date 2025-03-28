namespace LotteryGame.Services
{
    public interface IRandomProvider
    {
        int Next();
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
    }
}

using LotteryGame.Services;

namespace LotteryGame.Utilities
{
    public static class InputValidator
    {
        public static int ReadValidInteger(IConsoleService console, int min, int max)
        {
            int value;
            while (!int.TryParse(console.ReadLine(), out value) || value < min || value > max)
            {
                console.Write($"Invalid input. Please enter a number between {min} and {max}: ");
            }
            return value;
        }
    }
}

namespace Roulette
{
    internal class Program
    {
        private static readonly int MinPlayes = 20;
        private static readonly int MaxPlayers = 100;
        private static readonly int MinPlayerBalance = 2500;
        private static readonly int MaxPlayerBalance = 3000;
        private static readonly int MinRouletteNumber = 0;
        private static readonly int MaxRouletteNumber = 25;
        private static readonly int PlayersAtTable = 5;

        static void Main(string[] args)
        {
            int playersCount = new Random().Next(MinPlayes, MaxPlayers + 1);
            RouletteGame game = new(MinPlayerBalance, MaxPlayerBalance, MinRouletteNumber, MaxRouletteNumber, playersCount, PlayersAtTable);
            game.Start();
            Console.ReadKey();
        }
    }
}
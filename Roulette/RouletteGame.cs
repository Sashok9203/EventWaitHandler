using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    internal class RouletteGame
    {
        private readonly string logPath = @"D:\GameLog.txt";
        private readonly int maxPlayers, maxNumber, minNumber;
        private int rouletteNumber,blocked,playersInGame;
        private List<Player> players;
        private Random rnd;
        private Semaphore tableSemaphore;
        private Semaphore playersSemaphore;
        private AutoResetEvent rouletteResetEvent;

        public RouletteGame(int minBalance, int maxBalance, int minNumber, int maxNumber,int playersCount,int maxPlayers)
        {
            this.maxPlayers = maxPlayers;
            this.minNumber = minNumber;
            this.maxNumber = maxNumber;
            playersInGame = playersCount;
            rnd = new();
            players = new();
            tableSemaphore = new(5, maxPlayers);
            playersSemaphore = new(0, maxPlayers);
            rouletteResetEvent = new(true);
            for (int i = 1; i <= playersCount; i++)
                players.Add(new(i,rnd.Next(minBalance, maxBalance+1), minNumber, maxNumber));
        }

        public void Start()
        {
            foreach (var player in players)
                ThreadPool.QueueUserWorkItem(playerGame,player);
            Thread roulette = new(spinRoulette);
            roulette.Start();
            roulette.Join();
            Console.WriteLine($"\nGame over...");
            try
            {
                using StreamWriter sw = new(logPath);
                foreach (var player in players)
                    sw.WriteLine(player.ToString());
                Console.WriteLine($"Game log saved in {logPath} ");
            }
            catch
            {
                Console.WriteLine($"Error save game log in {logPath} ");
            }
            
        }

        private void spinRoulette() 
        {
            Thread.Sleep(100);
            do
            {
                rouletteResetEvent.WaitOne();
                rouletteNumber = rnd.Next(minNumber,maxNumber+1);
                Console.WriteLine($"-= Roulet number {rouletteNumber} =-");
                blocked = 0;
                playersSemaphore.Release(maxPlayers);
            }
            while ( playersInGame >= maxPlayers);
        }

        private void playerGame(object player)
        {
            Player pl = player as Player;
            bool inGame;
            tableSemaphore.WaitOne();
            Console.WriteLine($"-= A new player at the table \"Player {pl.PlayerNumber}\" =-");
            do
            {
                playersSemaphore.WaitOne();
                inGame = pl.Play(rouletteNumber);
                if(!inGame) Interlocked.Decrement(ref playersInGame);
                if (Interlocked.Increment(ref blocked) == maxPlayers) rouletteResetEvent.Set();
            }
            while (inGame);
            tableSemaphore.Release();
        }
    }
}

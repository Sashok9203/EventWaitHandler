using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    internal class Player
    {
        Random rnd;
        private int rouletteStartNumber, rouletteEndNumber;
        private int bet { get; set; }
        private int betNumber { get; set; }
        private int balance { get; init; }
        private int currentBalance { get; set; }
        public  int PlayerNumber { get; init; }

        public Player(int playerNumber, int balance, int roulStartNumber, int roulEndNumber)
        {
            rnd = new();
            this.balance = balance;
            currentBalance = this.balance;
            rouletteStartNumber = roulStartNumber;
            rouletteEndNumber = roulEndNumber;
            this.PlayerNumber = playerNumber;
            makeBet();
        }

        public bool Play(int rouletteNumber)
        {
            if (betNumber != rouletteNumber)
            {
                currentBalance -= bet;
                Console.WriteLine($"Player {PlayerNumber}  -  Bet number [{betNumber}] Bet was lost - [{bet}]  balance - [{currentBalance}]");
                if (currentBalance <= 0) return false;
            }
            else
            {
                currentBalance += bet;
                Console.WriteLine($"Player {PlayerNumber}  -  Bet number [{betNumber}] Bet won      - [{bet}]  Balance - [{currentBalance}]");
            }
            makeBet();
            return true;
        }

        private void makeBet()
        {
            bet = currentBalance > 1 ? rnd.Next(1, currentBalance / 2) : currentBalance;
            betNumber = rnd.Next(rouletteStartNumber, rouletteEndNumber + 1);
        }

        public override string ToString() => $"Player {PlayerNumber} [{balance}] [{currentBalance}]";
    }
}

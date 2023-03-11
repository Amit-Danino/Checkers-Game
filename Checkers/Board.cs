using System;

namespace Checkers
{
    public class Board
    {
        public static void StartGame(Player io_Player1, Player io_Player2)
        {
            io_Player1.InitSoldiers();
            io_Player2.InitSoldiers();
        }
        
        public static int CountPoints(Player i_Player1, Player i_Player2)
        {
            int countPoints = 0;

            countPoints += playerPoints(i_Player1);
            countPoints -= playerPoints(i_Player2);

            return Math.Abs(countPoints);
        }

        private static int playerPoints(Player i_Player)
        {
            int countPoints = 0;

            foreach (Soldier soldier in i_Player.m_Soldiers)
            {
                if (soldier != null)
                {
                    if (soldier.IsKing())
                    {
                        countPoints += 4;
                    }
                    else
                    {
                        countPoints++;
                    }
                }
            }

            return countPoints;
        }
    }
}

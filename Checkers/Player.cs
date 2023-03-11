using System;
using System.Collections.Generic;
using System.Drawing;

namespace Checkers
{
    public class Player
    {
        private readonly string r_Name;
        private readonly int r_BoardSize;
        private readonly bool r_IsComputer;
        private readonly int r_PlayerId;
        private int m_points = 0;
        public Soldier[,] m_Soldiers;

        public Player(string i_Name, int i_BoardSize, int i_PlayerNumber, bool i_IsComputer)
        {
            this.r_Name = i_Name;
            this.r_BoardSize = i_BoardSize;
            this.r_IsComputer = i_IsComputer;
            this.r_PlayerId = i_PlayerNumber;
        }

        public List<Move> GetSoldierEatingMoves(Player i_Opponent, int i_Row, int i_Col, bool i_AfterEating)
        {
            List<Move> possibleMoves = getSoldierPossibleMovesList(i_Opponent, i_Row, i_Col);
            List<Move> listToReturn = returnListToOnlyEatingMovesIfPossible(possibleMoves, i_AfterEating);

            return listToReturn;
        }

        public List<Move> GetPossibleMoves(Player i_Opponent)
        {
            List<Move> possibleMoves = new List<Move>();

            for (int i = 0; i < this.r_BoardSize; i++)
            {
                for (int j = 0; j < this.r_BoardSize; j++)
                {
                    possibleMoves.AddRange(getSoldierPossibleMovesList(i_Opponent, i, j));
                }
            }

            possibleMoves = returnListToOnlyEatingMovesIfPossible(possibleMoves, false);

            return possibleMoves;
        }

        private List<Move> returnListToOnlyEatingMovesIfPossible(List<Move> i_ListToCheck, bool i_AfterEating)
        {
            List<Move> returnList = new List<Move>();
            foreach (Move move in i_ListToCheck)
            {
                if (Math.Abs(move.GetOriginCol() - move.GetTargetCol()) > 1)
                {
                    returnList.Add(move);
                }
            }

            if (returnList.Count == 0 && !i_AfterEating)
            {
                returnList = i_ListToCheck;
            }

            return returnList;
        }

        private List<Move> getSoldierPossibleMovesList(Player i_Opponent, int i_Row, int i_Col)
        {
            List<Move> listToReturn = new List<Move>();

            if (this.m_Soldiers[i_Row, i_Col] == null)
            {
                goto end;
            }

            bool isKing = this.m_Soldiers[i_Row, i_Col].IsKing();
            Point origin = new Point(i_Row, i_Col);
            if (isKing)
            {
                appendsMoveDownRight(listToReturn, origin, i_Opponent, i_Row, i_Col);
                appendsMoveDownLeft(listToReturn, origin, i_Opponent, i_Row, i_Col);
                appendsMoveUpLeft(listToReturn, origin, i_Opponent, i_Row, i_Col);
                appendsMoveUpRight(listToReturn, origin, i_Opponent, i_Row, i_Col);
            }
            else if (this.r_PlayerId == 1)
            {
                appendsMoveDownRight(listToReturn, origin, i_Opponent, i_Row, i_Col);
                appendsMoveDownLeft(listToReturn, origin, i_Opponent, i_Row, i_Col);
            }
            else
            {
                appendsMoveUpLeft(listToReturn, origin, i_Opponent, i_Row, i_Col);
                appendsMoveUpRight(listToReturn, origin, i_Opponent, i_Row, i_Col);
            }

            end:

            return listToReturn;
        }

        private void appendsMoveDownRight(List<Move> io_AppendedList, Point i_Origin, Player i_Opponent, int i_Row,
            int i_Col)
        {
            if (isEmptySquare(i_Row + 1, i_Col + 1) && i_Opponent.isEmptySquare(i_Row + 1, i_Col + 1))
            {
                addsPossibleMoveToList(io_AppendedList, i_Origin, i_Opponent, i_Row + 1, i_Col + 1);
            }
            else if (!i_Opponent.isEmptySquare(i_Row + 1, i_Col + 1))
            {
                if (isEmptySquare(i_Row + 2, i_Col + 2) && i_Opponent.isEmptySquare(i_Row + 2, i_Col + 2))
                {
                    addsPossibleMoveToList(io_AppendedList, i_Origin, i_Opponent, i_Row + 2, i_Col + 2);
                }
            }
        }

        private void appendsMoveDownLeft(List<Move> io_AppendedList, Point i_Origin, Player i_Opponent, int i_Row,
            int i_Col)
        {
            if (isEmptySquare(i_Row + 1, i_Col - 1) && i_Opponent.isEmptySquare(i_Row + 1, i_Col - 1))
            {
                addsPossibleMoveToList(io_AppendedList, i_Origin, i_Opponent, i_Row + 1, i_Col - 1);
            }
            else if (!i_Opponent.isEmptySquare(i_Row + 1, i_Col - 1))
            {
                if (isEmptySquare(i_Row + 2, i_Col - 2) && i_Opponent.isEmptySquare(i_Row + 2, i_Col - 2))
                {
                    addsPossibleMoveToList(io_AppendedList, i_Origin, i_Opponent, i_Row + 2, i_Col - 2);
                }
            }
        }

        private void appendsMoveUpRight(List<Move> io_AppendedList, Point i_Origin, Player i_Opponent, int i_Row,
            int i_Col)
        {
            if (isEmptySquare(i_Row - 1, i_Col + 1) && i_Opponent.isEmptySquare(i_Row - 1, i_Col + 1))
            {
                addsPossibleMoveToList(io_AppendedList, i_Origin, i_Opponent, i_Row - 1, i_Col + 1);
            }
            else if (!i_Opponent.isEmptySquare(i_Row - 1, i_Col + 1))
            {
                if (isEmptySquare(i_Row - 2, i_Col + 2) && i_Opponent.isEmptySquare(i_Row - 2, i_Col + 2))
                {
                    addsPossibleMoveToList(io_AppendedList, i_Origin, i_Opponent, i_Row - 2, i_Col + 2);
                }
            }
        }

        private void appendsMoveUpLeft(List<Move> io_AppendedList, Point i_Origin, Player i_Opponent, int i_Row,
            int i_Col)
        {
            if (isEmptySquare(i_Row - 1, i_Col - 1) && i_Opponent.isEmptySquare(i_Row - 1, i_Col - 1))
            {
                addsPossibleMoveToList(io_AppendedList, i_Origin, i_Opponent, i_Row - 1, i_Col - 1);
            }
            else if (!i_Opponent.isEmptySquare(i_Row - 1, i_Col - 1))
            {
                if (isEmptySquare(i_Row - 2, i_Col - 2) && i_Opponent.isEmptySquare(i_Row - 2, i_Col - 2))
                {
                    addsPossibleMoveToList(io_AppendedList, i_Origin, i_Opponent, i_Row - 2, i_Col - 2);
                }
            }
        }

        private void addsPossibleMoveToList(List<Move> io_AppendedList, Point i_Origin, Player i_Opponent, int i_Row, int i_Col)
        {
            Point target = new Point(i_Row, i_Col);
            io_AppendedList.Add(new Move(i_Origin, target));
        }

        private bool isEmptySquare(int i_Row, int i_Col)
        {
            bool valueToReturn = i_Row < 0 || i_Row >= r_BoardSize || i_Col < 0 || i_Col >= r_BoardSize || this.m_Soldiers[i_Row, i_Col] != null;

            return !valueToReturn;
        }

        public bool HasSoldier(int i_Row, int i_Col)
        {
            return this.m_Soldiers[i_Row, i_Col] != null;
        }

        public void InitSoldiers()
        {
            int numOfRows = this.r_BoardSize / 2 - 1;
            int numOfSoldiersPerRow = this.r_BoardSize / 2;
            Soldier[,] soldiersMatrix = new Soldier[this.r_BoardSize, this.r_BoardSize];

            if (this.r_PlayerId == 1)
            {
                for (int i = 0; i < numOfRows; i++)
                {
                    for (int j = 0; j < numOfSoldiersPerRow; j++)
                    {
                        Soldier soldier = new Soldier('O');
                        soldiersMatrix[i, (j * 2) + (i + 1) % 2] = soldier;
                    }
                }
            }
            else
            {
                for (int i = 0; i < numOfRows; i++)
                {
                    for (int j = 0; j < numOfSoldiersPerRow; j++)
                    {
                        Soldier soldier = new Soldier('X');
                        soldiersMatrix[this.r_BoardSize - i - 1, (j * 2) + i % 2] = soldier;
                    }
                }
            }

            this.m_Soldiers = soldiersMatrix;
        }

        public int GetPoints()
        {
            return this.m_points;
        }

        public void IncreasePoints(int i_PointsToIncrease)
        {
            this.m_points += i_PointsToIncrease;
        }

        public int GetBoardSize()
        {
            return this.r_BoardSize;
        }

        public int GetId()
        {
            return r_PlayerId;
        }

        public string GetName()
        {
            return this.r_Name;
        }

        public bool IsComputer()
        {
            return this.r_IsComputer;
        }
    }
}

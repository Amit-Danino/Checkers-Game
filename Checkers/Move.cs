using System;
using System.Collections.Generic;
using System.Drawing;

namespace Checkers
{
    public class Move
    {
        public Point m_Origin;
        public Point m_Target;

        public Move(Point i_Origin, Point i_Target)
        {
            this.m_Origin = i_Origin;
            this.m_Target = i_Target;
        }

        public static List<Move> GetSmartList(Player io_ComputerPlayer, Player i_OpponentPlayer,
            List<Move> i_ListOfValidMoves)
        {
            List<Move> listToReturn = new List<Move>();
            foreach (Move moveToCheck in i_ListOfValidMoves)
            {
                bool doesThisMoveEat = Math.Abs(moveToCheck.GetOriginCol() - moveToCheck.GetTargetCol()) > 1;
                if (doesThisMoveEat || !canBeEatenAfterThisStep(io_ComputerPlayer, i_OpponentPlayer, moveToCheck))
                {
                    listToReturn.Add(moveToCheck);
                }
            }

            if (listToReturn.Count == 0)
            {
                listToReturn = i_ListOfValidMoves;
            }

            return listToReturn;
        }

        private static bool canBeEatenAfterThisStep(Player io_ComputerPlayer, Player i_OpponentPlayer, Move i_MoveToCheck)
        {
            bool canBeEaten = false;
            int originRow = i_MoveToCheck.GetOriginRow();
            int originCol = i_MoveToCheck.GetOriginCol();
            int targetRow = i_MoveToCheck.GetTargetRow();
            int targetCol = i_MoveToCheck.GetTargetCol();

            Soldier solderToRemoveAndReturn = new Soldier(io_ComputerPlayer.m_Soldiers[originRow, originCol].GetSymbol());
            io_ComputerPlayer.m_Soldiers[originRow, originCol] = null;
            io_ComputerPlayer.m_Soldiers[targetRow, targetCol] = new Soldier('O');
            List<Move> opponentNewMoves = i_OpponentPlayer.GetPossibleMoves(io_ComputerPlayer);
            foreach (Move moveToCheck in opponentNewMoves)
            {
                int colAvg = (moveToCheck.GetOriginCol() + moveToCheck.GetTargetCol()) / 2;
                int rowAvg = (moveToCheck.GetOriginRow() + moveToCheck.GetTargetRow()) / 2;
                if (rowAvg == targetRow && colAvg == targetCol)
                {
                    canBeEaten = true;
                    break;
                }
            }

            io_ComputerPlayer.m_Soldiers[originRow, originCol] = solderToRemoveAndReturn;
            io_ComputerPlayer.m_Soldiers[targetRow, targetCol] = null;

            return canBeEaten;
        }

        public static bool PlayerTurn(Player io_PlayerTurn, Player io_PlayerNoTurn, List<Move> io_ListOfMoves, int i_BoardSize)
        {
            bool playerHasMoved = true;
            bool doingPlayerTurn = true;

            while (doingPlayerTurn)
            {
                if (io_ListOfMoves.Count == 0)
                {
                    playerHasMoved = false;
                    break;
                }

                if (io_PlayerTurn.IsComputer())
                {
                    io_ListOfMoves = GetSmartList(io_PlayerTurn, io_PlayerNoTurn, io_ListOfMoves);
                    Random random = new Random();
                    int index = random.Next(io_ListOfMoves.Count);
                    DoMove(io_PlayerTurn, io_PlayerNoTurn, io_ListOfMoves[index], i_BoardSize);
                    break;
                }
            }

            return playerHasMoved;
        }

        public static bool DoMove(Player io_PlayerTurn, Player io_PlayerNoTurn, Move i_Move, int i_BoardSize)
        {
            bool soldierIsEaten = isEaten(i_Move);
            char symbol = io_PlayerTurn.m_Soldiers[i_Move.m_Origin.X, i_Move.m_Origin.Y].GetSymbol();
            bool isKing = io_PlayerTurn.m_Soldiers[i_Move.m_Origin.X, i_Move.m_Origin.Y].IsKing();
            io_PlayerTurn.m_Soldiers[i_Move.m_Origin.X, i_Move.m_Origin.Y] = null;
            io_PlayerTurn.m_Soldiers[i_Move.m_Target.X, i_Move.m_Target.Y] = new Soldier(symbol);

            if (!isKing && ((io_PlayerTurn.GetId() != 2 && i_Move.m_Target.X == i_BoardSize - 1) ||
    (io_PlayerTurn.GetId() == 2 && i_Move.m_Target.X == 0)))
            {
                io_PlayerTurn.m_Soldiers[i_Move.m_Target.X, i_Move.m_Target.Y].Coronation();
            }

            if (soldierIsEaten)
            {
                int rowAvg = (i_Move.GetOriginRow() + i_Move.GetTargetRow()) / 2;
                int colAvg = (i_Move.GetOriginCol() + i_Move.GetTargetCol()) / 2;
                io_PlayerNoTurn.m_Soldiers[rowAvg, colAvg] = null;
                List<Move> eatenList = io_PlayerTurn.GetSoldierEatingMoves(io_PlayerNoTurn, i_Move.m_Target.X, i_Move.m_Target.Y, true);
            }

            return soldierIsEaten;
        }

        private static bool isEaten(Move i_Move)
        {
            return Math.Abs(i_Move.m_Target.X - i_Move.m_Origin.X) > 1;
        }

        public int GetOriginRow()
        {
            return this.m_Origin.X;
        }

        public int GetOriginCol()
        {
            return this.m_Origin.Y;
        }

        public int GetTargetRow()
        {
            return this.m_Target.X;
        }

        public int GetTargetCol()
        {
            return this.m_Target.Y;
        }
    }
}
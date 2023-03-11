using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using B22_Chen_Amit.Properties;
using Checkers;

namespace B22_Chen_Amit
{
    public partial class CheckersForm : Form
    {
        private int counter = 0;
        private Player m_Player1;
        private Player m_Player2;
        private Button[,] m_Buttons;
        private int m_BoardSize;
        private int m_PlayerTurn = 1;
        private Point m_OriginPosition = new Point(-1, -1);

        public CheckersForm(Player i_player1, Player i_player2)
        {
            m_Player1 = i_player1;
            m_Player2 = i_player2;
            initBoard();
            InitializeComponent();
            updateNames();
            initializeSpecialComponents();
            timer1.Start();
        }

        private void updateNames()
        {
            labelPlayer1.Text = string.Format("{0} : ", m_Player1.GetName().ToString());
            labelPlayer2.Text = string.Format("{0} : ", m_Player2.GetName().ToString());
        }

        public void EndGame(Player i_PlayerLoser, Player i_PlayerWinner, bool i_teko)
        {
            string message = @"Tie!
Another Round?";

            if (!i_teko)
            {

                int pointsToIncrease = Board.CountPoints(i_PlayerWinner, i_PlayerLoser);
                i_PlayerWinner.IncreasePoints(pointsToIncrease);
                labelScore1.Text = m_Player1.GetPoints().ToString();
                labelScore2.Text = m_Player2.GetPoints().ToString();

                message = string.Format(@"{0} Won!
Another Round?", i_PlayerWinner.GetName());
            }

            DialogResult dialogResult = MessageBox.Show(message, "Damka", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                initBoard();
                clearButtons();
                drawButtons();
            }
            else if (dialogResult == DialogResult.No)
            {
                userEndsTheGame(i_PlayerLoser, i_PlayerWinner);
            }
        }

        private void userEndsTheGame(Player i_PlayerLoser, Player i_PlayerWinner)
        {
            Player finalWinner = null;
            Player finalLoser = null;
            if (i_PlayerWinner.GetPoints() > i_PlayerLoser.GetPoints())
            {
                finalWinner = i_PlayerWinner;
                finalLoser = i_PlayerLoser;
            }
            else
            {
                finalWinner = i_PlayerLoser;
                finalLoser = i_PlayerWinner;
            }

            string messageWinner = "";
            if (i_PlayerWinner.GetPoints() > i_PlayerLoser.GetPoints())
            {
                messageWinner = String.Format(@"The final score is:
{0} : {1}
{2} : {3}
The final winner is {4}", finalWinner.GetName(), finalWinner.GetPoints(), finalLoser.GetName(), finalLoser.GetPoints(),
                    finalWinner.GetName());
            }

            MessageBox.Show(messageWinner);
            this.Dispose();
        }

        private void clearButtons()
        {
            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    m_Buttons[i, j].Image = null;
                }
            }
        }

        private void initBoard()
        {
            Board.StartGame(m_Player1, m_Player2);
            m_BoardSize = m_Player1.GetBoardSize();
        }

        private void initializeSpecialComponents()
        {
            m_Buttons = new Button[m_BoardSize, m_BoardSize];
            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    m_Buttons[i, j] = new Button()
                    {
                        Width = 70,
                        Height = 70,
                        Location = new Point(i * 69 + 20, j * 69 + 60)


                    };
                    if ((i + j) % 2 != 0)
                    {
                        m_Buttons[i, j].Enabled = false;
                        m_Buttons[i, j].BackColor = Color.LightSteelBlue;
                    }
                    else
                    {
                        m_Buttons[i, j].BackColor = Color.LightGray;
                    }

                    m_Buttons[i, j].TabStop = false;
                    m_Buttons[i, j].FlatStyle = FlatStyle.Flat;
                    m_Buttons[i, j].Click += new EventHandler(button_OnClick);
                    this.Controls.Add(m_Buttons[i, j]);
                }
            }

            drawButtons();
        }

        private void drawButtons()
        {
            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    if (m_Player1.HasSoldier(j, i))
                    {
                        m_Buttons[(i + 1) % m_BoardSize, j].Image =
                            new Bitmap(new Bitmap(Resources.whiteSoldier), new Size(60, 60));

                    }
                    else if (m_Player2.HasSoldier(j, i))
                    {
                        m_Buttons[(i + 1) % m_BoardSize, j].Image =
                            new Bitmap(new Bitmap(Resources.blackSoldier), new Size(60, 60));

                    }
                }
            }
        }

        private Point normalizePoint(Point i_Point)
        {
            int xDifference = this.m_Buttons[1, 1].Location.X - this.m_Buttons[0, 0].Location.X;
            int yDifference = this.m_Buttons[1, 1].Location.Y - this.m_Buttons[0, 0].Location.Y;
            int x = i_Point.X / xDifference;
            int y = i_Point.Y / yDifference;
            return new Point(m_BoardSize - y - 1, x);
        }


        private Move reverseNormalization(Move i_SoldierMove)
        {
            Point origin = new Point(i_SoldierMove.m_Origin.Y, m_BoardSize - i_SoldierMove.m_Origin.X - 1);
            Point target = new Point(i_SoldierMove.m_Target.Y, m_BoardSize - i_SoldierMove.m_Target.X - 1);
            return new Move(origin, target);
        }

        private void updateBoard(Move i_soldierMove)
        {
            i_soldierMove = reverseNormalization(i_soldierMove);
            int eatenRow = Math.Abs(i_soldierMove.GetOriginRow() + i_soldierMove.GetTargetRow()) / 2;
            int eatenCol = Math.Abs(i_soldierMove.GetOriginCol() + i_soldierMove.GetTargetCol()) / 2;
            
            if (eatenRow != i_soldierMove.GetOriginRow() && eatenRow != i_soldierMove.GetTargetRow() &&
                eatenCol != i_soldierMove.GetOriginCol() && eatenCol != i_soldierMove.GetTargetCol())
            {
                m_Buttons[eatenRow, eatenCol].Image = null;
            }

            if (this.m_PlayerTurn == 1 && i_soldierMove.m_Target.Y == 0)
            {
                m_Buttons[i_soldierMove.GetTargetRow(), i_soldierMove.GetTargetCol()].Image =
                    new Bitmap(new Bitmap(Resources.KingblackSoldier), new Size(60, 60));
            }
            else if (this.m_PlayerTurn == 2 && i_soldierMove.m_Target.Y == this.m_BoardSize-1)
            {
                m_Buttons[i_soldierMove.GetTargetRow(), i_soldierMove.GetTargetCol()].Image =
                    new Bitmap(new Bitmap(Resources.KingwhiteSoldier), new Size(60, 60));
            }
            else
            {
                m_Buttons[i_soldierMove.GetTargetRow(), i_soldierMove.GetTargetCol()].Image = m_Buttons[i_soldierMove.GetOriginRow(),
                    i_soldierMove.GetOriginCol()].Image;
            }

            m_Buttons[i_soldierMove.GetOriginRow(), i_soldierMove.GetOriginCol()].Image = null;
        }

        private void doPlayerMove(Player i_PlayerTurn, Player i_PlayerNoTurn, Move i_MoveToDo)
        {
            List<Move> movesList = i_PlayerTurn.GetPossibleMoves(i_PlayerNoTurn);
            bool moveExists = false;
            foreach (Move move in movesList)
            {
                if (move.m_Origin.Equals(i_MoveToDo.m_Origin) && move.m_Target.Equals(i_MoveToDo.m_Target))
                {
                    moveExists = true;
                }
            }

            if (moveExists)
            {
                bool isEaten = Checkers.Move.DoMove(i_PlayerTurn, i_PlayerNoTurn, i_MoveToDo, m_BoardSize);
                updateBoard(i_MoveToDo);
                if (!isEaten || i_PlayerTurn.GetSoldierEatingMoves(i_PlayerNoTurn, i_MoveToDo.GetTargetRow(), i_MoveToDo.GetTargetCol(),
                        isEaten).Count == 0)
                {
                    m_PlayerTurn = i_PlayerNoTurn.GetId();
                }
            }
            else
            {
                MessageBox.Show("Illegal move!");
            }

            Move moveToClear = reverseNormalization(i_MoveToDo);
            this.m_Buttons[moveToClear.GetOriginRow(), moveToClear.GetOriginCol()].BackColor = Color.LightGray;
            this.m_Buttons[moveToClear.GetTargetRow(), moveToClear.GetTargetCol()].BackColor = Color.LightGray;
            labelPlayer1.ForeColor = Color.Black;
            labelPlayer2.ForeColor = Color.Black;
        }

        private void button_OnClick(object sender, EventArgs e)
        {
            if ((sender as Button).BackColor == Color.Aquamarine)
            {
                (sender as Button).BackColor = Color.LightGray;
                m_OriginPosition = new Point(-1, -1);
                goto end;
            }

            (sender as Button).BackColor = Color.Aquamarine;
            if (m_OriginPosition.Equals(new Point(-1, -1)))
            {
                m_OriginPosition = (sender as Button).Location;
            }
            else
            {
                Point targetPosition = (sender as Button).Location;

                targetPosition = normalizePoint(targetPosition);
                m_OriginPosition = normalizePoint(m_OriginPosition);

                Move moveToDo = new Move(m_OriginPosition, targetPosition);
                if (m_PlayerTurn == 1)
                {
                    doPlayerMove(m_Player1, m_Player2, moveToDo);
                }
                else
                {
                    doPlayerMove(m_Player2, m_Player1, moveToDo);
                }

                m_OriginPosition = new Point(-1, -1);
                doComputerTurn();
                checkEndGame();
            }

            end: ;
        }

        private void doComputerTurn()
        {
            while (m_Player2.IsComputer() && this.m_PlayerTurn == 2)
            {
                List<Move> listOfMoves = m_Player2.GetPossibleMoves(m_Player1);
                List<Move> listOfSmartMoves = Checkers.Move.GetSmartList(m_Player2, m_Player1, listOfMoves);
                Random random = new Random();
                int index = random.Next(listOfSmartMoves.Count);
                if (listOfSmartMoves.Count() > 0)
                {
                    doPlayerMove(m_Player2, m_Player1, listOfSmartMoves[index]);
                }
                else
                {
                    this.m_PlayerTurn = 1;
                }
            }
        }

        private void checkEndGame()
        {
            bool aTie = false;

            if (m_Player1.GetPossibleMoves(m_Player2).Count() == 0 && m_Player2.GetPossibleMoves(m_Player1).Count() == 0)
            {
                aTie = true;
                EndGame(m_Player1, m_Player2, aTie);
            }
            else if (m_Player1.GetPossibleMoves(m_Player2).Count() == 0)
            {
                EndGame(m_Player1, m_Player2, aTie);
            }
            else if (m_Player2.GetPossibleMoves(m_Player1).Count() == 0)
            {
                EndGame(m_Player2, m_Player1, aTie);
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            counter++;
            if (counter == 5)
            {
                if (labelPlayer1.ForeColor == Color.Black && m_PlayerTurn ==1)
                {
                    labelPlayer1.ForeColor = Color.Aquamarine;
                }
                else
                {
                    labelPlayer1.ForeColor = Color.Black;
                }

                if (labelPlayer2.ForeColor == Color.Black  && m_PlayerTurn != 1)
                {
                    labelPlayer2.ForeColor = Color.Aquamarine;
                }
                else
                {
                    labelPlayer2.ForeColor = Color.Black;
                }

                counter = 0;
            }
        }
    }
}

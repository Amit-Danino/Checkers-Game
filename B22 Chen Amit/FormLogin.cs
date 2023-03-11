using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Checkers;

namespace B22_Chen_Amit
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPlayer2.Checked)
            {
                textBoxPlayer2.Enabled = true;
                textBoxPlayer2.Text = "";
            }
            else
            {
                textBoxPlayer2.Enabled = false;
                textBoxPlayer2.Text = "[Computer]";
            }
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            int boardSize = 0;
            if (radioButton6X6.Checked)
            {
                boardSize = 6;
            }else if (radioButton8X8.Checked)
            {
                boardSize = 8;
            }else if (radioButton10X10.Checked)
            {
                boardSize = 10;
            }
            
            string playerTwoName = this.textBoxPlayer2.Text;
            string playerOneName = this.textBoxPlayer1.Text;

            bool isComputer = !this.textBoxPlayer2.Enabled;

            if (playerTwoName != "" && playerOneName != "" && boardSize != 0)
            {
                Player player1 = new Player(playerOneName, boardSize, 1, false);
                Player player2 = new Player(playerTwoName, boardSize, 2, isComputer);

                this.Dispose();
                CheckersForm form = new CheckersForm(player1, player2);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please fill the form");
            }
        }
    }
}

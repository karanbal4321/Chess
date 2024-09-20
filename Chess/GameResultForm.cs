using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class GameResultForm : Form
    {
        Form1 mainGameForm;

        public GameResultForm(Form1 gameForm, string colourThatWon)
        {
            InitializeComponent();
            mainGameForm = gameForm;

            if (colourThatWon != "None") 
            {
                gameResultText.Text = colourThatWon + " Wins The Game!";
            }
            else
            {
                gameResultText.Text = "The Game Ends In A Draw!";
            }
        }

        private void startGameButton_Click(object sender, EventArgs e)
        {
            mainGameForm.RestartGame();
            this.Close();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            mainGameForm.ExitGame();
            this.Close();
        }
    }
}
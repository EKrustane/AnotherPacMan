using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnotherPacMan
{
    public partial class Game : Form
    {
        private Level level = new Level();
        public Game()
        {
            InitializeComponent();
            InitalizeGame();
        }

        private void InitalizeGame()
        {
            //adjust game form size
            this.Size = new Size(500, 500);

            //adding level to the game
            this.Controls.Add(level);
        }
    }
}

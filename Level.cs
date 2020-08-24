using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnotherPacMan
{
    class Level:PictureBox
    {
        public Level()
        {
            InitalizeLevel();
        }

        private void InitalizeLevel()
        {
            this.BackColor = Color.Black;
            this.Size = new Size(400, 400);
            this.Location = new Point(40, 30);
            this.Name = "Level";
        }
    }
}

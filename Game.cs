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
        private int initialEnemyCount = 4;
        private Random rand = new Random();
        private Level level = new Level();
        private Hero hero = new Hero();
        private Timer mainTimer = null;
        private List<Enemy> enemies = new List<Enemy>();
        public Game()
        {
            InitializeComponent();
            InitalizeGame();
            InitializeMainTimer();
        }

        private void InitalizeGame()
        {
            //adjust game form size
            this.Size = new Size(500, 500);

            //add key down event handler
            this.KeyDown += Game_KeyDown;

            //adding level to the game
            AddLevel();

            //adding hero to the game
            AddHero();

            //adding enemy to the game
            AddEnemies();

        }

        private void AddHero()
        {
            this.Controls.Add(hero);
            hero.Parent = level;
            hero.BringToFront();
        }

        private void AddLevel()
        {
            this.Controls.Add(level);
        }

        private void InitializeMainTimer()
        {
            mainTimer = new Timer();
            mainTimer.Tick += MainTimer_Tick;
            mainTimer.Interval = 20;
            mainTimer.Start();
        }

        private void MoveHero()
        {
            hero.Left += hero.HorizontalVelocity;
            hero.Top += hero.VerticalVelocity;
        }

        private void MoveEnemies()
        {
            foreach (var enemy in enemies)
            {
                enemy.Left += enemy.HorizontalVelocity;
                enemy.Top += enemy.VerticalVelocity;
            }
        }
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            MoveHero();
            HeroBorderCollision();
            MoveEnemies();
            EnemyBorderCollision();
            HeroEnemyColission();
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    hero.Direction = "right";
                    hero.HorizontalVelocity = hero.Step;
                    hero.VerticalVelocity = 0;
                    break;
                case Keys.Down:
                    hero.Direction = "down";
                    hero.HorizontalVelocity = 0;
                    hero.VerticalVelocity = hero.Step;
                    break;
                case Keys.Left:
                    hero.Direction = "left";
                    hero.HorizontalVelocity = -hero.Step;
                    hero.VerticalVelocity = 0;
                    break;
                case Keys.Up:
                    hero.Direction = "up";
                    hero.HorizontalVelocity = 0;
                    hero.VerticalVelocity = -hero.Step;
                    break;
            }
            SetRandomEnemyDirection();
        }

        private void HeroBorderCollision()
        {
            if (hero.Left > level.Left + level.Width)
            {
                hero.Left = level.Left - hero.Width;
            }
            if (hero.Left + hero.Width < level.Left)
            {
                hero.Left = level.Left + level.Width;
            }
            if (hero.Top > level.Top + level.Height)
            {
                hero.Top = level.Top - hero.Height;
            }
            if (hero.Top + hero.Height < level.Top)
            {
                hero.Top = level.Top + level.Height;
            }
        }

        private void EnemyBorderCollision()
        {
            foreach(var enemy in enemies)
            {
                if (enemy.Top < level.Top) //From "up" to "down"
                {
                    enemy.HorizontalVelocity = 0;
                    enemy.VerticalVelocity = +enemy.Step;
                }
                if (enemy.Top > level.Height - enemy.Width) //From "down" to "up"
                {
                    enemy.HorizontalVelocity = 0;
                    enemy.VerticalVelocity = -enemy.Step;
                }
                if (enemy.Left < level.Left) //From "left" to "right"
                {
                    enemy.HorizontalVelocity = +enemy.Step;
                    enemy.VerticalVelocity = 0;
                }
                if (enemy.Left > level.Width - enemy.Width) //From "right" to "left"
                {
                    enemy.HorizontalVelocity = -enemy.Step;
                    enemy.VerticalVelocity = 0;
                }
            }
        }
        
        private void AddEnemies()
        {
            Enemy enemy;
            for (int i = 0; i < initialEnemyCount; i++)
            {
                enemy = new Enemy();
                enemy.Location = new Point(rand.Next(100, 500), rand.Next(100, 500));
                enemy.SetDirection(rand.Next(1, 5));
                enemies.Add(enemy);
                this.Controls.Add(enemy);
                enemy.Parent = level;
                enemy.BringToFront();
            }
        }

        private void SetRandomEnemyDirection()
        {
            foreach (var enemy in enemies)
            {
                enemy.SetDirection(rand.Next(1, 5));
            }
        }

        private void GameOver()
        {
            mainTimer.Stop();
            PictureBoxGameOver();
            ButtonYes();
            ButtonNo();
        }

        private void PictureBoxGameOver()
        {
            pictureBoxGameOver.Visible = true;
            pictureBoxGameOver.Location = new Point(40, 30);
            pictureBoxGameOver.Size = level.Size;
            pictureBoxGameOver.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxGameOver.BringToFront();
        }

        private void HeroEnemyColission()
        {
            foreach (var enemy in enemies)
            {
                if (enemy.Bounds.IntersectsWith(hero.Bounds))
                {
                    GameOver();
                }
            }
        }

        private void ButtonYes()
        {
            buttonYes.Parent = pictureBoxGameOver;
            buttonYes.Size = new Size(60, 25);
            buttonYes.Location = new Point(135,340);
            buttonYes.Visible = true;
            buttonYes.BringToFront();
        }

        private void ButtonNo()
        {
            buttonNo.Parent = pictureBoxGameOver;
            buttonNo.Size = new Size(60, 25);
            buttonNo.Location = new Point(207, 340);
            buttonNo.Visible = true;
            buttonNo.BringToFront();
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

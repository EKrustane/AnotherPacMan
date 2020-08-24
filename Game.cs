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
        private int initialEnemyCount = 0;
        private Random rand = new Random();
        private Level level = new Level();
        private Hero hero = new Hero();
        private Food food = new Food();
        private Timer mainTimer = null;
        private Timer enemySpawningTimer = null;
        private List<Enemy> enemies = new List<Enemy>();
        private PictureBox startPicture=new PictureBox();
        private Button buttonEasy = new Button();
        private Button buttonMedium = new Button();
        private Button buttonHard = new Button();

        public Game()
        {
            InitializeComponent();
            InitializeGame();
            InitializeMainTimer();
            InitializeEnemySpawningTimer();
            //PictureStartGame();
            
        }

        private void RestartGame()
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            this.Close();
        }

        private void InitializeGame()
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
            AddEnemies(initialEnemyCount);

            //adding food to the game
            AddFood();
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

        private void AddFood()
        {
            this.Controls.Add(food);
            FoodLocation();
            food.Parent = level;
            food.BringToFront();
        }

        private void FoodLocation()
        {
            food.Location = new Point(rand.Next(100, 400), rand.Next(100, 400));
        }

        private void InitializeMainTimer()
        {
            mainTimer = new Timer();
            mainTimer.Tick += MainTimer_Tick;
            mainTimer.Interval = 20;
            mainTimer.Start();
        }

        private void InitializeEnemySpawningTimer()
        {
            enemySpawningTimer = new Timer();
            enemySpawningTimer.Tick += EnemySpawningTimer_Tick;
            enemySpawningTimer.Interval = 3000;
            enemySpawningTimer.Start();
        }

        private void EnemySpawningTimer_Tick(object sender, EventArgs e)
        {
            AddEnemies(1);
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
            HeroFoodCollision();
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

        private void HeroFoodCollision()
        {
            if(hero.Bounds.IntersectsWith(food.Bounds))
            {
                hero.Step += 1;
                RespawnFood();
            }
        }

        private void RespawnFood()
        {
            FoodLocation();
            food.SetType(rand.Next(1, 5));
        }

        private void EnemyBorderCollision()
        {
            foreach(var enemy in enemies)
            {
                if (enemy.Top < level.Top - enemy.Width) //From "up" to "down"
                {
                    enemy.SetDirection(2);
                }
                if (enemy.Top > level.Height - enemy.Width) //From "down" to "up"
                {
                    enemy.SetDirection(4);
                }
                if (enemy.Left < level.Left - enemy.Width) //From "left" to "right"
                {
                    enemy.SetDirection(1);
                }
                if (enemy.Left > level.Width - enemy.Width) //From "right" to "left"
                {
                    enemy.SetDirection(3);
                }
            }
        }
        
        private void AddEnemies(int enemyCount)
        {
            Enemy enemy;
            for (int i = 0; i < enemyCount; i++)
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
            buttonYes.Size = new Size(80, 40);
            buttonYes.Location = new Point(120,300);
            buttonYes.Visible = true;
            buttonYes.BringToFront();
        }

        private void ButtonNo()
        {
            buttonNo.Parent = pictureBoxGameOver;
            buttonNo.Size = new Size(80, 40);
            buttonNo.Location = new Point(215, 300);
            buttonNo.Visible = true;
            buttonNo.BringToFront();
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            PictureStartGame();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PictureStartGame()
        {
            this.Controls.Add(startPicture);
            startPicture.Visible = true;
            startPicture.Location = new Point(40, 30);
            startPicture.Size = level.Size;
            startPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            startPicture.BringToFront();
            startPicture.Image = (Image)Properties.Resources.ResourceManager.GetObject("pacman_ready");
            ButtonEasy();
            ButtonMedium();
            ButtonHard();
        }

        private void ButtonEasy()
        {
            buttonEasy.Parent = startPicture;
            buttonEasy.Size = new Size(100, 40);
            buttonEasy.Location = new Point(80, 100);
            buttonEasy.BackColor = Color.Black;
            buttonEasy.ForeColor = Color.White;
            buttonEasy.Text = "Easy";
            buttonEasy.Font = new Font("Consolas", 14, FontStyle.Bold);
            buttonEasy.Visible = true;
            buttonEasy.BringToFront();
            buttonEasy.Click += buttonEasy_Click;
        }

        private void ButtonMedium()
        {
            buttonMedium.Parent = startPicture;
            buttonMedium.Size = new Size(100, 40);
            buttonMedium.Location = new Point(80, 145);
            buttonMedium.BackColor = Color.Black;
            buttonMedium.ForeColor = Color.White;
            buttonMedium.Text = "Medium";
            buttonMedium.Font = new Font("Consolas", 14, FontStyle.Bold);
            buttonMedium.Visible = true;
            buttonMedium.BringToFront();
            buttonMedium.Click += buttonMedium_Click;
        }

        private void ButtonHard()
        {
            buttonHard.Parent = startPicture;
            buttonHard.Size = new Size(100, 40);
            buttonHard.Location = new Point(80, 190);
            buttonHard.BackColor = Color.Black;
            buttonHard.ForeColor = Color.White;
            buttonHard.Text = "Hard";
            buttonHard.Font = new Font("Consolas", 14, FontStyle.Bold);
            buttonHard.Visible = true;
            buttonHard.BringToFront();
            buttonHard.Click += buttonHard_Click;
        }

        private void buttonEasy_Click(object sender, EventArgs e)
        {
            initialEnemyCount = 1;
            VisibleFalse();
            RestartGame();
        }

        private void buttonMedium_Click(object sender, EventArgs e)
        {
            initialEnemyCount = 4;
            VisibleFalse();
            RestartGame();
        }

        private void buttonHard_Click(object sender, EventArgs e)
        {
            initialEnemyCount = 10;
            VisibleFalse();
            RestartGame();
        }

        private void VisibleFalse()
        {
            startPicture.Visible = false;
            buttonEasy.Visible = false;
            buttonMedium.Visible = false;
            buttonHard.Visible = false;
            
        }
    }
}

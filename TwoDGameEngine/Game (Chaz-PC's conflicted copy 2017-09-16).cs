using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Timers;
using System.Numerics;

namespace TwoDGameEngine
{
    public partial class Game : Form
    {
        //Game instance
        public static Game GameInstance = new Game();

        //Move form variables
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        public Game()
        {
            InitializeComponent();

            //Default values:
            Player.Dir_None = true;
            Player.Speed = 2f;
            Player.MaxSpeed = 1f;
            Player.Size = 30f;
            Player.Location.X = Canvas.Width / 2;
            Player.Location.Y = Canvas.Height / 2;
            Player.MoveTick = 1;
            Player.MovementEnabled = true;
            Player.Score = 0;
            Player.Shooting = false;

            PowerUp.SpeedUp_Size = 50f;
            PowerUp.DrawPowerUp = true;
            PowerUp.SpeedUp_Location.X = 200f;
            PowerUp.SpeedUp_Location.Y = 200f;
            PowerUp.SpeedUpIsActive = false;
            PowerUp.SpeedUp_DecayRate = 10;

            GraphicsDrawing.GraphicsFrameDelay = 1;
            GraphicsDrawing.DrawGame = true;
            GraphicsDrawing.Running = true;
            GraphicsDrawing.DrawBorder = true;

            UIElements.Border_Brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0)); ;
            UIElements.BorderSize = 10;

            UIElements.MapSize = new Size(1280, 720);
            UIElements.Score_Location.X = 0f;
            UIElements.Score_Location.Y = Canvas.Height - 50;
            UIElements.Score_FontSize = 25;
            UIElements.Score_Font = new Font("Segoe UI", UIElements.Score_FontSize);

            UIElements.ScoreBack_Brush = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
            UIElements.ScoreBack_LocX = 0f;
            UIElements.ScoreBack_LocY = Canvas.Height - 50;
            UIElements.ScoreBack_Width = 100f;
            UIElements.ScoreBack_Height = 50f;

            UIElements.Zone_Location.X = 10f;
            UIElements.Zone_Location.Y = UIElements.MapSize.Height / 4;
            UIElements.Zone_Height = 200f;
            UIElements.Zone_Width = 50f;

            UIElements.GraphicsOffset_X = -(UIElements.MapSize.Width / 5);
            UIElements.GraphicsOffset_Y = -(UIElements.MapSize.Height / 5) - 20;

            Projectiles.Bullet_Count = 0;
            Projectiles.Bullet_Size = 10;
            Projectiles.MovementEnabled = true;
            Projectiles.MoveTick = 1;
            Projectiles.Bullet_Delay = 50;

            Canvas.BackColor = Color.White;
            this.DoubleBuffered = true;

            Canvas.Size = UIElements.MapSize;
        }

        private void PanelMenu_MouseDown_1(object sender, MouseEventArgs e)
        {
            //Left Mouseclick
            if (e.Button == MouseButtons.Left)
            {
                //Stops form moving
                ReleaseCapture();

                //Refreshes graphics
                Invalidate();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        void GEInvalidate()
        {
            while (GraphicsDrawing.Running)
            {
                //Refreshes graphics
                Canvas.Invalidate();
                PanelMenu.Invalidate();

                //Loop Delay
                Thread.Sleep(GraphicsDrawing.GraphicsFrameDelay);
            }
        }

        public static async void ProjectileMovementThread()
        {
            while (Projectiles.MovementEnabled)
            {
                //Loops through all projectiles
                for (int i = 1; i < Projectiles.Bullets.Length; i++)
                {
                    Projectiles.Bullets_Location[i].X += Projectiles.Bullets_Dir_X[i];
                    Projectiles.Bullets_Location[i].Y += Projectiles.Bullets_Dir_Y[i];

                    //Remove bullet
                    if (Projectiles.Bullet_Count >= Projectiles.MaxBullets - 2)
                    {
                        Projectiles.Bullet_Count = 0;
                    }
                }

            }
            //Loop Delay
            await Task.Delay(Projectiles.MoveTick);
        }

        static async void MovementThread()
        {
            while (Player.MovementEnabled)
            {
                //Check collition with Zone
                if (Player.PlayerObj.IntersectsWith(UIElements.ZoneObj))
                {
                    Player.Score++;
                }
                //Left Border collision
                if (Player.Dir_Left)
                {
                    if (Player.PlayerObj.IntersectsWith(UIElements.BorderLeft_Obj) == false)
                    {
                        Player.Location.X -= Player.Speed;
                        UIElements.GraphicsOffset_X += Player.Speed;

                        //Loops through all projectiles
                        for (int i = 1; i < Projectiles.Bullets.Length; i++)
                        {
                            //Sets camera offset for bullets
                            Projectiles.Bullets_Location[i].X += Player.Speed;
                        }
                    }
                }
                //Right Border collision
                if (Player.Dir_Right)
                {
                    if (Player.PlayerObj.IntersectsWith(UIElements.BorderRight_Obj) == false)
                    {
                        Player.Location.X += Player.Speed;
                        UIElements.GraphicsOffset_X -= Player.Speed;

                        //Loops through all projectiles
                        for (int i = 1; i < Projectiles.Bullets.Length; i++)
                        {
                            //Sets camera offset for bullets
                            Projectiles.Bullets_Location[i].X -= Player.Speed;
                        }
                    }
                }
                //Top Border collision
                if (Player.Dir_Up)
                {
                    if (Player.PlayerObj.IntersectsWith(UIElements.BorderTop_Obj) == false)
                    {
                        Player.Location.Y -= Player.Speed;
                        UIElements.GraphicsOffset_Y += Player.Speed;

                        //Loops through all projectiles
                        for (int i = 1; i < Projectiles.Bullets.Length; i++)
                        {
                            //Sets camera offset for bullets
                            Projectiles.Bullets_Location[i].Y += Player.Speed;
                        }
                    }
                }
                //Bottom Border collision
                if (Player.Dir_Down)
                {
                    if (Player.PlayerObj.IntersectsWith(UIElements.BorderBottom_Obj) == false)
                    {
                        Player.Location.Y += Player.Speed;
                        UIElements.GraphicsOffset_Y -= Player.Speed;

                        //Loops through all projectiles
                        for (int i = 1; i < Projectiles.Bullets.Length; i++)
                        {
                            //Sets camera offset for bullets
                            Projectiles.Bullets_Location[i].Y -= Player.Speed;
                        }
                    }
                }
                //Speed up powerup collision
                if (Player.PlayerObj.IntersectsWith(PowerUp.PowerUpObj) & PowerUp.SpeedUpIsActive == false)
                {
                    PowerUp.SpeedUpIsActive = true;
                    Player.Speed += 3f;
                }

                //Loop Delay
                await Task.Delay(Player.MoveTick);
            }
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            //Player direction changes
            if (e.KeyCode == Keys.W)
            {
                Player.Dir_Up = true;
            }

            if (e.KeyCode == Keys.D)
            {
                Player.Dir_Right = true;
            }

            if (e.KeyCode == Keys.S)
            {
                Player.Dir_Down = true;
            }

            if (e.KeyCode == Keys.A)
            {
                Player.Dir_Left = true;
            }

            //Shooting direction changes
            if (e.KeyCode == Keys.Up)
            {
                Projectiles.Dir_Up = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                Projectiles.Dir_Right = true;
            }

            if (e.KeyCode == Keys.Down)
            {
                Projectiles.Dir_Down = true;
            }

            if (e.KeyCode == Keys.Left)
            {
                Projectiles.Dir_Left = true;
            }


            if (e.KeyCode == Keys.Space)
            {
                Player.Shooting = true;
            }

            if (e.KeyCode == Keys.Escape)
            {
                Environment.Exit(0);
            }

        }

        static async void CheckShooting()
        {
            while (GraphicsDrawing.Running)
            {
                while (Player.Shooting)
                {
                    //Sets direction for the shot bullet
                    if (Projectiles.Dir_Up)
                    {
                        Projectiles.Bullets_Dir_X[Projectiles.Bullet_Count] = 0f;
                        Projectiles.Bullets_Dir_Y[Projectiles.Bullet_Count] = -Projectiles.Bullet_Speed;
                    }

                    if (Projectiles.Dir_Right)
                    {
                        Projectiles.Bullets_Dir_X[Projectiles.Bullet_Count] = Projectiles.Bullet_Speed;
                        Projectiles.Bullets_Dir_Y[Projectiles.Bullet_Count] = 0f;
                    }

                    if (Projectiles.Dir_Down)
                    {
                        Projectiles.Bullets_Dir_X[Projectiles.Bullet_Count] = 0f;
                        Projectiles.Bullets_Dir_Y[Projectiles.Bullet_Count] = Projectiles.Bullet_Speed;
                    }

                    if (Projectiles.Dir_Left)
                    {
                        Projectiles.Bullets_Dir_X[Projectiles.Bullet_Count] = -Projectiles.Bullet_Speed;
                        Projectiles.Bullets_Dir_Y[Projectiles.Bullet_Count] = 0f;
                    }

                    //Sets diagonal direction for the shot bullet
                    if (Projectiles.Dir_Up && Projectiles.Dir_Right)
                    {
                        Projectiles.Bullets_Dir_X[Projectiles.Bullet_Count] = Projectiles.Bullet_Speed;
                        Projectiles.Bullets_Dir_Y[Projectiles.Bullet_Count] = -Projectiles.Bullet_Speed;
                    }

                    if (Projectiles.Dir_Right && Projectiles.Dir_Down)
                    {
                        Projectiles.Bullets_Dir_X[Projectiles.Bullet_Count] = Projectiles.Bullet_Speed;
                        Projectiles.Bullets_Dir_Y[Projectiles.Bullet_Count] = Projectiles.Bullet_Speed;
                    }

                    if (Projectiles.Dir_Down && Projectiles.Dir_Left)
                    {
                        Projectiles.Bullets_Dir_X[Projectiles.Bullet_Count] = -Projectiles.Bullet_Speed;
                        Projectiles.Bullets_Dir_Y[Projectiles.Bullet_Count] = Projectiles.Bullet_Speed;
                    }

                    if (Projectiles.Dir_Left && Projectiles.Dir_Up)
                    {
                        Projectiles.Bullets_Dir_X[Projectiles.Bullet_Count] = -Projectiles.Bullet_Speed;
                        Projectiles.Bullets_Dir_Y[Projectiles.Bullet_Count] = -Projectiles.Bullet_Speed;
                    }

                    //Adds bullets to array and sets location to player
                    Projectiles.Bullets_Location[Projectiles.Bullet_Count] = new Point(Convert.ToInt16(Player.Location.X + Player.Size / 3), Convert.ToInt16(Player.Location.Y + Player.Size / 3));
                    Projectiles.Bullets[Projectiles.Bullet_Count] = new RectangleF(Projectiles.Bullets_Location[Projectiles.Bullet_Count].X, Projectiles.Bullets_Location[Projectiles.Bullet_Count].Y, Projectiles.Bullet_Size, Projectiles.Bullet_Size);

                    Projectiles.Bullets_Dir_X[Projectiles.Bullet_Count + 1] = Projectiles.Bullet_Speed;
                    Projectiles.Bullets_Dir_Y[Projectiles.Bullet_Count + 1] = 0;

                    Projectiles.Bullet_Count += 1;

                    //Delay between bullet shots
                    await Task.Delay(Projectiles.Bullet_Delay);
                }
            }
        }

        public static async void Collisions()
        {
            while (GraphicsDrawing.Running)
            {
                for (int i1 = 1; i1 < Projectiles.Bullets.Length; i1++)
                {
                    for (int i2 = 1; i2 < Enemies.AllEnemies.Length; i2++)
                    {
                        if (Projectiles.Bullets[i1].IntersectsWith(Enemies.AllEnemies[i2]))
                        {
                            Enemies.AllEnemies[i2].Location = new PointF(0, 0);
                        }
                    }
                }

                await Task.Delay(1);
            }
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            //Player direction change
            if (e.KeyCode == Keys.W & Player.Dir_Up == true)
            {
                Player.Dir_Up = false;
            }

            else if (e.KeyCode == Keys.D & Player.Dir_Right == true)
            {
                Player.Dir_Right = false;
            }

            else if (e.KeyCode == Keys.S & Player.Dir_Down == true)
            {
                Player.Dir_Down = false;
            }

            else if (e.KeyCode == Keys.A & Player.Dir_Left == true)
            {
                Player.Dir_Left = false;
            }

            //Shooting direction change
            if (e.KeyCode == Keys.Up & Projectiles.Dir_Up == true)
            {
                Projectiles.Dir_Up = false;
            }

            else if (e.KeyCode == Keys.Right & Projectiles.Dir_Right == true)
            {
                Projectiles.Dir_Right = false;
            }

            else if (e.KeyCode == Keys.Down & Projectiles.Dir_Down == true)
            {
                Projectiles.Dir_Down = false;
            }

            else if (e.KeyCode == Keys.Left & Projectiles.Dir_Left == true)
            {
                Projectiles.Dir_Left = false;
            }


            if (e.KeyCode == Keys.Space)
            {
                Player.Shooting = false;
            }

        }

        private void Game_Load(object sender, EventArgs e)
        {
            Thread MovementT = new Thread(new ThreadStart(MovementThread));
            MovementT.Start();

            Thread ProjectileMovementT = new Thread(new ThreadStart(ProjectileMovementThread));
            ProjectileMovementT.Start();

            Thread GInvalT = new Thread(new ThreadStart(GEInvalidate));
            GInvalT.Start();

            Thread ShootingT = new Thread(new ThreadStart(CheckShooting));
            ShootingT.Start();

            Thread EnemiesSpawnT = new Thread(new ThreadStart(EnemiesSpawner));
            EnemiesSpawnT.Start();

            Thread EnemiesMovementT = new Thread(new ThreadStart(EnemiesMovement));
            EnemiesMovementT.Start();

            Thread CollisionsT = new Thread(new ThreadStart(Collisions));
            CollisionsT.Start();

            for (int x = -2; x < 25; x++)
            {
                for (int y = -2; y < 15; y++)
                {
                    using (Graphics g = this.Canvas.CreateGraphics())
                    {
                        //Tile background
                    }
                }
            }
        }

        public static async void EnemiesMovement()
        {
            while (GraphicsDrawing.Running)
            {
                Enemies.MoveTo(Player.Location.X - UIElements.GraphicsOffset_X + Player.Size / 2 - Enemies.Enemies_Size / 2, Player.Location.Y - UIElements.GraphicsOffset_Y + Player.Size / 2 - Enemies.Enemies_Size / 2, 3f);
                await Task.Delay(1);
            }
        }

        public static void EnemiesSpawner()
        {
            while(GraphicsDrawing.Running)
            {
                if (Enemies.Enemies_Count >= Enemies.MaxEnemies)
                {
                    Enemies.Enemies_Count = 0;
                }

                Random rand = new Random();

                //Adds Enemies to array and sets location to spawn point
                Enemies.AllEnemies[Enemies.Enemies_Count].X = UIElements.MapSize.Width - 100 + rand.Next(-50, 50);
                Enemies.AllEnemies[Enemies.Enemies_Count].Y = UIElements.MapSize.Height / 2 + rand.Next(-200, 200);

                Enemies.Enemies_Count += 1;

                Thread.Sleep(100);
            }
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {

            if (GraphicsDrawing.DrawGame)
            {

                    //PowerUp
                    if (PowerUp.DrawPowerUp)
                {
                    e.Graphics.DrawString("Speed Up", new Font("Segoe UI", 12), Brushes.Black, PowerUp.PowerUpObj.X -8, PowerUp.PowerUpObj.Y - 20);

                    PowerUp.PowerUpObj = new RectangleF(PowerUp.SpeedUp_Location.Y + UIElements.GraphicsOffset_X, PowerUp.SpeedUp_Location.Y + UIElements.GraphicsOffset_Y, PowerUp.SpeedUp_Size, PowerUp.SpeedUp_Size);
                    e.Graphics.FillRectangle(Brushes.Blue, PowerUp.PowerUpObj);
                }

                //Zone
                UIElements.ZoneObj = new RectangleF(UIElements.Zone_Location.X + UIElements.GraphicsOffset_X, UIElements.Zone_Location.Y + UIElements.GraphicsOffset_Y, UIElements.Zone_Width, UIElements.Zone_Height);
                e.Graphics.FillRectangle(Brushes.Lime, UIElements.ZoneObj);

                //Line
                e.Graphics.DrawLine(Pens.Green, Player.Location.X + Player.Size / 2, Player.Location.Y + Player.Size / 2, UIElements.Zone_Width / 2 + UIElements.GraphicsOffset_X, UIElements.MapSize.Height / 4 + UIElements.Zone_Height / 2 + UIElements.GraphicsOffset_Y);

                //Player
                Player.PlayerObj = new RectangleF(Player.Location.X, Player.Location.Y, Player.Size, Player.Size);
                e.Graphics.FillRectangle(Brushes.Purple, Player.PlayerObj);

                //Borders
                UIElements.BorderLeft_Obj = new RectangleF(UIElements.BorderSize + UIElements.GraphicsOffset_X, UIElements.BorderSize + UIElements.GraphicsOffset_Y, UIElements.BorderSize, UIElements.MapSize.Height - PanelMenu.Height - (UIElements.BorderSize * 2));
                e.Graphics.FillRectangle(UIElements.Border_Brush, UIElements.BorderLeft_Obj);

                UIElements.BorderTop_Obj = new RectangleF(UIElements.BorderSize + UIElements.GraphicsOffset_X, UIElements.BorderSize + UIElements.GraphicsOffset_Y, UIElements.MapSize.Width - (UIElements.BorderSize * 2), UIElements.BorderSize);
                e.Graphics.FillRectangle(UIElements.Border_Brush, UIElements.BorderTop_Obj);

                UIElements.BorderRight_Obj = new RectangleF(UIElements.MapSize.Width - (UIElements.BorderSize * 2) + UIElements.GraphicsOffset_X, UIElements.BorderSize + UIElements.GraphicsOffset_Y, UIElements.BorderSize, UIElements.MapSize.Height - PanelMenu.Height - (UIElements.BorderSize * 2));
                e.Graphics.FillRectangle(UIElements.Border_Brush, UIElements.BorderRight_Obj);

                UIElements.BorderBottom_Obj = new RectangleF(UIElements.BorderSize + UIElements.GraphicsOffset_X, UIElements.MapSize.Height - PanelMenu.Height - (UIElements.BorderSize * 2) + UIElements.GraphicsOffset_Y, UIElements.MapSize.Width - (UIElements.BorderSize * 2), UIElements.BorderSize);
                e.Graphics.FillRectangle(UIElements.Border_Brush, UIElements.BorderBottom_Obj);

                //ScoreBack
                e.Graphics.FillRectangle(UIElements.ScoreBack_Brush, UIElements.ScoreBack_LocX + UIElements.BorderSize, UIElements.ScoreBack_LocY - UIElements.BorderSize, UIElements.ScoreBack_Width, UIElements.ScoreBack_Height);

                //Score
                e.Graphics.DrawString(Player.Score.ToString(), UIElements.Score_Font, Brushes.White, UIElements.Score_Location.X + UIElements.BorderSize, UIElements.Score_Location.Y - UIElements.BorderSize);

                //Speed
                e.Graphics.DrawString(Convert.ToInt16((Player.Speed * 100)).ToString(), new Font("Segoe UI", 16), Brushes.Black, UIElements.Score_Location.X + UIElements.BorderSize, UIElements.Score_Location.Y - UIElements.BorderSize - 50);

                //Bullets
                e.Graphics.FillRectangles(Brushes.Black, Projectiles.Bullets);

                //Loop through all bullets (Drawing all bullets)
                for (int i = 1; i < Projectiles.Bullets.Length; i++)
                {
                    Projectiles.Bullets[i].Location = Projectiles.Bullets_Location[i];

                    //Removes glitch bullet outside map
                    Projectiles.Bullets[0].Location = new PointF(-9999, -9999);
                }

                //Draws all enemies
                for (int i = 0; i < Enemies.MaxEnemies; i++)
                {
                    e.Graphics.FillRectangle(Brushes.Red, new RectangleF(Enemies.AllEnemies[i].X + UIElements.GraphicsOffset_X, Enemies.AllEnemies[i].Y + UIElements.GraphicsOffset_Y, Enemies.Enemies_Size, Enemies.Enemies_Size));
                }

                //Enemies to player line
                if (false)
                {
                    for (int i = 0; i < Enemies.MaxEnemies; i++)
                    {
                        if (Enemies.AllEnemies[i].X > 0 || Enemies.AllEnemies[i].Y > 0)
                        {
                            e.Graphics.DrawLine(Pens.Red, Player.Location.X + Player.Size / 2, Player.Location.Y + Player.Size / 2, Enemies.AllEnemies[i].X + UIElements.GraphicsOffset_X + Enemies.Enemies_Size / 2, Enemies.AllEnemies[i].Y + UIElements.GraphicsOffset_Y + Enemies.Enemies_Size / 2);
                        }
                    }
                }
            }
        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
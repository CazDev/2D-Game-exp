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
using System.Diagnostics;
using System.Media;

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

        //FPS display
        double FPS = 0;


        public Game()
        {
            InitializeComponent();

            //Default values:
            Player.Dir_None = true;
            Player.StartSpeed = 0.6f;
            Player.Speed = Player.StartSpeed;
            Player.MaxSpeed = 5f;
            Player.Size = 30f;
            Player.Location.X = Canvas.Width / 2 - Player.Size;
            Player.Location.Y = Canvas.Height / 2 - Player.Size;
            Player.Level = 1;
            Player.MoveTick = 1;
            Player.MovementEnabled = true;
            Player.Score = 0;
            Player.HP = 999999;
            Player.MaxHP = 999999;
            Player.Defence = 1;
            Player.Shooting = false;

            Enemies.SpawnRate = 1000;

            GraphicsDrawing.GraphicsFrameDelay = 1;
            GraphicsDrawing.DrawGame = true;
            GraphicsDrawing.Running = true;

            UIElements.Border_Draw = true;

            UIElements.Border_Brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0)); ;
            UIElements.BorderSize = 20;

            UIElements.MapSize = new Size(2400, 2400);

            UIElements.BarMenu_Draw = true;
            UIElements.BarMenu_Brush = new SolidBrush(Color.FromArgb(125, 0, 0, 0));

            UIElements.Zone_Location.X = 10f;
            UIElements.Zone_Location.Y = UIElements.MapSize.Height / 4;
            UIElements.Zone_Height = 200f;
            UIElements.Zone_Width = 50f;

            UIElements.MapRect = new RectangleF(UIElements.GraphicsOffset_X, UIElements.GraphicsOffset_X, UIElements.MapSize.Width, UIElements.MapSize.Height);

            Canvas.Size = UIElements.MapSize;

            UIElements.ScreenSize = this.Size;

            UIElements.GraphicsOffset_X = -(UIElements.MapSize.Width / 4) + Player.Size / 2 - 50;
            UIElements.GraphicsOffset_Y = -(UIElements.MapSize.Height / 4) + Player.Size / 2 - 200;

            Projectiles.Bullet_Size = 7f;
            Projectiles.Bullet_Speed = 3f;
            Projectiles.Bullet_Damage = 5;
            Projectiles.Bullet_Penetration = 2;
            Projectiles.MovementEnabled = true;
            Projectiles.MoveTick = 1;
            Projectiles.Bullet_Delay = 60;

            Weapons.Gun = Weapons.Guns.Rifle;

            this.DoubleBuffered = true;
        }

        /// <summary>
        /// FPS counter for calculating fps
        /// </summary>

        DateTime _lastCheckTime = DateTime.Now;
        long _frameCount = 0;

        // called whenever a map is updated
        void OnMapUpdated()
        {
            Interlocked.Increment(ref _frameCount);
        }

        // called every once in a while
        double GetFps()
        {
            double secondsElapsed = (DateTime.Now - _lastCheckTime).TotalSeconds;
            long count = Interlocked.Exchange(ref _frameCount, 0);
            double fps = count / secondsElapsed;
            _lastCheckTime = DateTime.Now;

            //Thread.Sleep(100); fake lag
            return fps;
        }



        //Used to simulate a more accurate and shorter delay
        public static void Delay(double durationMiliseconds)
        {
            durationMiliseconds /= 1000;

            var durationTicks = Math.Round(durationMiliseconds * Stopwatch.Frequency);
            var sw = Stopwatch.StartNew();

            while (sw.ElapsedTicks < durationTicks)
            { Thread.Sleep(1); }
        }

        void GEInvalidate()
        {
            while (GraphicsDrawing.Running)
            {
                //Refreshes graphics
                Canvas.Invalidate();
                TopMenu.Invalidate();

                //Reduces lag exponetially by synchronising events
                Application.DoEvents();
                Thread.Sleep(1);
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
                Player.Shooting = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                Projectiles.Dir_Right = true;
                Player.Shooting = true;
            }

            if (e.KeyCode == Keys.Down)
            {
                Projectiles.Dir_Down = true;
                Player.Shooting = true;
            }

            if (e.KeyCode == Keys.Left)
            {
                Projectiles.Dir_Left = true;
                Player.Shooting = true;
            }

            if (e.KeyCode == Keys.D1)
            {
                Weapons.EquipGun(Weapons.Guns.Pistol);
            }

            if (e.KeyCode == Keys.D2)
            {
                Weapons.EquipGun(Weapons.Guns.AutoPistol);
            }

            if (e.KeyCode == Keys.D3)
            {
                Weapons.EquipGun(Weapons.Guns.SMG);
            }

            if (e.KeyCode == Keys.D4)
            {
                Weapons.EquipGun(Weapons.Guns.Rifle);
            }

            if (e.KeyCode == Keys.D5)
            {
                Weapons.EquipGun(Weapons.Guns.Sniper);
            }

            if (e.KeyCode == Keys.D6)
            {
                Weapons.EquipGun(Weapons.Guns.Launcher);
            }

            if (e.KeyCode == Keys.D7)
            {
                Weapons.EquipGun(Weapons.Guns.MiniGun);
            }

            if (e.KeyCode == Keys.D8)
            {
                Weapons.EquipGun(Weapons.Guns.LaserGun);
            }

            if (e.KeyCode == Keys.Q)
            {
                foreach (Weapons.Inventory.Slot slot in Weapons.Inventory.Slots.ToList())
                {
                    if (Weapons.Inventory.SlotsHighlighted == slot.GunSlot)
                    {
                            Weapons.Inventory.Slots.Remove(slot);
                            Weapons.Inventory.Slots.TrimExcess();

                        Weapons.Inventory.SlotsTaken -= slot.SlotLength;
                    }
                }
            }

            if (e.KeyCode == Keys.Escape)
            {
                if (UIElements.DrawEscapeMenu)
                {
                    UIElements.DrawEscapeMenu = false;
                    GraphicsDrawing.FreezeState = false;

                    Player.Dir_Down = false;
                    Player.Dir_Left = false;
                    Player.Dir_Right = false;
                    Player.Dir_Up = false;

                    Player.Shooting = false;
                }
                else
                {
                    UIElements.DrawEscapeMenu = true;
                    GraphicsDrawing.FreezeState = true;

                    Player.Dir_Down = false;
                    Player.Dir_Left = false;
                    Player.Dir_Right = false;
                    Player.Dir_Up = false;

                    Player.Shooting = false;
                }
            }
        }

        public static void Collisions()
        {
            while (GraphicsDrawing.Running)
            {
                try
                {

                    //Bullets ArrayList lock causing lag in this Collisions loop.
                    foreach (Projectiles.Bullet Bullet in Projectiles.Bullets.ToList())
                    {
                        //null value check
                        if (Bullet == null)
                        { continue; }

                        foreach (Enemies.Enemy Enemy in Enemies.E.ToList())
                        {
                            //null value check
                            if (Enemy == null)
                            { continue; }

                            if (Enemy.HP >= 0)
                            {

                                if (new RectangleF(Bullet.Rect.X + UIElements.GraphicsOffset_X, Bullet.Rect.Y + UIElements.GraphicsOffset_Y, Bullet.Size, Bullet.Size)
                                    .IntersectsWith(new RectangleF(Enemy.Rect.X + UIElements.GraphicsOffset_X, Enemy.Rect.Y + UIElements.GraphicsOffset_Y, Enemy.Rect.Width, Enemy.Rect.Height)))
                                {

                                    Random rand1 = new Random();
                                    Random rand2 = new Random();

                                    if (rand1.Next(1, 1001) <= Projectiles.Bullet_CritRate)
                                    {
                                        //Enemy taking crit damage
                                        Enemy.HP -= (Bullet.Damage + Player.DmgIncrease * Projectiles.Bullet_CritDamage) / Enemy.Defence;

                                        Particles.AddDmgNum(Color.Red, 255f, new PointF(Enemy.Rect.X, Enemy.Rect.Y), 10, ((Bullet.Damage * Projectiles.Bullet_CritDamage) / Enemy.Defence));

                                        Particles.DmgNumCoolDown.Start();
                                    }
                                    else
                                    {
                                        //Enemy taking normal damage
                                        Enemy.HP -= (Bullet.Damage + Player.DmgIncrease) / Enemy.Defence;

                                        Particles.AddDmgNum(Color.Black, 255f, new PointF(Bullet.Rect.X + Bullet.Size / 2, Bullet.Rect.Y + Bullet.Size / 2), 8, Bullet.Damage / Enemy.Defence);

                                        Particles.DmgNumCoolDown.Start();
                                    }

                                    Bullet.Contact = true;

                                    //Bullet penetation value
                                    Bullet.Penetration -= 0.1f;

                                    if (Bullet.Penetration <= 0)
                                    {
                                            Projectiles.Bullets.Remove(Bullet);
                                            Projectiles.Bullets.TrimExcess();
                                    }

                                    Random rand = new Random();
                                    float size = rand.Next(3, 6);

                                    PointF vector;
                                    vector = Projectiles.Vector[rand.Next(0, 15)];
                                    lock (Particles.BloodParticles)
                                    {
                                        //adds blood
                                        Particles.AddBlood(Color.Red, 255, new RectangleF(Bullet.Rect.X, Bullet.Rect.Y, size, size), 1, 1);
                                        //sets directional vector             //vector is random vector value         //bullet.dir moves blood in direction of bullet
                                        Particles.BloodParticles.ElementAtOrDefault(Particles.BloodParticles.Count - 1).DirX = vector.X * Projectiles.BloodSpread + Bullet.DirX * Projectiles.BulletInfluenceOnBlood;
                                        Particles.BloodParticles.ElementAtOrDefault(Particles.BloodParticles.Count - 1).DirY = vector.Y * Projectiles.BloodSpread + Bullet.DirY * Projectiles.BulletInfluenceOnBlood;

                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            if (GraphicsDrawing.FreezeState == false)
            {
                //Player direction change
                if (e.KeyCode == Keys.W & Player.Dir_Up == true)
                {
                    Player.Dir_Up = false;
                }

                if (e.KeyCode == Keys.D & Player.Dir_Right == true)
                {
                    Player.Dir_Right = false;
                }

                if (e.KeyCode == Keys.S & Player.Dir_Down == true)
                {
                    Player.Dir_Down = false;
                }

                if (e.KeyCode == Keys.A & Player.Dir_Left == true)
                {
                    Player.Dir_Left = false;
                }

                //Shooting direction change
                if (e.KeyCode == Keys.Up & Projectiles.Dir_Up == true)
                {
                    Projectiles.Dir_Up = false;
                    Player.Shooting = false;
                }

                if (e.KeyCode == Keys.Right & Projectiles.Dir_Right == true)
                {
                    Projectiles.Dir_Right = false;
                    Player.Shooting = false;
                }

                if (e.KeyCode == Keys.Down & Projectiles.Dir_Down == true)
                {
                    Projectiles.Dir_Down = false;
                    Player.Shooting = false;
                }

                if (e.KeyCode == Keys.Left & Projectiles.Dir_Left == true)
                {
                    Projectiles.Dir_Left = false;
                    Player.Shooting = false;
                }
            }
            else
            {
                Thread.Sleep(50);
            }
        }

        private void Game_Load(object sender, EventArgs e)
        {
            SaveState.LoadConfig();

            foreach (Enemies.Enemy enemy in Enemies.E)
            {
                if (enemy.Type == "Normal")
                { enemy.Colour = Color.MediumVioletRed; enemy.Alpha = 200f; }
                if (enemy.Type == "Big")
                { enemy.Colour = Color.Plum; enemy.Alpha = 200f; }
                if (enemy.Type == "Huge")
                { enemy.Colour = Color.Fuchsia; enemy.Alpha = 200f; }
                if (enemy.Type == "Ninja")
                { enemy.Colour = Color.Pink; enemy.Alpha = 100f; }
                if (enemy.Type == "Suicide")
                { enemy.Colour = Color.Black; enemy.Alpha = 220f; }
                if (enemy.Type == "Summoner")
                { enemy.Colour = Color.DarkBlue; enemy.Alpha = 220f; }
                if (enemy.Type == "Minion")
                { enemy.Colour = Color.DarkOliveGreen; enemy.Alpha = 220f; }
            }

            Thread MovementT = new Thread(new ThreadStart(Player.MovementThread));
            MovementT.Start();

            Thread ProjectileMovementT = new Thread(new ThreadStart(Projectiles.ProjectileMovementThread));
            ProjectileMovementT.Start();

            Thread GInvalT = new Thread(new ThreadStart(GEInvalidate));
            GInvalT.Start();

            Thread CollisionsT = new Thread(new ThreadStart(Collisions));
            CollisionsT.Start();

            Thread ShootingT = new Thread(new ThreadStart(Weapons.Shooting));
            ShootingT.Start();

            Thread EnemiesSpawnT = new Thread(new ThreadStart(Enemies.EnemiesSpawner));
            EnemiesSpawnT.Start();

            Thread EnemiesMovementT = new Thread(new ThreadStart(Enemies.EnemiesMovement));
            EnemiesMovementT.Start();

            Thread AddBloodT = new Thread(new ThreadStart(Particles.DamageBloodMovement));
            AddBloodT.Start();

            Thread SpawnItemT = new Thread(new ThreadStart(SpawnItems.GenerateItem));
            SpawnItemT.Start();
            
            Thread AutoSaveValuesT = new Thread(new ThreadStart(SaveState.AutoSaveValues));
            AutoSaveValuesT.Start();

            //Loads vectors
            Projectiles.Vectors();

            //Apply zooming
            UIElements.ZoomScreenAndFitUI();
        }

        public static async void LevelUpTimer()
        {
            if (GraphicsDrawing.Running)
            {
                UIElements.LevelUpBack = true;

                if (Player.Level >= 5)
                {
                    Player.SkillPoints += 1;
                }

                await Task.Delay(80);
                UIElements.LevelUp = false;
                await Task.Delay(80);
                UIElements.LevelUp = true;
                await Task.Delay(80);
                UIElements.LevelUp = false;
                await Task.Delay(80);
                UIElements.LevelUp = true;
                await Task.Delay(80);
                UIElements.LevelUp = false;
                await Task.Delay(80);
                UIElements.LevelUp = true;
                await Task.Delay(2000);

                UIElements.LevelUp = false;
                UIElements.LevelUpBack = false;
            }
        }

        public static Bitmap img = new Bitmap(Properties.Resources.greenpentagons2x);

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            OnMapUpdated();

            //move rotation point to center of image
            //e.Graphics.TranslateTransform((float)UIElements.MapSize.Width / 6, (float)UIElements.MapSize.Height / 6);
            //rotate
            //e.Graphics.RotateTransform(60);

            if (GraphicsDrawing.DrawGame)
            {
                //Do not use CreateGraphics()

                e.Graphics.ScaleTransform(UIElements.ZoomScreen, UIElements.ZoomScreen);

                //NearestNeighbor interpolation
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

                //Compositing mode stop game from lagging when drawing images
                e.Graphics.CompositingMode = CompositingMode.SourceCopy;

                e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;

                //Background Image
                e.Graphics.DrawImage(img, UIElements.GraphicsOffset_X * UIElements.BackgroundParallax - 400, UIElements.GraphicsOffset_Y * UIElements.BackgroundParallax - 400);

                //Reset Compositing mode to draw non-images
                e.Graphics.CompositingMode = CompositingMode.SourceOver;

                //Zone
                UIElements.ZoneObj = new RectangleF(UIElements.Zone_Location.X + UIElements.GraphicsOffset_X, UIElements.Zone_Location.Y + UIElements.GraphicsOffset_Y, UIElements.Zone_Width, UIElements.Zone_Height);
                e.Graphics.FillRectangle(Brushes.Lime, UIElements.ZoneObj);

                //Borders
                UIElements.BorderLeft_Obj = new RectangleF(UIElements.BorderSize + UIElements.GraphicsOffset_X, UIElements.BorderSize + UIElements.GraphicsOffset_Y, UIElements.BorderSize, UIElements.MapSize.Height - TopMenu.Height - (UIElements.BorderSize * 2));
                e.Graphics.FillRectangle(UIElements.Border_Brush, UIElements.BorderLeft_Obj);

                UIElements.BorderTop_Obj = new RectangleF(UIElements.BorderSize + UIElements.GraphicsOffset_X, UIElements.BorderSize + UIElements.GraphicsOffset_Y, UIElements.MapSize.Width - (UIElements.BorderSize * 2), UIElements.BorderSize);
                e.Graphics.FillRectangle(UIElements.Border_Brush, UIElements.BorderTop_Obj);

                UIElements.BorderRight_Obj = new RectangleF(UIElements.MapSize.Width - (UIElements.BorderSize * 2) + UIElements.GraphicsOffset_X, UIElements.BorderSize + UIElements.GraphicsOffset_Y, UIElements.BorderSize, UIElements.MapSize.Height - TopMenu.Height - (UIElements.BorderSize * 2));
                e.Graphics.FillRectangle(UIElements.Border_Brush, UIElements.BorderRight_Obj);

                UIElements.BorderBottom_Obj = new RectangleF(UIElements.BorderSize + UIElements.GraphicsOffset_X, UIElements.MapSize.Height - TopMenu.Height - (UIElements.BorderSize * 2) + UIElements.GraphicsOffset_Y, UIElements.MapSize.Width - (UIElements.BorderSize * 2), UIElements.BorderSize);
                e.Graphics.FillRectangle(UIElements.Border_Brush, UIElements.BorderBottom_Obj);

                //PowerUps
                foreach (SpawnItems.PowerUp powerup in SpawnItems.PowerUps.ToList())
                {
                    if (powerup.Type == "HP")
                    {
                        e.Graphics.DrawImage(Properties.Resources.hpsmall, powerup.Rect.X + UIElements.GraphicsOffset_X, powerup.Rect.Y + UIElements.GraphicsOffset_Y);
                    }

                    if (powerup.Type == "Speed")
                    {
                        e.Graphics.DrawImage(Properties.Resources.SpeedUp, powerup.Rect.X + UIElements.GraphicsOffset_X, powerup.Rect.Y + UIElements.GraphicsOffset_Y);
                    }

                    if (powerup.Type == "Defence")
                    {
                        e.Graphics.DrawImage(Properties.Resources.DefenceUp, powerup.Rect.X + UIElements.GraphicsOffset_X, powerup.Rect.Y + UIElements.GraphicsOffset_Y);
                    }

                }

                //Weapons and items
                foreach (SpawnItems.Item item in SpawnItems.Items.ToList())
                {
                    if (item.Type == "AutoPistol")
                    {

                    }

                    Brush itembrush = new SolidBrush(Color.FromArgb((int)item.Alpha, Color.Azure));
                    e.Graphics.FillRectangle(itembrush, new RectangleF(item.Rect.X + UIElements.GraphicsOffset_X, item.Rect.Y + UIElements.GraphicsOffset_Y, 48, 48));
                }

                //Player
                Player.PlayerObj = new RectangleF(Player.Location.X, Player.Location.Y, Player.Size, Player.Size);
                UIElements.RotateRectangle(e.Graphics, Player.PlayerObj, Player.Vector);

                    //Bullets
                    foreach (Projectiles.Bullet Bullet in Projectiles.Bullets.ToList())
                    {
                        Brush bulletbrush = new SolidBrush(Color.FromArgb((int)Bullet.Alpha, Color.Black));
                        e.Graphics.FillRectangle(bulletbrush, new RectangleF(Bullet.Rect.X + UIElements.GraphicsOffset_X, Bullet.Rect.Y + UIElements.GraphicsOffset_Y, Bullet.Size, Bullet.Size));
                    }

                //Draws all enemies
                foreach (Enemies.Enemy Enemy in Enemies.E.ToList())
                {
                    Brush brush1 = new SolidBrush(Color.FromArgb((int)Enemy.Alpha, Enemy.Colour.R, Enemy.Colour.G, Enemy.Colour.B));
                    e.Graphics.FillRectangle(brush1, new RectangleF(Enemy.Rect.X + UIElements.GraphicsOffset_X, Enemy.Rect.Y + UIElements.GraphicsOffset_Y, Enemy.Size, Enemy.Size));

                    //HP bars for enemys
                    if (Enemy.HP >= 0)
                    {
                        e.Graphics.DrawString($"LVL {Enemy.Level}", new Font("Segoe UI", 10 + Enemy.Size / 12), Brushes.Black, Enemy.Rect.X + UIElements.GraphicsOffset_X - (Enemy.Level.ToString().Length * 4) + Enemy.Size / 3 - 7, Enemy.Rect.Y + UIElements.GraphicsOffset_Y - 45 - Enemy.Size / 8);

                        e.Graphics.FillRectangle(Brushes.Black, new RectangleF(Enemy.Rect.X + UIElements.GraphicsOffset_X - 1 + Enemy.Size / 2f - Enemy.HP / 10 / 3f, (Enemy.Rect.Y + UIElements.HPBarOffset) + UIElements.GraphicsOffset_Y - 1, Enemy.HP / 1.5f / 10 + 2, UIElements.HPBarHeight + 2));
                        e.Graphics.FillRectangle(Brushes.Orange, new RectangleF(Enemy.Rect.X + UIElements.GraphicsOffset_X + Enemy.Size / 2f - Enemy.HP / 10 / 3f, (Enemy.Rect.Y + UIElements.HPBarOffset) + UIElements.GraphicsOffset_Y, Enemy.HP / 1.5f / 10, UIElements.HPBarHeight));
                    }
                }

                if (UIElements.LevelUpBack)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 255, 255, 255)), new RectangleF(Player.Location.X - 40, Player.Location.Y - 70, 115, 65));

                    if (UIElements.LevelUp)
                    {
                        e.Graphics.DrawString($"LEVEL UP{Environment.NewLine}  Level {Player.Level}", new Font("Segoe UI", 18, FontStyle.Bold), Brushes.Blue, Player.Location.X - 40, Player.Location.Y - 70);
                    }
                }

                if (Weapons.Ammo <= 0)
                {
                    if (Weapons.Gun != Weapons.Guns.None)
                    {
                        Random rand = new Random();
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 255, 255, 255)), new RectangleF(Player.Location.X - 40, Player.Location.Y + 45, 100, 20));
                        e.Graphics.DrawString("RELOADING", new Font("Segoe UI", 12, FontStyle.Bold), Brushes.Black, Player.Location.X - 40, Player.Location.Y + 45);
                    }
                }

                //Blood particles
                    foreach (Particles.BloodParticle Blood in Particles.BloodParticles.ToList())
                    {

                    if (Blood == null) { continue; }

                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb((int)Blood.Alpha, Blood.Colour.R, Blood.Colour.G, Blood.Colour.B)), new RectangleF(Blood.Rect.X + UIElements.GraphicsOffset_X, Blood.Rect.Y + UIElements.GraphicsOffset_Y, Blood.Rect.Width, Blood.Rect.Height));
                    }
                    foreach (Particles.XPParticle xp in Particles.XPParticles.ToList())
                    {

                    if (xp == null) { continue; }

                        e.Graphics.FillRectangle(new SolidBrush(xp.Colour), new RectangleF(xp.Rect.X + UIElements.GraphicsOffset_X, xp.Rect.Y + UIElements.GraphicsOffset_Y, xp.Size, xp.Size));
                    }

                foreach (Particles.DmgNum dmgnum in Particles.DmgNums.ToList())
                {
                    e.Graphics.DrawString(Math.Round(dmgnum.Dmg, 2).ToString(), new Font("Segoe UI", dmgnum.FontSize, FontStyle.Bold), new SolidBrush(Color.FromArgb((int)dmgnum.Alpha, dmgnum.Colour.R, dmgnum.Colour.G, dmgnum.Colour.B)), dmgnum.Loc.X + UIElements.GraphicsOffset_X, dmgnum.Loc.Y + UIElements.GraphicsOffset_Y);
                }

                //Bar Menu
                if (UIElements.BarMenu_Draw)
                {
                    //BarMenu semitransparent background
                    e.Graphics.FillRectangle(UIElements.BarMenu_Brush, 0, 0, UIElements.ScreenSize.Width, 82);

                    //Enemy and Bullet count
                    e.Graphics.DrawString($"{Enemies.E.Count} Enemies {Projectiles.Bullets.Count} Bullets  -  Speed: {Convert.ToInt16((Player.Speed * 10)).ToString()}  -  Score: {Player.Score.ToString()} - Level: {Player.Level}", new Font("Segoe UI", 12), Brushes.White, 0, 0);
                    e.Graphics.DrawString($"Gun: {Weapons.Gun.ToString()}, Ammo: {Weapons.Ammo} / {Weapons.MaxAmmo}, Type: {"#####"}, Firerate: {Math.Round((float)Projectiles.Bullet_Delay * (1f / 30f), 1, MidpointRounding.ToEven)}, Bullet Size: {Projectiles.Bullet_Size} Bullet Speed: {Projectiles.Bullet_Speed}", new Font("Segoe UI", 12), Brushes.White, 0, 20);
                    e.Graphics.FillRectangle(Brushes.PaleVioletRed, new RectangleF(5, 50, UIElements.HPBarWidth, 20));
                    e.Graphics.FillRectangle(Brushes.White, new RectangleF(5, 50, (Player.HP / Player.MaxHP) * UIElements.HPBarWidth, 20));
                    e.Graphics.DrawString($"{(int)Player.HP} / {(int)Player.MaxHP}", new Font("Segoe UI", 11, FontStyle.Bold), Brushes.Black, UIElements.HPBarWidth / 2 - 20, 50);

                    //FPS display
                    Random rand = new Random();
                    int temp = rand.Next(0, 5);
                    if (temp == 1)
                    { FPS = GetFps(); }
                    e.Graphics.DrawString($"{(int)(FPS)}", new Font("Segoe UI", 14), Brushes.White, this.Width - 60, 0);

                    //Inventory slots
                    for (int x = 0; x < 12; x++)
                    {
                        e.Graphics.FillRectangle(Brushes.White, new RectangleF((32 * x) + 5 + UIElements.HPBarWidth + 10, 45, 32, 32));

                        e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)((32 * x) + 5 + UIElements.HPBarWidth + 10), 45, 32, 32));

                        for (int i = Weapons.Inventory.SlotsHighlighted; i < Weapons.Inventory.SlotsHighlightedLength; i++)
                        {
                            e.Graphics.FillRectangle(Brushes.PaleVioletRed, new RectangleF((32 * i) + 5 + UIElements.HPBarWidth + 10, 46, 32, 31));
                        }


                        //Draw weapons in weapons slot testing
                        //e.Graphics.DrawImage(Properties.Resources.lasercannon, 5 + UIElements.HPBarWidth + 10 + 1, 47, 192 - 3, 29);
                        //e.Graphics.DrawImage(Properties.Resources.pistol, 5 + UIElements.HPBarWidth + 10 + 192, 47, 32 - 2, 29);
                        //e.Graphics.DrawImage(Properties.Resources.smg, 5 + UIElements.HPBarWidth + 10 + 224, 47, 32 - 2, 29);

                        //Draws weapons in their slots

                        foreach (Weapons.Inventory.Slot slot in Weapons.Inventory.Slots.ToList())
                        {

                            if (slot.GunSlot + slot.SlotLength <= 12)
                            {

                                if (slot.Gun == Weapons.Guns.Pistol)
                            {
                                e.Graphics.DrawImage(Properties.Resources.pistol, 5 + UIElements.HPBarWidth + 11 + slot.GunSlot * 32, 47, Weapons.Inventory.Pistol * 32 - 2, 29);
                            }

                            if (slot.Gun == Weapons.Guns.AutoPistol)
                            {
                                e.Graphics.DrawImage(Properties.Resources.autopistol, 5 + UIElements.HPBarWidth + 11 + slot.GunSlot * 32, 47, Weapons.Inventory.AutoPistol * 32 - 2, 29);
                            }

                            if (slot.Gun == Weapons.Guns.SMG)
                            {
                                e.Graphics.DrawImage(Properties.Resources.smg, 5 + UIElements.HPBarWidth + 11 + slot.GunSlot * 32, 47, Weapons.Inventory.SMG * 32 - 2, 29);
                            }

                            if (slot.Gun == Weapons.Guns.Rifle)
                            {
                                e.Graphics.DrawImage(Properties.Resources.rifle, 5 + UIElements.HPBarWidth + 11 + slot.GunSlot * 32, 47, Weapons.Inventory.Rifle * 32 - 2, 29);
                            }

                            if (slot.Gun == Weapons.Guns.Sniper)
                            {
                                e.Graphics.DrawImage(Properties.Resources.sniper, 5 + UIElements.HPBarWidth + 11 + slot.GunSlot * 32, 47, Weapons.Inventory.Sniper * 32 - 2, 29);
                            }

                            if (slot.Gun == Weapons.Guns.Launcher)
                            {
                                e.Graphics.DrawImage(Properties.Resources.launcher, 5 + UIElements.HPBarWidth + 11 + slot.GunSlot * 32, 47, Weapons.Inventory.Launcher * 32 - 2, 29);
                            }

                            if (slot.Gun == Weapons.Guns.MiniGun)
                            {
                                e.Graphics.DrawImage(Properties.Resources.lasercannon, 5 + UIElements.HPBarWidth + 11 + slot.GunSlot * 32, 47, Weapons.Inventory.LaserCannon * 32 - 2, 29);
                            }

                            if (slot.Gun == Weapons.Guns.LaserGun)
                            {
                                e.Graphics.DrawImage(Properties.Resources.lasercannon, 5 + UIElements.HPBarWidth + 11 + slot.GunSlot * 32, 47, Weapons.Inventory.LaserCannon * 32 - 2, 29);
                            }
                            }
                            else
                            {
                                    Weapons.Inventory.SlotsTaken -= slot.SlotLength;

                                    Weapons.Inventory.Slots.Remove(slot);
                                    Weapons.Inventory.Slots.TrimExcess();
                            }
                        }

                    }

                    e.Graphics.FillRectangle(Brushes.Gray, 0, 82, UIElements.ScreenSize.Width, 5);
                    e.Graphics.FillRectangle(Brushes.DeepSkyBlue, 0, 82, Player.XP, 5);
                }

                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb((int)UIElements.ScreenAlpha, 0, 0, 0)), 0, 0, UIElements.ScreenSize.Width, UIElements.ScreenSize.Height);

                if (UIElements.ScreenAlpha > 245)
                {
                    e.Graphics.DrawString($"You died...", new Font("Segoe UI", 36), Brushes.DarkRed, UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 2 - 70);
                }

                if (UIElements.DrawEscapeMenu == true)
                {

                    Font font = new Font("Segoe UI", 24, FontStyle.Bold);

                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), new RectangleF(0, 0, UIElements.ScreenSize.Width, UIElements.ScreenSize.Height));
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), new RectangleF(UIElements.ScreenSize.Width / 3, 0, UIElements.ScreenSize.Width / 3, UIElements.ScreenSize.Height));

                    e.Graphics.DrawString("GAME PAUSE", new Font("Segoe UI", 30, FontStyle.Bold), Brushes.White, UIElements.ScreenSize.Width / 2 - 130, 60);

                    e.Graphics.FillRectangle(UIElements.btnResumeCol, UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 50, 200, 50);
                    e.Graphics.DrawString("RESUME", font, Brushes.White, UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 50);
                    e.Graphics.FillRectangle(UIElements.btnSkillTreeCol, UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 150, 200, 50);
                    e.Graphics.DrawString("SKILL TREE", font, Brushes.White, UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 150);
                    e.Graphics.FillRectangle(UIElements.btnOptionsCol, UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 250, 200, 50);
                    e.Graphics.DrawString("STATS", font, Brushes.White, UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 250);
                    e.Graphics.FillRectangle(UIElements.btnStatsCol, UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 350, 200, 50);
                    e.Graphics.DrawString("OPTIONS", font, Brushes.White, UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 350);
                    e.Graphics.FillRectangle(UIElements.btnExitCol, UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 450, 200, 50);
                    e.Graphics.DrawString("EXIT", font, Brushes.White, UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 450);
                }

                if (UIElements.DrawSkillTree)
                {
                    //Spend skill points in exchange for stat and weapon upgrades

                    e.Graphics.FillRectangle(Brushes.LightGray, new RectangleF(0 , 0, UIElements.ScreenSize.Width, UIElements.ScreenSize.Height));
                    e.Graphics.FillRectangle(Brushes.Red, new RectangleF(100, 110, 100, 50));
                    e.Graphics.FillRectangle(Brushes.Red, new RectangleF(100, 220, 100, 50));
                    e.Graphics.FillRectangle(Brushes.Red, new RectangleF(100, 330, 100, 50));
                    e.Graphics.FillRectangle(Brushes.Red, new RectangleF(100, 440, 100, 50));
                    e.Graphics.FillRectangle(Brushes.Red, new RectangleF(100, 550, 100, 50));

                    e.Graphics.FillRectangle(Brushes.CornflowerBlue, UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height - 150, 200, 50);
                }

                if (UIElements.DrawOptions)
                {
                    //Fullscreen, Drawing modes / Antialiasing, Zoom (Needs to be fixed)
                }

                if (UIElements.DrawStats)
                {
                    //HP, DMG, SPD, DEF, LVL, EXP, CRIT Damage, CRIT Chance,

                    e.Graphics.DrawString($"{Player.HP}", new Font("Segoe UI", 18), Brushes.White, UIElements.ScreenSize.Width - 40, 100);
                }
                //DEBUG MODE
                if (false)
                {
                    Stopwatch timer = new Stopwatch();
                    timer.Start();

                    e.Graphics.DrawString($"{GraphicsDrawing.DrawingTime}", new Font("Segoe UI", 18), Brushes.White, UIElements.ScreenSize.Width - 40, 5);

                    foreach (Enemies.Enemy Enemy in Enemies.E.ToList())
                    {
                        e.Graphics.DrawLine(Pens.Red, Player.Location.X + Player.Size / 2, Player.Location.Y + Player.Size / 2, Enemy.Rect.X + UIElements.GraphicsOffset_X + Enemy.Size / 2, Enemy.Rect.Y + UIElements.GraphicsOffset_Y + Enemy.Size / 2);
                        e.Graphics.FillRectangle(Brushes.White, Enemy.Rect.X + UIElements.GraphicsOffset_X - 300, Enemy.Rect.Y + UIElements.GraphicsOffset_Y + Enemy.Size, 800, 18);
                        e.Graphics.DrawString($"Alpha:{Enemy.Alpha}, Col:{Enemy.Colour}, Dmg:{Enemy.Damage}, Def:{Enemy.Defence}, HP:{Enemy.HP}, Lvl:{Enemy.Level}, Size:{Enemy.Size}, Spd:{Enemy.Speed}, Type:{Enemy.Type}, XP:{Enemy.XP}", new Font("Segoe UI", 11), Brushes.Black, Enemy.Rect.X + UIElements.GraphicsOffset_X- 300, Enemy.Rect.Y + UIElements.GraphicsOffset_Y + Enemy.Size);
                    }

                    foreach (Projectiles.Bullet Bullet in Projectiles.Bullets.ToList())
                    {
                        e.Graphics.DrawLine(Pens.Black, Player.Location.X + Player.Size / 2, Player.Location.Y + Player.Size / 2, Bullet.Rect.X + UIElements.GraphicsOffset_X + Bullet.Size / 2, Bullet.Rect.Y + UIElements.GraphicsOffset_Y + Bullet.Size / 2);
                    }

                    foreach (Particles.BloodParticle Blood in Particles.BloodParticles.ToList())
                    {
                        e.Graphics.DrawLine(Pens.Purple, Player.Location.X + Player.Size / 2, Player.Location.Y + Player.Size / 2, Blood.Rect.X + UIElements.GraphicsOffset_X, Blood.Rect.Y + UIElements.GraphicsOffset_Y);
                    }

                    foreach (Particles.XPParticle XP in Particles.XPParticles.ToList())
                    {
                        e.Graphics.DrawLine(Pens.BlueViolet, Player.Location.X + Player.Size / 2, Player.Location.Y + Player.Size / 2, XP.Rect.X + UIElements.GraphicsOffset_X + XP.Size / 2, XP.Rect.Y + UIElements.GraphicsOffset_Y + XP.Size / 2);
                    }

                    //e.Graphics.DrawLine(Pens.Blue, Player.Location.X + Player.Size / 2, Player.Location.Y + Player.Size / 2, SpawnItems.PowerUpObj.X + SpawnItems.PowerUpObj.Size.Width / 2, SpawnItems.PowerUpObj.Y + SpawnItems.PowerUpObj.Size.Height / 2);

                    GraphicsDrawing.DrawingTime = timer.ElapsedMilliseconds;
                    timer.Stop();
                }
            }
        }

        private void TopMenu_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.DarkRed, TopMenu.Width - 30, 0, 20, 30);

            e.Graphics.FillRectangle(Brushes.CornflowerBlue, TopMenu.Width - 55, 0, 20, 30);

            e.Graphics.FillRectangle(Brushes.CornflowerBlue, TopMenu.Width - 80, 0, 20, 30);
        }

        private void TopMenu_MouseDown(object sender, MouseEventArgs e)
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

            if (new RectangleF(TopMenu.Width - 30, 0, 20, 30).Contains(e.Location))
            {
                Menu menu = new Menu();
                menu.Show();

                this.Hide();

                GraphicsDrawing.Running = false;
                GraphicsDrawing.DrawGame = false;
                GraphicsDrawing.FreezeState = true;
            }

            if (new RectangleF(TopMenu.Width - 55, 0, 20, 30).Contains(e.Location))
            {

            }

            if (new RectangleF(TopMenu.Width - 80, 0, 20, 30).Contains(e.Location))
            {

            }
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (GraphicsDrawing.FreezeState)
            {
                if (UIElements.DrawEscapeMenu)
                {
                    if (new RectangleF(UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 50, 200, 50).Contains(e.Location))
                    {
                        //Resume button
                        UIElements.DrawEscapeMenu = false;
                        GraphicsDrawing.FreezeState = false;
                    }

                    if (new RectangleF(UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 150, 200, 50).Contains(e.Location))
                    {
                        //Skill tree button
                        //TODO Skill tree
                        UIElements.DrawEscapeMenu = false;
                        UIElements.DrawSkillTree = true;
                    }

                    if (new RectangleF(UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 250, 200, 50).Contains(e.Location))
                    {
                        //Stats button
                        //TODO Stats
                        UIElements.DrawEscapeMenu = false;
                        UIElements.DrawStats = true;
                    }

                    if (new RectangleF(UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 350, 200, 50).Contains(e.Location))
                    {
                        //Options button
                        //TODO Options
                        UIElements.DrawEscapeMenu = false;
                        UIElements.DrawOptions = true;
                    }

                    if (new RectangleF(UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 450, 200, 50).Contains(e.Location))
                    {
                        GraphicsDrawing.Running = false;
                        GraphicsDrawing.DrawGame = false;

                        //Exit button
                        Environment.Exit(0);
                    }
                }

                if (UIElements.DrawSkillTree)
                {
                    if (new RectangleF(100, 110, 100, 50).Contains(e.Location))
                    {

                    }

                    if (new RectangleF(100, 220, 100, 50).Contains(e.Location))
                    {

                    }

                    if (new RectangleF(100, 330, 100, 50).Contains(e.Location))
                    {

                    }

                    if (new RectangleF(100, 440, 100, 50).Contains(e.Location))
                    {

                    }
                }
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (UIElements.DrawEscapeMenu)
            {
                if (new RectangleF(UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 50, 200, 50).Contains(e.Location))
                {
                    //Resume button
                    UIElements.btnResumeCol = Brushes.DeepSkyBlue;
                }
                else
                {
                    UIElements.btnResumeCol = Brushes.CornflowerBlue;
                }

                if (new RectangleF(UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 150, 200, 50).Contains(e.Location))
                {
                    //Skill tree button
                    UIElements.btnSkillTreeCol = Brushes.DeepSkyBlue;
                }
                else
                {
                    UIElements.btnSkillTreeCol = Brushes.CornflowerBlue;
                }

                if (new RectangleF(UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 250, 250, 50).Contains(e.Location))
                {
                    //Options button
                    UIElements.btnOptionsCol = Brushes.DeepSkyBlue;
                }
                else
                {
                    UIElements.btnOptionsCol = Brushes.CornflowerBlue;
                }

                if (new RectangleF(UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 350, 200, 50).Contains(e.Location))
                {
                //Skill tree button
                UIElements.btnStatsCol = Brushes.DeepSkyBlue;
            }
            else
            {
                UIElements.btnStatsCol = Brushes.CornflowerBlue;
            }

            if (new RectangleF(UIElements.ScreenSize.Width / 2 - 100, UIElements.ScreenSize.Height / 4.5f - 50 + 450, 200, 50).Contains(e.Location))
                {
                    //Exit button
                    UIElements.btnExitCol = Brushes.DeepSkyBlue;
                }
                else
                {
                    UIElements.btnExitCol = Brushes.CornflowerBlue;
                }
            }

            if (UIElements.DrawOptions)
            {

            }

            if (UIElements.DrawSkillTree)
            {

            }

            if (UIElements.DrawStats)
            {

            }
        }
    }
}
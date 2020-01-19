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

namespace TwoDGameEngine
{
    public class Enemies
    {
        public static List<Enemy> E = new List<Enemy>();

        public static int SpawnRate;

        public class Enemy
        {
            public RectangleF Rect { get; set; }
            public string Type { get; set; }
            public float HP { get; set; }
            public float Damage { get; set; }
            public float Defence { get; set; }
            public float Size { get; set; }
            public float Speed { get; set; }
            public Color Colour { get; set; }
            public float Alpha { get; set; }
            public float XP { get; set; }
            public int Level { get; set; }
        }

        public static void AddEnemy(string type, RectangleF rect, float hp, float damage, float defence, float size, float speed, Color colour, float alpha, float xp, int level)
        {
            E.Add(new Enemy()
            {
                Type = type,
                Rect = rect,
                HP = hp + hp * level / 60,
                Damage = damage + damage * level / 60,
                Defence = defence + defence * level / 60,
                Size = size + size * level / 80,
                Speed = speed + speed * level / 80,
                Colour = colour,
                Alpha = alpha,
                XP = xp + xp * level / 70,
                Level = level
            });
        }

        private static float dx;
        private static float dy;
        private static double length;

        //---------------------------------------
        //UNUSED CODE  \/
        public static void MoveTo(float x, float y)
        {
            try
            {
                foreach (Enemies.Enemy Enemy in Enemies.E.ToList())
                {
                    dx = x - Enemy.Rect.X;
                    dy = y - Enemy.Rect.Y;
                    length = Math.Sqrt(dx * dx + dy * dy);

                    if (length >= Enemy.Speed)
                    {
                        //Move towards the destination
                        Enemy.Rect = new RectangleF((float)(Enemy.Rect.X + Enemy.Speed * dx / length),
                            (float)(Enemy.Rect.Y + Enemy.Speed * dy / length), Enemy.Size, Enemy.Size);
                    }
                    else
                    {
                        //Destination reached
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }
        //UNUSED CODE  /\
        //---------------------------------------

        public static async void EnemiesSpawner()
        {
            while (GraphicsDrawing.Running)
            {
                if (GraphicsDrawing.FreezeState == false)
                {
                    Random rand = new Random();

                    //Adds Enemies to array and sets location to spawn point
                    lock (Enemies.E)
                    {
                        Random r = new Random();

                        //Percentage chance of spawning
                        float SpawnChance_Normal = 30;
                        float SpawnChance_Big = 25;
                        float SpawnChance_Huge = 15;
                        float SpawnChance_Ninja = 15;
                        float SpawnChance_Suicide = 10;
                        float SpawnChance_Summoner = 5;

                        float RandLevelMin = Player.Level / 1.2f;
                        float RandLevelMax = Player.Level * 2f;

                        //Generates enemy level with a large bias for smaller levels and a small chance for stronger enemies
                        int ScaledLevel = (int)Math.Floor(RandLevelMin + (RandLevelMax - RandLevelMin) * Math.Pow(r.NextDouble(), 6)); //Larger power decreases variation

                        if (rand.Next(1, 101) <= SpawnChance_Normal && Player.Level > 0)
                        {
                            //          type     rect                                                                                                                                hp  dmg  def size  spd   colour                alpha  xp    level
                            AddEnemy("Normal", new RectangleF(UIElements.MapSize.Width - 100 + rand.Next(-50, 50), UIElements.MapSize.Height / 2 + rand.Next(-200, 200), 20f, 20f), 5f, 0.1f, 1f, 15f, 3.5f, Color.MediumVioletRed, 200f, 0.1f, ScaledLevel);
                        }
                        if (rand.Next(1, 101) <= SpawnChance_Big && Player.Level > 0)
                        {
                            AddEnemy("Big", new RectangleF(UIElements.MapSize.Width - 100 + rand.Next(-50, 50), UIElements.MapSize.Height / 2 + rand.Next(-200, 200), 20f, 20f), 700f, 0.3f, 4f, 40f, 2.5f, Color.Plum, 200f, 0.2f, ScaledLevel);
                        }
                        if (rand.Next(1, 101) <= SpawnChance_Huge && Player.Level > 0)
                        {
                            AddEnemy("Huge", new RectangleF(UIElements.MapSize.Width - 100 + rand.Next(-50, 50), UIElements.MapSize.Height / 2 + rand.Next(-200, 200), 20f, 20f), 2000f, 0.5f, 8f, 100f, 1f, Color.Fuchsia, 200f, 0.25f, ScaledLevel);
                        }
                        if (rand.Next(1, 101) <= SpawnChance_Ninja && Player.Level > 0)
                        {
                            AddEnemy("Ninja", new RectangleF(UIElements.MapSize.Width - 100 + rand.Next(-50, 50), UIElements.MapSize.Height / 2 + rand.Next(-200, 200), 20f, 20f), 500f, 0.8f, 5f, 30f, 2f, Color.Pink, 100f, 0.35f, ScaledLevel);
                        }
                        if (rand.Next(1, 101) <= SpawnChance_Suicide && Player.Level > 0)
                        {
                            AddEnemy("Suicide", new RectangleF(UIElements.MapSize.Width - 100 + rand.Next(-50, 50), UIElements.MapSize.Height / 2 + rand.Next(-200, 200), 20f, 20f), 200f, 2f, 4f, 30f, 3f, Color.Black, 220f, 0.0f, ScaledLevel);
                        }
                        if (rand.Next(1, 101) <= SpawnChance_Summoner && Player.Level > 0)
                        {
                            AddEnemy("Summoner", new RectangleF(UIElements.MapSize.Width - 100 + rand.Next(-50, 50), UIElements.MapSize.Height / 2 + rand.Next(-200, 200), 20f, 20f), 300f, 1f, 4f, 50f, 0.8f, Color.DarkBlue, 220f, 0.65f, ScaledLevel);
                        }
                    }

                    await Task.Delay(Enemies.SpawnRate - Player.Level * 4);
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }

        public static async void EnemiesMovement()
        {
            while (GraphicsDrawing.Running)
            {
                if (!GraphicsDrawing.FreezeState)
                {
                    lock (Enemies.E)
                    {
                        foreach (Enemies.Enemy Enemy in Enemies.E.ToList())
                        {

                            if (Enemy.Type == "Suicide" && Enemy.HP <= 0)
                            {
                                //Suicide enemy
                                Enemy.Size += Enemy.Size / 15;
                                Enemy.Rect = new RectangleF(Enemy.Rect.X - Enemy.Size / 30, Enemy.Rect.Y - Enemy.Size / 30, Enemy.Rect.Width, Enemy.Rect.Height);

                                //Player taking damage
                                if (new RectangleF(Enemy.Rect.X + UIElements.GraphicsOffset_X, Enemy.Rect.Y + UIElements.GraphicsOffset_Y, Enemy.Size, Enemy.Size).IntersectsWith(Player.PlayerObj))
                                {
                                    Player.HP -= Enemy.Damage * 5 / Player.Defence;
                                }

                                if (Enemy.Size > 300)
                                {
                                    //Enemy dead
                                    //Removes enemy
                                    Enemies.E.Remove(Enemy);
                                    Enemies.E.TrimExcess();
                                }
                            }

                            if (Enemy.Type == "Summoner")
                            {
                                Random rand1 = new Random();
                                if (rand1.Next(1, 101) <= 1 && Player.Level > 0)
                                {
                                    AddEnemy("Minion", new RectangleF(Enemy.Rect.X + Enemy.Size / 2, Enemy.Rect.Y + Enemy.Size / 2, 20, 20), 50, 1, 2, 15, 1.8f, Color.DarkOliveGreen, 220, 0.01f, Enemy.Level);
                                }
                            }

                            if (Player.HP >= 0)
                            {
                                //Player taking damage
                                if (new RectangleF(Enemy.Rect.X + UIElements.GraphicsOffset_X, Enemy.Rect.Y + UIElements.GraphicsOffset_Y, Enemy.Rect.Width, Enemy.Rect.Height).IntersectsWith(Player.PlayerObj))
                                {
                                    Player.HP -= Enemy.Damage / Player.Defence;
                                }
                            }
                            else
                            {
                                //Player dead
                                if (UIElements.ScreenAlpha < 250)
                                {
                                    GraphicsDrawing.FreezeState = true;
                                    UIElements.ScreenAlpha += 5f;
                                }
                            }

                            if (GraphicsDrawing.FreezeState == false)
                            {

                                if (Enemy.HP >= 0)
                                {
                                    //Enemy movement
                                    float x = Player.Location.X - UIElements.GraphicsOffset_X + Player.Size / 2 - Enemy.Size / 2;
                                    float y = Player.Location.Y - UIElements.GraphicsOffset_Y + Player.Size / 2 - Enemy.Size / 2;

                                    float dx = x - Enemy.Rect.X;
                                    float dy = y - Enemy.Rect.Y;
                                    double length = Math.Sqrt(dx * dx + dy * dy);

                                    if (length >= Enemy.Speed)
                                    {
                                        //Move towards the destination
                                        Enemy.Rect = new RectangleF((float)(Enemy.Rect.X + Enemy.Speed * dx / length),
                                            (float)(Enemy.Rect.Y + Enemy.Speed * dy / length), Enemy.Size, Enemy.Size);
                                    }
                                }
                                else
                                {

                                    //enemy death animation
                                    Player.XP += Enemy.XP / (Player.Level / 20f);

                                    //Level up
                                    if (Player.XP > UIElements.ScreenSize.Width)
                                    {
                                        Player.XP = 0;
                                        Player.Level += 1;
                                        UIElements.LevelUp = true;
                                        Game.LevelUpTimer();

                                        if (Player.HP - Player.MaxHP * Player.Level / 160 < Player.HP)
                                        {
                                            Player.HP += Player.MaxHP * Player.Level / 160;
                                        }
                                        else
                                        {
                                            Player.HP = Player.MaxHP;
                                        }

                                        Player.MaxHP += Player.MaxHP * Player.Level / 160;
                                        Player.Defence += Player.Defence * Player.Level / 180;
                                        Player.Speed += 0.01f;
                                    }

                                    //Add xp particles
                                    Random rand = new Random();
                                    if (rand.Next(0, 22) > 20)
                                    {
                                        Particles.AddXP(Color.Blue, 255, Enemy.Rect, 5, 10f);
                                    }


                                    if (Enemy.Size >= 5)
                                    {

                                        //Other enemies
                                        Enemy.Size -= Enemy.Size / 80;
                                        Enemy.Rect = new RectangleF(Enemy.Rect.X + Enemy.Size / 160, Enemy.Rect.Y + Enemy.Size / 160, Enemy.Rect.Width, Enemy.Rect.Height);

                                        if (Enemy.Alpha >= 5)
                                        {
                                            Enemy.Alpha -= 2.7f;
                                        }

                                        Enemy.Colour = Color.FromArgb((int)Enemy.Alpha, Enemy.Colour);
                                    }
                                    else
                                    {
                                        //Enemy dead
                                        //Removes enemy
                                        Enemies.E.Remove(Enemy);
                                        Enemies.E.TrimExcess();
                                    }
                                }

                            }
                            else
                            {
                                Thread.Sleep(50);
                            }
                        }
                    }
                }

                await Task.Delay(10);
            }
        }
    }
}
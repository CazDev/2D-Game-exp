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
    class Projectiles
    {
        //Direction
        public static bool Dir_Left = false;
        public static bool Dir_Right = false;
        public static bool Dir_Up = false;
        public static bool Dir_Down = false;

        public static float Bullet_CritRate = 50f;
        public static float Bullet_CritDamage = 3f;
        public static float Bullet_Size;
        public static float Bullet_Speed;
        public static float Bullet_Damage;
        public static float Bullet_Penetration;

        public static float BloodDecayRate = 5;
        public static float BloodSpread = 2;
        public static float BulletInfluenceOnBlood = 2;
        public static float BloodSpeedMultiplier = 0.2f;

        public static List<Bullet> Bullets = new List<Bullet>();

        public class Bullet
        {
            public RectangleF Rect { get; set; }
            public string Type { get; set; }
            public float Damage { get; set; }
            public float Size { get; set; }
            public float Speed { get; set; }
            public float Penetration { get; set; }
            public bool Bounce { get; set; }
            public float DirX { get; set; }
            public float DirY { get; set; }
            public bool Contact { get; set; }
            public float Alpha { get; set; }
        }

        public static PointF[] Vector = new PointF[16];

        public static void Vectors()
        {
            //16 directional vectors
            Vector[0] = new PointF(0f, -1f);
            Vector[1] = new PointF(1.5f, -0.5f);
            Vector[2] = new PointF(1f, 1f);
            Vector[3] = new PointF(1.5f, 0.5f);
            Vector[4] = new PointF(1f, 0f);
            Vector[5] = new PointF(1.5f, -0.5f);
            Vector[6] = new PointF(1f, 0f);
            Vector[7] = new PointF(1.5f, 0.5f);
            Vector[8] = new PointF(1f, -1f);
            Vector[9] = new PointF(0.5f, 1.5f);
            Vector[10] = new PointF(-0.5f, 1.5f);
            Vector[11] = new PointF(-1f, 1f);
            Vector[12] = new PointF(-1.5f, 0.5f);
            Vector[13] = new PointF(-1f, 0f);
            Vector[14] = new PointF(-1.5f, -0.5f);
            Vector[15] = new PointF(-0.5f, -1.5f);
        }

        public static void AddBullet(RectangleF rect, string type, float damage, float size, float speed, float penetration, bool bounce, float dirX, float dirY, bool contact, float alpha)
        {
            Bullets.Add(new Bullet()
            {
                Type = type,
                Rect = rect,
                Damage = damage,
                Size = size,
                Speed = speed,
                Penetration = penetration,
                Bounce =  bounce,
                DirX = dirX,
                DirY = dirY,
                Contact = contact,
                Alpha = alpha,
            });
        }

        public static int Bullet_Delay;

        public static bool MovementEnabled;

        public static int MoveTick;


        public static async void ProjectileMovementThread()
        {
            while (MovementEnabled)
            {
                if (GraphicsDrawing.FreezeState == false)
                {
                    try
                    {
                        foreach (Bullet Bullet in Bullets.ToList())
                        {
                            if (!new RectangleF(-300, -300, UIElements.MapSize.Height + 600, UIElements.MapSize.Width + 600).Contains(Bullet.Rect))
                            {
                                lock (Bullets)
                                {
                                    Bullets.Remove(Bullet);
                                    Bullets.TrimExcess();
                                }
                            }

                            if (Weapons.Gun == Weapons.Guns.Launcher)
                            {
                                if (Bullet.Contact == true)
                                {
                                    lock (Bullets)
                                    {
                                        //ensures movement still has a value but not enough to move.
                                        Bullet.DirX = 0.00000000001f;
                                        Bullet.DirY = 0.00000000001f;
                                        Bullet.Size += 1f;
                                        Bullet.Rect = new RectangleF(Bullet.Rect.X - 0.5f, Bullet.Rect.Y - 0.5f, Bullet.Rect.Width, Bullet.Rect.Height);

                                        if (Bullet.Size > 200)
                                        {
                                            Bullets.Remove(Bullet);
                                            Bullets.TrimExcess();
                                        }
                                    }
                                }
                            }

                            if (Bullet.DirX == 0 && Bullet.DirY == 0)
                            {
                                lock (Bullets)
                                {
                                    Bullets.Remove(Bullet);
                                    Bullets.TrimExcess();
                                }
                            }

                            //Update bullets to next location
                            Bullet.Rect = new RectangleF(Bullet.Rect.X + Bullet.DirX, Bullet.Rect.Y + Bullet.DirY, Bullet.Size, Bullet.Size);
                        }

                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                    //Delay sometime effected by cpu can be longer than intended
                    //Causes bullet speed problems
                    Thread.Sleep(1);

                    //Loop Delay
                    //Game.Delay(MoveTick);
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }
        //  public static Random rg = new Random();
        //
        //  public static PointF RandomVector(float speed)
        //  {
        //      float rangle = (float)((rg.NextDouble() * Math.PI));
        //
        //      speed *= (float)rg.NextDouble();
        //
        //      return new PointF((float)Math.Cos(rangle) * speed, (float)Math.Sin(rangle) * speed);
        //  }
        //
        //  public static PointF NewVector(float speed, float angle)
        //  {
        //      float rangle = (float)(angle * Math.PI);
        //
        //      speed *= angle;
        //
        //      return new PointF((float)Math.Cos(rangle) * speed, (float)Math.Sin(rangle) * speed);
        //  }
    }
}

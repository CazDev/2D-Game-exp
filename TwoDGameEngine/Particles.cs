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
    class Particles
    {

        public static List<XPParticle> XPParticles = new List<XPParticle>();

        public class XPParticle
        {
            public Color Colour { get; set; }
            public float Alpha { get; set; }
            public RectangleF Rect { get; set; }
            public float Size { get; set; }
            public float Speed { get; set; }
        }

        public static void AddXP(Color colour, float alpha, RectangleF rect, float size, float speed)
        {
            lock (XPParticles)
            {
                XPParticles.Add(new XPParticle()
                {
                    Colour = colour,
                    Alpha = alpha,
                    Rect = rect,
                    Size = size,
                    Speed = speed,
                });
            }
        }

        public static List<DmgNum> DmgNums = new List<DmgNum>();

        public class DmgNum
        {
            public Color Colour { get; set; }
            public float Alpha { get; set; }
            public float Dmg { get; set; }
            public float FontSize { get; set; }
            public PointF Loc { get; set; }
        }

        public static Stopwatch DmgNumCoolDown = new Stopwatch();

        public static void AddDmgNum(Color colour, float alpha, PointF loc, float fontsize, float dmg)
        {
            if (DmgNumCoolDown.ElapsedMilliseconds > 80)
            {
                lock (DmgNums)
                {
                    DmgNums.Add(new DmgNum()
                    {
                        Colour = colour,
                        Dmg = dmg,
                        Alpha = alpha,
                        FontSize = fontsize,
                        Loc = loc,
                    });
                }

                DmgNumCoolDown.Reset();
            }
        }

        public static List<BloodParticle> BloodParticles = new List<BloodParticle>();

        public class BloodParticle
        {
            public Color Colour { get; set; }
            public float Alpha { get; set; }
            public RectangleF Rect { get; set; }
            public float DirX { get; set; }
            public float DirY { get; set; }
        }

        public static void AddBlood(Color colour, float alpha, RectangleF rect, float dirX, float dirY)
        {
            lock (BloodParticles)
            {
                BloodParticles.Add(new BloodParticle()
                {
                    Colour = colour,
                    Alpha = alpha,
                    Rect = rect,
                    DirX = dirX,
                    DirY = dirY
                });
            }
        }

        public static async void DamageBloodMovement()
        {

            while (GraphicsDrawing.Running)
            {
                if (GraphicsDrawing.FreezeState == false)
                {
                    lock (BloodParticles)
                    {
                        foreach (BloodParticle Blood in BloodParticles.ToList())
                        {
                            Random rand = new Random();

                            Blood.Rect = new RectangleF(Blood.Rect.X + Blood.DirX * Projectiles.BloodSpeedMultiplier, Blood.Rect.Y + Blood.DirY * Projectiles.BloodSpeedMultiplier, Blood.Rect.Width, Blood.Rect.Height);

                            if (Blood.Alpha <= 10)
                            {
                                BloodParticles.Remove(Blood);
                                BloodParticles.TrimExcess();
                            }
                            else
                            {
                                Blood.Alpha -= Projectiles.BloodDecayRate;
                            }

                            Blood.Colour = Color.FromArgb((int)Blood.Alpha, Blood.Colour);
                        }
                    }

                    lock (DmgNums)
                    {
                        foreach (DmgNum dmgnum in DmgNums.ToList())
                        {
                            dmgnum.Loc = new PointF(dmgnum.Loc.X, dmgnum.Loc.Y - 2f);
                            dmgnum.Alpha -= 2;
                            if (dmgnum.Alpha < 10)
                            {
                                DmgNums.Remove(dmgnum);
                                DmgNums.TrimExcess();
                            }
                        }
                    }

                    lock (XPParticles)
                    {
                        foreach (XPParticle xp in XPParticles.ToList())
                        {
                            //XP movement
                            float x = Player.XP - UIElements.GraphicsOffset_X - xp.Size / 2;
                            float y = 84 - UIElements.GraphicsOffset_Y - xp.Size / 2;

                            float dx = x - xp.Rect.X;
                            float dy = y - xp.Rect.Y;
                            double length = Math.Sqrt(dx * dx + dy * dy);

                            if (length >= xp.Speed)
                            {
                                //Move towards the destination
                                xp.Rect = new RectangleF((float)(xp.Rect.X + xp.Speed * dx / length),
                                    (float)(xp.Rect.Y + xp.Speed * dy / length), xp.Size, xp.Size);
                            }
                            else
                            {
                                XPParticles.Remove(xp);
                                XPParticles.TrimExcess();
                            }

                        }
                    }

                    await Task.Delay(1);
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }

    }
}

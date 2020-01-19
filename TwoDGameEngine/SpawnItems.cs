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

namespace TwoDGameEngine
{
    class SpawnItems
    {
        public static int ItemSpawnRate = 1000;

        public static List<PowerUp> PowerUps = new List<PowerUp>();

        public class PowerUp
        {
            public string Type { get; set; }
            public RectangleF Rect { get; set; }
            public float Lifetime { get; set; }
            public Color Colour { get; set; }
            public float Duration { get; set; }
            public float Alpha { get; set; }
        }

        public static void AddPowerUp(string type, RectangleF rect, float lifetime, float duration, float alpha)
        {
            PowerUps.Add(new PowerUp()
            {
                Type = type,
                Rect = rect,
                Lifetime = lifetime,
                Duration = duration,
                Alpha = alpha,
            });
        }

        public static List<Item> Items = new List<Item>();

        public class Item
        {
            public string Type { get; set; }
            public RectangleF Rect { get; set; }
            public float Lifetime { get; set; }
            public float Alpha { get; set; }
        }

        public static void AddItem(string type, RectangleF rect, float lifetime, float alpha)
        {
            PowerUps.Add(new PowerUp()
            {
                Type = type,
                Rect = rect,
                Lifetime = lifetime,
                Alpha = alpha,
            });
        }

        public static async void GenerateItem()
        {
            while (true)
            {
                if (!UIElements.DrawEscapeMenu)
                {
                    {
                        lock (Items)
                        {
                            Random rand = new Random();

                            AddItem("AutoPistol", new RectangleF(rand.Next(0, UIElements.MapSize.Width), rand.Next(0, UIElements.MapSize.Height), 32, 32), 8, 255);
                        }

                        lock (PowerUps)
                        {
                            Random rand = new Random();

                            AddPowerUp("Speed", new RectangleF(rand.Next(0, UIElements.MapSize.Width), rand.Next(0, UIElements.MapSize.Height), 32, 32), 8f, 10, 255f);

                            AddPowerUp("HP", new RectangleF(rand.Next(0, UIElements.MapSize.Width), rand.Next(0, UIElements.MapSize.Height), 32, 32), 8f, 10, 255f);

                            AddPowerUp("XPBoost", new RectangleF(rand.Next(0, UIElements.MapSize.Width), rand.Next(0, UIElements.MapSize.Height), 32, 32), 8f, 5, 255f);

                            AddPowerUp("Defence", new RectangleF(rand.Next(0, UIElements.MapSize.Width), rand.Next(0, UIElements.MapSize.Height), 32, 32), 8f, 10, 255f);
                        }

                        await Task.Delay(ItemSpawnRate);
                    }

                    foreach (PowerUp powerup in PowerUps.ToList())
                    {
                        if (powerup.Lifetime >= 0)
                        {
                            powerup.Lifetime -= 1;
                        }
                        else
                        {
                            lock (PowerUps.ToList())
                            {
                                SpawnItems.PowerUps.Remove(powerup);
                                SpawnItems.PowerUps.TrimExcess();
                            }
                        }
                    }
                }
            }
        }

        public static void PowerUpCollision()
        {
            try
            {
                //Speed up powerup collision
                foreach (SpawnItems.PowerUp powerup in SpawnItems.PowerUps.ToList())
                {
                    if (Player.PlayerObj.IntersectsWith(new RectangleF(powerup.Rect.X + UIElements.GraphicsOffset_X, powerup.Rect.Y + UIElements.GraphicsOffset_Y, 48, 48)))
                    {

                        lock (PowerUps)
                        {
                            //duration power ups
                            if (powerup.Type == "Speed")
                            {
                                Player.Speed += 0.1f; Console.WriteLine("speedup");
                            }
                            //no duration power ups
                            if (powerup.Type == "HP")
                            {
                                if (Player.HP >= Player.HP + Player.HP * 0.05f)
                                {
                                    Player.HP += Player.HP * 0.05f;
                                }
                                else
                                {
                                    Player.HP = Player.MaxHP;
                                }
                            }
                        }

                        lock (PowerUps)
                        {
                            SpawnItems.PowerUps.Remove(powerup);
                            SpawnItems.PowerUps.TrimExcess();
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public static void ItemCollision()
        {
            //Speed up powerup collision
            foreach (SpawnItems.Item item in SpawnItems.Items.ToList())
            {
                if (Player.PlayerObj.IntersectsWith(new RectangleF(item.Rect.X + UIElements.GraphicsOffset_X, item.Rect.Y + UIElements.GraphicsOffset_Y, 32, 32)))
                {
                    if (item.Type == "AutoPistol") { Weapons.Gun = Weapons.Guns.AutoPistol; Weapons.EquipGun(Weapons.Guns.AutoPistol); }

                    lock (Items)
                    {
                        SpawnItems.Items.Remove(item);
                        SpawnItems.Items.TrimExcess();
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace TwoDGameEngine
{
    class Weapons
    {
        public static Throwables Throwable = Throwables.Grenade;
        public static Passives Passive = Passives.AOE;

        public static int Ammo = 0;
        public static int MaxAmmo = 0;

        public enum Guns
        {
            None,
            Pistol,
            AutoPistol,
            SMG,
            Rifle,
            Sniper,
            MiniGun,
            Launcher,
            LaserGun,
        };

        private static Guns gun;

        public static Guns Gun
        {
            get { return gun; }

            set
            {
                gun = value;
                //Guns enumeration has been set
                Ammo = 0;
            }
        }

        public enum Throwables
        {
            Grenade,
            FireBomb,
            BlackHole,
        };

        public enum Passives
        {
            AOE,
            Pulse,
            AutoTurret,
        };

        public class GunReloadTime
        {
            public static int Pistol = 100;
            public static int AutoPistol = 200;
            public static int SMG = 500;
            public static int Rifle = 500;
            public static int Sniper = 800;
            public static int MiniGun = 1500;
            public static int Launcher = 600;
            public static int LaserCannon = 1000;
        }

        public class Inventory
        {
            public static int SlotsHighlighted = 0;
            public static int SlotsHighlightedLength = 0;
            public static int SlotsTaken = 0;

            public static int Pistol = 1;
            public static int AutoPistol = 1;
            public static int SMG = 1;
            public static int Rifle = 3;
            public static int Sniper = 4;
            public static int MiniGun = 5;
            public static int Launcher = 5;
            public static int LaserCannon = 6;

            public static List<Slot> Slots = new List<Slot>();

            public class Slot
            {
                public Weapons.Guns Gun { get; set; }
                public int GunSlot { get; set; }
                public int SlotLength { get; set; }
            }

            public static void AddSlot(Weapons.Guns gun, int slot, int slotlength)
            {
                Slots.Add(new Slot()
                {
                    Gun = gun,
                    GunSlot = slot,
                    SlotLength = slotlength
                });
            }

        }

        public static void EquipGun(Weapons.Guns gun)
        {
            if (gun == Guns.Pistol)
            {
                Weapons.Gun = Weapons.Guns.Pistol;

                Weapons.Inventory.AddSlot(Weapons.Guns.Pistol, Weapons.Inventory.SlotsTaken, Weapons.Inventory.Pistol);
                Weapons.Inventory.SlotsHighlightedLength = Weapons.Inventory.Pistol;
                Weapons.Inventory.SlotsHighlighted = 0;
                Weapons.Inventory.SlotsTaken += Weapons.Inventory.Pistol;
            }

            if (gun == Guns.AutoPistol)
            {
                Weapons.Gun = Weapons.Guns.AutoPistol;

                Weapons.Inventory.AddSlot(Weapons.Guns.AutoPistol, Weapons.Inventory.SlotsTaken, Weapons.Inventory.AutoPistol);
                Weapons.Inventory.SlotsHighlightedLength = Weapons.Inventory.AutoPistol;
                Weapons.Inventory.SlotsHighlighted = 0;
                Weapons.Inventory.SlotsTaken += Weapons.Inventory.AutoPistol;
            }

            if (gun == Guns.SMG)
            {
                Weapons.Gun = Weapons.Guns.SMG;

                Weapons.Inventory.AddSlot(Weapons.Guns.SMG, Weapons.Inventory.SlotsTaken, Weapons.Inventory.SMG);
                Weapons.Inventory.SlotsHighlightedLength = Weapons.Inventory.SMG;
                Weapons.Inventory.SlotsHighlighted = 0;
                Weapons.Inventory.SlotsTaken += Weapons.Inventory.SMG;
            }

            if (gun == Guns.Rifle)
            {
                Weapons.Gun = Weapons.Guns.Rifle;

                Weapons.Inventory.AddSlot(Weapons.Guns.Rifle, Weapons.Inventory.SlotsTaken, Weapons.Inventory.Rifle);
                Weapons.Inventory.SlotsHighlightedLength = Weapons.Inventory.Rifle;
                Weapons.Inventory.SlotsHighlighted = 0;
                Weapons.Inventory.SlotsTaken += Weapons.Inventory.Rifle;
            }

            if (gun == Guns.Sniper)
            {
                Weapons.Gun = Weapons.Guns.Sniper;

                Weapons.Inventory.AddSlot(Weapons.Guns.Sniper, Weapons.Inventory.SlotsTaken, Weapons.Inventory.Sniper);
                Weapons.Inventory.SlotsHighlightedLength = Weapons.Inventory.Sniper;
                Weapons.Inventory.SlotsHighlighted = 0;
                Weapons.Inventory.SlotsTaken += Weapons.Inventory.Sniper;
            }

            if (gun == Guns.Launcher)
            {
                Weapons.Gun = Weapons.Guns.Launcher;

                Weapons.Inventory.AddSlot(Weapons.Guns.Launcher, Weapons.Inventory.SlotsTaken, Weapons.Inventory.Launcher);
                Weapons.Inventory.SlotsHighlightedLength = Weapons.Inventory.Launcher;
                Weapons.Inventory.SlotsHighlighted = 0;
                Weapons.Inventory.SlotsTaken += Weapons.Inventory.Launcher;
            }

            if (gun == Guns.MiniGun)
            {
                Weapons.Gun = Weapons.Guns.MiniGun;

                Weapons.Inventory.AddSlot(Weapons.Guns.MiniGun, Weapons.Inventory.SlotsTaken, Weapons.Inventory.MiniGun);
                Weapons.Inventory.SlotsHighlightedLength = Weapons.Inventory.MiniGun;
                Weapons.Inventory.SlotsHighlighted = 0;
                Weapons.Inventory.SlotsTaken += Weapons.Inventory.MiniGun;
            }

            if (gun == Guns.LaserGun)
            {
                Weapons.Gun = Weapons.Guns.LaserGun;

                Weapons.Inventory.AddSlot(Weapons.Guns.LaserGun, Weapons.Inventory.SlotsTaken, Weapons.Inventory.LaserCannon);
                Weapons.Inventory.SlotsHighlightedLength = Weapons.Inventory.LaserCannon;
                Weapons.Inventory.SlotsHighlighted = 0;
                Weapons.Inventory.SlotsTaken += Weapons.Inventory.LaserCannon;
            }
        }

        public static float DirX = 0f;
        public static float DirY = 0f;

        public static async void Shooting()
        {
            while (GraphicsDrawing.Running)
            {
                if (GraphicsDrawing.FreezeState == false)
                {
                    while (Player.Shooting)
                    {
                        if (Weapons.Gun != Weapons.Guns.None)
                        {
                            DirX = 0f;
                            DirY = 0f;

                            //Sets direction for the shot bullet
                            if (Projectiles.Dir_Up)
                            {
                                DirY = -Projectiles.Bullet_Speed;
                            }

                            if (Projectiles.Dir_Right)
                            {
                                DirX = Projectiles.Bullet_Speed;
                            }

                            if (Projectiles.Dir_Down)
                            {
                                DirY = Projectiles.Bullet_Speed;
                            }

                            if (Projectiles.Dir_Left)
                            {
                                DirX = -Projectiles.Bullet_Speed;
                            }

                            if (Weapons.Ammo > 0)
                            {
                                Weapons.Ammo--;
                            }


                            if (Weapons.Gun == Weapons.Guns.Pistol)
                            {
                                Projectiles.Bullet_Size = 7f;
                                Projectiles.Bullet_Speed = 1f * 2;
                                Projectiles.Bullet_Damage = 80f;
                                Projectiles.Bullet_Penetration = 0.2f;
                                Projectiles.Bullet_Delay = 300;

                                Weapons.MaxAmmo = 7;
                                lock (Projectiles.Bullets)
                                {
                                    if (Weapons.Ammo > 0)
                                    {
                                        Projectiles.AddBullet(new RectangleF(Convert.ToInt16(Player.Location.X + Player.Size / 3) - UIElements.GraphicsOffset_X, Convert.ToInt16(Player.Location.Y + Player.Size / 3) - UIElements.GraphicsOffset_Y, Projectiles.Bullet_Size, Projectiles.Bullet_Size), "Pistol", Projectiles.Bullet_Damage, Projectiles.Bullet_Size, Projectiles.Bullet_Speed, Projectiles.Bullet_Penetration, false, DirX, DirY, false, 255);
                                        UIElements.ShakeScreen(3);
                                    }
                                }

                                if (Weapons.Ammo <= 0)
                                {
                                    Game.Delay(Weapons.GunReloadTime.Pistol);
                                    Weapons.Ammo = Weapons.MaxAmmo;
                                }
                            }
                            else if (Weapons.Gun <= Weapons.Guns.AutoPistol)
                            {
                                Projectiles.Bullet_Size = 7f;
                                Projectiles.Bullet_Speed = 1f * 2;
                                Projectiles.Bullet_Damage = 50f;
                                Projectiles.Bullet_Penetration = 1f;
                                Projectiles.Bullet_Delay = 100;

                                Weapons.MaxAmmo = 15;
                                lock (Projectiles.Bullets)
                                    if (Weapons.Ammo > 0)
                                    {
                                        {
                                            Projectiles.AddBullet(new RectangleF(Convert.ToInt16(Player.Location.X + Player.Size / 3) - UIElements.GraphicsOffset_X, Convert.ToInt16(Player.Location.Y + Player.Size / 3) - UIElements.GraphicsOffset_Y, Projectiles.Bullet_Size, Projectiles.Bullet_Size), "AutoPistol", Projectiles.Bullet_Damage, Projectiles.Bullet_Size, Projectiles.Bullet_Speed, Projectiles.Bullet_Penetration, false, DirX, DirY, false, 255);
                                            UIElements.ShakeScreen(3);
                                        }
                                    }

                                if (Weapons.Ammo <= 0)
                                {
                                    Game.Delay(Weapons.GunReloadTime.AutoPistol);
                                    Weapons.Ammo = Weapons.MaxAmmo;
                                }
                            }
                            else if (Weapons.Gun == Weapons.Guns.SMG)
                            {
                                Projectiles.Bullet_Size = 8f;
                                Projectiles.Bullet_Speed = 2f * 2;
                                Projectiles.Bullet_Damage = 26f;
                                Projectiles.Bullet_Penetration = 1.8f;
                                Projectiles.Bullet_Delay = 50;

                                Weapons.MaxAmmo = 30;
                                lock (Projectiles.Bullets)
                                {
                                    if (Weapons.Ammo > 0)
                                    {
                                        Projectiles.AddBullet(new RectangleF(Convert.ToInt16(Player.Location.X + Player.Size / 3) - UIElements.GraphicsOffset_X, Convert.ToInt16(Player.Location.Y + Player.Size / 3) - UIElements.GraphicsOffset_Y, Projectiles.Bullet_Size, Projectiles.Bullet_Size), "SMG", Projectiles.Bullet_Damage, Projectiles.Bullet_Size, Projectiles.Bullet_Speed, Projectiles.Bullet_Penetration, false, DirX, DirY, false, 255);
                                        UIElements.ShakeScreen(5);
                                    }
                                }

                                if (Weapons.Ammo <= 0)
                                {
                                    Game.Delay(Weapons.GunReloadTime.SMG);
                                    Weapons.Ammo = Weapons.MaxAmmo;
                                }
                            }
                            else if (Weapons.Gun == Weapons.Guns.Rifle)
                            {
                                Projectiles.Bullet_Size = 12f;
                                Projectiles.Bullet_Speed = 3f * 2;
                                Projectiles.Bullet_Damage = 45f;
                                Projectiles.Bullet_Penetration = 3f;
                                Projectiles.Bullet_Delay = 90;

                                Weapons.MaxAmmo = 20;
                                lock (Projectiles.Bullets)
                                {
                                    if (Weapons.Ammo > 0)
                                    {
                                        Projectiles.AddBullet(new RectangleF(Convert.ToInt16(Player.Location.X + Player.Size / 3) - UIElements.GraphicsOffset_X, Convert.ToInt16(Player.Location.Y + Player.Size / 3) - UIElements.GraphicsOffset_Y, Projectiles.Bullet_Size, Projectiles.Bullet_Size), "Rifle", Projectiles.Bullet_Damage, Projectiles.Bullet_Size, Projectiles.Bullet_Speed, Projectiles.Bullet_Penetration, false, DirX, DirY, false, 255);
                                        UIElements.ShakeScreen(8);
                                    }
                                }

                                if (Weapons.Ammo <= 0)
                                {
                                    Game.Delay(Weapons.GunReloadTime.Rifle);
                                    Weapons.Ammo = Weapons.MaxAmmo;
                                }
                            }
                            else if (Weapons.Gun == Weapons.Guns.Sniper)
                            {
                                Projectiles.Bullet_Size = 14f;
                                Projectiles.Bullet_Speed = 3f * 2;
                                Projectiles.Bullet_Damage = 80f;
                                Projectiles.Bullet_Penetration = 20f;
                                Projectiles.Bullet_Delay = 1000;

                                Weapons.MaxAmmo = 10;
                                lock (Projectiles.Bullets)
                                {
                                    if (Weapons.Ammo > 0)
                                    {
                                        Projectiles.AddBullet(new RectangleF(Convert.ToInt16(Player.Location.X + Player.Size / 3) - UIElements.GraphicsOffset_X, Convert.ToInt16(Player.Location.Y + Player.Size / 3) - UIElements.GraphicsOffset_Y, Projectiles.Bullet_Size, Projectiles.Bullet_Size), "Sniper", Projectiles.Bullet_Damage, Projectiles.Bullet_Size, Projectiles.Bullet_Speed, Projectiles.Bullet_Penetration, false, DirX, DirY, false, 255);
                                        UIElements.ShakeScreen(22);
                                    }
                                }

                                if (Weapons.Ammo <= 0)
                                {
                                    Game.Delay(Weapons.GunReloadTime.Sniper);
                                    Weapons.Ammo = Weapons.MaxAmmo;
                                }
                            }
                            else if (Weapons.Gun == Weapons.Guns.MiniGun)
                            {
                                Projectiles.Bullet_Size = 9f;
                                Projectiles.Bullet_Speed = 1.3f * 2;
                                Projectiles.Bullet_Damage = 35f;
                                Projectiles.Bullet_Penetration = 0.5f;
                                Projectiles.Bullet_Delay = 1;

                                Weapons.MaxAmmo = 100;
                                lock (Projectiles.Bullets)
                                {
                                    if (Weapons.Ammo > 0)
                                    {
                                        Projectiles.AddBullet(new RectangleF(Convert.ToInt16(Player.Location.X + Player.Size / 3) - UIElements.GraphicsOffset_X, Convert.ToInt16(Player.Location.Y + Player.Size / 3) - UIElements.GraphicsOffset_Y, Projectiles.Bullet_Size, Projectiles.Bullet_Size), "MiniGun", Projectiles.Bullet_Damage, Projectiles.Bullet_Size, Projectiles.Bullet_Speed, Projectiles.Bullet_Penetration, false, DirX, DirY, false, 255);
                                        UIElements.ShakeScreen(8);
                                    }
                                }

                                if (Weapons.Ammo <= 0)
                                {
                                    Game.Delay(Weapons.GunReloadTime.MiniGun);
                                    Weapons.Ammo = Weapons.MaxAmmo;
                                }
                            }
                            else if (Weapons.Gun == Weapons.Guns.Launcher)
                            {
                                Projectiles.Bullet_Size = 15f;
                                Projectiles.Bullet_Speed = 1.2f * 2;
                                Projectiles.Bullet_Damage = 15f;
                                Projectiles.Bullet_Penetration = 50f;
                                Projectiles.Bullet_Delay = 750;

                                Weapons.MaxAmmo = 10;
                                lock (Projectiles.Bullets)
                                {
                                    if (Weapons.Ammo > 0)
                                    {
                                        Projectiles.AddBullet(new RectangleF(Convert.ToInt16(Player.Location.X + Player.Size / 3) - UIElements.GraphicsOffset_X, Convert.ToInt16(Player.Location.Y + Player.Size / 3) - UIElements.GraphicsOffset_Y, Projectiles.Bullet_Size, Projectiles.Bullet_Size), "MiniGun", Projectiles.Bullet_Damage, Projectiles.Bullet_Size, Projectiles.Bullet_Speed, Projectiles.Bullet_Penetration, false, DirX, DirY, false, 255);
                                        UIElements.ShakeScreen(8);
                                    }
                                }


                                if (Weapons.Ammo <= 0)
                                {
                                    Game.Delay(Weapons.GunReloadTime.Launcher);
                                    Weapons.Ammo = Weapons.MaxAmmo;
                                }
                            }
                            else if (Weapons.Gun == Weapons.Guns.LaserGun)
                            {
                                Projectiles.Bullet_Size = 8f;
                                Projectiles.Bullet_Speed = 4f * 2;
                                Projectiles.Bullet_Damage = 5f;
                                Projectiles.Bullet_Penetration = 17f;
                                Projectiles.Bullet_Delay = 1;

                                Weapons.MaxAmmo = 60;

                                lock (Projectiles.Bullets)
                                {
                                    if (Weapons.Ammo > 0)
                                    {
                                        Projectiles.AddBullet(new RectangleF(Convert.ToInt16(Player.Location.X + Player.Size / 3) - UIElements.GraphicsOffset_X, Convert.ToInt16(Player.Location.Y + Player.Size / 3) - UIElements.GraphicsOffset_Y, Projectiles.Bullet_Size, Projectiles.Bullet_Size), "MiniGun", Projectiles.Bullet_Damage, Projectiles.Bullet_Size, Projectiles.Bullet_Speed, Projectiles.Bullet_Penetration, false, DirX, DirY, false, 255);
                                        UIElements.ShakeScreen(1);
                                    }
                                }

                                if (Weapons.Ammo <= 0)
                                {
                                    Game.Delay(Weapons.GunReloadTime.LaserCannon);
                                    Weapons.Ammo = Weapons.MaxAmmo;
                                }
                            }

                            //Delay between bullet shots
                            await Task.Delay(Projectiles.Bullet_Delay);
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
}

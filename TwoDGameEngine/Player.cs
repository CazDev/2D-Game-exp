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
    class Player
    {
        //Direction
        public static bool Dir_None = true;
        public static bool Dir_Left = false;
        public static bool Dir_Right = false;
        public static bool Dir_Up = false;
        public static bool Dir_Down = false;

        //Player location, speed and size
        public static PointF Location;
        public static float Speed;
        public static float DmgIncrease;
        public static float StartSpeed;
        public static float Size;
        public static float MaxSpeed;
        public static float Defence;
        public static float MaxHP;
        public static float HP;
        public static float Vector;
        public static float AccelValue;
        public static float DamageMultiplier;

        public static float XP;
        public static float MaxXP;
        public static float XPMultiplier;
        public static int Level = 1;
        public static int SkillPoints = 0;

        public static bool MovementEnabled;

        public static int MoveTick;

        public static int Score;

        public static bool Shooting;

        public static RectangleF PlayerObj;

        private static float dx;
        private static float dy;
        private static double length;

        public static void MoveTo(float x, float y, float speed)
        {
            dx = x - PlayerObj.X;
            dy = y - PlayerObj.Y;
            length = Math.Sqrt(dx * dx + dy * dy);

            if (length > speed)
            {
                //Move towards the destination
                PlayerObj.X = (float)(PlayerObj.X + speed * dx / length);
                PlayerObj.Y = (float)(PlayerObj.Y + speed * dy / length);
            }
            else
            {
                //Destination reached
            }
        }

        //   public static PointF NewVector(float speed, float angle)
        //   {
        //       float rangle = (float)(angle * Math.PI);
        //
        //       speed *= angle;
        //
        //       return new PointF((float)Math.Cos(rangle) * speed, (float)Math.Sin(rangle) * speed);
        //   }


        public static async void MovementThread()
        {
            while (Player.MovementEnabled)
            {
                if (GraphicsDrawing.FreezeState == false)
                {
                    //Check collition with Zone
                    if (Player.PlayerObj.IntersectsWith(UIElements.ZoneObj))
                    {
                        Player.Score++;
                    }

                    //Ensures diagonal and normal movement is even
                    //Otherwise diagonal movement would be slightly faster
                    bool DirDiagonal = false;
                    if (Player.Dir_Left && Player.Dir_Up) { DirDiagonal = true; }
                    else if (Player.Dir_Up && Player.Dir_Right) { DirDiagonal = true; }
                    else if (Player.Dir_Right && Player.Dir_Down) { DirDiagonal = true; }
                    else if (Player.Dir_Down && Player.Dir_Left) { DirDiagonal = true; } else { DirDiagonal = false; }


                    //Left Border collision
                    if (Player.Dir_Left)
                    {
                        if (Player.PlayerObj.IntersectsWith(UIElements.BorderLeft_Obj) == false)
                        {
                            //Player movement
                            if (DirDiagonal)
                            {
                                Player.Location.X -= Player.Speed / 1.5f;

                                //Camera movement
                                UIElements.GraphicsOffset_X += Player.Speed / 1.5f * UIElements.CameraSpeed;
                            }
                            else
                            {
                                Player.Location.X -= Player.Speed;

                                //Camera movement
                                UIElements.GraphicsOffset_X += Player.Speed * UIElements.CameraSpeed;
                            }
                        }
                    }
                    //Right Border collision
                    if (Player.Dir_Right)
                    {

                        if (Player.PlayerObj.IntersectsWith(UIElements.BorderRight_Obj) == false)
                        {
                            //Player movement
                            if (DirDiagonal)
                            {
                                Player.Location.X += Player.Speed / 1.5f;

                                //Camera movement
                                UIElements.GraphicsOffset_X -= Player.Speed / 1.5f * UIElements.CameraSpeed;
                            }
                            else
                            {
                                Player.Location.X += Player.Speed;

                                //Camera movement
                                UIElements.GraphicsOffset_X -= Player.Speed * UIElements.CameraSpeed;
                            }
                        }
                    }
                    //Top Border collision
                    if (Player.Dir_Up)
                    {

                        if (Player.PlayerObj.IntersectsWith(UIElements.BorderTop_Obj) == false)
                        {
                            if (DirDiagonal)
                            {
                                Player.Location.Y -= Player.Speed / 1.5f;

                                //Camera movement
                                UIElements.GraphicsOffset_Y += (Player.Speed / 1.5f * UIElements.CameraSpeed);
                            }
                            else
                            {
                                Player.Location.Y -= Player.Speed;

                                //Camera movement
                                UIElements.GraphicsOffset_Y += (Player.Speed * UIElements.CameraSpeed);
                            }
                        }
                    }
                    //Bottom Border collision
                    if (Player.Dir_Down)
                    {

                        if (Player.PlayerObj.IntersectsWith(UIElements.BorderBottom_Obj) == false)
                        {
                            //Player movement
                            if (DirDiagonal)
                            {
                                Player.Location.Y += Player.Speed / 1.5f;

                                //Camera movement
                                UIElements.GraphicsOffset_Y -= (Player.Speed / 1.5f * UIElements.CameraSpeed);
                            }
                            else
                            {
                                Player.Location.Y += Player.Speed;

                                //Camera movement
                                UIElements.GraphicsOffset_Y -= (Player.Speed * UIElements.CameraSpeed);
                            }
                        }
                    }

                    //Rotates player
                    if (Player.Dir_Down && Player.Dir_Right)
                    {
                        Player.Vector += Player.Speed * 3.5f;
                    }
                    else if (Player.Dir_Down && Player.Dir_Left)
                    {
                        Player.Vector -= Player.Speed * 3.5f;
                    }
                    else if (Player.Dir_Up && Player.Dir_Right)
                    {
                        Player.Vector += Player.Speed * 3.5f;
                    }
                    else if (Player.Dir_Up && Player.Dir_Left)
                    {
                        Player.Vector -= Player.Speed * 3.5f;
                    }
                    else if (Player.Dir_Up)
                    {
                        Player.Vector += Player.Speed * 3.5f;
                    }
                    else if (Player.Dir_Down)
                    {
                        Player.Vector -= Player.Speed * 3.5f;
                    }
                    else if (Player.Dir_Left)
                    {
                        Player.Vector -= Player.Speed * 3.5f;
                    }
                    else if (Player.Dir_Right)
                    {
                        Player.Vector += Player.Speed * 3.5f;
                    }

                    SpawnItems.PowerUpCollision();

                    SpawnItems.ItemCollision();

                    //Loop Delay
                    await Task.Delay(Player.MoveTick);
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }
    }
}

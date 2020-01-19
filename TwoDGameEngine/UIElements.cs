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
using System.Diagnostics;

namespace TwoDGameEngine
{
    class UIElements
    {

        public static Size MapSize;

        public static Size ScreenSize;

        public static float ScreenAlpha = 0;

        public static bool DrawEscapeMenu;
        public static bool DrawSkillTree;
        public static bool DrawOptions;
        public static bool DrawStats;

        public static bool BarMenu_Draw;
        public static Brush BarMenu_Brush;

        public static int HPBarWidth = 300;

        public static bool Border_Draw;
        public static Brush Border_Brush;

        public static int BorderSize;

        public static float ZoomScreen = 1f;

        public static RectangleF BorderTop_Obj;
        public static RectangleF BorderRight_Obj;
        public static RectangleF BorderBottom_Obj;
        public static RectangleF BorderLeft_Obj;

        public static PointF Zone_Location;
        public static float Zone_Height;
        public static float Zone_Width;
        public static RectangleF ZoneObj;

        public static float GraphicsOffset_X;
        public static float GraphicsOffset_Y;

        public static float BackgroundParallax = 1.3f;

        public static float CameraSpeed = 4f;

        public static float HPBarOffset = -20;

        public static float HPBarHeight = 5;

        public static bool LevelUp = false;
        public static bool LevelUpBack = false;

        public static RectangleF MapRect = new RectangleF();

        public static Brush btnResumeCol = Brushes.CornflowerBlue;
        public static Brush btnSkillTreeCol = Brushes.CornflowerBlue;
        public static Brush btnOptionsCol = Brushes.CornflowerBlue;
        public static Brush btnExitCol = Brushes.CornflowerBlue;
        public static Brush btnStatsCol = Brushes.CornflowerBlue;

        public async static void ShakeScreen(int ShakeValue)
        {
            if (GraphicsDrawing.FreezeState == false)
            {
                Random rand = new Random();
                int RandShakeVal_X = rand.Next(1, ShakeValue);
                int RandShakeVal_Y = rand.Next(1, ShakeValue);

                GraphicsOffset_X += RandShakeVal_X;
                GraphicsOffset_Y += RandShakeVal_Y;

                await Task.Delay(20);

                GraphicsOffset_X -= RandShakeVal_X;
                GraphicsOffset_Y -= RandShakeVal_Y;

                await Task.Delay(200);
            }
        }

        public static void ZoomScreenAndFitUI()
        {
            //Screen zooming
            ScreenSize = Game.GameInstance.Size;

            ScreenSize.Width = (int)(ScreenSize.Width / ZoomScreen);
            ScreenSize.Height = (int)(ScreenSize.Height / ZoomScreen);
        }

        public static void RotateRectangle(Graphics g, RectangleF r, float angle)
        {
            using (Matrix m = new Matrix())
            {
                m.RotateAt(angle, new PointF(r.Left + (r.Width / 2),
                                          r.Top + (r.Height / 2)));
                g.Transform = m;
                g.FillRectangle(Brushes.Purple, r);
                g.ResetTransform();
            }
        }
    }
}

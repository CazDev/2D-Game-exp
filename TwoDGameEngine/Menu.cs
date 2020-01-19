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
    public partial class Menu : Form
    {

        public static Menu MenuInstance = new Menu();

        //Move form variables
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public static Brush BtnPlayCol = Brushes.Red;
        public static RectangleF BtnPlay = new RectangleF(MenuInstance.Width / 2 - 100, MenuInstance.Height / 2 - 50, 200, 100);

        public Menu()
        {
            InitializeComponent();
        }

        public void GEInvalidate()
        {
            while (true)
            {
                MainMenuBox.Invalidate();
                Thread.Sleep(1);
            }
        }

        private void MainMenuBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(70, 0, 0, 0)), new RectangleF(0, 0, MainMenuBox.Width, MainMenuBox.Height));

            e.Graphics.DrawString("PLAY" , new Font("Segoe UI", 50, FontStyle.Regular), Brushes.White, BtnPlay.X + 10, BtnPlay.Y);
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            Thread GEInvalT = new Thread(new ThreadStart(GEInvalidate));
            GEInvalT.Start();
        }

        private void MainMenuBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (BtnPlay.Contains(e.Location))
            {
                this.Hide();

                Game game = new Game();
                game.Show();

                GraphicsDrawing.Running = true;
                GraphicsDrawing.DrawGame = true;
                GraphicsDrawing.FreezeState = true;
                UIElements.DrawEscapeMenu = true;
            }
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
        }

        private void MainMenuBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (BtnPlay.Contains(e.Location))
            {
                BtnPlayCol = Brushes.Black;
            }
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

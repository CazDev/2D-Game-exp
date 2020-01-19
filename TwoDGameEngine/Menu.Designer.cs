namespace TwoDGameEngine
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Canvas = new System.Windows.Forms.PictureBox();
            this.MainMenuBox = new System.Windows.Forms.PictureBox();
            this.TopMenu = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(Canvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainMenuBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            Canvas.Location = new System.Drawing.Point(0, 0);
            Canvas.Name = "Canvas";
            Canvas.Size = new System.Drawing.Size(626, 352);
            Canvas.TabIndex = 0;
            Canvas.TabStop = false;
            // 
            // MainMenuBox
            // 
            this.MainMenuBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainMenuBox.Image = global::TwoDGameEngine.Properties.Resources.greenpentagons;
            this.MainMenuBox.Location = new System.Drawing.Point(0, 0);
            this.MainMenuBox.Name = "MainMenuBox";
            this.MainMenuBox.Size = new System.Drawing.Size(626, 352);
            this.MainMenuBox.TabIndex = 2;
            this.MainMenuBox.TabStop = false;
            this.MainMenuBox.Paint += new System.Windows.Forms.PaintEventHandler(this.MainMenuBox_Paint);
            this.MainMenuBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainMenuBox_MouseDown);
            this.MainMenuBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainMenuBox_MouseMove);
            // 
            // TopMenu
            // 
            this.TopMenu.BackColor = System.Drawing.Color.Gray;
            this.TopMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopMenu.Location = new System.Drawing.Point(0, 0);
            this.TopMenu.Name = "TopMenu";
            this.TopMenu.Size = new System.Drawing.Size(626, 30);
            this.TopMenu.TabIndex = 3;
            this.TopMenu.TabStop = false;
            this.TopMenu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TopMenu_MouseDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox1.Location = new System.Drawing.Point(0, 347);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(626, 5);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 352);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.TopMenu);
            this.Controls.Add(this.MainMenuBox);
            this.Controls.Add(Canvas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Menu";
            this.Text = "Menu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Menu_FormClosing);
            this.Load += new System.EventHandler(this.Menu_Load);
            ((System.ComponentModel.ISupportInitialize)(Canvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainMenuBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.PictureBox MainMenuBox;
        private System.Windows.Forms.PictureBox TopMenu;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
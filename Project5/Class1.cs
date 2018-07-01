using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Project5
{
    enum choix
    {
        platformblanche, nul
    }

    class Fenetre : Form
    {
        Button fermer = new Button();
        private Color[,] cam = new Color[150, 270];
        private Color[,] map = new Color[1000, 2000];
        private int posx = 10;
        private int posy = 350;
        private SplitContainer grille = new SplitContainer();
        private Button plateformblanche = new Button();
        private bool bol, boool;
        choix choix =  choix.nul;

        public Fenetre()
        {

            fermer.Location = new Point(70, 30);
            BackColor = Color.FromArgb(53, 53, 53);
            grille.Dock = DockStyle.Fill;
            grille.Panel1MinSize = 1410;
            grille.IsSplitterFixed = true;

            plateformblanche.Size = new Size(200, 200);
            plateformblanche.Location = new Point(50, 50);
            plateformblanche.Text = "PLATEFORME BLANCHE";
            plateformblanche.ForeColor = Color.White;

            PictureBox pic = new PictureBox();

            pic.BackColor = Color.FromArgb(53, 53, 53);
            pic.Paint += new PaintEventHandler(chargerImage);
            pic.Dock = DockStyle.Fill;
            for (int i = 0; i < 1000; i++)
                for (int o = 0; o < 2000; o++)
                    map.SetValue(Color.Black, i, o);
            for (int i = 0; i < 150; i++)
                for (int o = 0; o < 270; o++)
                    cam.SetValue(map[i + posx, o + posy], i, o);
            SuspendLayout();
            Text = "Team Cumin's Game Engine";
            Size = new Size(1800, 1000);
            MinimumSize = new Size(1800, 1000);
            Icon = new Icon("Logo.ico");

            plateformblanche.MouseClick += PlateformeBlanche ;
            MouseClick += click;
            fermer.Click += fermerclick;

            grille.Panel1.Controls.Add(pic);
            grille.Panel2.Controls.Add(plateformblanche);
            Controls.Add(grille);
            ResumeLayout(false);
            PerformLayout();

        }

        private void chargerImage(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < 150; i++)
                for (int o = 0; o < 270; o++)
                {
                    Rectangle pixel = new Rectangle(50 + 5 * o, 50 + 5 * i, 10, 10);
                    SolidBrush n = new SolidBrush(cam[i, o]);
                    e.Graphics.FillRectangle(n, pixel);
                    Pen p = new Pen(Color.Black);
                    e.Graphics.DrawRectangle(p, pixel);
                    n.Dispose();
                }
        }

        static void Main()
        {
            Application.Run(new Fenetre());
        }

        private void PlateformeBlanche(object sender, EventArgs evt)
        {
            grille.Panel2.Controls.Add(fermer);
            choix = choix.platformblanche;
        }

        private void click(object e, EventArgs evt)
        {
            Console.WriteLine("vat'il marcheer");
            switch (choix)
            {
                case choix.platformblanche:
                    Console.WriteLine("mhf1");
                    if (MousePosition.X > Location.X + 50 && MousePosition.X < Location.X + 800 && MousePosition.Y > Location.Y + 50 && MousePosition.Y < Location.Y + 800)
                    {
                        map[MousePosition.X - Location.X + 50 + posx, MousePosition.Y - Location.Y + 50 + posy] = Color.White;
                        cam[MousePosition.X - Location.X + 50, MousePosition.Y - Location.Y + 50] = Color.White;
                        Console.WriteLine("mhf2");
                    }
                    break;
            }
        }




        private void fermerclick(object e, EventArgs evt)
        {
            choix = choix.nul;
            grille.Panel2.Controls.Remove(fermer);
        }
    }
}

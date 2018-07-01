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
        private choix choix =  choix.nul;
        private TrackBar bar = new TrackBar();
        private int tailleplatform = 0;
        private PictureBox pic = new PictureBox();
        private Label tailplatfrm = new Label();

        public Fenetre()
        {

            fermer.Location = new Point(190, 10);
            fermer.Text = "fermer";
            fermer.ForeColor = Color.Red;

            bar.Location = new Point(50, 300);

            BackColor = Color.FromArgb(53, 53, 53);
            grille.Dock = DockStyle.Fill;
            grille.Panel1MinSize = 1410;
            grille.IsSplitterFixed = true;

            plateformblanche.Size = new Size(200, 200);
            plateformblanche.Location = new Point(50, 50);
            plateformblanche.Text = "PLATEFORME BLANCHE";
            plateformblanche.ForeColor = Color.White;

            tailplatfrm.Location = new Point(200, 300);

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

            bar.ValueChanged += barre;
            plateformblanche.MouseClick += PlateformeBlanche ;
            pic.MouseDown += click;
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
                    Pen p = new Pen(cam[i, o]);
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
            grille.Panel2.Controls.Add(bar);
            choix = choix.platformblanche;
        }

        private void click(object e, EventArgs evt)
        {
            switch (choix)
            {
                case choix.platformblanche:
                    if (MousePosition.X -Location.X> 50 && MousePosition.X - Location.X < 800 && MousePosition.Y - Location.Y > 30 && MousePosition.Y - Location.Y < 800)
                    {
                        for (int i = 0; i <= tailleplatform; i++)
                        {
                            map[(MousePosition.Y - Location.Y - 80) / 5 + posy, (MousePosition.X - Location.X - 60) / 5 + posx] = Color.White;
                            cam[(MousePosition.Y - Location.Y - 80) / 5, (MousePosition.X - Location.X - 60) / 5] = Color.White;
                        }
                        pic.Refresh();
                    }
                    break;
            }
        }

        private void barre(object e, EventArgs evt)
        {
            tailleplatform = bar.Value;
            tailplatfrm.Text = tailleplatform.ToString();
        }


        private void fermerclick(object e, EventArgs evt)
        {
            choix = choix.nul;
            grille.Panel2.Controls.Remove(fermer);
        }
    }
}

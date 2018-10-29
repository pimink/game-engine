using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Input;

namespace Project5
{
    enum choix
    {
        platformblanche, effacer, nul
    }

    class Fenetre : Form
    {
        private Button fermer = new Button(), effacer = new Button(), grilleAff = new Button();
        private Color[,] cam = new Color[20, 10];
        private SplitContainer grille = new SplitContainer();
        private Button plateformblanche = new Button();
        private choix choix = choix.nul;
        private TrackBar bar = new TrackBar();
        private PictureBox pic = new PictureBox();
        private PictureBox _couleurDessin= new PictureBox();
        private Color grillecol = Color.Black, _dessin = Color.White;
        private bool clic = false;
        private ListBox listBox = new ListBox();
        private List<Bitmap> _bitmaps = new List<Bitmap>();
        private List<List<Bitmap>> _mainBitmaps = new List<List<Bitmap>>();

        public Fenetre()
        {
            listBox.Items.Add("Personnage");
            listBox.Items.Add("Entitée");
            listBox.Items.Add("Objet fixe");
            listBox.Items.Add("Npc (Mob, Pnj...)");
            listBox.Location = new Point(50, 145);
            listBox.SelectedItem = "Personnage";
            listBox.BackColor = Color.FromArgb(53, 53, 53);
            listBox.ForeColor = Color.White;
            listBox.BorderStyle = BorderStyle.None;

            fermer.Location = new Point(190, 20);
            fermer.Text = "fermer";
            fermer.ForeColor = Color.Red;

            effacer.Location = new Point(200, 135);
            effacer.Text = "Effacer";
            effacer.ForeColor = Color.White;

            bar.Location = new Point(50, 100);
            bar.Maximum = 4;

            BackColor = Color.FromArgb(53, 53, 53);
            grille.Dock = DockStyle.Fill;
            grille.Panel1MinSize = 70;
            grille.IsSplitterFixed = true;

            grilleAff.Location = new Point(50, 20);
            grilleAff.Text = "Grille";
            grilleAff.ForeColor = Color.White;

            plateformblanche.Size = new Size(200, 200);
            plateformblanche.Location = new Point(50, 50);
            plateformblanche.Text = "Dessiner";
            plateformblanche.ForeColor = Color.White;

            _couleurDessin.Location = new Point(160, 105);
            _couleurDessin.Paint += new PaintEventHandler(chargerCouleure);
            _couleurDessin.Size = new Size(20, 20);

            pic.BackColor = Color.FromArgb(53, 53, 53);
            pic.Paint += new PaintEventHandler(chargerImage);
            pic.Dock = DockStyle.Fill;
            for (int i = 0; i < 20; i++)
                for (int o = 0; o < 10 ; o++)
                    cam[i,o] = Color.Black;

            Text = "Team Cumin's Game Engine";
            Size = new Size(500, 500);
            MinimumSize = new Size(600, 600);
            SuspendLayout();

            bar.ValueChanged += barre;
            plateformblanche.MouseClick += PlateformeBlanche;
            pic.MouseDown += click;
            pic.MouseUp += up;
            fermer.Click += fermerclick;
            effacer.MouseClick += efface;
            grilleAff.Click += grilleaff;
            

            grille.Panel1.Controls.Add(pic);
            grille.Panel2.Controls.Add(plateformblanche);
            Controls.Add(grille);
            ResumeLayout(false);
            PerformLayout();
            Thread t = new Thread(new ThreadStart(drawer));
            t.Start();

        }

        //Affiche la caméra sur l'image
        private void chargerImage(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < 20; i++)
                for (int o = 0; o < 10; o++)
                {
                    Rectangle pixel = new Rectangle(50 + 20 * o, 50 + 20 * i, 20, 20);
                    SolidBrush n = new SolidBrush(cam[i, o]);
                    e.Graphics.FillRectangle(n, pixel);
                    Pen p = new Pen(cam[i, o]);
                    if (grillecol != Color.Black)
                        p = new Pen(grillecol);
                    e.Graphics.DrawRectangle(p, pixel);
                    n.Dispose();
                }
        }

        private void chargerCouleure(object sender, PaintEventArgs e)
        {
            Rectangle pixel = new Rectangle(0, 0, 20, 20);
            SolidBrush n = new SolidBrush(_dessin);
            e.Graphics.FillRectangle(n, pixel);
            Pen p = new Pen(_dessin);
            e.Graphics.DrawRectangle(p, pixel);
            n.Dispose();

        }

        static void Main()
        {
            //Lance l'application
            Application.Run(new Fenetre());
            
        }

        //Déclenché lorsqu'on appuie sur le bouton "PLATEFORME BLANCHE" et ajoute tout les outils a la fenetre
        private void PlateformeBlanche(object sender, EventArgs evt)
        {
            grille.Panel2.Controls.Add(fermer);
            grille.Panel2.Controls.Add(_couleurDessin);
            grille.Panel2.Controls.Add(bar);
            grille.Panel2.Controls.Remove(plateformblanche);
            grille.Panel2.Controls.Add(effacer);
            grille.Panel2.Controls.Add(grilleAff);
            grille.Panel2.Controls.Add(listBox);
            grillecol = Color.FromArgb(53, 53, 53);
            pic.Refresh();
            choix = choix.platformblanche;
        }


        //Déclenché lorsqu'on clique n'importe où sur l'ecran
        private void click(object e, EventArgs evt)
        {
            clic = true;
        }

        private void drawer()
        {
            pic.Paint += new PaintEventHandler(chargerImage);
            while (true)
            {
                if (clic == true && MousePosition.X - Location.X > 50 && MousePosition.X - Location.X < 250 && MousePosition.Y - Location.Y > 30 && MousePosition.Y - Location.Y < 470)
                {
                    Color couler = new Color();
                    switch (choix)
                    {
                        case choix.platformblanche:
                            couler = _dessin;
                            break;
                        case choix.effacer:
                            couler = Color.Black;
                            break;
                        default:
                            return;
                    }
                    cam[(MousePosition.Y - Location.Y - 85) / 20, (MousePosition.X - Location.X - 60) / 20] = couler;

                    pic.Invoke(new MethodInvoker(delegate
                    {
                        pic.Refresh();
                    }));
                }
            }
        }

        private void up(object e, EventArgs evt)
        {
            clic = false;
        }
        private void barre(object e, EventArgs evt)
        {
            switch(bar.Value)
            {
                case 0:
                    _dessin = Color.White;
                    break;
                case 1:
                    _dessin = Color.LightGray;
                    break;
                case 2:
                    _dessin = Color.Gray;
                    break;
                case 3:
                    _dessin = Color.FromArgb(73, 73, 73);
                    break;
                case 4:
                    _dessin = Color.FromArgb(53,53,53);
                    break;
            }
            _couleurDessin.Refresh();
        }

        private void grilleaff(object e, EventArgs evt)
        {
            if (grillecol == Color.Black)
            {
                grillecol = Color.FromArgb(53, 53, 53); 
            }
            else
            if (grillecol == Color.FromArgb(53, 53, 53))
            {
                grillecol = Color.Black;
            }
            pic.Refresh();
        }


        private void fermerclick(object e, EventArgs evt)
        {
            choix = choix.nul;
            grille.Panel2.Controls.Remove(fermer);
            grille.Panel2.Controls.Remove(_couleurDessin);
            grille.Panel2.Controls.Remove(bar);
            grille.Panel2.Controls.Add(plateformblanche);
            grille.Panel2.Controls.Remove(effacer);
            grille.Panel2.Controls.Remove(grilleAff);
            grille.Panel2.Controls.Remove(listBox);
            grillecol = Color.Black;
            pic.Refresh();
        }

        private void efface(object sender, EventArgs evt)
        {
            if (choix == choix.effacer)
            {
                choix = choix.platformblanche;
                effacer.Text = "Effacer";
            }
            else
            if (choix == choix.platformblanche)
            {
                choix = choix.effacer;
                effacer.Text = "Dessiner";
            }
        }
    }
}


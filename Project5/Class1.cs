using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using Project5.objet;

namespace Project5
{
    enum Choix
    {
        platformblanche, effacer, nul, selection, ligne, rectangle, sprite
    }

    class Fenetre : Form
    {
        private Button fermer, effacer, grilleAff = new Button(), ligne = new Button(), creerLigne = new Button(), rectangl = new Button(), creerRect = new Button(), select = new Button(), ajoutSprite = new Button(), sprite, haut, bas, gauche, droite, fermersprite, retourner, ajouter;
        private Color[,] cam = new Color[100, 180];
        private Color[,] camBef = new Color[100, 180];
        private Color[,] map = new Color[1000, 2000], selection = new Color[100, 180], camDeBase = new Color[100, 180];
        private int posx = 500, posy = 350, linx1 = -1, liny1 = -1, linx2 = -1, liny2 = -1, recx1 = -1, recy1 = -1, recx2 = -1, recy2 = -1;
        private SplitContainer grille = new SplitContainer();
        private Button plateformblanche = new Button();
        private Choix choix = Choix.nul;
        private TrackBar bar = new TrackBar();
        private int tailleplatform = 1;
        private int selx = 0, sely = 0, selDepX = 0, selDepY = 0, actSpritY = 85, actSpritX = 40;
        private PictureBox pic = new PictureBox();
        private Label tailplatfrm = new Label();
        private Color grillecol = Color.Black;
        private Bitmap camb = new Bitmap(1260, 700);
        private bool clic = false, selectionné = false, linset = false, persDef = false, retourné = false;
        private Pen p;
        private Entite personnage;
        private List<Entite> mobs;
        private Bitmap actualSprite = new Bitmap(70, 140);
        private SolidBrush noir = new SolidBrush(Color.Black);
        private SolidBrush blan = new SolidBrush(Color.White);
        private Spritejson actualSpriteJSon;

        public Fenetre()
        {
            //Revenir a l'"accueil" (pour la section plateforme)
            fermer = new Button
            {
                Location = new Point(190, 20),
                Text = "fermer",
                ForeColor = Color.Red
            };

            //Accès a la section "sprite"
            sprite = new Button
            {
                Text = "Outils sprite",
                ForeColor = Color.White,
                Location = new Point(50, 300),
                Size = new Size(200, 200)
            };

            //Ajout d'un sprite au niveau
            ajouter = new Button()
            {
                Location = new Point(50, 300),
                Size = new Size(200, 20),
                Text = "Ajouter au niveau",
                ForeColor = Color.White
            };

            //------------------------------------------------------
            //Boutons de naviguation a travers la map
            haut = new Button
            {
                BackgroundImage = new Bitmap("Ressources/haut.png"),
                Location = new Point(90, 520),
                Size = new Size(30, 30)
            };
            bas = new Button
            {
                BackgroundImage = new Bitmap("Ressources/bas.png"),
                Location = new Point(90, 590),
                Size = new Size(30, 30)
            };
            gauche = new Button
            {
                BackgroundImage = new Bitmap("Ressources/gauche.png"),
                Location = new Point(55, 555),
                Size = new Size(30, 30)
            };
            droite = new Button
            {
                BackgroundImage = new Bitmap("Ressources/droite.png"),
                Location = new Point(125, 555),
                Size = new Size(30, 30)
            };
            //------------------------------------------------------

            //Changer l'orientation d'un sprite
            retourner = new Button()
            {
                BackgroundImage = new Bitmap("Ressources/inverser.png"),
                Location = new Point(205, 555),
                Size = new Size(30, 30)
            };

            //Revenir a l'"accueil" (pour la section Sprite)
            fermersprite = fermer;

            //Outil "gomme"
            effacer = new Button
            {
                Location = new Point(200, 135),
                Text = "Effacer",
                ForeColor = Color.White
            };
            
            //Trackbar définissant la taille du curseur
            bar.Location = new Point(50, 100);
            
            //Bah la couleur d'arrière plan
            BackColor = Color.FromArgb(53, 53, 53);

            //container pricipal (bon a partir de là flemme d'optimiser)
            grille.Dock = DockStyle.Fill;
            grille.Panel1MinSize = 1410;
            grille.IsSplitterFixed = true;

            //Affichage de la grille sur le niveau (rien a voir avec le control précédent)
            grilleAff.Location = new Point(50, 20);
            grilleAff.Text = "Grille";
            grilleAff.ForeColor = Color.White;

            //Accès a la section "Plateforme"
            plateformblanche.Size = new Size(200, 200);
            plateformblanche.Location = new Point(50, 50);
            plateformblanche.Text = "PLATEFORME BLANCHE";
            plateformblanche.ForeColor = Color.White;

            //ouverture d'un sprite
            ajoutSprite.Location = new Point(50, 300);
            ajoutSprite.Size = new Size(200, 20);
            ajoutSprite.Text = "Ajouter une entité/personnage/objet";
            ajoutSprite.ForeColor = Color.White;

            //Afficheur de la taille du curseur
            tailplatfrm.Location = new Point(160, 105);
            tailplatfrm.Text = "1 pixels";
            tailplatfrm.ForeColor = Color.White;

            //Ouverture de l'outil pour faire des lignes
            ligne.BackgroundImage = new Bitmap("Ressources/Capture.png");
            ligne.Location = new Point(50, 180);
            ligne.Size = new Size(30, 30);

            //ouverture de l'outil pour faire des rectangle
            rectangl.BackgroundImage = new Bitmap("Ressources/rectangle.png");
            rectangl.Location = new Point(50, 240);
            rectangl.Size = new Size(30, 30);

            //Ouverture de l'outil selection puis déplacement
            select.Text = "Deplacer";
            select.Location = new Point(50, 290);
            select.ForeColor = Color.White;

            //Création de la ligne sur le niveau
            creerLigne.Text = "Creer";
            creerLigne.Location = new Point(190, 180);
            creerLigne.ForeColor = Color.White;

            //Création de la ligne sur le niveau
            creerRect.Text = "Creer";
            creerRect.Location = new Point(190, 240);
            creerRect.ForeColor = Color.White;

            //Niveau (caméra)
            pic.BackColor = Color.FromArgb(53, 53, 53);
            pic.Paint += new PaintEventHandler(chargerImage);
            pic.Dock = DockStyle.Fill;

            //Création de la map puis de la caméra
            for (int i = 0; i < 1000; i++)
                for (int o = 0; o < 2000; o++)
                    map.SetValue(Color.Black, i, o);
            for (int i = 0; i < 100; i++)
                for (int o = 0; o < 180; o++)
                {
                    cam.SetValue(map[i + posx, o + posy], i, o);
                    camBef.SetValue(Color.White, i, o);
                }
            RectangleF rectf = new RectangleF(0, 0, 1260, 700);
            Graphics g = Graphics.FromImage(camb);
            g.FillRectangle(new SolidBrush(Color.Black), rectf);
            g.Flush();


            SuspendLayout();

            //Paramétrage de la fenètre
            Text = "Team Cumin's Game Engine";
            Size = new Size(1800, 1000);
            MinimumSize = new Size(1600, 1000);
            Icon = new Icon("Logo.ico");

            //Mise en relation des evenements avec leurs handlers
            MouseUp += Pic_MouseUp;
            bar.ValueChanged += barre;
            plateformblanche.MouseClick += PlateformeBlanche;
            pic.MouseDown += click;
            pic.MouseUp += Pic_MouseUp;
            fermer.Click += fermerclick;
            fermersprite.Click += Fermersprite_Click;
            effacer.MouseClick += efface;
            grilleAff.Click += grilleaff;
            ligne.Click += Ligne_Click;
            rectangl.Click += Rectangl_Click;
            creerRect.Click += CreerRect_Click;
            creerLigne.Click += CreerLigne_Click;
            select.Click += Select_Click;
            ajoutSprite.Click += AjoutSprite_Click;
            sprite.Click += Sprite_Click;
            haut.Click += Haut_Click;
            bas.Click += Bas_Click;
            droite.Click += Droite_Click;
            gauche.Click += Gauche_Click;
            retourner.Click += (x, y) => { if (retourné) retourné = false; else retourné = true; pic.Refresh(); };
            ajouter.Click += Ajouter_Click;

            //ajout des controles sur la fenetres
            grille.Panel1.Controls.Add(pic);
            grille.Panel2.Controls.Add(plateformblanche);
            grille.Panel2.Controls.Add(haut);
            grille.Panel2.Controls.Add(bas);
            grille.Panel2.Controls.Add(droite);
            grille.Panel2.Controls.Add(gauche);
            grille.Panel2.Controls.Add(sprite);
            Controls.Add(grille);

            ResumeLayout(false);
            PerformLayout();

            //Lancement des différents threads
            Thread t = new Thread(new ThreadStart(drawer));
            t.Start();
            Thread th = new Thread(new ThreadStart(outilsThread));
            th.Start();

        }
        
        private void Ajouter_Click(object sender, EventArgs e)
        {
            if (actualSpriteJSon.type == "Personnage")
            {
                personnage = new Entite();
                Bitmap tempFrame = new Bitmap(70, 140);
                Graphics g = Graphics.FromImage(tempFrame);
                for (int k = 0; k < actualSpriteJSon.breathAnim.Count; k++)
                {
                    for (int i = 0; i < 20; i++)
                        for (int o = 0; o < 10; o++)
                        {
                            RectangleF rectf = new RectangleF(o * 7, i * 7, 7, 7);
                            if (actualSpriteJSon.breathAnim[k][o + i * 10] == 0)
                                g.FillRectangle(new SolidBrush(Color.Empty), rectf);
                            else
                                switch (actualSpriteJSon.breathAnim[0][o + i * 10])
                                {
                                    case 5:
                                        g.FillRectangle(new SolidBrush(Color.White), rectf);
                                        break;
                                    case 4:
                                        g.FillRectangle(new SolidBrush(Color.LightGray), rectf);
                                        break;
                                    case 3:
                                        g.FillRectangle(new SolidBrush(Color.Gray), rectf);
                                        break;
                                    case 2:
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(73,73,73)), rectf);
                                        break;
                                    case 1:
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(53, 53, 53)), rectf);
                                        break;
                                }
                        }
                    personnage.BreathAnim.Add(tempFrame);
                }
                for (int k = 0; k < actualSpriteJSon.deathAnim.Count; k++)
                {
                    for (int i = 0; i < 20; i++)
                        for (int o = 0; o < 10; o++)
                        {
                            RectangleF rectf = new RectangleF(o * 7, i * 7, 7, 7);
                            if (actualSpriteJSon.deathAnim[k][o + i * 10] == 0)
                                g.FillRectangle(new SolidBrush(Color.Empty), rectf);
                            else
                                switch (actualSpriteJSon.deathAnim[0][o + i * 10])
                                {
                                    case 5:
                                        g.FillRectangle(new SolidBrush(Color.White), rectf);
                                        break;
                                    case 4:
                                        g.FillRectangle(new SolidBrush(Color.LightGray), rectf);
                                        break;
                                    case 3:
                                        g.FillRectangle(new SolidBrush(Color.Gray), rectf);
                                        break;
                                    case 2:
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(73, 73, 73)), rectf);
                                        break;
                                    case 1:
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(53, 53, 53)), rectf);
                                        break;
                                }
                        }
                    personnage.DeathAnim.Add(tempFrame);
                }
                for (int k = 0; k < actualSpriteJSon.walkAnim.Count; k++)
                {
                    for (int i = 0; i < 20; i++)
                        for (int o = 0; o < 10; o++)
                        {
                            RectangleF rectf = new RectangleF(o * 7, i * 7, 7, 7);
                            if (actualSpriteJSon.walkAnim[k][o + i * 10] == 0)
                                g.FillRectangle(new SolidBrush(Color.Empty), rectf);
                            else
                                switch (actualSpriteJSon.walkAnim[0][o + i * 10])
                                {
                                    case 5:
                                        g.FillRectangle(new SolidBrush(Color.White), rectf);
                                        break;
                                    case 4:
                                        g.FillRectangle(new SolidBrush(Color.LightGray), rectf);
                                        break;
                                    case 3:
                                        g.FillRectangle(new SolidBrush(Color.Gray), rectf);
                                        break;
                                    case 2:
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(73, 73, 73)), rectf);
                                        break;
                                    case 1:
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(53, 53, 53)), rectf);
                                        break;
                                }
                        }
                    personnage.WalkAnim.Add(tempFrame);
                }
                for (int k = 0; k < actualSpriteJSon.jumpAnim.Count; k++)
                {
                    for (int i = 0; i < 20; i++)
                        for (int o = 0; o < 10; o++)
                        {
                            RectangleF rectf = new RectangleF(o * 7, i * 7, 7, 7);
                            if (actualSpriteJSon.jumpAnim[k][o + i * 10] == 0)
                                g.FillRectangle(new SolidBrush(Color.Empty), rectf);
                            else
                                switch (actualSpriteJSon.jumpAnim[0][o + i * 10])
                                {
                                    case 5:
                                        g.FillRectangle(new SolidBrush(Color.White), rectf);
                                        break;
                                    case 4:
                                        g.FillRectangle(new SolidBrush(Color.LightGray), rectf);
                                        break;
                                    case 3:
                                        g.FillRectangle(new SolidBrush(Color.Gray), rectf);
                                        break;
                                    case 2:
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(73, 73, 73)), rectf);
                                        break;
                                    case 1:
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(53, 53, 53)), rectf);
                                        break;
                                }
                        }
                    personnage.JumpAnim.Add(tempFrame);
                }
                for (int k = 0; k < actualSpriteJSon.sneakAnim.Count; k++)
                {
                    for (int i = 0; i < 20; i++)
                        for (int o = 0; o < 10; o++)
                        {
                            RectangleF rectf = new RectangleF(o * 7, i * 7, 7, 7);
                            if (actualSpriteJSon.sneakAnim[k][o + i * 10] == 0)
                                g.FillRectangle(new SolidBrush(Color.Empty), rectf);
                            else
                                switch (actualSpriteJSon.sneakAnim[0][o + i * 10])
                                {
                                    case 5:
                                        g.FillRectangle(new SolidBrush(Color.White), rectf);
                                        break;
                                    case 4:
                                        g.FillRectangle(new SolidBrush(Color.LightGray), rectf);
                                        break;
                                    case 3:
                                        g.FillRectangle(new SolidBrush(Color.Gray), rectf);
                                        break;
                                    case 2:
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(73, 73, 73)), rectf);
                                        break;
                                    case 1:
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(53, 53, 53)), rectf);
                                        break;
                                }
                        }
                    personnage.SneakAnim.Add(tempFrame);
                }
                for (int k = 0; k < actualSpriteJSon.runAnim.Count; k++)
                {
                    for (int i = 0; i < 20; i++)
                        for (int o = 0; o < 10; o++)
                        {
                            RectangleF rectf = new RectangleF(o * 7, i * 7, 7, 7);
                            if (actualSpriteJSon.runAnim[k][o + i * 10] == 0)
                                g.FillRectangle(new SolidBrush(Color.Empty), rectf);
                            else
                                switch (actualSpriteJSon.runAnim[0][o + i * 10])
                                {
                                    case 5:
                                        g.FillRectangle(new SolidBrush(Color.White), rectf);
                                        break;
                                    case 4:
                                        g.FillRectangle(new SolidBrush(Color.LightGray), rectf);
                                        break;
                                    case 3:
                                        g.FillRectangle(new SolidBrush(Color.Gray), rectf);
                                        break;
                                    case 2:
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(73, 73, 73)), rectf);
                                        break;
                                    case 1:
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(53, 53, 53)), rectf);
                                        break;
                                }
                        }
                    personnage.RunAnim.Add(tempFrame);
                }
                persDef = true;
                g.Flush();
                personnage.Gen = actualSpriteJSon.gen;
                personnage.X = actSpritX;
                personnage.Y = actSpritY;
                personnage.Type = actualSpriteJSon.type;
                personnage.Retourné = retourné;
            }
            choix = Choix.nul;
            pic.Invoke(new MethodInvoker(delegate
            {
                pic.Refresh();
            }));
        }

        private void Fermersprite_Click(object sender, EventArgs e)
        {
            grille.Panel2.Controls.Remove(ajoutSprite);
            grille.Panel2.Controls.Add(plateformblanche);
            grille.Panel2.Controls.Add(sprite);
            grille.Panel2.Controls.Remove(fermersprite);
        }

        private void Gauche_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
                for (int o = 0; o < 180; o++)
                {
                    map.SetValue(cam[i, o], i + posx, o + posy);
                }
            posy = posy + 5;
            for (int i = 0; i < 100; i++)
                for (int o = 0; o < 180; o++)
                {
                    cam.SetValue(map[i + posx, o + posy], i, o);
                }
            if (grillecol == Color.FromArgb(53, 53, 53))
            {
                grillecol = Color.FromArgb(53, 53, 53);
                for (int i = 0; i < 100; i++)
                    for (int o = 0; o < 180; o++)
                    {
                        Graphics g = Graphics.FromImage(camb);
                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                        SolidBrush n = new SolidBrush(cam[i, o]);
                        g.FillRectangle(n, pixel);
                        Pen p = new Pen(Color.FromArgb(53, 53, 53));
                        g.DrawRectangle(p, pixel);
                        g.Flush();
                    }
            }
            else
            if (grillecol == Color.Black)
            {
                grillecol = Color.Black;
                for (int i = 0; i < 100; i++)
                    for (int o = 0; o < 180; o++)
                    {
                        Graphics g = Graphics.FromImage(camb);
                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                        SolidBrush n = new SolidBrush(cam[i, o]);
                        g.FillRectangle(n, pixel);
                        Pen p = new Pen(cam[i, o]);
                        g.DrawRectangle(p, pixel);
                        g.Flush();
                    }
            }
            pic.Refresh();
        }

        private void Droite_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
                for (int o = 0; o < 180; o++)
                {
                    map.SetValue(cam[i, o], i + posx, o + posy);
                }
            posy = posy - 5;
            for (int i = 0; i < 100; i++)
                for (int o = 0; o < 180; o++)
                {
                    cam.SetValue(map[i + posx, o + posy], i, o);
                }
            if (grillecol == Color.FromArgb(53, 53, 53))
            {
                grillecol = Color.FromArgb(53, 53, 53);
                for (int i = 0; i < 100; i++)
                    for (int o = 0; o < 180; o++)
                    {
                        Graphics g = Graphics.FromImage(camb);
                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                        SolidBrush n = new SolidBrush(cam[i, o]);
                        g.FillRectangle(n, pixel);
                        Pen p = new Pen(Color.FromArgb(53, 53, 53));
                        g.DrawRectangle(p, pixel);
                        g.Flush();
                    }
            }
            else
            if (grillecol == Color.Black)
            {
                grillecol = Color.Black;
                for (int i = 0; i < 100; i++)
                    for (int o = 0; o < 180; o++)
                    {
                        Graphics g = Graphics.FromImage(camb);
                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                        SolidBrush n = new SolidBrush(cam[i, o]);
                        g.FillRectangle(n, pixel);
                        Pen p = new Pen(cam[i, o]);
                        g.DrawRectangle(p, pixel);
                        g.Flush();
                    }
            }
            pic.Refresh();
        }

        private void Bas_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
                for (int o = 0; o < 180; o++)
                {
                    map.SetValue(cam[i, o], i + posx, o + posy);
                }
            posx = posx - 5;
            for (int i = 0; i < 100; i++)
                for (int o = 0; o < 180; o++)
                {
                    cam.SetValue(map[i + posx, o + posy], i, o);
                }
            if (grillecol == Color.FromArgb(53, 53, 53))
            {
                grillecol = Color.FromArgb(53, 53, 53);
                for (int i = 0; i < 100; i++)
                    for (int o = 0; o < 180; o++)
                    {
                        Graphics g = Graphics.FromImage(camb);
                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                        SolidBrush n = new SolidBrush(cam[i, o]);
                        g.FillRectangle(n, pixel);
                        Pen p = new Pen(Color.FromArgb(53, 53, 53));
                        g.DrawRectangle(p, pixel);
                        g.Flush();
                    }
            }
            else
            if (grillecol == Color.Black)
            {
                grillecol = Color.Black;
                for (int i = 0; i < 100; i++)
                    for (int o = 0; o < 180; o++)
                    {
                        Graphics g = Graphics.FromImage(camb);
                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                        SolidBrush n = new SolidBrush(cam[i, o]);
                        g.FillRectangle(n, pixel);
                        Pen p = new Pen(cam[i, o]);
                        g.DrawRectangle(p, pixel);
                        g.Flush();
                    }
            }
            pic.Refresh();
        }

        private void Haut_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
                for (int o = 0; o < 180; o++)
                {
                    map.SetValue(cam[i, o], i + posx, o + posy);
                }
            posx = posx + 5;
            for (int i = 0; i < 100; i++)
                for (int o = 0; o < 180; o++)
                {
                    cam.SetValue(map[i + posx, o + posy], i, o);
                }
            if (grillecol == Color.FromArgb(53, 53, 53))
            {
                grillecol = Color.FromArgb(53, 53, 53);
                for (int i = 0; i < 100; i++)
                    for (int o = 0; o < 180; o++)
                    {
                        Graphics g = Graphics.FromImage(camb);
                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                        SolidBrush n = new SolidBrush(cam[i, o]);
                        g.FillRectangle(n, pixel);
                        Pen p = new Pen(Color.FromArgb(53, 53, 53));
                        g.DrawRectangle(p, pixel);
                        g.Flush();
                    }
            }
            else
            if (grillecol == Color.Black)
            {
                grillecol = Color.Black;
                for (int i = 0; i < 100; i++)
                    for (int o = 0; o < 180; o++)
                    {
                        Graphics g = Graphics.FromImage(camb);
                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                        SolidBrush n = new SolidBrush(cam[i, o]);
                        g.FillRectangle(n, pixel);
                        Pen p = new Pen(cam[i, o]);
                        g.DrawRectangle(p, pixel);
                        g.Flush();
                    }
            }
            pic.Refresh();
        }

        private void Sprite_Click(object sender, EventArgs e)
        {
            grille.Panel2.Controls.Add(ajoutSprite);
            grille.Panel2.Controls.Remove(plateformblanche);
            grille.Panel2.Controls.Remove(sprite);
            grille.Panel2.Controls.Add(fermersprite);
        }

        private void AjoutSprite_Click(object sender, EventArgs e)
        {
            grille.Panel2.Controls.Add(retourner);
            grille.Panel2.Controls.Add(ajouter);
            grille.Panel2.Controls.Remove(ajoutSprite);
            grille.Panel2.Controls.Remove(fermersprite);
            retourné = false;
            choix = Choix.sprite;
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                AddExtension = true,
                DefaultExt = "json"
            };
            dialog.ShowDialog();
            Console.WriteLine(dialog.FileName);
            FileStream stream = new FileStream(dialog.FileName, FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            actualSpriteJSon = (Spritejson)ser.ReadObject(stream);
            Graphics g = Graphics.FromImage(actualSprite);
            for (int i = 0; i < 20; i++)
                for (int o = 0; o < 10; o++)
                {
                    RectangleF rectf = new RectangleF(o * 7, i * 7, 7, 7);
                    Console.WriteLine(actualSpriteJSon.breathAnim[0][o + i * 10].ToString());
                    if (actualSpriteJSon.breathAnim[0][o + i * 10] == 0)
                        g.FillRectangle(new SolidBrush(Color.Empty), rectf);
                    else
                        switch (actualSpriteJSon.breathAnim[0][o + i * 10])
                        {
                            case 5:
                                g.FillRectangle(new SolidBrush(Color.White), rectf);
                                break;
                            case 4:
                                g.FillRectangle(new SolidBrush(Color.LightGray), rectf);
                                break;
                            case 3:
                                g.FillRectangle(new SolidBrush(Color.Gray), rectf);
                                break;
                            case 2:
                                g.FillRectangle(new SolidBrush(Color.FromArgb(73, 73, 73)), rectf);
                                break;
                            case 1:
                                g.FillRectangle(new SolidBrush(Color.FromArgb(53, 53, 53)), rectf);
                                break;
                        }
                }
            actSpritX = 40 + posx;
            actSpritY = 85 + posy;
            g.Flush();
            pic.Refresh();
            Thread t = new Thread(new ThreadStart(spriteThread));
            t.Start();
        }

        private void spriteThread()
        {
            int depDepX = 0, depDepY = 0, posDepDepX = 0, posDepDepY = 0;
            bool dep = false;
            while (true)
            {
                if (clic && (MousePosition.X - Location.X > 50 && MousePosition.X - Location.X < 1400 &&
                        MousePosition.Y - Location.Y > 50 && MousePosition.Y - Location.Y < 800))
                {
                    if (MousePosition.X - Location.X >= 60 + (actSpritY - posy) * 7 && MousePosition.X - Location.X <= 60 + (actSpritY - posy) * 7 + 70 && MousePosition.Y - Location.Y >= 85 + (actSpritX - posx) * 7 && MousePosition.Y - Location.Y <= 85 + 140 + (actSpritX - posx) * 7)
                    {
                        if (!dep)
                        {
                            posDepDepX = actSpritX - posx;
                            posDepDepY = actSpritY - posy;
                            depDepY = (MousePosition.X - Location.X - 60);
                            depDepX = (MousePosition.Y - Location.Y - 85);
                            dep = true;
                        }
                    }
                    if (dep)
                    {
                        actSpritX = posDepDepX * 7 + ((MousePosition.Y - Location.Y - 85) - depDepX);
                        actSpritY = posDepDepY * 7 + ((MousePosition.X - Location.X - 60) - depDepY);
                        actSpritX /= 7;
                        actSpritY /= 7;
                        actSpritX += posx;
                        actSpritY += posy;
                        pic.Invoke(new MethodInvoker(delegate
                        {
                            pic.Refresh();
                        }));
                    }

                }
                else
                {
                    if (dep)
                    {
                        depDepX = 0; depDepY = 0; posDepDepX = 0; posDepDepY = 0;
                        dep = false;
                    }
                }

                if (!clic && (MousePosition.X - Location.X >= 60 + (actSpritY - posy) * 7 && MousePosition.X - Location.X <= 60 + (actSpritY - posy) * 7 + 70 && MousePosition.Y - Location.Y >= 85 + (actSpritX - posx) * 7 && MousePosition.Y - Location.Y <= 85 + 140 + (actSpritX - posx) * 7))
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        Cursor = Cursors.Hand;
                    }));
                }

                if (!clic && !(MousePosition.X - Location.X >= 60 + (actSpritY - posy) * 7 && MousePosition.X - Location.X <= 60 + (actSpritY - posy) * 7 + 70 && MousePosition.Y - Location.Y >= 85 + (actSpritX - posx) * 7 && MousePosition.Y - Location.Y <= 85 + 140 + (actSpritX - posx) * 7))
                    Invoke(new MethodInvoker(delegate
                    {
                        Cursor = Cursors.Default;
                    }));

            }
        }

        private void Select_Click(object sender, EventArgs e)
        {
            if (choix != Choix.selection)
            {
                choix = Choix.selection;
                select.Text = "Arrêter";
            }
            else
            {
                selDepY /= 7;
                selDepY *= 7;
                selDepX /= 7;
                selDepX *= 7;
                sely /= 7;
                sely *= 7;
                selx /= 7;
                selx *= 7;
                pic.Invoke(new MethodInvoker(delegate
                {
                    pic.Refresh();
                }));
                if (effacer.Text == "Effacer")
                    choix = Choix.platformblanche;
                else
                    choix = Choix.effacer;
                select.Text = "Déplacer";
            }
        }

        private void CreerRect_Click(object sender, EventArgs e)
        {
            int y1 = recy1 - 50,
                x1 = recx1 - 50,
                y2 = recy2 - 50,
                x2 = recx2 - 50;

            var g = Graphics.FromImage(camb);

            if (recy2 >= recy1)
            {
                if (recx2 >= recx1)
                {
                    RectangleF rectf = new RectangleF(x1, y1, x2 - x1, y2 - y1);
                    g.FillRectangle(new SolidBrush(Color.White), rectf);
                    g.Flush();
                    for (int i = x1 / 7 + 1; i < x2 / 7 + 1; i++)
                        for (int o = y1 / 7 + 1; o < y2 / 7 + 1; o++)
                        {
                            cam[o, i] = Color.White;
                        }
                }
                else
                {
                    RectangleF rectf = new RectangleF(x2, y1, x1 - x2, y2 - y1);
                    g.FillRectangle(new SolidBrush(Color.White), rectf);
                    g.Flush();
                    for (int i = x2 / 7 + 1; i < x1 / 7 + 1; i++)
                        for (int o = y1 / 7 + 1; o < y2 / 7 + 1; o++)
                        {
                            cam[o, i] = Color.White;
                        }
                }
            }
            else
            {
                if (recx2 >= recx1)
                {
                    RectangleF rectf = new RectangleF(x1, y2, x2 - x1, y1 - y2);
                    g.FillRectangle(new SolidBrush(Color.White), rectf);
                    g.Flush();
                    for (int i = x1 / 7 + 1; i < x2 / 7 + 1; i++)
                        for (int o = y2 / 7 + 1; o < y1 / 7 + 1; o++)
                        {
                            cam[o, i] = Color.White;
                        }
                }
                else
                {
                    RectangleF rectf = new RectangleF(x2, y2, x1 - x2, y1 - y2);
                    g.FillRectangle(new SolidBrush(Color.White), rectf);
                    g.Flush();
                    for (int i = x2 / 7 + 1; i < x1 / 7 + 1; i++)
                        for (int o = y2 / 7 + 1; o < y1 / 7 + 1; o++)
                        {
                            cam[o, i] = Color.White;
                        }
                }
            }
            pic.Refresh();
        }

        private void Rectangl_Click(object sender, EventArgs e)
        {
            if (choix != Choix.rectangle)
            {
                choix = Choix.rectangle;
                grille.Panel2.Controls.Add(creerRect);
            }
            else
            {
                choix = Choix.platformblanche;
                grille.Panel2.Controls.Remove(creerRect);
            }
        }

        private void CreerLigne_Click(object sender, EventArgs e)
        {
            int y1 = liny1 - 50,
                x1 = linx1 - 50,
                y2 = liny2 - 50,
                x2 = linx2 - 50,
                longueurx = Math.Abs(x1 - x2),
                longueury = Math.Abs(y1 - y2);
            int nb;
            long segx, segy;
            if (longueurx > longueury)
            {
                nb = Math.Abs((y1 - y2) / 7);
                if (nb != 0)
                    segx = longueurx / nb;
                else
                    segx = longueurx;

                Graphics g = Graphics.FromImage(camb);
                if (y2 >= y1)
                {
                    if (x2 >= x1)
                    {
                        for (int i = 0; i <= nb; i++)
                        {
                            for (int o = 0; o <= segx / 7; o++)
                            {

                                cam[(y1 + i * 7) / 7, (x1 + o * 7 + i * segx) / 7] = Color.White;
                                RectangleF rectf = new RectangleF(((int)((x1 + o * 7 + i * segx - ((tailleplatform - 1) * 7)) / 7)) * 7, ((int)((y1 + i * 7 - ((tailleplatform - 1) * 7)) / 7)) * 7, 14 * (tailleplatform - 1) + 7, 14 * (tailleplatform - 1) + 7);
                                g.FillRectangle(new SolidBrush(Color.White), rectf);
                                g.Flush();
                                if (tailleplatform > 1)
                                {
                                    for (int x = 1; x <= tailleplatform - 1; x++)
                                    {
                                        cam[(y1 + i * 7) / 7, (x1 + o * 7 + i * segx) / 7 + x] = Color.White;
                                        cam[(y1 + i * 7) / 7, (x1 + o * 7 + i * segx) / 7 - x] = Color.White;
                                        cam[(y1 + i * 7) / 7 + x, (x1 + o * 7 + i * segx) / 7] = Color.White;
                                        cam[(y1 + i * 7) / 7 - x, (x1 + o * 7 + i * segx) / 7] = Color.White;
                                        for (int y = 1; y <= tailleplatform - 1; y++)
                                        {
                                            cam[(y1 + i * 7) / 7 + y, (x1 + o * 7 + i * segx) / 7 + x] = Color.White;
                                            cam[(y1 + i * 7) / 7 - y, (x1 + o * 7 + i * segx) / 7 + x] = Color.White;
                                            cam[(y1 + i * 7) / 7 + y, (x1 + o * 7 + i * segx) / 7 - x] = Color.White;
                                            cam[(y1 + i * 7) / 7 - y, (x1 + o * 7 + i * segx) / 7 - x] = Color.White;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        for (int i = 0; i <= nb; i++)
                        {
                            for (int o = (int)segx / 7; o >= 0; o--)
                            {

                                cam[(y1 + i * 7) / 7, (x1 + o * 7 - i * segx) / 7] = Color.White;
                                RectangleF rectf = new RectangleF(((int)((x1 + o * 7 - i * segx - ((tailleplatform - 1) * 7)) / 7)) * 7, ((int)((y1 + i * 7 - ((tailleplatform - 1) * 7)) / 7)) * 7, 14 * (tailleplatform - 1) + 7, 14 * (tailleplatform - 1) + 7);
                                g.FillRectangle(new SolidBrush(Color.White), rectf);
                                g.Flush();
                                if (tailleplatform > 1)
                                {
                                    for (int x = 1; x <= tailleplatform - 1; x++)
                                    {
                                        cam[(y1 + i * 7) / 7, (x1 + o * 7 - i * segx) / 7 + x] = Color.White;
                                        cam[(y1 + i * 7) / 7, (x1 + o * 7 - i * segx) / 7 - x] = Color.White;
                                        cam[(y1 + i * 7) / 7 + x, (x1 + o * 7 - i * segx) / 7] = Color.White;
                                        cam[(y1 + i * 7) / 7 - x, (x1 + o * 7 - i * segx) / 7] = Color.White;
                                        for (int y = 1; y <= tailleplatform - 1; y++)
                                        {
                                            cam[(y1 + i * 7) / 7 + y, (x1 + o * 7 - i * segx) / 7 + x] = Color.White;
                                            cam[(y1 + i * 7) / 7 - y, (x1 + o * 7 - i * segx) / 7 + x] = Color.White;
                                            cam[(y1 + i * 7) / 7 + y, (x1 + o * 7 - i * segx) / 7 - x] = Color.White;
                                            cam[(y1 + i * 7) / 7 - y, (x1 + o * 7 - i * segx) / 7 - x] = Color.White;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (x2 >= x1)
                    {
                        for (int i = nb; i >= 0; i--)
                        {
                            for (int o = 0; o <= segx / 7; o++)
                            {
                                cam[(y1 - i * 7) / 7, (x1 + o * 7 + i * segx) / 7] = Color.White;
                                RectangleF rectf = new RectangleF(((int)((x1 + o * 7 + i * segx - ((tailleplatform - 1) * 7)) / 7)) * 7, ((int)((y1 - i * 7 - ((tailleplatform - 1) * 7)) / 7)) * 7, 14 * (tailleplatform - 1) + 7, 14 * (tailleplatform - 1) + 7);
                                g.FillRectangle(new SolidBrush(Color.White), rectf);
                                g.Flush();
                                if (tailleplatform > 1)
                                {
                                    for (int x = 1; x <= tailleplatform - 1; x++)
                                    {
                                        cam[(y1 - i * 7) / 7, (x1 + o * 7 + i * segx) / 7 + x] = Color.White;
                                        cam[(y1 - i * 7) / 7, (x1 + o * 7 + i * segx) / 7 - x] = Color.White;
                                        cam[(y1 - i * 7) / 7 + x, (x1 + o * 7 + i * segx) / 7] = Color.White;
                                        cam[(y1 - i * 7) / 7 - x, (x1 + o * 7 + i * segx) / 7] = Color.White;
                                        for (int y = 1; y <= tailleplatform - 1; y++)
                                        {
                                            cam[(y1 - i * 7) / 7 + y, (x1 + o * 7 + i * segx) / 7 + x] = Color.White;
                                            cam[(y1 - i * 7) / 7 - y, (x1 + o * 7 + i * segx) / 7 + x] = Color.White;
                                            cam[(y1 - i * 7) / 7 + y, (x1 + o * 7 + i * segx) / 7 - x] = Color.White;
                                            cam[(y1 - i * 7) / 7 - y, (x1 + o * 7 + i * segx) / 7 - x] = Color.White;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        for (int i = nb; i >= 0; i--)
                        {
                            for (int o = (int)segx / 7; o >= 0; o--)
                            {

                                cam[(y1 - i * 7) / 7, (x1 + o * 7 - i * segx) / 7] = Color.White;
                                RectangleF rectf = new RectangleF(((int)((x1 + o * 7 - i * segx - ((tailleplatform - 1) * 7)) / 7)) * 7, ((int)((y1 - i * 7 - ((tailleplatform - 1) * 7)) / 7)) * 7, 14 * (tailleplatform - 1) + 7, 14 * (tailleplatform - 1) + 7);
                                g.FillRectangle(new SolidBrush(Color.White), rectf);
                                g.Flush();
                                if (tailleplatform > 1)
                                {
                                    for (int x = 1; x <= tailleplatform - 1; x++)
                                    {
                                        cam[(y1 - i * 7) / 7, (x1 + o * 7 - i * segx) / 7 + x] = Color.White;
                                        cam[(y1 - i * 7) / 7, (x1 + o * 7 - i * segx) / 7 - x] = Color.White;
                                        cam[(y1 - i * 7) / 7 + x, (x1 + o * 7 - i * segx) / 7] = Color.White;
                                        cam[(y1 - i * 7) / 7 - x, (x1 + o * 7 - i * segx) / 7] = Color.White;
                                        for (int y = 1; y <= tailleplatform - 1; y++)
                                        {
                                            cam[(y1 - i * 7) / 7 + y, (x1 + o * 7 - i * segx) / 7 + x] = Color.White;
                                            cam[(y1 - i * 7) / 7 - y, (x1 + o * 7 - i * segx) / 7 + x] = Color.White;
                                            cam[(y1 - i * 7) / 7 + y, (x1 + o * 7 - i * segx) / 7 - x] = Color.White;
                                            cam[(y1 - i * 7) / 7 - y, (x1 + o * 7 - i * segx) / 7 - x] = Color.White;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                nb = Math.Abs((x1 - x2) / 7);
                if (nb != 0)
                    segy = longueury / nb;
                else
                    segy = longueury;


                Graphics g = Graphics.FromImage(camb);
                if (y2 >= y1)
                {
                    if (x2 >= x1)
                    {
                        for (int i = 0; i <= nb; i++)
                        {
                            for (int o = 0; o <= segy / 7; o++)
                            {

                                cam[(y1 + o * 7 + i * segy) / 7, (x1 + i * 7) / 7] = Color.White;
                                RectangleF rectf = new RectangleF(((int)((x1 + i * 7 - ((tailleplatform - 1) * 7)) / 7)) * 7, ((int)((y1 + o * 7 + i * segy - ((tailleplatform - 1) * 7)) / 7)) * 7, 14 * (tailleplatform - 1) + 7, 14 * (tailleplatform - 1) + 7);
                                g.FillRectangle(new SolidBrush(Color.White), rectf);
                                g.Flush();
                                if (tailleplatform > 1)
                                {
                                    for (int x = 1; x <= tailleplatform - 1; x++)
                                    {
                                        cam[(y1 + o * 7 + i * segy) / 7 + x, (x1 + i * 7) / 7] = Color.White;
                                        cam[(y1 + o * 7 + i * segy) / 7 - x, (x1 + i * 7) / 7] = Color.White;
                                        cam[(y1 + o * 7 + i * segy) / 7, (x1 + i * 7) / 7 + x] = Color.White;
                                        cam[(y1 + o * 7 + i * segy) / 7, (x1 + i * 7) / 7 - x] = Color.White;
                                        for (int y = 1; y <= tailleplatform - 1; y++)
                                        {
                                            cam[(y1 + o * 7 + i * segy) / 7 + x, (x1 + i * 7) / 7 + y] = Color.White;
                                            cam[(y1 + o * 7 + i * segy) / 7 + x, (x1 + i * 7) / 7 - y] = Color.White;
                                            cam[(y1 + o * 7 + i * segy) / 7 - x, (x1 + i * 7) / 7 + y] = Color.White;
                                            cam[(y1 + o * 7 + i * segy) / 7 - x, (x1 + i * 7) / 7 - y] = Color.White;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        Console.WriteLine(segy);
                        for (int i = nb; i >= 0; i--)
                        {
                            for (int o = 0; o <= (segy / 7); o++)
                            {
                                Console.WriteLine($"{(y1 + o * 7 + i * segy) / 7},{(x1 - i * 7) / 7}");
                                cam[(y1 + o * 7 + i * segy) / 7, (x1 - i * 7) / 7] = Color.White;
                                RectangleF rectf = new RectangleF(((int)((x1 - i * 7 - ((tailleplatform - 1) * 7)) / 7)) * 7, ((int)((y1 + o * 7 + i * segy - ((tailleplatform - 1) * 7)) / 7)) * 7, 14 * (tailleplatform - 1) + 7, 14 * (tailleplatform - 1) + 7);
                                g.FillRectangle(new SolidBrush(Color.White), rectf);
                                g.Flush();
                                if (tailleplatform > 1)
                                {
                                    for (int x = 1; x <= tailleplatform - 1; x++)
                                    {
                                        cam[(y1 + o * 7 + i * segy) / 7 + x, (x1 - i * 7) / 7] = Color.White;
                                        cam[(y1 + o * 7 + i * segy) / 7 - x, (x1 - i * 7) / 7] = Color.White;
                                        cam[(y1 + o * 7 + i * segy) / 7, (x1 - i * 7) / 7 + x] = Color.White;
                                        cam[(y1 + o * 7 + i * segy) / 7, (x1 - i * 7) / 7 - x] = Color.White;
                                        for (int y = 1; y <= tailleplatform - 1; y++)
                                        {
                                            cam[(y1 + o * 7 + i * segy) / 7 + x, (x1 - i * 7) / 7 + y] = Color.White;
                                            cam[(y1 + o * 7 + i * segy) / 7 + x, (x1 - i * 7) / 7 - y] = Color.White;
                                            cam[(y1 + o * 7 + i * segy) / 7 - x, (x1 - i * 7) / 7 + y] = Color.White;
                                            cam[(y1 + o * 7 + i * segy) / 7 - x, (x1 - i * 7) / 7 - y] = Color.White;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (x2 >= x1)
                    {
                        for (int i = 0; i <= nb; i++)
                        {
                            for (int o = (int)(segy / 7); o >= 0 / 7; o--)
                            {
                                Console.WriteLine($"{(y1 + o * 7 - i * segy) / 7},{(x1 + i * 7) / 7}");
                                cam[(y1 + o * 7 - i * segy) / 7, (x1 + i * 7) / 7] = Color.White;
                                RectangleF rectf = new RectangleF(((int)((x1 + i * 7 - ((tailleplatform - 1) * 7)) / 7)) * 7, ((int)((y1 + o * 7 - i * segy - ((tailleplatform - 1) * 7)) / 7)) * 7, 14 * (tailleplatform - 1) + 7, 14 * (tailleplatform - 1) + 7);
                                g.FillRectangle(new SolidBrush(Color.White), rectf);
                                g.Flush();
                                if (tailleplatform > 1)
                                {
                                    for (int x = 1; x <= tailleplatform - 1; x++)
                                    {
                                        cam[(y1 + o * 7 - i * segy) / 7 + x, (x1 + i * 7) / 7] = Color.White;
                                        cam[(y1 + o * 7 - i * segy) / 7 - x, (x1 + i * 7) / 7] = Color.White;
                                        cam[(y1 + o * 7 - i * segy) / 7, (x1 + i * 7) / 7 + x] = Color.White;
                                        cam[(y1 + o * 7 - i * segy) / 7, (x1 + i * 7) / 7 - x] = Color.White;
                                        for (int y = 1; y <= tailleplatform - 1; y++)
                                        {
                                            cam[(y1 + o * 7 - i * segy) / 7 + x, (x1 + i * 7) / 7 + y] = Color.White;
                                            cam[(y1 + o * 7 - i * segy) / 7 + x, (x1 + i * 7) / 7 - y] = Color.White;
                                            cam[(y1 + o * 7 - i * segy) / 7 - x, (x1 + i * 7) / 7 + y] = Color.White;
                                            cam[(y1 + o * 7 - i * segy) / 7 - x, (x1 + i * 7) / 7 - y] = Color.White;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        for (int i = nb; i >= 0; i--)
                        {
                            for (int o = (int)(segy / 7); o >= 0 / 7; o--)
                            {
                                cam[(y1 + o * 7 - i * segy) / 7, (x1 - i * 7) / 7] = Color.White;
                                RectangleF rectf = new RectangleF(((int)((x1 - i * 7 - ((tailleplatform - 1) * 7)) / 7)) * 7, ((int)((y1 + o * 7 - i * segy - ((tailleplatform - 1) * 7)) / 7)) * 7, 14 * (tailleplatform - 1) + 7, 14 * (tailleplatform - 1) + 7);
                                g.FillRectangle(new SolidBrush(Color.White), rectf);
                                g.Flush();
                                if (tailleplatform > 1)
                                {
                                    for (int x = 1; x <= tailleplatform - 1; x++)
                                    {
                                        cam[(y1 + o * 7 - i * segy) / 7 + x, (x1 - i * 7) / 7] = Color.White;
                                        cam[(y1 + o * 7 - i * segy) / 7 - x, (x1 - i * 7) / 7] = Color.White;
                                        cam[(y1 + o * 7 - i * segy) / 7, (x1 - i * 7) / 7 + x] = Color.White;
                                        cam[(y1 + o * 7 - i * segy) / 7, (x1 - i * 7) / 7 - x] = Color.White;
                                        for (int y = 1; y <= tailleplatform - 1; y++)
                                        {
                                            cam[(y1 + o * 7 - i * segy) / 7 + x, (x1 - i * 7) / 7 + y] = Color.White;
                                            cam[(y1 + o * 7 - i * segy) / 7 + x, (x1 - i * 7) / 7 - y] = Color.White;
                                            cam[(y1 + o * 7 - i * segy) / 7 - x, (x1 - i * 7) / 7 + y] = Color.White;
                                            cam[(y1 + o * 7 - i * segy) / 7 - x, (x1 - i * 7) / 7 - y] = Color.White;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            pic.Refresh();
        }

        private void Ligne_Click(object sender, EventArgs e)
        {
            if (choix != Choix.ligne)
            {
                choix = Choix.ligne;
                grille.Panel2.Controls.Add(creerLigne);
            }
            else
            {
                choix = Choix.platformblanche;
                grille.Panel2.Controls.Remove(creerLigne);
            }
        }

        private void Pic_MouseUp(object sender, MouseEventArgs e)
        {
            clic = false;
        }

        //Affiche la caméra sur l'image
        private void chargerImage(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(camb, 50, 50);
            if (choix == Choix.ligne && linset)
            {
                var g = e.Graphics;
                Pen pen = new Pen(Color.Gray);
                g.DrawLine(pen, linx1, liny1, linx2, liny2);
            }
            if (choix == Choix.rectangle && linset)
            {
                var g = e.Graphics;
                Pen pen = new Pen(Color.Gray);

                if (recy2 >= recy1)
                {
                    if (recx2 >= recx1)
                        g.DrawRectangle(pen, recx1, recy1, recx2 - recx1, recy2 - recy1);
                    else
                        g.DrawRectangle(pen, recx2, recy1, recx1 - recx2, recy2 - recy1);
                }
                else
                {
                    if (recx2 >= recx1)
                        g.DrawRectangle(pen, recx1, recy2, recx2 - recx1, recy1 - recy2);
                    else
                        g.DrawRectangle(pen, recx2, recy2, recx1 - recx2, recy1 - recy2);
                }
            }
            if (choix == Choix.sprite)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Gray), 50 + (actSpritY - posy) * 7, 50 + (actSpritX - posx) * 7, 70, 140);
                if (retourné == false)
                {
                    e.Graphics.DrawImage(actualSprite, 50 + (actSpritY - posy) * 7, 50 + (actSpritX - posx) * 7);
                }
                else
                {
                    e.Graphics.DrawImage(actualSprite, 120 + (actSpritY - posy) * 7, 50 + (actSpritX - posx) * 7, -70, 140);
                }
            }
            if (selectionné)
            {
                for (int o = 0; o < selx / 7; o++)
                    for (int i = 0; i < sely / 7; i++)
                    {
                        Rectangle pixel = new Rectangle(7 * (o + selDepX / 7) + 50, 7 * (i + selDepY / 7) + 50, 7, 7);
                        SolidBrush n = new SolidBrush(selection[i, o]);
                        e.Graphics.FillRectangle(n, pixel);
                        p = new Pen(selection[i, o]);
                        if (grillecol != Color.Black)
                            p = new Pen(grillecol);
                        e.Graphics.DrawRectangle(p, pixel);
                        n.Dispose();
                    }
                p = new Pen(Color.Gray);
                Rectangle selectionr = new Rectangle(selDepX + 50, selDepY + 50, selx, sely);
                e.Graphics.DrawRectangle(p, selectionr);
            }
            if (persDef && posx < personnage.X + 20 && posy < personnage.Y + 10)
            {
                if (personnage.Retourné == false)
                {
                    e.Graphics.DrawImage(personnage.BreathAnim[0], 50 + (personnage.Y - posy) * 7, 50 + (personnage.X - posx) * 7);
                }
                else
                {
                    e.Graphics.DrawImage(personnage.BreathAnim[0], 120 + (personnage.Y - posy) * 7, 50 + (personnage.X - posx) * 7, -70, 140);
                }
            }
        }

        [STAThread]
        static void Main()
        {
            //Lance l'application
            Application.Run(new Fenetre());
        }

        //Déclenché lorsqu'on appuie sur le bouton "PLATEFORME BLANCHE" et ajoute tout les outils a la fenetre
        private void PlateformeBlanche(object sender, EventArgs evt)
        {
            grille.Panel2.Controls.Add(fermer);
            grille.Panel2.Controls.Add(tailplatfrm);
            grille.Panel2.Controls.Add(bar);
            grille.Panel2.Controls.Remove(plateformblanche);
            grille.Panel2.Controls.Remove(sprite);
            grille.Panel2.Controls.Add(ligne);
            grille.Panel2.Controls.Add(effacer);
            grille.Panel2.Controls.Add(grilleAff);
            grille.Panel2.Controls.Add(rectangl);
            grille.Panel2.Controls.Add(select);
            grillecol = Color.FromArgb(53, 53, 53);
            for (int i = 0; i < 100; i++)
                for (int o = 0; o < 180; o++)
                {
                    Graphics g = Graphics.FromImage(camb);
                    Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                    SolidBrush n = new SolidBrush(cam[i, o]);
                    g.FillRectangle(n, pixel);
                    Pen p = new Pen(Color.FromArgb(53, 53, 53));
                    g.DrawRectangle(p, pixel);
                    g.Flush();
                }
            pic.Refresh();
            choix = Choix.platformblanche;
        }

        //Déclenché lorsqu'on clique n'importe où sur l'ecran
        private void click(object e, EventArgs evt)
        {
            clic = true;
        }


        //thread permanent
        private void outilsThread()
        {
            bool selDepSeted = false, posCursSet = false, deplacemt = false, camchargée = false;
            const bool tru = true;
            int posX = 0, posY = 0, selDepX2 = 0, selDepY2 = 0, selx2 = 0, sely2 = 0;
            pic.Paint += new PaintEventHandler(chargerImage);
            while (tru == true)
            {

                //Selection
                if (choix == Choix.selection)
                {
                    if (clic &&
                        MousePosition.X - Location.X > 50 && MousePosition.X - Location.X <= 1400 &&
                        MousePosition.Y - Location.Y > 50 && MousePosition.Y - Location.Y <= 800 &&
                        !(MousePosition.X - Location.X >= 50 + selDepX && MousePosition.X - Location.X <= 50 + selx + selDepX &&
                        MousePosition.Y - Location.Y >= 50 + selDepY && MousePosition.Y - Location.Y <= 50 + selDepY + sely))
                    {
                        if (!selDepSeted)
                        {
                            selDepX2 = selDepX;
                            selDepY2 = selDepY;
                            selDepX = 0;
                            selDepY = 0;
                            selx2 = selx;
                            sely2 = sely;
                            selx = 0;
                            sely = 0;
                            selDepY = (MousePosition.Y - Location.Y - 85);
                            selDepX = (MousePosition.X - Location.X - 60);
                            selDepSeted = true;
                            deplacemt = false;
                            camchargée = false;
                        }
                        sely = (MousePosition.Y - Location.Y - 85) - selDepY;
                        selx = (MousePosition.X - Location.X - 60) - selDepX;
                        selDepY /= 7;
                        selDepY *= 7;
                        selDepX /= 7;
                        selDepX *= 7;
                        sely /= 7;
                        sely *= 7;
                        selx /= 7;
                        selx *= 7;
                        if (selectionné && !deplacemt)
                        {
                            cam = camDeBase;
                            for (int o = selDepX2 / 7; o < (selDepX2 / 7 + selx2 / 7); o++)
                                for (int i = selDepY2 / 7; i < (selDepY2 + sely2) / 7; i++)
                                {
                                    cam[i, o] = selection[i - selDepY2 / 7, o - selDepX2 / 7];
                                }
                            selectionné = false;
                        }
                        if (!camchargée)
                        {
                            if (grillecol == Color.FromArgb(53, 53, 53))
                            {
                                for (int i = 0; i < 100; i++)
                                    for (int o = 0; o < 180; o++)
                                    {
                                        Graphics g = Graphics.FromImage(camb);
                                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                                        SolidBrush n = new SolidBrush(cam[i, o]);
                                        g.FillRectangle(n, pixel);
                                        Pen p = new Pen(Color.FromArgb(53, 53, 53));
                                        g.DrawRectangle(p, pixel);
                                        g.Flush();
                                    }
                            }
                            else
                            if (grillecol == Color.Black)
                            {
                                for (int i = 0; i < 100; i++)
                                    for (int o = 0; o < 180; o++)
                                    {
                                        Graphics g = Graphics.FromImage(camb);
                                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                                        SolidBrush n = new SolidBrush(cam[i, o]);
                                        g.FillRectangle(n, pixel);
                                        Pen p = new Pen(cam[i, o]);
                                        g.DrawRectangle(p, pixel);
                                        g.Flush();
                                    }
                            }
                            camchargée = true;
                        }
                        pic.Invoke(new MethodInvoker(delegate
                    {
                        pic.Refresh();
                    }));
                    }

                    //Déplacement
                    if (!selDepSeted && clic && (MousePosition.X - Location.X >= 50 + selDepX && MousePosition.X - Location.X <= 50 + selx + selDepX && MousePosition.Y - Location.Y >= 50 + selDepY && MousePosition.Y - Location.Y <= 50 + selDepY + sely) && sely != (MousePosition.Y - Location.Y) - selDepY && selx != (MousePosition.X - Location.X) - selDepX)
                    {

                        if (posCursSet == false)
                        {
                            posX = (MousePosition.X - Location.X - 60);
                            posY = (MousePosition.Y - Location.Y - 85);
                            selDepX2 = selDepX;
                            selDepY2 = selDepY;
                            posCursSet = true;
                            deplacemt = true;
                        }
                        selDepY = (MousePosition.Y - Location.Y - 85) - posY + selDepY2;
                        selDepX = (MousePosition.X - Location.X - 60) - posX + selDepX2;
                        selDepY /= 7;
                        selDepY *= 7;
                        selDepX /= 7;
                        selDepX *= 7;
                        sely /= 7;
                        sely *= 7;
                        selx /= 7;
                        selx *= 7;
                        pic.Invoke(new MethodInvoker(delegate
                        {
                            pic.Refresh();
                        }));
                    }

                    //selection : lache de souris
                    if (!clic || !(MousePosition.X - Location.X >= 50 && MousePosition.X - Location.X <= 1400 && MousePosition.Y - Location.Y >= 50 && MousePosition.Y - Location.Y <= 800))
                    {
                        if (selDepSeted)
                        {
                            selDepY /= 7;
                            selDepX /= 7;
                            sely /= 7;
                            selx /= 7;
                            if (sely != 0 && selx != 0)
                            {
                                camDeBase = cam;
                                for (int o = selDepX; o < selDepX + selx; o++)
                                    for (int i = selDepY; i < selDepY + sely; i++)
                                    {
                                        selection[i - selDepY, o - selDepX] = cam[i, o];
                                        camDeBase[i, o] = Color.Black;
                                    }
                                selectionné = true;
                            }
                            selDepY *= 7;
                            selDepX *= 7;
                            sely *= 7;
                            selx *= 7;
                            selDepSeted = false;
                            if (grillecol == Color.FromArgb(53, 53, 53))
                            {
                                for (int i = 0; i < 100; i++)
                                    for (int o = 0; o < 180; o++)
                                    {
                                        Graphics g = Graphics.FromImage(camb);
                                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                                        SolidBrush n = new SolidBrush(cam[i, o]);
                                        g.FillRectangle(n, pixel);
                                        Pen p = new Pen(Color.FromArgb(53, 53, 53));
                                        g.DrawRectangle(p, pixel);
                                        g.Flush();
                                    }
                            }
                            else
                            if (grillecol == Color.Black)
                            {
                                for (int i = 0; i < 100; i++)
                                    for (int o = 0; o < 180; o++)
                                    {
                                        Graphics g = Graphics.FromImage(camb);
                                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                                        SolidBrush n = new SolidBrush(cam[i, o]);
                                        g.FillRectangle(n, pixel);
                                        Pen p = new Pen(cam[i, o]);
                                        g.DrawRectangle(p, pixel);
                                        g.Flush();
                                    }
                            }
                            pic.Invoke(new MethodInvoker(delegate
                            {
                                pic.Refresh();
                            }));
                        }
                        if (posCursSet)
                            posCursSet = false;
                    }

                    //Selection :: changement de curseurs
                    if (!clic && (MousePosition.X - Location.X >= 50 + selDepX && MousePosition.X - Location.X <= 50 + selx + selDepX && MousePosition.Y - Location.Y >= 50 + selDepY && MousePosition.Y - Location.Y <= 50 + selDepY + sely))
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                            Cursor = Cursors.Hand;
                        }));
                    }

                    if (!clic && !(MousePosition.X - Location.X >= 50 + selDepX && MousePosition.X - Location.X <= 50 + selx + selDepX && MousePosition.Y - Location.Y >= 50 + selDepY && MousePosition.Y - Location.Y <= 50 + selDepY + sely))
                        Invoke(new MethodInvoker(delegate
                        {
                            Cursor = Cursors.Default;
                        }));
                }
                else
                    if (selectionné)
                {
                    selDepX = 0;
                    selDepY = 0;
                    selx = 0;
                    sely = 0;
                    selectionné = false;
                }

                //ligne
                if (choix == Choix.ligne)
                {
                    if (clic &&
                    MousePosition.X - Location.X > 50 && MousePosition.X - Location.X < 1400 - 7 * (tailleplatform - 1) &&
                    MousePosition.Y - Location.Y > 50 && MousePosition.Y - Location.Y < 800 - 7 * (tailleplatform - 1))
                    {
                        if (!linset)
                        {
                            liny1 = (MousePosition.Y - Location.Y - 35);
                            linx1 = (MousePosition.X - Location.X - 10);
                            linset = true;

                        }

                        liny2 = (MousePosition.Y - Location.Y - 35);
                        linx2 = (MousePosition.X - Location.X - 10);
                        liny1 /= 7;
                        liny1 *= 7;
                        linx1 /= 7;
                        linx1 *= 7;
                        liny2 /= 7;
                        liny2 *= 7;
                        linx2 /= 7;
                        linx2 *= 7;
                        pic.Invoke(new MethodInvoker(delegate
                        {
                            pic.Refresh();
                        }));
                    }
                    if (!clic && linset)
                    {
                        linset = false;
                    }
                }

                //rectangle

                if (choix == Choix.rectangle)
                {
                    if (clic &&
                    MousePosition.X - Location.X > 50 && MousePosition.X - Location.X < 1400 - 7 * (tailleplatform - 1) &&
                    MousePosition.Y - Location.Y > 50 && MousePosition.Y - Location.Y < 800 - 7 * (tailleplatform - 1))
                    {
                        if (!linset)
                        {
                            recy1 = (MousePosition.Y - Location.Y - 35);
                            recx1 = (MousePosition.X - Location.X - 10);
                            linset = true;

                        }

                        recy2 = (MousePosition.Y - Location.Y - 35);
                        recx2 = (MousePosition.X - Location.X - 10);
                        recy1 /= 7;
                        recy1 *= 7;
                        recx1 /= 7;
                        recx1 *= 7;
                        recy2 /= 7;
                        recy2 *= 7;
                        recx2 /= 7;
                        recx2 *= 7;
                        pic.Invoke(new MethodInvoker(delegate
                        {
                            pic.Refresh();
                        }));
                    }
                    if (!clic && linset)
                    {
                        linset = false;
                    }
                }

            }
        }

        private void drawer()
        {
            while (true)
            {
                //dessin
                if (clic == true &&
                    MousePosition.X - Location.X > 50 && MousePosition.X - Location.X < 1400 - 7 * (tailleplatform - 1) &&
                    MousePosition.Y - Location.Y > 50 && MousePosition.Y - Location.Y < 800 - 7 * (tailleplatform - 1)
                    && (choix == Choix.effacer || choix == Choix.platformblanche))
                {
                    Color couler = new Color();
                    switch (choix)
                    {
                        case Choix.platformblanche:
                            couler = Color.White;
                            break;
                        case Choix.effacer:
                            couler = Color.Black;
                            break;
                    }
                    int y = Location.Y;
                    int X = Location.X;
                    int aY = MousePosition.Y;
                    int aX = MousePosition.X;
                    cam[(aY - y - 85) / 7, (aX - X - 60) / 7] = couler;
                    RectangleF rectf = new RectangleF((aX - X - 60) / 7 * 7 - (int)((tailleplatform - 1) * 7), (aY - y - 85) / 7 * 7 - (int)((tailleplatform - 1) * 7), 14 * (tailleplatform - 1) + 7, 14 * (tailleplatform - 1) + 7);
                    Graphics g = Graphics.FromImage(camb);
                    g.FillRectangle(new SolidBrush(couler), rectf);
                    g.Flush();
                    if (tailleplatform > 1)
                    {
                        for (int o = 1; o <= tailleplatform - 1; o++)
                        {
                            cam[(aY - y - 85) / 7, (aX - X - 60) / 7 + o] = couler;
                            cam[(aY - y - 85) / 7, (aX - X - 60) / 7 - o] = couler;
                            cam[(aY - y - 85) / 7 + o, (aX - X - 60) / 7] = couler;
                            cam[(aY - y - 85) / 7 - o, (aX - X - 60) / 7] = couler;
                            for (int i = 1; i <= tailleplatform - 1; i++)
                            {
                                cam[(aY - y - 85) / 7 + i, (aX - X - 60) / 7 + o] = couler;
                                cam[(aY - y - 85) / 7 - i, (aX - X - 60) / 7 + o] = couler;
                                cam[(aY - y - 85) / 7 + i, (aX - X - 60) / 7 - o] = couler;
                                cam[(aY - y - 85) / 7 - i, (aX - X - 60) / 7 - o] = couler;
                            }
                        }
                    }
                    pic.Invoke(new MethodInvoker(delegate
                    {
                        pic.Refresh();
                    }));
                }
            }
        }


        private void barre(object e, EventArgs evt)
        {
            tailleplatform = bar.Value + 1;
            tailplatfrm.Text = tailleplatform.ToString() + " pixels";
        }

        private void grilleaff(object e, EventArgs evt)
        {
            if (grillecol == Color.Black)
            {
                grillecol = Color.FromArgb(53, 53, 53);
                for (int i = 0; i < 100; i++)
                    for (int o = 0; o < 180; o++)
                    {
                        Graphics g = Graphics.FromImage(camb);
                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                        SolidBrush n = new SolidBrush(cam[i, o]);
                        g.FillRectangle(n, pixel);
                        Pen p = new Pen(Color.FromArgb(53, 53, 53));
                        g.DrawRectangle(p, pixel);
                        g.Flush();
                    }
            }
            else
            if (grillecol == Color.FromArgb(53, 53, 53))
            {
                grillecol = Color.Black;
                for (int i = 0; i < 100; i++)
                    for (int o = 0; o < 180; o++)
                    {
                        Graphics g = Graphics.FromImage(camb);
                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                        SolidBrush n = new SolidBrush(cam[i, o]);
                        g.FillRectangle(n, pixel);
                        Pen p = new Pen(cam[i, o]);
                        g.DrawRectangle(p, pixel);
                        g.Flush();
                    }
            }
            pic.Refresh();
        }


        private void fermerclick(object e, EventArgs evt)
        {
            choix = Choix.nul;
            grille.Panel2.Controls.Remove(fermer);
            grille.Panel2.Controls.Remove(tailplatfrm);
            grille.Panel2.Controls.Remove(bar);
            grille.Panel2.Controls.Add(plateformblanche);
            grille.Panel2.Controls.Add(sprite);
            grille.Panel2.Controls.Remove(effacer);
            grille.Panel2.Controls.Remove(ligne);
            grille.Panel2.Controls.Remove(grilleAff);
            grille.Panel2.Controls.Remove(rectangl);
            grille.Panel2.Controls.Remove(select);
            if (grillecol != Color.Black)
            {
                for (int i = 0; i < 100; i++)
                    for (int o = 0; o < 180; o++)
                    {
                        Graphics g = Graphics.FromImage(camb);
                        Rectangle pixel = new Rectangle(7 * o, 7 * i, 7, 7);
                        SolidBrush n = new SolidBrush(cam[i, o]);
                        g.FillRectangle(n, pixel);
                        Pen p = new Pen(cam[i, o]);
                        g.DrawRectangle(p, pixel);
                        g.Flush();
                    }
            }
            grillecol = Color.Black;
            pic.Refresh();
        }

        private void efface(object sender, EventArgs evt)
        {
            if (choix == Choix.effacer)
            {
                choix = Choix.platformblanche;
                effacer.Text = "Effacer";
            }
            else
            if (choix == Choix.platformblanche)
            {
                choix = Choix.effacer;
                effacer.Text = "Plateforme";
            }
        }
    }
}

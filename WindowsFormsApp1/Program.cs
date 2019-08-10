using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.IO;
using BumpKit;
using System.Windows.Input;

namespace Project5
{
    enum choix
    {
        platformblanche, effacer, nul, selection
    }

    class Fenetre : Form
    {
        private Button fermer = new Button(), effacer = new Button(), grilleAff = new Button(), _frameSuivante = new Button(), _framePrecedente = new Button(), _compiller = new Button(), _playAnim = new Button(), _stopAnim = new Button(), plateformblanche = new Button(), _selection = new Button();
        private Color[,] cam, selection, camDeBase;
        private SplitContainer grille = new SplitContainer();
        private choix choix = choix.nul;
        private TrackBar bar = new TrackBar();
        private PictureBox pic = new PictureBox(), _couleurDessin = new PictureBox();
        private Color grillecol = Color.Black, _dessin = Color.White;
        private bool clic = false, animState = false;
        private string choixdanim = "RESP";
        private int selx = 0, sely = 0, selDepX = 0, selDepY = 0;
        private int _tailleX, _tailleY;
        private TextBox _nom = new TextBox();
        private ListBox listBox = new ListBox(), _listBoxAnim = new ListBox();
        private int _actualFrame = 0, nbOfFrames = 0;
        private Label _frameDisplay = new Label();
        private List<Bitmap> _bitmaps = new List<Bitmap>(), _respiration = new List<Bitmap>(), _marche = new List<Bitmap>(), _saut = new List<Bitmap>(), _death = new List<Bitmap>(), _sneak = new List<Bitmap>(), _run = new List<Bitmap>();
        private bool _respSetBool = false, _marchSetBool = false, _sautSetBool = false, _deathSetBool = false, _sneakSetBool = false, _runSetBool = false, _buttonCompilerAff = false, selectionné = false, thread = true;
        
        public Fenetre(int x, int y)
        {
            _tailleX = x;
            _tailleY = y;
            cam = new Color[2*y, 2*x]; selection = new Color[2*y, 2*x]; camDeBase = new Color[2*y, 2*x];
            listBox.Items.Add("Personnage");
            listBox.Items.Add($"Entitée");
            listBox.Items.Add("Objet fixe");
            listBox.Items.Add("Npc (Mob, Pnj...)");
            listBox.Size = new Size(90, 100);
            listBox.Location = new Point(50, 295);
            listBox.SelectedItem = "Personnage";
            listBox.BackColor = Color.FromArgb(53, 53, 53);
            listBox.ForeColor = Color.White;
            listBox.BorderStyle = BorderStyle.None;

            _listBoxAnim.Items.Add("Respiration");
            _listBoxAnim.Items.Add("Marche");
            _listBoxAnim.Items.Add("Saut");
            _listBoxAnim.Items.Add("Accroupissement");
            _listBoxAnim.Items.Add("Course");
            _listBoxAnim.Items.Add("Mort");
            _listBoxAnim.Size = new Size(90, 100);
            _listBoxAnim.Location = new Point(150, 295);
            _listBoxAnim.SelectedItem = "Respiration";
            _listBoxAnim.BackColor = Color.FromArgb(53, 53, 53);
            _listBoxAnim.ForeColor = Color.White;
            _listBoxAnim.BorderStyle = BorderStyle.None;

            _selection.Location = new Point(50,20);
            _selection.Text = "Déplacer";
            _selection.ForeColor = Color.White;

            _frameSuivante.Location = new Point(175, 100 + 20*y);
            _frameSuivante.Text = "Suivant";
            _frameSuivante.ForeColor = Color.White;

            _compiller.Location = new Point(175, 500);
            _compiller.Text = "Compiller";
            _compiller.ForeColor = Color.White;

            _nom.Location = new Point(50, 500);
            _nom.Text= "Nom...";

            _playAnim.Location = new Point(50, 65 + 20 * y);
            _playAnim.Text = "Jouer";
            _playAnim.ForeColor = Color.White;

            _stopAnim.Location = new Point(175, 65 + 20 * y);
            _stopAnim.Text = "Stop";
            _stopAnim.ForeColor = Color.White;

            _frameDisplay.Text = "1/1";
            _frameDisplay.Location = new Point(140, 100 + 20 * y);
            _frameDisplay.ForeColor = Color.White;

            _framePrecedente.Location = new Point(50, 100 + 20 * y);
            _framePrecedente.Text = "Précédent";
            _framePrecedente.ForeColor = Color.White;

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
            grille.Panel1MinSize = 7*x;
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
            pic.Location = new Point(50, 50);
            pic.Size = new Size(20 * x, 20 * y);
            pic.Paint += new PaintEventHandler(chargerImage);
            for (int i = 0; i < y; i++)
                for (int o = 0; o < x ; o++)
                    cam[i,o] = Color.Black;

            Text = "Team Cumin's Game Engine";
            Size = new Size(300 + 20 * x, 100 + 20 * y);
            MinimumSize = new Size(400 + 20 * x, 200 + 20 * y);
            SuspendLayout();

            bar.ValueChanged += barre;
            plateformblanche.MouseClick += PlateformeBlanche;
            pic.MouseDown += click;
            pic.MouseUp += up;
            fermer.Click += fermerclick;
            _frameSuivante.Click += _frameSuivante_Click;
            _framePrecedente.Click += _framePrecedente_Click; 
            effacer.MouseClick += efface;
            grilleAff.Click += grilleaff;
            _playAnim.Click += _playAnim_Click;
            _stopAnim.Click += _stopAnim_Click;
            _listBoxAnim.SelectedIndexChanged += _listBoxAnim_SelectedIndexChanged;
            _nom.TextChanged += _nom_TextChanged;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            _compiller.Click += _compiller_Click;
            _selection.Click += _selection_Click;

            grille.Panel1.Controls.Add(pic);
            grille.Panel1.Controls.Add(_selection);
            grille.Panel2.Controls.Add(plateformblanche);
            grille.Panel2.Controls.Add(_listBoxAnim);
            grille.Panel2.Controls.Add(listBox);
            grille.Panel2.Controls.Add(_nom);
            grille.Panel1.Controls.Add(_frameSuivante);
            grille.Panel1.Controls.Add(_framePrecedente);
            grille.Panel1.Controls.Add(_playAnim);
            grille.Panel1.Controls.Add(_stopAnim);
            grille.Panel1.Controls.Add(_frameDisplay);
            Controls.Add(grille);
            ResumeLayout(false);
            PerformLayout();
            Thread t = new Thread(new ThreadStart(drawer));
            t.Start();

        }

        private void _selection_Click(object sender, EventArgs e)
        {
            if (choix != choix.selection)
            {
                choix = choix.selection;
                _selection.Text = "Arreter";
            }
            else
            if (choix == choix.selection)
            {
                selDepY /= 20;
                selDepY *= 20;
                selDepX /= 20;
                selDepX *= 20;
                sely /= 20;
                sely *= 20;
                selx /= 20;
                selx *= 20;
                pic.Invoke(new MethodInvoker(delegate
                {
                    pic.Refresh();
                }));
                if (effacer.Text == "Effacer")
                    choix = choix.platformblanche;
                else
                    choix = choix.effacer;
                if (grille.Panel2.Controls.Contains(plateformblanche))
                    choix = choix.nul;
                _selection.Text = "Déplacer";
            }
        }
        private void _compiller_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItem == "Objet fixe")
            {

                Color[] frame = new Color[_tailleY*_tailleX];

                for (int i = 0; i < _tailleX; i++)
                    for (int o = 0; o < _tailleY; o++)
                    {
                        frame[o * _tailleX+ i]  = cam[o, i];
                    }
                int[] frameInt = new int[frame.Length];
                for(int i = 0; i < frame.Length; i++)
                {
                    switch (frame[i].ToString())
                    {
                        case "Color[A = 255, R = 255, G = 255, B = 255]":
                            frameInt[i] = 5;
                            break;
                        case "Color[A = 255, R = 211, G = 211, B = 211]":
                            frameInt[i] = 4;
                            break;
                        case "Color[A = 255, R = 128, G = 128, B = 128]":
                            frameInt[i] = 3;
                            break;
                        case "Color[A = 255, R = 73, G = 73, B = 73]":
                            frameInt[i] = 2;
                            break;
                        case "Color[A = 255, R = 53, G = 53, B = 53]":
                            frameInt[i] = 1;
                            break;
                        case "Color[A = 255, R = 0, G = 0, B = 0]":
                            frameInt[i] = 0;
                            break;
                    }
                }
                    
                Sprite sprite = new Sprite(_nom.Text, frameInt);
                SpriteSetter.Settype(_nom.Text, "objet");
                SpriteSetter.SetDims(_nom.Text, new int[] { _tailleX, _tailleY });
            }
            if (listBox.SelectedItem == "Personnage" || listBox.SelectedItem == "Npc (Mob, Pnj...)")
            {
                switch (choixdanim)
                {
                    case "RESP":
                        _respiration = _bitmaps;
                        break;
                    case "MARCH":
                        _marche = _bitmaps;
                        break;
                    case "SAUT":
                        _saut = _bitmaps;
                        break;
                    case "SNEAK":
                        _sneak = _bitmaps;
                        break;
                    case "RUN":
                        _run = _bitmaps;
                        break;
                    case "DEATH":
                        _death = _bitmaps;
                        break;
                }

                Sprite sprite = new Sprite(_nom.Text);
                FileStream stream1 = new FileStream(_nom.Text + ".json", FileMode.Open);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
                Spritejson s = (Spritejson)ser.ReadObject(stream1);

                List<int[]> liste = new List<int[]>();
                Color[] frame = new Color[_tailleY * _tailleX];
                for (int f = 0; f < _death.Count; f++)
                {
                    for (int i = 0; i < _tailleY; i++)
                        for (int o = 0; o < _tailleX; o++)
                        {
                            frame[o * _tailleX+ i] = _death[f].GetPixel(o, i);
                        }
                    int[] frameInt = new int[frame.Length];
                    for (int i = 0; i < frame.Length; i++)
                    {
                        switch (frame[i].ToString())
                        {
                            case "{\"knownColor\":164,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 5;
                                break;
                            case "{\"knownColor\":95,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 4;
                                break;
                            case "{\"knownColor\":78,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 3;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":2,\"value\":4282992969}":
                                frameInt[i] = 2;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":1,\"value\":4281677109}":
                                frameInt[i] = 1;
                                break;
                            case "{\"knownColor\":35,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 0;
                                break;
                        }
                    }
                    liste.Add(frameInt);
                }
                s.deathAnim = liste;

                liste = new List<int[]>();
                for (int f = 0; f < _respiration.Count; f++)
                {
                    for (int i = 0; i < _tailleY; i++)
                        for (int o = 0; o < _tailleX; o++)
                        {
                            frame[o * _tailleX + i] = _respiration[f].GetPixel(o, i);
                        }
                    int[] frameInt = new int[frame.Length];
                    for (int i = 0; i < frame.Length; i++)
                    {
                        switch (frame[i].ToString())
                        {
                            case "{\"knownColor\":164,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 5;
                                break;
                            case "{\"knownColor\":95,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 4;
                                break;
                            case "{\"knownColor\":78,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 3;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":2,\"value\":4282992969}":
                                frameInt[i] = 2;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":1,\"value\":4281677109}":
                                frameInt[i] = 1;
                                break;
                            case "{\"knownColor\":35,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 0;
                                break;
                        }
                    }
                    liste.Add(frameInt);
                }
                s.breathAnim = liste;

                liste = new List<int[]>();
                for (int f = 0; f < _marche.Count; f++)
                {
                    for (int i = 0; i < _tailleY; i++)
                        for (int o = 0; o < _tailleX; o++)
                        {
                            frame[o * _tailleX + i] = _marche[f].GetPixel(o, i);
                        }
                    int[] frameInt = new int[frame.Length];
                    for (int i = 0; i < frame.Length; i++)
                    {
                        switch (frame[i].ToString())
                        {
                            case "{\"knownColor\":164,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 5;
                                break;
                            case "{\"knownColor\":95,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 4;
                                break;
                            case "{\"knownColor\":78,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 3;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":2,\"value\":4282992969}":
                                frameInt[i] = 2;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":1,\"value\":4281677109}":
                                frameInt[i] = 1;
                                break;
                            case "{\"knownColor\":35,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 0;
                                break;
                        }
                    }
                    liste.Add(frameInt);
                }
                s.walkAnim = liste;

                liste = new List<int[]>();
                for (int f = 0; f < _saut.Count; f++)
                {
                    for (int i = 0; i < _tailleY; i++)
                        for (int o = 0; o < _tailleX; o++)
                        {
                            frame[o * _tailleX + i] = _saut[f].GetPixel(o, i);
                        }
                    int[] frameInt = new int[frame.Length];
                    for (int i = 0; i < frame.Length; i++)
                    {
                        switch (frame[i].ToString())
                        {
                            case "{\"knownColor\":164,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 5;
                                break;
                            case "{\"knownColor\":95,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 4;
                                break;
                            case "{\"knownColor\":78,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 3;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":2,\"value\":4282992969}":
                                frameInt[i] = 2;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":1,\"value\":4281677109}":
                                frameInt[i] = 1;
                                break;
                            case "{\"knownColor\":35,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 0;
                                break;
                        }
                    }
                    liste.Add(frameInt);
                }
                s.jumpAnim = liste;

                liste = new List<int[]>();
                for (int f = 0; f < _sneak.Count; f++)
                {
                    for (int i = 0; i < _tailleY; i++)
                        for (int o = 0; o < _tailleX; o++)
                        {
                            frame[o * _tailleX + i] = _sneak[f].GetPixel(o, i);
                        }
                    int[] frameInt = new int[frame.Length];
                    for (int i = 0; i < frame.Length; i++)
                    {
                        switch (frame[i].ToString())
                        {
                            case "{\"knownColor\":164,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 5;
                                break;
                            case "{\"knownColor\":95,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 4;
                                break;
                            case "{\"knownColor\":78,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 3;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":2,\"value\":4282992969}":
                                frameInt[i] = 2;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":1,\"value\":4281677109}":
                                frameInt[i] = 1;
                                break;
                            case "{\"knownColor\":35,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 0;
                                break;
                        }
                    }
                    liste.Add(frameInt);
                }
                s.sneakAnim = liste;

                liste = new List<int[]>();
                for (int f = 0; f < _run.Count; f++)
                {
                    for (int i = 0; i < _tailleY; i++)
                        for (int o = 0; o < _tailleX; o++)
                        {
                            frame[o * _tailleX + i] = _run[f].GetPixel(o, i);
                        }
                    int[] frameInt = new int[frame.Length];
                    for (int i = 0; i < frame.Length; i++)
                    {
                        switch (frame[i].ToString())
                        {
                            case "{\"knownColor\":164,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 5;
                                break;
                            case "{\"knownColor\":95,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 4;
                                break;
                            case "{\"knownColor\":78,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 3;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":2,\"value\":4282992969}":
                                frameInt[i] = 2;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":1,\"value\":4281677109}":
                                frameInt[i] = 1;
                                break;
                            case "{\"knownColor\":35,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 0;
                                break;
                        }
                    }
                    liste.Add(frameInt);
                }
                s.runAnim = liste;
                stream1.Position = 0;
                ser.WriteObject(stream1, s);
                stream1.Close();
                SpriteSetter.Settype(_nom.Text, listBox.SelectedItem.ToString());
                SpriteSetter.SetDims(_nom.Text, new int[] { _tailleX, _tailleY });

            }

            if (listBox.SelectedItem == "Entitée")
            {

                Sprite sprite = new Sprite(_nom.Text);
                List<int[]> liste = new List<int[]>();
                Color[] frame = new Color[_tailleY * _tailleX];
                for (int f = 0; f < _respiration.Count; f++)
                {
                    for (int i = 0; i < _tailleY; i++)
                        for (int o = 0; o < _tailleX; o++)
                        {
                            frame[o * _tailleX + i] = _respiration[f].GetPixel(o, i);
                        }
                    int[] frameInt = new int[frame.Length];
                    for (int i = 0; i < frame.Length; i++)
                    {
                        switch (frame[i].ToString())
                        {
                            case "{\"knownColor\":164,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 5;
                                break;
                            case "{\"knownColor\":95,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 4;
                                break;
                            case "{\"knownColor\":78,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 3;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":2,\"value\":4282992969}":
                                frameInt[i] = 2;
                                break;
                            case "{\"knownColor\":0,\"name\":null,\"state\":1,\"value\":4281677109}":
                                frameInt[i] = 1;
                                break;
                            case "{\"knownColor\":35,\"name\":null,\"state\":1,\"value\":0}":
                                frameInt[i] = 0;
                                break;
                        }
                    }
                    liste.Add(frameInt);
                }
                SpriteSetter.SetbreathAnim(_nom.Text, liste);
                SpriteSetter.Settype(_nom.Text, "entitée");
                SpriteSetter.SetDims(_nom.Text, new int[] { _tailleX, _tailleY });
            }
        }

        private void _nom_TextChanged(object sender, EventArgs e)
        {
            if ((_respSetBool && _marchSetBool && _sautSetBool && _sneakSetBool && _runSetBool && !_buttonCompilerAff && _deathSetBool && _nom.Text != "Nom..." && _nom.Text != "") || listBox.SelectedItem == "Entitée" || listBox.SelectedItem == "Objet fixe")
                grille.Panel2.Controls.Add(_compiller);
            else
                grille.Panel2.Controls.Remove(_compiller);
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedItem == "Objet fixe")
            {
                grille.Panel2.Controls.Remove(_listBoxAnim);
                grille.Panel1.Controls.Remove(_frameSuivante);
                grille.Panel1.Controls.Remove(_framePrecedente);
                grille.Panel1.Controls.Remove(_playAnim);
                grille.Panel1.Controls.Remove(_stopAnim);
                grille.Panel1.Controls.Remove(_frameDisplay);
                grille.Panel2.Controls.Add(_compiller);
                if (_nom.Text != "Nom..." && _nom.Text != "")
                    grille.Panel2.Controls.Add(_compiller);
                else
                    grille.Panel2.Controls.Remove(_compiller);
            }
            if (listBox.SelectedItem == "Personnage" || listBox.SelectedItem == "Npc (Mob, Pnj...)")
            {
                grille.Panel2.Controls.Add(_listBoxAnim);
                grille.Panel1.Controls.Add(_frameSuivante);
                grille.Panel1.Controls.Add(_framePrecedente);
                grille.Panel1.Controls.Add(_playAnim);
                grille.Panel1.Controls.Add(_stopAnim);
                grille.Panel1.Controls.Add(_frameDisplay);
                if (_respSetBool && _marchSetBool && _sautSetBool && _sneakSetBool && _runSetBool && !_buttonCompilerAff && _nom.Text != "Nom..." && _nom.Text != "")
                    grille.Panel2.Controls.Add(_compiller);
                else
                    grille.Panel2.Controls.Remove(_compiller);
            }
            if (listBox.SelectedItem == "Entitée")
            {
                grille.Panel2.Controls.Remove(_listBoxAnim);
                grille.Panel1.Controls.Add(_frameSuivante);
                grille.Panel1.Controls.Add(_framePrecedente);
                grille.Panel1.Controls.Add(_playAnim);
                grille.Panel1.Controls.Add(_stopAnim);
                grille.Panel1.Controls.Add(_frameDisplay);
                if (_nom.Text != "Nom..." && _nom.Text != "")
                    grille.Panel2.Controls.Add(_compiller);
                else
                    grille.Panel2.Controls.Remove(_compiller);
            }
        }

        private void _listBoxAnim_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(choixdanim)
            {
                case "RESP":
                    _respiration = _bitmaps;
                    break;
                case "MARCH":
                    _marche = _bitmaps;
                    break;
                case "SAUT":
                    _saut = _bitmaps;
                    break;
                case "SNEAK":
                    _sneak = _bitmaps;
                    break;
                case "RUN":
                    _run = _bitmaps;
                    break;
                case "DEATH":
                    _death = _bitmaps;
                    break;
            }
            switch(_listBoxAnim.SelectedItem)
            {
                case "Respiration":
                    choixdanim = "RESP";
                    if(_respSetBool)
                    {
                        _bitmaps = _respiration;
                        for (int a = 0; a < 20; a++)
                            for (int o = 0; o < 10; o++)
                            {
                                cam[a, o] = _bitmaps[_bitmaps.Count-1].GetPixel(o, a);
                            }
                        nbOfFrames = _bitmaps.Count;
                        _actualFrame = _bitmaps.Count - 1;
                    }
                    else
                    {
                        _bitmaps = new List<Bitmap>();
                        nbOfFrames = 0;
                        _actualFrame = 0;
                    }
                    break;
                case "Mort":
                    choixdanim = "DEATH";
                    if (_deathSetBool)
                    {
                        _bitmaps = _death;
                        for (int a = 0; a < 20; a++)
                            for (int o = 0; o < 10; o++)
                            {
                                cam[a, o] = _bitmaps[_bitmaps.Count - 1].GetPixel(o, a);
                            }
                        nbOfFrames = _bitmaps.Count;
                        _actualFrame = _bitmaps.Count - 1;
                    }
                    else
                    {
                        _bitmaps = new List<Bitmap>();
                        nbOfFrames = 0;
                        _actualFrame = 0;
                    }
                    break;
                case "Marche":
                    choixdanim = "MARCH";
                    if (_marchSetBool)
                    {
                        _bitmaps = _marche;
                        for (int a = 0; a < 20; a++)
                            for (int o = 0; o < 10; o++)
                            {
                                cam[a, o] = _bitmaps[_bitmaps.Count - 1].GetPixel(o, a);
                            }
                        nbOfFrames = _bitmaps.Count;
                        _actualFrame = _bitmaps.Count - 1;
                    }
                    else
                    {
                        _bitmaps = new List<Bitmap>();
                        nbOfFrames = 0;
                        _actualFrame = 0;
                    }
                    break;
                case "Saut":
                    choixdanim = "SAUT";
                    if (_sautSetBool)
                    {
                        _bitmaps = _saut;
                        for (int a = 0; a < 20; a++)
                            for (int o = 0; o < 10; o++)
                            {
                                cam[a, o] = _bitmaps[_bitmaps.Count - 1].GetPixel(o, a);
                            }
                        nbOfFrames = _bitmaps.Count;
                        _actualFrame = _bitmaps.Count - 1;
                    }
                    else
                    {
                        _bitmaps = new List<Bitmap>();
                        nbOfFrames = 0;
                        _actualFrame = 0;
                    }
                    break;
                case "Accroupissement":
                    choixdanim = "SNEAK";
                    if (_sneakSetBool)
                    {
                        _bitmaps = _sneak;
                        for (int a = 0; a < 20; a++)
                            for (int o = 0; o < 10; o++)
                            {
                                cam[a, o] = _bitmaps[_bitmaps.Count - 1].GetPixel(o, a);
                            }
                        nbOfFrames = _bitmaps.Count;
                        _actualFrame = _bitmaps.Count - 1;
                    }
                    else
                    {
                        _bitmaps = new List<Bitmap>();
                        nbOfFrames = 0;
                        _actualFrame = 0;
                    }
                    break;
                case "Course":
                    choixdanim = "RUN";
                    if (_runSetBool)
                    {
                        _bitmaps = _run;
                        for (int a = 0; a < 20; a++)
                            for (int o = 0; o < 10; o++)
                            {
                                cam[a, o] = _bitmaps[_bitmaps.Count - 1].GetPixel(o, a);
                            }
                        nbOfFrames = _bitmaps.Count;
                        _actualFrame = _bitmaps.Count - 1;
                    }
                    else
                    {
                        _bitmaps = new List<Bitmap>();
                        nbOfFrames = 0;
                        _actualFrame = 0;
                    }
                    break;
            }
            _frameDisplay.Text = $"{_actualFrame + 1}/{nbOfFrames + 1}";
            pic.Refresh();
        }

        private void _framePrecedente_Click(object sender, EventArgs e)
        {
            Bitmap frame = new Bitmap(_tailleX, _tailleY);
            for (int i = 0; i < _tailleY; i++)
                for (int o = 0; o < _tailleX; o++)
                {
                    frame.SetPixel(o, i, cam[i, o]);
                }
            if (_actualFrame == nbOfFrames)
            {
                _bitmaps.Add(frame);
            }
            else
            {
                _bitmaps[_actualFrame] = frame;
                for (int a = 0; a < _tailleY; a++)
                    for (int o = 0; o < _tailleX; o++)
                    {
                        cam[a, o] = _bitmaps[_actualFrame - 1].GetPixel(o, a);
                    }
            }
            _actualFrame--;
            for (int a = 0; a < _tailleY; a++)
                for (int o = 0; o < _tailleX; o++)
                {
                    cam[a, o] = _bitmaps[_actualFrame].GetPixel(o, a);
                }
            pic.Refresh();
            _frameDisplay.Text = $"{_actualFrame + 1}/{nbOfFrames + 1}";
        }

        private void _stopAnim_Click(object sender, EventArgs e)
        {
            animState = false;
        }

        private void _playAnim_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(anim));
            t.Start();
        }
        
        private async void anim()
        {
            animState = true;
            while (animState)
            {
                for (int i = 0; i < _bitmaps.Count; i++)
                {
                    for (int a = 0; a < _tailleY; a++)
                        for (int o = 0; o < _tailleX; o++)
                        {
                            cam[a, o] = _bitmaps[i].GetPixel(o, a);
                        }
                    pic.Invoke(new MethodInvoker(delegate
                    {
                        pic.Refresh();
                    }));
                    if (!animState)
                        i = _bitmaps.Count - 1;
                    await Task.Delay(200);
                }
            }
            if(_actualFrame != _bitmaps.Count)
                for (int a = 0; a < _tailleY; a++)
                    for (int o = 0; o < _tailleX; o++)
                    {
                        cam[a, o] = _bitmaps[_actualFrame].GetPixel(o, a);
                    }
            else
                for (int a = 0; a < _tailleY; a++)
                    for (int o = 0; o < _tailleX; o++)
                    {
                        cam[a, o] = _bitmaps[_actualFrame - 1].GetPixel(o, a);
                    }
            pic.Invoke(new MethodInvoker(delegate
            {
                pic.Refresh();
            }));
        }

        private void _frameSuivante_Click(object sender, EventArgs e)
        {            
            switch (choixdanim)
            {
                case "RESP":
                    _respSetBool = true;
                    break;
                case "MARCH":
                    _marchSetBool = true;
                    break;
                case "SAUT":
                    _sautSetBool = true;
                    break;
                case "SNEAK":
                    _sneakSetBool = true;
                    break;
                case "RUN":
                    _runSetBool = true;
                    break;
                case "DEATH":
                    _deathSetBool = true;
                    break;
            }
            if((_respSetBool && _marchSetBool && _sautSetBool && _sneakSetBool && _runSetBool && _deathSetBool && !_buttonCompilerAff && _nom.Text != "Nom..." && _nom.Text != "") || listBox.SelectedItem == "Entitée")
            {
                grille.Panel2.Controls.Add(_compiller);
            }
            Bitmap frame = new Bitmap(_tailleX, _tailleY);
            for (int i = 0; i < _tailleY; i++)
                for (int o = 0; o < _tailleX; o++)
                {
                    frame.SetPixel(o, i, cam[i, o]);
                }
            if (_actualFrame == nbOfFrames)
            {
                _bitmaps.Add(frame);
                nbOfFrames++;
                listBox.Items.Remove("Objet fixe");
            }
            else
            {
                _bitmaps[_actualFrame] = frame;
                for (int a = 0; a < _tailleY; a++)
                    for (int o = 0; o < _tailleX; o++)
                    {
                        cam[a, o] = _bitmaps[_actualFrame + 1].GetPixel(o, a);
                    }
            }
            _actualFrame++;
            pic.Refresh();
            _frameDisplay.Text = $"{_actualFrame + 1}/{nbOfFrames + 1}";
        }

        //Affiche la caméra sur l'image
        private void chargerImage(object sender, PaintEventArgs e)
        {
            if (!selectionné)
                for (int i = 0; i < _tailleY; i++)
                    for (int o = 0; o < _tailleX; o++)
                    {
                        Rectangle pixel = new Rectangle(20 * o, 20 * i, 20, 20);
                        SolidBrush n = new SolidBrush(cam[i, o]);
                        e.Graphics.FillRectangle(n, pixel);
                        Pen p = new Pen(cam[i, o]);
                        if (grillecol != Color.Black)
                            p = new Pen(grillecol);
                        e.Graphics.DrawRectangle(p, pixel);
                        n.Dispose();
                    }
            if (choix == choix.selection && selx != 0 && sely != 0)
            {
                Rectangle selection = new Rectangle(selDepX, selDepY, selx, sely);
                Pen p = new Pen(Color.White);
                e.Graphics.DrawRectangle(p, selection);
            }
            if(selectionné)
            {
                Rectangle selectionr = new Rectangle(selDepX, selDepY, selx, sely);
                Pen p = new Pen(Color.White);
                for (int i = 0; i < _tailleY; i++)
                    for (int o = 0; o < _tailleX; o++)
                    {
                        Rectangle pixel = new Rectangle(20 * o, 20 * i, 20, 20);
                        SolidBrush n = new SolidBrush(camDeBase[i, o]);
                        e.Graphics.FillRectangle(n, pixel);
                        p = new Pen(camDeBase[i, o]);
                        if (grillecol != Color.Black)
                            p = new Pen(grillecol);
                        e.Graphics.DrawRectangle(p, pixel);
                        n.Dispose();
                    }
                for (int o = 0; o < selx/20; o++)
                    for (int i = 0; i < sely/20; i++)
                    {
                        Rectangle pixel = new Rectangle(20 * (o + selDepX / 20), 20 * (i + selDepY / 20), 20, 20);
                        SolidBrush n = new SolidBrush(selection[i, o]);
                        e.Graphics.FillRectangle(n, pixel);
                        p = new Pen(selection[i, o]);
                        if (grillecol != Color.Black)
                            p = new Pen(grillecol);
                        e.Graphics.DrawRectangle(p, pixel);
                        n.Dispose();
                    }
                p = new Pen(Color.White);
                e.Graphics.DrawRectangle(p, selectionr);
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
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            Int _tailleX = new Int(10), _tailleY = new Int(20);
            var prefen = new PreFenetre(_tailleX,_tailleY);
            Application.Run(prefen);
            //return;
            
            var fen = new Fenetre(_tailleX.valeur, _tailleY.valeur);
            Application.Run(fen);
            /*fen.FormClosing += (x, y) =>
            {
                tcs.SetResult(true);
            };
            tcs = new TaskCompletionSource<bool>();
            tcs.Task.GetAwaiter().GetResult();*/
            fen.thread = false;
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
            bool selDepSeted = false, posCursSet = false, deplacemt = false;
            int posX = 0, posY = 0, selDepX2 = 0, selDepY2 = 0, selx2 = 0, sely2 = 0;
            pic.Paint += new PaintEventHandler(chargerImage);
            while (thread)
            {
                //dessin
                if (clic == true && MousePosition.X - Location.X > 60 && MousePosition.X - Location.X < 60 + 20 * _tailleX && MousePosition.Y - Location.Y > 80 && MousePosition.Y - Location.Y < 80 + 20 * _tailleY && choix != choix.selection)
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
                    cam[(MousePosition.Y - Location.Y - 80) / 20, (MousePosition.X - Location.X - 60) / 20] = couler;

                    pic.Invoke(new MethodInvoker(delegate
                    {
                        pic.Refresh();
                    }));
                }

                //Selection
                if (choix == choix.selection)
                {
                    if (clic && MousePosition.X - Location.X > 60 && MousePosition.X - Location.X <= 65 + 20 * _tailleX &&
                        MousePosition.Y - Location.Y > 80 && MousePosition.Y - Location.Y <= 85 + 20 * _tailleY &&
                        !(MousePosition.X - Location.X >= 60 + selDepX && MousePosition.X - Location.X <= 60 + selx + selDepX &&
                        MousePosition.Y - Location.Y >= 80 + selDepY && MousePosition.Y - Location.Y <= 80 + selDepY + sely)) 
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
                            selDepY = (MousePosition.Y - Location.Y - 80);
                            selDepX = (MousePosition.X - Location.X - 60);
                            selDepSeted = true;
                            deplacemt = false;
                        }
                        sely = (MousePosition.Y - Location.Y - 80) - selDepY;
                        selx = (MousePosition.X - Location.X - 60) - selDepX;
                        selDepY /= 20;
                        selDepY *= 20;
                        selDepX /= 20;
                        selDepX *= 20;
                        sely /= 20;
                        sely *= 20;
                        selx /= 20;
                        selx *= 20;
                        if (selectionné && !deplacemt)
                        {
                            cam = camDeBase;
                            for (int o = selDepX2 / 20; o < (selDepX2 / 20 + selx2 / 20); o++)
                                for (int i = selDepY2 / 20; i < (selDepY2 + sely2) / 20; i++)
                                {
                                    cam[i, o] = selection[i - selDepY2 / 20, o - selDepX2 / 20];
                                }
                            selectionné = false;
                        }

                        pic.Invoke(new MethodInvoker(delegate
                        {
                            pic.Refresh();
                        }));
                    }

                    //Déplacement
                    if (!selDepSeted && clic && (MousePosition.X - Location.X >= 60 + selDepX && MousePosition.X - Location.X <= 60 + selx + selDepX && MousePosition.Y - Location.Y >= 85 + selDepY && MousePosition.Y - Location.Y <= 85 + selDepY + sely) && sely != (MousePosition.Y - Location.Y - 85) - selDepY && selx != (MousePosition.X - Location.X - 60) - selDepX)
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
                        selDepY /= 20;
                        selDepY *= 20;
                        selDepX /= 20;
                        selDepX *= 20;
                        sely /= 20;
                        sely *= 20;
                        selx /= 20;
                        selx *= 20;
                        pic.Invoke(new MethodInvoker(delegate
                        {
                            pic.Refresh();
                        }));
                    }

                    //selection : lache de souris
                    if (!clic /*|| !(MousePosition.X - Location.X >= 60 && MousePosition.X - Location.X <= 260 && MousePosition.Y - Location.Y >= 80 && MousePosition.Y - Location.Y <= 480)*/)
                    {
                        if (selDepSeted)
                        {
                            selDepY /= 20;
                            selDepX /= 20;
                            sely /= 20;
                            selx /= 20;
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
                            selDepY *= 20;
                            selDepX *= 20;
                            sely *= 20;
                            selx *= 20;
                            selDepSeted = false;
                            pic.Invoke(new MethodInvoker(delegate
                            {
                                pic.Refresh();
                            }));
                        }
                        if (posCursSet)
                            posCursSet = false;
                    }

                    //Selection :: changement de curseurs
                    if (!clic && choix == choix.selection && (MousePosition.X - Location.X >= 60 + selDepX && MousePosition.X - Location.X <= 60 + selx + selDepX && MousePosition.Y - Location.Y >= 85 + selDepY && MousePosition.Y - Location.Y <= 85 + selDepY + sely))
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                            Cursor = Cursors.Hand;
                        }));
                    }

                    if (!clic && !(MousePosition.X - Location.X >= 60 + selDepX && MousePosition.X - Location.X <= 60 + selx + selDepX && MousePosition.Y - Location.Y >= 85 + selDepY && MousePosition.Y - Location.Y <= 85 + selDepY + sely))
                        Invoke(new MethodInvoker(delegate
                        {
                            Cursor = Cursors.Default;
                        }));
                }
                //changement de choix
                if (choix != choix.selection)
                {
                    selDepX = 0;
                    selDepY = 0;
                    selx = 0;
                    sely = 0;
                    selectionné = false;
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

    class Int
    {
        public int valeur { get; set; }
        public Int(int i)
        {
            valeur = i;
        }
    }

    class PreFenetre : Form
    {
        private Button _valider;
        private Label _taillePixX, _taillePixY, _state;
        private TextBox _pixX, _pixY;
        private Int x, y;

        public PreFenetre(Int PixelsX, Int PixelsY)
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            x = PixelsX;
            y = PixelsY;
            _state = new Label()
            {
                ForeColor = Color.Red,
                Text = "*Veuillez entrer des valeurs valides, entre 1 et 60 pour valeur horizontalle et entre 1 et 40 pour valeur verticalle.",
                Size = new Size(150, 70),
                Location = new Point(55, 90)
            };
            _taillePixX = new Label()
            {
                Location = new Point(10, 10),
                Size = new Size(120, 30),
                ForeColor = Color.White,
                Text = "Taille horizontale de l'entité en pixel :"
            };

            _taillePixY = new Label()
            {
                Location = new Point(150, 10),
                Size = new Size(100, 30),
                ForeColor = Color.White,
                Text = "Taille verticale de l'entité en pixel :"
            };

            _pixX = new TextBox()
            {
                Text = "10",
                Location = new Point(10, 40)
            };

            _pixY = new TextBox()
            {
                Text = "20",
                Location = new Point(150, 40)
            };

            _valider = new Button()
            {
                Text = "Valider",
                Location = new Point(100, 70),
                Size = new Size(60, 20),
                ForeColor = Color.LightGray
            };
            _valider.Click += _valider_Click; 
            BackColor = Color.FromArgb(53, 53, 53);
            Size = new Size(270, 200);
            SuspendLayout();
            Controls.Add(_taillePixX);
            Controls.Add(_taillePixY);
            Controls.Add(_pixX);
            Controls.Add(_pixY);
            Controls.Add(_valider);
            ResumeLayout(false);
            PerformLayout();
        }

        private void _valider_Click(object sender, EventArgs e)
        {
            int i = 0;
            if (!int.TryParse(_pixX.Text, out i))
                Controls.Add(_state);
            else if (!(i > 0 && i < 61))
                Controls.Add(_state);
            else if (!int.TryParse(_pixY.Text, out i))
                Controls.Add(_state);
            else if (!(i > 0 && i < 41))
                Controls.Add(_state);
            else
            {
                int f, g;
                int.TryParse(_pixX.Text, out f);
                int.TryParse(_pixY.Text, out g);
                x.valeur = f;
                y.valeur = g;
                Close();
            }

        }
    }
}
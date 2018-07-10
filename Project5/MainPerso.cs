using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Project5
{
    namespace objet
    {
        class MainPerso : objeet
        {
            private int xpos, ypos;
            public MainPerso(int posy, int posx)
            {
                xpos = posx; ypos = posy;
            }
        }
    }
}

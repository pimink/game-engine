using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Project5
{
    namespace objet
    {
        abstract class objet
        {
            private Color[,] PFrame;
            private Bitmap Frame;
            public Color GetPixel(int x, int y)
            {
                return PFrame[x, y];
            }
            private void chargerFrame()
            {
                for(int i = 0; i < Frame.Height; i++)
                    for(int o = 0; o< Frame.Width; i++)
                    {
                        PFrame[i,o] = Frame.GetPixel(i,o);
                    }
            }
        }
    }
}

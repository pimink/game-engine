using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Project5
{
    namespace objet
    {
        class Plateformecs : objeet
        {
            private bool solid;
            public Plateformecs(Color couleur)
            {
                SetPixel(couleur);
                if (couleur != Color.Black)
                    solid = true;
            }

            public bool IsSolid()
            {
                return solid;
            }
        }
    }
}

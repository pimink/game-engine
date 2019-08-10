using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project5
{
    class Entite
    {
        public Entite()
        {
            BreathAnim = new List<Bitmap>(); DeathAnim = new List<Bitmap>(); WalkAnim = new List<Bitmap>(); JumpAnim = new List<Bitmap>(); SneakAnim = new List<Bitmap>(); RunAnim = new List<Bitmap>(); X = 0; Y = 0; Gen = 0; Type = "";
        }

        public Entite(List<Bitmap> breath_Anim, List<Bitmap> death_Anim, List<Bitmap> walk_Anim, List<Bitmap> jump_Anim, List<Bitmap> sneak_Anim, List<Bitmap> run_Anim, int posX, int posY, int generation, string type, bool retourné)
        {
            BreathAnim = breath_Anim; DeathAnim = death_Anim; WalkAnim = walk_Anim; JumpAnim = jump_Anim; SneakAnim = sneak_Anim; RunAnim = run_Anim; X = posX; Y = posY;Gen = generation; this.Type = type; Retourné = retourné ;
        }

        public List<Bitmap> BreathAnim { get; set; }
        public List<Bitmap> DeathAnim { get; set; }
        public List<Bitmap> WalkAnim { get; set; }
        public List<Bitmap> JumpAnim { get; set; }
        public List<Bitmap> SneakAnim { get; set; }
        public List<Bitmap> RunAnim { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Gen { get; set; }
        public string Type { get; set; }
        public bool Retourné { get; set; }
    }
}

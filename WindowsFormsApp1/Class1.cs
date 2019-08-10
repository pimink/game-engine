using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Project5
{
    public class Sprite
    { 
        private string type;
        private int gen;
        private List<int[]> breathAnim;
        private List<int[]> walkAnim;
        private List<int[]> jumpAnim;
        private List<int[]> sneakAnim;
        private List<int[]> runAnim;

        public Sprite(string name)
        {
            type = ""; gen = 1; breathAnim = new List<int[]>(); walkAnim = new List<int[]>(); jumpAnim = new List<int[]>(); sneakAnim = new List<int[]>(); runAnim = new List<int[]>();
            Spritejson j = new Spritejson();
            j.name = name;
            j.type = type;
            j.gen = gen;
            j.breathAnim = breathAnim;
            j.walkAnim = walkAnim;
            j.jumpAnim = jumpAnim;
            j.sneakAnim = sneakAnim;
            j.runAnim = runAnim;
            j.deathAnim = new List<int[]>();
            FileStream stream = new FileStream(name + ".json", FileMode.OpenOrCreate);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            ser.WriteObject(stream, j);
            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            stream.Close();
            sr.Close();
        }

        public Sprite(string name, int[] frame)
        {
            type = ""; gen = 1; breathAnim = new List<int[]>(); walkAnim = new List<int[]>(); jumpAnim = new List<int[]>(); sneakAnim = new List<int[]>(); runAnim = new List<int[]>();

            Spritejson j = new Spritejson();
            j.name = name;
            j.type = type;
            j.gen = gen;
            j.breathAnim = breathAnim;
            j.breathAnim.Add(frame);
            j.walkAnim = walkAnim;
            j.jumpAnim = jumpAnim;
            j.sneakAnim = sneakAnim;
            j.dimensions = new int[] { 0, 0 };
            j.runAnim = runAnim;
            j.deathAnim = new List<int[]>();
            FileStream stream = new FileStream(name + ".json", FileMode.OpenOrCreate);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            ser.WriteObject(stream, j);
            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            stream.Close();
            sr.Close();
        }
    }

    [DataContract]
    internal class Spritejson
    {
        [DataMember]
        internal string name;
        [DataMember]
        internal string type;
        [DataMember]
        internal int gen;
        [DataMember]
        internal int[] dimensions;
        [DataMember]
        internal List<int[]> breathAnim;
        [DataMember]
        internal List<int[]> walkAnim;
        [DataMember]
        internal List<int[]> jumpAnim;
        [DataMember]
        internal List<int[]> deathAnim;
        [DataMember]
        internal List<int[]> sneakAnim;
        [DataMember]
        internal List<int[]> runAnim;
    }
    public static class SpriteGetter
    {
        public static string Getname(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.name;
        }
        public static string Gettype(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.type;
        }
        public static int Getgen(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.gen;
        }
        public static List<int[]> GetbreathAnim(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.breathAnim;
        }
        public static List<int[]> GetwalkAnim(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.walkAnim;
        }
        public static List<int[]> GetjumpAnim(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.jumpAnim;
        }
        public static List<int[]> GetsneakAnim(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.sneakAnim;
        }
        public static List<int[]> GetdeathAnim(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.deathAnim;
        }
        public static List<int[]> GetrunAnim(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.runAnim;
        }
    }
    public static class SpriteSetter
    {
        public static void Setname(string name, string objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.name = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void SetDims(string name, int[] objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.dimensions = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void Settype(string name, string objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.type = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void Setgen(string name, int objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.gen = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void SetbreathAnim(string name, List<int[]> objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.breathAnim = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
            
        }
        public static void SetdeathAnim(string name, List<int[]> objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.deathAnim = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void SetwalkAnim(string name, List<int[]> objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.walkAnim = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void SetjumpAnim(string name, List<int[]> objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.jumpAnim = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void SetsneakAnim(string name, List<int[]> objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.sneakAnim = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void SetrunAnim(string name, List<int[]> objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.runAnim = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
    }
}

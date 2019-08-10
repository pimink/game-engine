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
        private List<Color[]> breathAnim;
        private List<Color[]> walkAnim;
        private List<Color[]> jumpAnim;
        private List<Color[]> sneakAnim;
        private List<Color[]> runAnim;

        public Sprite(string name)
        {
            type = ""; gen = 1; breathAnim = new List<Color[]>(); walkAnim = new List<Color[]>(); jumpAnim = new List<Color[]>(); sneakAnim = new List<Color[]>(); runAnim = new List<Color[]>();
            Spritejson j = new Spritejson();
            j.name = name;
            j.type = type;
            j.gen = gen;
            j.breathAnim = breathAnim;
            j.walkAnim = walkAnim;
            j.jumpAnim = jumpAnim;
            j.sneakAnim = sneakAnim;
            j.runAnim = runAnim;
            j.deathAnim = new List<Color[]>();
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
        internal List<Color[]> breathAnim;
        [DataMember]
        internal List<Color[]> walkAnim;
        [DataMember]
        internal List<Color[]> jumpAnim;
        [DataMember]
        internal List<Color[]> deathAnim;
        [DataMember]
        internal List<Color[]> sneakAnim;
        [DataMember]
        internal List<Color[]> runAnim;
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
        public static List<Color[]> GetbreathAnim(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.breathAnim;
        }
        public static List<Color[]> GetwalkAnim(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.walkAnim;
        }
        public static List<Color[]> GetjumpAnim(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.jumpAnim;
        }
        public static List<Color[]> GetsneakAnim(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.sneakAnim;
        }
        public static List<Color[]> GetdeathAnim(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            stream.Close();
            return s.deathAnim;
        }
        public static List<Color[]> GetrunAnim(string name)
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
        public static void SetbreathAnim(string name, List<Color[]> objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.breathAnim = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
            
        }
        public static void SetdeathAnim(string name, List<Color[]> objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.deathAnim = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void SetwalkAnim(string name, List<Color[]> objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.walkAnim = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void SetjumpAnim(string name, List<Color[]> objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.jumpAnim = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void SetsneakAnim(string name, List<Color[]> objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Spritejson));
            Spritejson s = (Spritejson)ser.ReadObject(stream);
            s.sneakAnim = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void SetrunAnim(string name, List<Color[]> objet)
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

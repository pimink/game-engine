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
    public class objetFixe
    {
        private string nom;
        private int gen;
        private int[] bitmap;
        private string type;

        public objetFixe(string name, int[] obj)
        {
            nom = name; gen = 1; bitmap = obj; type = "";
            objetFixejson j = new objetFixejson();
            j.nom = nom;
            j.gen = gen;
            j.bitmap = bitmap;
            j.type = type;
            FileStream stream = new FileStream(name + ".json", FileMode.OpenOrCreate);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(objetFixejson));
            ser.WriteObject(stream, j);
            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            Console.Write("JSON form of objetFixe object: ");
            Console.WriteLine(sr.ReadToEnd());
            stream.Close();
            sr.Close();
        }
    }

    [DataContract]
    internal class objetFixejson
    {
        [DataMember]
        internal string nom;
        [DataMember]
        internal int gen;
        [DataMember]
        internal int[] bitmap;
        [DataMember]
        internal string type;
    }
    public static class objetFixeGetter
    {
        public static string Getnom(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(objetFixejson));
            objetFixejson s = (objetFixejson)ser.ReadObject(stream);
            stream.Close();
            return s.nom;
        }
        public static int Getgen(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(objetFixejson));
            objetFixejson s = (objetFixejson)ser.ReadObject(stream);
            stream.Close();
            return s.gen;
        }
        public static int[] Getbitmap(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(objetFixejson));
            objetFixejson s = (objetFixejson)ser.ReadObject(stream);
            stream.Close();
            return s.bitmap;
        }
        public static string Gettype(string name)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(objetFixejson));
            objetFixejson s = (objetFixejson)ser.ReadObject(stream);
            stream.Close();
            return s.type;
        }
    }
    public static class objetFixeSetter
    {
        public static void Setnom(string name, string objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(objetFixejson));
            objetFixejson s = (objetFixejson)ser.ReadObject(stream);
            s.nom = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void Setgen(string name, int objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(objetFixejson));
            objetFixejson s = (objetFixejson)ser.ReadObject(stream);
            s.gen = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void Setbitmap(string name, int[] objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(objetFixejson));
            objetFixejson s = (objetFixejson)ser.ReadObject(stream);
            s.bitmap = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
        public static void Settype(string name, string objet)
        {
            FileStream stream = new FileStream(name + ".json", FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(objetFixejson));
            objetFixejson s = (objetFixejson)ser.ReadObject(stream);
            s.type = objet;
            stream.Position = 0;
            ser.WriteObject(stream, s);
            stream.Close();
        }
    }
}

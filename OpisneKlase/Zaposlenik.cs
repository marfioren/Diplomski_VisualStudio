using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerapijaRaspored.OpisneKlase
{
     public class Zaposlenik
    {
        private int Id;
        private String Ime;
        private String Prezime;
        private String Auto;
        private String Registracija;

        public Zaposlenik(int id, string ime, string prezime, string auto, string registracija)
        {
            Id = id;
            Ime = ime;
            Prezime = prezime;
            Auto = auto;
            Registracija = registracija;
        }


        public string Prezime1 { get => Prezime; }
        public int Id1 { get => Id; }
        public string Ime1 { get => Ime; }
        public string Auto1 { get => Auto; }
        public string Registracija1 { get => Registracija; }
    }
}
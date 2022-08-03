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
    public class Klijent
    {
        private String id;
        private String ime;
        private String prezime;
        private String Adresa;
        private String Lat;
        private String Lon;

        public Klijent(string id, string ime, string prezime, string adresa, string lat, string lon)
        {
            this.Id = id;
            this.Ime = ime;
            this.Prezime = prezime;
            this.Adresa1 = adresa;
            this.Lat1 = lat;
            this.Lon1 = lon;
        }

        public string Id { get => id; set => id = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public string Adresa1 { get => Adresa; set => Adresa = value; }
        public string Lat1 { get => Lat; set => Lat = value; }
        public string Lon1 { get => Lon; set => Lon = value; }
    }
}
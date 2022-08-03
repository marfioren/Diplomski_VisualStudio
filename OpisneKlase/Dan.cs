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
    public class Dan
    {
        private String Id;
        private String Datum;
        private List<Klijent> Popis = new List<Klijent>();
        private String Km;
        private int Mjesec;
        private int Godina;
        private String Pstanje;
        private String Zstanje;

        public Dan(String id, String datum, String pstanje, String zstanje, String km, List<Klijent> popis, int mjesec, int godina)
        {
            Id1 = id;
            Datum1 = datum;
            Popis1 = popis;
            Km1 = km;
            Mjesec1 = mjesec;
            Godina1 = godina;
            Pstanje1 = pstanje;
            Zstanje1 = zstanje;
        }

        public string Id1 { get => Id; set => Id = value; }
        public string Datum1 { get => Datum; set => Datum = value; }
        public List<Klijent> Popis1 { get => Popis; set => Popis = value; }
        public string Km1 { get => Km; set => Km = value; }
        public int Mjesec1 { get => Mjesec; set => Mjesec = value; }
        public int Godina1 { get => Godina; set => Godina = value; }
        public string Pstanje1 { get => Pstanje; set => Pstanje = value; }
        public string Zstanje1 { get => Zstanje; set => Zstanje = value; }
    }
}
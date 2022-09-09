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
using TerapijaRaspored.OpisneKlase;

namespace TerapijaRaspored.Baza
{
    public sealed class BazaPod
    {
        private List<Klijent> sviKlijenti;
        private List<Dan> sviDani;
        private static BazaPod instanca = null;
        private Context context;
        private int stavljeno = 0;
        private Zaposlenik UlogiraniZaposlenik;
        public int Stavljeno { get => stavljeno;}
        internal Zaposlenik UlogiraniZaposlenik1 { get => UlogiraniZaposlenik; }

        public List<Klijent> getSviKlijenti() { return sviKlijenti; }
        public List<Dan> getSviDani() { return sviDani; }

        public static BazaPod getInstanca()
        {
            if (instanca == null)
            {
                instanca = new BazaPod();
            }
            return instanca;
        }

        private BazaPod()
        {
            sviKlijenti = new List<Klijent>();
            sviDani = new List<Dan>();
        }

        public void Odjava() {

            instanca = new BazaPod();
        }

        public void PrijaviZaposlenika(String PodaciZaposlenika)
        {
            List<String> polje = new List<String>(PodaciZaposlenika.Split('#', 5));
            UlogiraniZaposlenik = new Zaposlenik(Convert.ToInt32(polje[0]),polje[1], polje[2], polje[3], polje[4]);

        }
            public void StaviSveKlijente(List<String> SviKlijenti)
        {
            List<String> podaci = new List<String>();
            podaci = SviKlijenti;           
            for (int i = 0; i < podaci.Count-1; i++)
            {
                List<String> polje = new List<String>(podaci[i].Split('#', 6));
                Klijent k = new Klijent(polje[0], polje[1], polje[2], polje[3], polje[4], polje[5]);
                this.sviKlijenti.Add(k);                               
            }
           stavljeno = 1;
        }

        public void StaviSveDane(List<String> SviDani)
        {
            List<String> podaci = new List<String>();
            podaci = SviDani;
            for (int i = 0; i < podaci.Count-1; i++)
            {
                List<String> polje = new List<String>(podaci[i].Split('#', 8));               
                List<String> list = new List<String>(polje[5].Split(','));
                
                List<Klijent> dodaniKorisnici = new List<Klijent>();
                for (int j = 0; j < list.Count; j++)
                {
                    
                    Klijent k = new Klijent("", "", "", "", "", "");
                    int r = 0;
                    Boolean ok = false;
                    while (ok = false || r < sviKlijenti.Count)
                    {
                        String t = sviKlijenti[r].Id;
                        String s = list[j];
                       
                        if (s.Equals(t))
                        {
                            
                            k = sviKlijenti[r];
                            dodaniKorisnici.Add(k);
                            ok = true;
                        }
                        r++;

                    }
                }
                
                Dan d = new Dan(polje[0], polje[1], polje[2], polje[3], polje[4], dodaniKorisnici, Int32.Parse(polje[6]), Int32.Parse(polje[7]));
                sviDani.Add(d);
            }
            stavljeno = 2;
        }

        public Dan provjeriDatum(string datum) {
            for (int i = 0; i < sviDani.Count; i++)
            {
                string dat = sviDani[i].Datum1;
                if (datum==dat)
                {
                    return sviDani[i];
                }
            }
            List<Klijent> popis = new List<Klijent>();
            Dan d = new Dan("test", datum, "0", "0", "0", popis, 11, 2021);
            sviDani.Add(d);
            return d;

        }

        public void updateDan(string datum, string km, List<Klijent> popis) {
            int i = 0;
            Boolean ok = false;
            while (ok = false || i < sviDani.Count)
            {
                if (datum==sviDani[i].Datum1)
                {
                    sviDani[i].Km1=km;
                    sviDani[i].Popis1=popis;
                    ok = true;
                }
                i++;
            }


        }

    }
}
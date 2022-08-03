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
namespace TerapijaRaspored.Baza
{
    public class Podaci : Async
    {
        private Context context;
        private int zadatak;
        private string url;
        private List<String> rez = new List<String>();
        public Podaci(Context context, int zadatak)
        {
            this.context = context;
            this.zadatak = zadatak;
        }
        protected override void DoInBackground()
        {
            Konekcija con = new Konekcija();
            con.Zadatak = zadatak;        
            switch (this.zadatak)
                {
                    case 1:                        
                        rez.Add(con.DohvatiPodatke(url));
                        break;
                    case 2:                       
                        String podaci = con.DohvatiPodatke(url);
                        String d = con.SviKlijenti(podaci);
                        rez.Add(d);
                        break;
                    case 3:
                        String podaci2 = con.DohvatiPodatke(url);
                        List<String> dani = con.SviDani(podaci2);
                        rez = dani;
                        break;

            }

            }

        protected override void PreExecute(String link)
        {
            url = link;
        }

        protected override string  onPostExecute()
        {
            return "";
        }

        public List<String> getRez() {
            return rez;
        }
    }
}
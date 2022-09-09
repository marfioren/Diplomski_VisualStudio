using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TerapijaRaspored.Baza
{
    class Konekcija
    {
        private int zadatak;
        public String SviKlijenti(String podaci)
        {           
            List<String> razdvojeniPodaci = new List<String>();
            razdvojeniPodaci = RazdvojiPodatke(podaci);           
            BazaPod con = BazaPod.getInstanca();
            if (con.Stavljeno == 0)
            {
                con.StaviSveKlijente(razdvojeniPodaci);
            }
            String t = "OK";
            return t;
        }
        public List<String> SviDani(String podaci)
        {
            List<String> razdvojeniPodaci = new List<String>();
            if (podaci.Length != 0)
            {
                razdvojeniPodaci = RazdvojiPodatke(podaci);
            }
            return razdvojeniPodaci;
        }
        public String DohvatiPodatke(String link)
        {
            String odgovor = "test";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream(); 
                StreamReader reader = new StreamReader(dataStream); 
                string responseFromServer = reader.ReadToEnd();
                odgovor = responseFromServer;
            }
            catch (Exception e)
            {
                odgovor= "error";
            }
            return odgovor;
        }

        private List<String> RazdvojiPodatke(String podaci)
        {
            List<String> razdvojeniPodaci = new List<String>(podaci.Split("///"));
            return razdvojeniPodaci;

        }
        public int Zadatak { set => zadatak = value; }
    }
}
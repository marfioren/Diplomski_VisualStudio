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
using System.Threading.Tasks;
namespace TerapijaRaspored.Kilometri
{
    public class KorisniciLokacije
    {
        private string kilometri;
        int got = 0;
        public string Kilometri { get => kilometri;}
        public int Got { get => got;}

        public async void TraziKorisnike(List<Klijent> Klijenti)
        {
            double km=0;
           
            for (int i = 0; i < Klijenti.Count - 1; i++)
            {
                String pLat = Klijenti[i].Lat1;
                String Plon = Klijenti[i].Lon1;
                String zLat = Klijenti[i+1].Lat1;
                String Zlon = Klijenti[i+1].Lon1;
                try
                {
                    IzracunavanjeKm duljina = new IzracunavanjeKm(pLat, Plon, zLat, Zlon);

                    int r = 0;
                    int l = 0;
                    TimeSpan vrijeme = TimeSpan.FromMilliseconds(1);
                    Task t = Task.Run(() =>
                    {
                        duljina.Execute("");
                    });
                    while (r == 0)
                    {
                        if (duljina.getRez()=="No")
                        {
                            await Task.Delay(vrijeme);
                        }
                        else
                        {
                            String result= duljina.getRez();
                            if (result == "error" || result == "No" || result == null)
                            {
                                l = 0;
                            }
                            else
                            {
                                string brojkm=result.Replace("km", "");
                                km = km+Convert.ToDouble(brojkm);
                                l = 1;
                            }
                            r = 1;
                        }
                    }

                }
                catch (Exception e)
                {                 
                }
            }         
            kilometri = String.Format("{0:0.00}", km);
            got = 1;
        }
    }
}
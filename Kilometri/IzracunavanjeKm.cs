using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TerapijaRaspored.Baza;
using Newtonsoft.Json.Linq;

namespace TerapijaRaspored.Kilometri
{
    public class IzracunavanjeKm : Async
    {
        string pLat, Plon, zLat, Zlon;
        String rez = "No";
        public IzracunavanjeKm(string pLat, string Plon, string zLat, string Zlon) {
            this.pLat = pLat;
            this.Plon = Plon;
            this.zLat = zLat;
            this.Zlon = Zlon;

        }
        protected override void DoInBackground()
        {
            
            string link = @"https://maps.googleapis.com/maps/api/distancematrix/json?destinations=" + zLat + "%2C" + Zlon + "&origins=" + pLat + "%2C" + Plon + "&key=AIzaSyCiLTKcVOoWFXqSe-TpI0jyUty7dG4zFJw";
            try
            {
                WebRequest request = WebRequest.Create(link);
                WebResponse response = request.GetResponse();                
                Stream data = response.GetResponseStream();                
                StreamReader reader = new StreamReader(data);
                string responseFromServer = reader.ReadToEnd();
                response.Close();
                JObject json = JObject.Parse(responseFromServer);
                string rezultat;
                rezultat = json.SelectToken("rows[0].elements[0].distance.text").ToString();                
                rez = rezultat;
                
            }
            catch (Exception ex)
            {
                rez= "error";
            }
        }


        protected override string onPostExecute()
        {
            return rez;
        }

        protected override void PreExecute(string link)
        {
           
        }

        public String getRez()
        {
            return rez;
        }
    }
}
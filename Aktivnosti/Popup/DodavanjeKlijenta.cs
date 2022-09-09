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
using TerapijaRaspored.Baza;
using TerapijaRaspored.OpisneKlase;

namespace TerapijaRaspored.Aktivnosti.Popup
{

    public class DodavanjeKlijenta : DialogFragment
    {
       public class DialogEventArgs : EventArgs
        {
            public List<Klijent> DodaniKlijenti { get; set; }           
        }
        public delegate void DialogEventHandler(object sender, DialogEventArgs args);
        Spinner popis;
        BazaPod con;
        List<Klijent> K = new List<Klijent>();
        List<String> users = new List<String>();
        List<Klijent> DodUsers = new List<Klijent>();
        Button dodaj;
        public event DialogEventHandler Dismissed;
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {       
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            LayoutInflater inflater = Activity.LayoutInflater;
            con = BazaPod.getInstanca();
            K = con.getSviKlijenti();
            for (int i = 0; i < K.Count; i++)
            {
                users.Add(K[i].Prezime);
            }         
            View v = inflater.Inflate(Resource.Layout.dodavanje_klijenta, null);
            dodaj= v.FindViewById<Button>(Resource.Id.dod);
            popis = v.FindViewById<Spinner>(Resource.Id.sviKorisnici);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerDropDownItem, users);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            popis.Adapter = adapter;
            dodaj.Click += dodavanjeKorisnika;
            builder.SetView(v)
                   .SetTitle("Odaberi Klijenta")
                   .SetPositiveButton("Ok", (sender, args) =>
                    {
                        if (null != Dismissed)
                            Dismissed(this, new DialogEventArgs { DodaniKlijenti = DodUsers });
                    })
                   .SetNegativeButton("Cancel", (sender, args) =>
                   {
                       Console.WriteLine("Korisnik kliknuo Cancel");
                   });
            
            return builder.Create();
        }
        public void dodavanjeKorisnika(object sender, EventArgs e) {
            long index = popis.SelectedItemId;
            int i = Convert.ToInt32(index);
            DodUsers.Add(K[i]);

        }

       

    }


}
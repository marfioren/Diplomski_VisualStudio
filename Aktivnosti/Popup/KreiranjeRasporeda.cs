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

namespace TerapijaRaspored.Aktivnosti.Popup
{
    [Obsolete]
    public class KreiranjeRasporeda : DialogFragment
    {
        public class DialogEventArgs : EventArgs
        {
            public String pocetni { get; set; }
            public String zavrsni { get; set; }
        }
        public delegate void DialogEventHandler(object sender, DialogEventArgs args);
        Button promjeni1;
        Button promjeni2;
        TextView t1;
        TextView t2;
        public event DialogEventHandler Dismissed;
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            LayoutInflater inflater = Activity.LayoutInflater;
            View v = inflater.Inflate(Resource.Layout.kreiranje_rasporea, null);
            promjeni1 = v.FindViewById<Button>(Resource.Id.promjenid1);
            promjeni2 = v.FindViewById<Button>(Resource.Id.promjenid2);
            t1 = v.FindViewById<TextView>(Resource.Id.dat1);
            t2 = v.FindViewById<TextView>(Resource.Id.dat2);
            DateTime Trenutnovrijeme = DateTime.Now;
            String datumTrenutno = Trenutnovrijeme.Day + "/" + Trenutnovrijeme.Month + "/" + Trenutnovrijeme.Year;
            t1.Text = datumTrenutno;
            t2.Text = datumTrenutno;
            promjeni1.Click += odaberiDatum1;
            promjeni2.Click += odaberiDatum2;
            builder.SetView(v)
                   .SetTitle("Odaberi datume")
                   .SetPositiveButton("Kreiraj", (sender, args) =>
                   {
                       if (null != Dismissed)
                           Dismissed(this, new DialogEventArgs { pocetni = t1.Text, zavrsni = t2.Text });
                   })
                   .SetNegativeButton("Cancel", (sender, args) =>
                   {
                       Console.WriteLine("Korisnik kliknuo Cancel");
                   });
            return builder.Create();
        }

        public void odaberiDatum1(object sender, EventArgs e) {
            var odabirDat = DatumOdabir.NewInstance(delegate (DateTime vrijeme) {
                    string datumDana = vrijeme.Day + "/" + vrijeme.Month + "/" + vrijeme.Year;
                    t1.Text = datumDana;
            });
            odabirDat.Show(FragmentManager, DatumOdabir.TAG);
        }

        public void odaberiDatum2(object sender, EventArgs e)
        {
            var odabirDat = DatumOdabir.NewInstance(delegate (DateTime vrijeme) {
                string datumDana = vrijeme.Day + "/" + vrijeme.Month + "/" + vrijeme.Year;
                t2.Text = datumDana;
            });
            odabirDat.Show(FragmentManager, DatumOdabir.TAG);
        }
    }

}
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TerapijaRaspored.Aktivnosti.Popup;
using TerapijaRaspored.Baza;
using TerapijaRaspored.Kilometri;
using TerapijaRaspored.OpisneKlase;
using System.Threading.Tasks;
using Android;
using static Android.Manifest;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.Graphics.Pdf;
using TerapijaRaspored.Raspored;
using System.IO;
using Android.Graphics;
using Android.Graphics.Drawables;
using System.Globalization;

namespace TerapijaRaspored.Aktivnosti
{
    [Activity(Label = "KreiranjeDana")]
    public class KreiranjeDana : AppCompatActivity
    {
        TextView t1;
        TextView t2;
        Button odaberiDat;
        Button dodajK;
        Button brisiK;
        Button spremiDan;
        Spinner spin;
        List<String> users = new List<String>();
        List<Klijent> users2 = new List<Klijent>();
        BazaPod con;
        View v1;
        private static int STORAGE_PERMISSION_CODE = 101;
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SupportActionBar.SetBackgroundDrawable(new ColorDrawable(Color.ParseColor("#64C9A1")));
            SupportActionBar.SetWindowTitle(" ");
            con = BazaPod.getInstanca();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            var rezolucija = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(rezolucija.WidthPixels);
            var heightInDp = ConvertPixelsToDp(rezolucija.HeightPixels);
            if (widthInDp > heightInDp)
            {
                v1 = this.LayoutInflater.Inflate(Resource.Layout.activity_kreiranja_dana_land, null);
               
            }
            else
            {
                v1 = this.LayoutInflater.Inflate(Resource.Layout.activity_kreiranje_dana, null);
            }

            SetContentView(v1);
            checkPermission(Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage, STORAGE_PERMISSION_CODE);
            odaberiDat = v1.FindViewById<Button>(Resource.Id.button3);
            dodajK = v1.FindViewById<Button>(Resource.Id.dodajK);
            brisiK = v1.FindViewById<Button>(Resource.Id.brisiK);
            
            t1 = v1.FindViewById<TextView>(Resource.Id.dat);
            t2 = v1.FindViewById<TextView>(Resource.Id.kilometri);
            spin = v1.FindViewById<Spinner>(Resource.Id.spinner);
            DateTime Trenutnovrijeme = DateTime.Now;
            String datumTrenutno = Trenutnovrijeme.Day + "/" + Trenutnovrijeme.Month + "/" + Trenutnovrijeme.Year;
            t1.Text = datumTrenutno;
            provjeriKorisnikeDatuma(datumTrenutno);
            odaberiDat.Click += odaberiDatum;
            dodajK.Click += unesiKlijente;
            brisiK.Click += izbrisiKorisnika;
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.spremi:
                    {
                        spremanjeDana();
                        Toast.MakeText(Application.Context, "Promjene spremljene", ToastLength.Short).Show();
                        return true;
                    }
                case Resource.Id.odjavi:
                    {
                        var intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                        return true;
                    }
                case Resource.Id.kreirajRas:
                    {
                        KreirajRaspored();                     
                        return true;
                    }
            }

            return base.OnOptionsItemSelected(item);
        }
        public void odaberiDatum(object sender, EventArgs e)

        {
            spremanjeDana();
            var odabirDat = DatumOdabir.NewInstance(delegate (DateTime vrijeme) {
                string datumDana = vrijeme.Day + "/" + vrijeme.Month + "/" + vrijeme.Year;
                t1.Text = datumDana;
                provjeriKorisnikeDatuma(datumDana);
            });
            odabirDat.Show(FragmentManager, DatumOdabir.TAG);

        }

        public void updateSpinner() {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, users);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spin.Adapter = adapter;
        }

        public void unesiKlijente(object sender, EventArgs e)
        {
            var transaction = FragmentManager.BeginTransaction();
            var dialogFragment = new DodavanjeKlijenta();
            dialogFragment.Show(transaction, "Izaberi klijenta");
            dialogFragment.Dismissed += (s, e) => {
                dodajKorisnike(e.DodaniKlijenti);
            };
        }

        public void KreirajRaspored()
        {
            var transaction = FragmentManager.BeginTransaction();
            var dialogFragment = new KreiranjeRasporeda();
            dialogFragment.Show(transaction, "Izaberi datume");
            dialogFragment.Dismissed += (s, e) => {
                proslijediDatume(e.pocetni, e.zavrsni);
            };

        }

     

        public async void dodajKorisnike(List<Klijent> Dodani)
        {
            int pocetni = users.Count;
            for (int i = 0; i < Dodani.Count; i++)
            {
                pocetni = pocetni + 1;
                users.Add(pocetni + "." + Dodani[i].Prezime);
                users2.Add(Dodani[i]);
            }
            updateSpinner();
            KorisniciLokacije kl = new KorisniciLokacije();
            kl.TraziKorisnike(users2);
            string km;
            int l = 0;
            TimeSpan vrijeme = TimeSpan.FromMilliseconds(5);
            Task t = Task.Run(() => { });
            while (l == 0)
            {
                l = kl.Got;
                if (l == 0)
                {
                    await Task.Delay(vrijeme);
                }
                else
                {
                    km = kl.Kilometri;
                    t2.Text = km;
                }
            }

        }
        public async void izbrisiKorisnika(object sender, EventArgs e) {
            long broj = spin.SelectedItemId;
            int id = Convert.ToInt32(broj);
            if (users.Count != 1 || users.Count != 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (i == id)
                    {
                        users2.RemoveAt(i);
                        users.RemoveAt(i);
                    }
                }
                for (int i = 0; i < users.Count; i++)
                {
                    int noviBroj = i + 1;
                    String kor = users2[i].Prezime;
                    String novi = noviBroj + "." + kor;
                    users[i] = novi;
                }
            }
            else
            {
                users = new List<String>();
                users2 = new List<Klijent>();
            }
            updateSpinner();
            KorisniciLokacije kl = new KorisniciLokacije();
            kl.TraziKorisnike(users2);
            string km;
            int l = 0;
            TimeSpan vrijeme = TimeSpan.FromMilliseconds(1);
            Task t = Task.Run(() => { });
            while (l == 0)
            {
                l = kl.Got;
                if (l == 0)
                {
                    await Task.Delay(vrijeme);
                }
                else
                {
                    km = kl.Kilometri;
                    t2.Text = km;
                }
            }
        }
        public void provjeriKorisnikeDatuma(string dat) {
            Dan d = con.provjeriDatum(dat);
            users = new List<String>();
            users2 = new List<Klijent>();
            for (int i = 0; i < d.Popis1.Count; i++)
            {
                int x = i + 1;
                String id = x + ".";
                users.Add(id + d.Popis1[i].Prezime);
                users2.Add(d.Popis1[i]);
            }
            updateSpinner();
            string km = d.Km1;
            t2.Text = km;
        }

        public async void spremanjeDana()
        {
            string datum = t1.Text;
            string km = t2.Text;
            con.updateDan(datum, km, users2);
            String lista = "";
            for (int i = 0; i < users2.Count; i++)
            {
                lista = lista + users2[i].Id;
                if (i != users2.Count - 1)
                {
                    lista = lista + ",";
                }
            }
            string Datum = datum;
            string Poc = "0";
            string Zav = "0";
            string Pkm = km;
            string Popis = lista;
            int Mjesec = 10;
            int Godina = 2021;
            int Zaposlenik = con.ulogiranizaposlenik;
            string link = "http://crofi.com/assets/assets/images/RasporedBaza/UpdateDan.php?Datum=" + Datum + "&Poc=" + Poc + "&Zav=" + Zav + "&Pkm=" + Pkm + "&Popis=" + Popis + "&Mjesec=" + Mjesec + "&Godina=" + Godina + "&Zaposlenik=" + Zaposlenik;
            Podaci update = new Podaci(this, 1);
            int r = 0;
            int l = 0;
            TimeSpan vrijeme = TimeSpan.FromMilliseconds(2);
            Task t = Task.Run(() =>
            {
                update.Execute(link);
            });
            while (r == 0)
            {
                if (update.getRez().Count == 0)
                {
                    await Task.Delay(vrijeme);
                }
                else
                {
                    Console.WriteLine("Spremljeno");
                    r = 1;
                }
            }
        }
        public List<Dan> DaniRaspored(DateTime p, DateTime k)
        {
            List<Dan> sviDani = con.getSviDani();
            List<Dan> Dani = new List<Dan>();
            for (int i = 0; i < sviDani.Count; i++)
            {
                try
                {
                    DateTime d = DateTime.Parse(sviDani[i].Datum1, new System.Globalization.CultureInfo("pt-BR"));
                    int poslije = DateTime.Compare(d,p);
                    int prije = DateTime.Compare(d, k);
                    if (poslije>0 && prije<0)
                    {
                        Dani.Add(sviDani[i]);
                    }
                }
                catch (Exception e)
                {}
            }
           for (int i = 0; i < Dani.Count - 1; i++)
                for (int j = 0; j < Dani.Count - i - 1; j++)
                {
                    try
                    {
                        DateTime d =DateTime.Parse(Dani[j].Datum1, new System.Globalization.CultureInfo("pt-BR"));
                        DateTime d2 =DateTime.Parse(Dani[j + 1].Datum1, new System.Globalization.CultureInfo("pt-BR"));
                        int poslije = DateTime.Compare(d, d2);
                        if (poslije>0)
                        {
                            Dan dan = Dani[j];
                            Dani[j] = Dani[j + 1];
                            Dani[j + 1] = dan;
                        }
                    }
                    catch (Exception e)
                    {                      
                    }
                }
            return Dani;

        }
        public void proslijediDatume(String d1, String d2)
        {
           
            DateTime pocetak = DateTime.Parse(d1, new System.Globalization.CultureInfo("pt-BR"));
            DateTime zavrsetak = DateTime.Parse(d2, new System.Globalization.CultureInfo("pt-BR"));
            List<Dan> SortiraniDani = DaniRaspored(pocetak, zavrsetak);
            Pdf p = new Pdf();
            PdfDocument pdf = new PdfDocument();
            pdf = p.Kreirajpdf(SortiraniDani);
            var TerapijaRaspored = pdf;
            Java.IO.File file = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            FileStream fileStream = new FileStream(file.AbsolutePath + "/Raspored2.pdf", FileMode.Create);
            pdf.WriteTo(fileStream);
            Console.WriteLine("Kreiran pdf");
            fileStream.Flush();
            fileStream.Close();
            pdf.Close();
            Toast.MakeText(Application.Context, "Kreirana datoteka Raspored2.pdf", ToastLength.Short).Show();
        }
        public void checkPermission(String permission, String permission2, int requestCode){
        
            if (ContextCompat.CheckSelfPermission(this, permission) == (int)Android.Content.PM.Permission.Granted && ContextCompat.CheckSelfPermission(this, permission2) == (int)Android.Content.PM.Permission.Granted)
            {
                Console.WriteLine("Ima");
            }
            else
            {
                Console.WriteLine("Nema");
                ActivityCompat.RequestPermissions(this, new String[] { permission }, requestCode);
                ActivityCompat.RequestPermissions(this, new String[] { permission2 }, requestCode);
            }
        }


    }
}
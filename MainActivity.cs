using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using TerapijaRaspored.Baza;
using System;
using System.Threading;
using System.Threading.Tasks;
using TerapijaRaspored.OpisneKlase;
using System.Collections.Generic;
using Android.Content;
using TerapijaRaspored.Aktivnosti;
using Android.Graphics;

namespace TerapijaRaspored
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        BazaPod con;
        Button mybtn;
        EditText username;
        EditText password;
        TextView t1, t2;
        View v1;
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            con = BazaPod.getInstanca();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            var rezolucija = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(rezolucija.WidthPixels);
            var heightInDp = ConvertPixelsToDp(rezolucija.HeightPixels);
            if (widthInDp > heightInDp)
            {
                v1 = this.LayoutInflater.Inflate(Resource.Layout.main_activity_land, null);
            }
            else {
                v1 = this.LayoutInflater.Inflate(Resource.Layout.activity_main, null);
            }

            SetContentView(v1);
            Typeface font = Typeface.CreateFromAsset(Assets, "NBold.otf");
            mybtn = v1.FindViewById<Button>(Resource.Id.button);
            t1 = v1.FindViewById<TextView>(Resource.Id.textView2);
            t2 = v1.FindViewById<TextView>(Resource.Id.textView);
            username = v1.FindViewById<EditText>(Resource.Id.korime);
            password = v1.FindViewById<EditText>(Resource.Id.pass);
            mybtn.Typeface = font;
            t1.Typeface = font;
            t2.Typeface = font;
            password.Typeface = font;
            username.Typeface = font;
            mybtn.Typeface = font;
            mybtn.Click += prijava;
        }

        private async void prijava(object sender, EventArgs e)
        {
            String u = username.Text;
            String p = password.Text;
            String link1 = "http://crofi.com/assets/assets/images/RasporedBaza/Logiranje.php?username=" + u + "&password=" + p;
            String link2 = "http://crofi.com/assets/assets/images/RasporedBaza/Svilijenti.php";            
            String result = null;
            try
            {
                Podaci logiranje= new Podaci(this, 1);              
                int r = 0;
                int l = 0;
                TimeSpan vrijeme = TimeSpan.FromMilliseconds(10);
                Task t = Task.Run(() =>
                {
                    logiranje.Execute(link1);
                });                
                while (r == 0)
                {
                    if (logiranje.getRez().Count ==0)
                    {
                        await Task.Delay(vrijeme);                       
                    }
                    else {
                        result = logiranje.getRez()[0];
                        if (result == "error" || result == "No" || result == null)
                        {
                            l = 0;                           
                        }
                        else
                        {
                            con.PrijaviZaposlenika(result);
                            
                            l = 1;
                        }
                        r = 1;
                    }
                }

                if (l == 0)
                {
                    Toast.MakeText(Application.Context, "Pogrešni podaci", ToastLength.Short).Show();
                }

                else {
                    Podaci dohvatSvihKlijenta = new Podaci(this, 2);
                    Podaci dohvatSvihDana = new Podaci(this, 3);
                    String link3 = "http://crofi.com/assets/assets/images/RasporedBaza/SviDani.php?id="+con.UlogiraniZaposlenik1.Id1;
                    r = 0;
                    Task t2 = Task.Run(() =>
                    {
                        dohvatSvihKlijenta.Execute(link2);
                        dohvatSvihDana.Execute(link3);
                    });
                    while (r == 0)
                    {
                        if (dohvatSvihKlijenta.getRez().Count ==0 || dohvatSvihDana.getRez().Count==0)
                        {
                            await Task.Delay(vrijeme);
                            
                        }
                        else
                        {
                            
                            List<String> Daniresult = dohvatSvihDana.getRez();
                            con.StaviSveDane(Daniresult);                           
                            List<Dan> sviKlijenti = con.getSviDani();
                            r = 1;
                        }
                    }

                    var intent = new Intent(this, typeof(KreiranjeDana));
                    StartActivity(intent);
                }

            }
            catch (Exception r)
            {
                Console.WriteLine("greska");
            }

        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
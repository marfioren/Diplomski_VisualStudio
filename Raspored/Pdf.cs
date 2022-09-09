using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using Android.Graphics.Pdf;
using Android.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TerapijaRaspored.OpisneKlase;
using TerapijaRaspored.Baza;

namespace TerapijaRaspored.Raspored
{
    public class Pdf
    {
        List<Dan> Dani = new List<Dan>();
        List<int> index = new List<int>();
        BazaPod con;
        public PdfDocument Kreirajpdf(List<Dan> SortiraniDan)
        {
            Console.WriteLine("broj dana: " + SortiraniDan.Count);
            Dani = SortiraniDan;
            PdfDocument pdf = new PdfDocument();
            int b = 0;
            int bDani = 0;
            for (int i = 0; i < Dani.Count; i++)
            {
                int brojLjudi = b + Dani[i].Popis1.Count;
                Console.WriteLine("broj ljudi u danu : " + Dani[i].Datum1 + " " + brojLjudi);
                if (brojLjudi < 50)
                {
                    b = b + Dani[i].Popis1.Count;
                    if (Dani[i].Popis1.Count == 0)
                    {
                        b = b + 3;
                    }
                    bDani = bDani + 1;
                }
                else
                {
                    b = 0;
                    bDani = 0;
                    Console.WriteLine("dodani index: " + i);
                    index.Add(i);
                }
            }
            if (index.Count < 1)
            {
                index.Add(Dani.Count);
            }
            int stranice = index.Count + 1;
            for (int i = 0; i < index.Count; i++)
            {
                if (i != 0)
                {
                    KreirajStranicu(i + 1, pdf, index[i - 1], index[i]);
                }
                else
                {
                    KreirajStranicu(i + 1, pdf, 0, index[i]);
                }
            }
            KreirajStranicu(stranice, pdf, index[index.Count - 1], Dani.Count);
            return pdf;
        }
        public void KreirajStranicu(int broj, PdfDocument pdf, int brojIndexaP, int brojIndexaZ)
        {
            PdfDocument.PageInfo pi = new PdfDocument.PageInfo.Builder(1200, 2010, broj).Create();
            PdfDocument.Page myPage = pdf.StartPage(pi);
            con = BazaPod.getInstanca();
            Zaposlenik ulogiraniZap = con.UlogiraniZaposlenik1;
            int pageWidth = 1200;
            int pageHeight = 2010;
            //naslov
            Paint paint = new Paint();
            Paint title = new Paint();
            Canvas c = myPage.Canvas;
            title.TextAlign = Paint.Align.Left;
            title.SetTypeface(Typeface.DefaultBold);
            title.TextSize = 25;
            c.DrawText("USTANOVA ZA NJEGU U KUĆI DOMNIUS, JARUŠČICA 9E, ZAGREB", 150, 100, title);
            c.DrawText("EVIDENCIJA O KRETANJU PRIVATNOG AUTOMOBILA ZA RAZDOBLJE", 150, 160, title);
            c.DrawText("OD " + Dani[0].Datum1 + " DO " + Dani[Dani.Count - 1].Datum1 + " godine U SLUŽBENE SVRHE", 150, 190, title);
            c.DrawText("Korisnik: " +ulogiraniZap.Ime1+" "+ulogiraniZap.Prezime1, 150, 250, title);
            c.DrawText("Marka automobila: "+ulogiraniZap.Auto1, 150, 280, title);
            c.DrawText("Registarski broj automobila: "+ulogiraniZap.Registracija1, 150, 310, title);
            //informacije u desnom kutu
            paint.SetTypeface(Typeface.DefaultBold);
            paint.TextSize = 30f;
            paint.TextAlign = Paint.Align.Right;
            c.DrawText("Call: 093530953", 1160, 40, paint);
            c.DrawText("Call: 093530953", 1160, 80, paint);
            //tablica
            title.SetStyle(Paint.Style.Stroke);
            title.StrokeWidth = 3;
            c.DrawRect(20, 400, pageWidth - 20, 480, title);
            title.TextAlign = Paint.Align.Left;
            title.SetStyle(Paint.Style.Fill);
            paint.TextAlign = Paint.Align.Left;
            paint.SetStyle(Paint.Style.Fill);
            c.DrawText("Datum", 60, 450, paint);
            c.DrawText("Naziv lokacija", 260, 450, paint);
            c.DrawText("Broj prijeđenih km", 570, 450, paint);
            c.DrawText("Izvješće o radu", 890, 450, paint);
            c.DrawLine(20, 400, 20, pageHeight - 200, title);
            c.DrawLine(180, 400, 180, pageHeight - 200, title);
            c.DrawLine(550, 400, 550, pageHeight - 200, title);
            c.DrawLine(830, 400, 830, pageHeight - 200, title);
            c.DrawLine(pageWidth - 20, 400, pageWidth - 20, pageHeight - 200, title);
            //podaci
            paint.TextSize = 25f;
            paint.SetTypeface(Typeface.Default);
            int y = 510;
            int pocetak = brojIndexaP;
            int zavrsetak = brojIndexaZ;
            for (int i = pocetak; i < zavrsetak; i++)
            {
                y = y + 10;
                String datumDana = Dani[i].Datum1;
                String kilometri = Dani[i].Km1;
                c.DrawText(datumDana, 30, y, paint);
                c.DrawText(kilometri, 580, y, paint);
                Console.WriteLine("veliciina popisa: " + Dani[i].Popis1.Count);
                if (Dani[i].Popis1.Count > 3)
                {
                    int red = 0;
                    String lokacije = "";
                    String text = "";
                    for (int j = 0; j < Dani[i].Popis1.Count; j++)
                    {
                        if (red < 2 && j < Dani[i].Popis1.Count - 1)
                        {
                            lokacije = lokacije + Dani[i].Popis1[j].Adresa1 + ", ";
                            text = text + Dani[i].Popis1[j].Prezime + ", ";
                            red++;
                        }
                        else
                        {
                            red = 0;
                            lokacije = lokacije + Dani[i].Popis1[j].Adresa1;
                            text = text + Dani[i].Popis1[j].Prezime;
                            if (j != Dani[i].Popis1.Count - 1)
                            {
                                lokacije = lokacije + ", ";
                                text = text + ", ";
                            }
                            c.DrawText(lokacije, 190, y, paint);
                            c.DrawText(text, 840, y, paint);
                            text = "";
                            lokacije = "";
                            y = y + 35;
                        }
                    }
                }
                else
                {
                    String lokacije = "";
                    String text = "";
                    for (int j = 0; j < Dani[i].Popis1.Count; j++)
                    {
                        lokacije = lokacije + Dani[i].Popis1[j].Adresa1;
                        text = text + Dani[i].Popis1[j].Prezime;
                        if (j != Dani[i].Popis1.Count - 1)
                        {
                            lokacije = lokacije + ", ";
                            text = text + ", ";
                        }
                    }
                    c.DrawText(lokacije, 190, y, paint);
                    c.DrawText(text, 840, y, paint);
                    y = y + 35;
                }
                c.DrawLine(20, y, pageWidth - 20, y, title);
                y = y + 20;
            }

            pdf.FinishPage(myPage);
           
        }
    }
}
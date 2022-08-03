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
using System.ComponentModel;
namespace TerapijaRaspored.Baza
{
    public abstract class Async
    {
        private BackgroundWorker bw;
       
        public  Async()
        {
           
            bw = new BackgroundWorker();
            bw.DoWork += (s, e) => { DoInBackground(); };
            bw.RunWorkerCompleted += (s, e) => {onPostExecute(); };

        }

        protected abstract void PreExecute(String link);
        protected abstract void DoInBackground();
        protected abstract string onPostExecute();
        

    public void Execute(String link)
        {
            PreExecute(link);
            bw.RunWorkerAsync();
          
        }

    

    }
}
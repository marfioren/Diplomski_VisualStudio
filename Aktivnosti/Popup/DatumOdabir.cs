using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerapijaRaspored.Aktivnosti.Popup
{
    public class DatumOdabir : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        public static readonly string TAG = "";
        Action<DateTime> odabraniDatum = delegate { };
        public static DatumOdabir NewInstance(Action<DateTime> onDateSelected)
        {
            DatumOdabir frag = new DatumOdabir();
            frag.odabraniDatum = onDateSelected;
            return frag;
        }
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                           this,
                                                           currently.Year,
                                                           currently.Month - 1,
                                                           currently.Day);
            return dialog;
        }
        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {          
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            odabraniDatum(selectedDate);
        }
    }
}
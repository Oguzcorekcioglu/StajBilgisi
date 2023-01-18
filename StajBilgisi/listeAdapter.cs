using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace StajBilgisi
{
    class listeAdapter : BaseAdapter<stajmodel>
    {
        private Context mContext;
        private int mRowLayout;
        private List<stajmodel> mstajmodels;
        private int[] mAlternatingColors;

        public listeAdapter(Context context, int rowLayout, List<stajmodel> mstajmodelss)
        {
            mContext = context;
            mRowLayout = rowLayout;
            mstajmodels = mstajmodelss;
            mAlternatingColors = new int[] { 0xF2F2F2, 0xD3B659 }; //Asıl renk atamasının verildiği yer ! 
        }

        public override int Count
        {
            get { return mstajmodels.Count; }
        }

        public override stajmodel this[int position]
        {
            get { return mstajmodels [position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(mRowLayout, parent, false);
            }

            row.SetBackgroundColor(GetColorFromInteger(mAlternatingColors[position % mAlternatingColors.Length])); //Internetten buldum.


            TextView kurumAd = row.FindViewById<TextView>(Resource.Id.txtKurumadi);
            kurumAd.Text = mstajmodels[position].Kurumad;

            TextView kurumDepartman = row.FindViewById<TextView>(Resource.Id.txtDepartman);
            kurumDepartman.Text = mstajmodels[position].KurumDepartman;

            TextView kurumCalisan = row.FindViewById<TextView>(Resource.Id.txtCalisanSayisi);
            kurumCalisan.Text = mstajmodels[position].CalisanSayisi.ToString();

            TextView kurumGorev = row.FindViewById<TextView>(Resource.Id.txtSorumluluk);
            kurumGorev.Text = mstajmodels[position].KurumGorev; //Aslında alt kısım model'den cekiyor datayı.

            if ((position % 2) == 1)
            {
                //Buradada yazı beyaz istediğin renk.
                kurumAd.SetTextColor(Color.White);
                kurumDepartman.SetTextColor(Color.White);
                kurumCalisan.SetTextColor(Color.White);
                kurumGorev.SetTextColor(Color.White);
            }

            else
            {
                //Beyaz arka plan yazı siyah.
                kurumAd.SetTextColor(Color.Black);
                kurumDepartman.SetTextColor(Color.Black);
                kurumCalisan.SetTextColor(Color.Black);
                kurumGorev.SetTextColor(Color.Black);
            }

            return row;
        }

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }
    }
}
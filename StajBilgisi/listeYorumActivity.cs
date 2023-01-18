
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace StajBilgisi
{
    [Activity(Label = "listeYorum")]
    public class listeYorumActivity : Activity
    {
        public TextView mYorum;
        public TextView mTarih;
        public TextView mAdres;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.listeYorum);
            mYorum = FindViewById<TextView>(Resource.Id.txtYorumGozuken);
            mTarih = FindViewById<TextView>(Resource.Id.txtYorumTarih);
            mAdres = FindViewById<TextView>(Resource.Id.txtYorumAdres);
            mYorum.Text = Intent.GetStringExtra("Yorum");
            mTarih.Text = Intent.GetStringExtra("Tarih");
            mAdres.Text = Intent.GetStringExtra("Adres");

           
        }
    }
}
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace StajBilgisi
{
    [Activity(Label = "Ana Sayfa")]
    public class GirisActivity : Activity
    {
        private Button btnFirmalariListele;
        private Button btnKayitEkle;
        private Button btnYorumlarimiGor;
        int iduye;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.GirisEkrani);
            btnFirmalariListele = FindViewById<Button>(Resource.Id.btnFirmalariListele);
            btnKayitEkle = FindViewById<Button>(Resource.Id.btnKayitEkle);
            btnYorumlarimiGor = FindViewById<Button>(Resource.Id.btnYorumlarimiGor); 

            var _id = Intent.GetIntExtra("id",0);
             iduye = Convert.ToInt32(_id); //Gelen uyenin id'si diğer activity'den.
            btnFirmalariListele.Click += BtnFirmalariListele_Click; //Firmaları Listeleme Arama kısmı
            btnKayitEkle.Click += BtnKayitEkle_Click; //Kayıt Ekleme Kısmı
            btnYorumlarimiGor.Click += BtnYorumlarimiGor_Click; //Kendi Yorumlarımızı Görme Silme Kısmı
        }

        private void BtnYorumlarimiGor_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(listeYorumGorSilActivity));
            intent.PutExtra("idno", iduye); //Buralarda lazım olduğu için ilgili id numaralarını gönderiyoruz ! 
            StartActivity(intent); //Kayıt Ekleme Sayfasına Götürür !
        }

        private void BtnKayitEkle_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(kayitEkleActivity));
            intent.PutExtra("idno", iduye); //Buralarda lazım olduğu için ilgili id numaralarını gönderiyoruz ! 
            StartActivity(intent); //Kayıt Ekleme Sayfasına Götürür !
            OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }

        private void BtnFirmalariListele_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(listeActivity));
            StartActivity(intent); //Firma Listesi Sayfasına Götürür ! 
            OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }
    }
}
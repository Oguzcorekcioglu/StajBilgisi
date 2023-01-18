using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace StajBilgisi
{ //Burada Activity'e yardımcı olacak class'larımızı yazıyoruz ! 
    public class kayitOlEventArgs : EventArgs
    {
        private string mKullaniciAd;
        private string mKullaniciSifre;
        private string mAd;
        private string mSoyad;
        private string mCinsiyet;
        private string mIl;
        private string mIlce;
        private string mBolum;

        public string KullaniciAd { get => mKullaniciAd; set=> mKullaniciAd=value; } //C# 7.0 Prop kullanımı ! 
        public string KullaniciSifre { get=>mKullaniciSifre; set=> mKullaniciSifre=value; }
        public string Ad { get=>mAd; set=>mAd=value; }
        public string Soyad { get=>mSoyad; set=>mSoyad=value; }
        public string Cinsiyet { get=>mCinsiyet; set=>mCinsiyet=value; }
        public string Il { get=>mIl; set=>mIl=value; }
        public string Ilce { get=>mIlce; set=>mIlce=value; }
        public string Bolum { get=>mBolum; set=>mBolum=value; }

        public kayitOlEventArgs(string kullaniciad,string kullanicisifre,string ad,string soyad,string cinsiyet,string il,string ilce,string bolum) : base() //Bilindiği üzere javada super class ve base class diye ayrılan ayrımı görüyoruz burada ! 
        {
            KullaniciAd = kullaniciad;
            KullaniciSifre = kullanicisifre;
            Ad = ad;
            Soyad = soyad;
            Cinsiyet = cinsiyet;
            Il = il;
            Ilce = ilce;
            Bolum = bolum;
        }
    }
    class dialog_kayit_ol : DialogFragment
    {
        private EditText mEdtKullaniciAd;
        private EditText mEdtKullaniciSifre;
        private EditText mEdtAd;
        private EditText mEdtSoyad;
        private EditText mEdtCinsiyet;
        private EditText mEdtIl;
        private EditText mEdtIlce;
        private EditText mEdtBolum;
        private Button mBtnDialogKayitOl;

        public event EventHandler<kayitOlEventArgs> mKayitOlTamam;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            //Objeye erişmek için handle etmemiz lazım aslında activity kısmındada handle ediyoruz ! 
            var view = inflater.Inflate(Resource.Layout.dialog_kayit_ol, container, false);
            mEdtKullaniciAd = view.FindViewById<EditText>(Resource.Id.edtKullaniciAd);
            mEdtKullaniciSifre = view.FindViewById<EditText>(Resource.Id.edtKullaniciSifre);
            mEdtAd = view.FindViewById<EditText>(Resource.Id.edtAd);
            mEdtSoyad = view.FindViewById<EditText>(Resource.Id.edtSoyad);
            mEdtCinsiyet = view.FindViewById<EditText>(Resource.Id.edtCinsiyet);
            mEdtIl = view.FindViewById<EditText>(Resource.Id.edtIL);
            mEdtIlce = view.FindViewById<EditText>(Resource.Id.edtIlce);
            mEdtBolum = view.FindViewById<EditText>(Resource.Id.edtBolum);
            mBtnDialogKayitOl = view.FindViewById<Button>(Resource.Id.btnDialogKayıtOl);

           
            view.SetOnTouchListener(new ViewClickListener(Context ,mEdtBolum));
                
            
            mBtnDialogKayitOl.Click += MBtnDialogKayitOl_Click;

            return view;
        }

        private void MBtnDialogKayitOl_Click(object sender, EventArgs e)
        {
            //Invoke methodunda ilk nereye göndereceğimizi yani yayın broadcast yapacağımız veriyoruz ! 
            mKayitOlTamam.Invoke(this, new kayitOlEventArgs(mEdtKullaniciAd.Text,mEdtKullaniciSifre.Text,mEdtAd.Text,mEdtSoyad.Text,mEdtCinsiyet.Text,mEdtIl.Text,mEdtIlce.Text,mEdtBolum.Text));

            this.Dismiss(); //Dialog'u kapatır ! 
        }
        //Şimdi ise başka bir method'u override ederek animasyon işlemini gerçekleştireceğiz ! 
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Bu dialog fragment'in üst kısmını visible hale getirmek için bir enum, title bar'ı visible hale getirir ! 
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation; //Animasyonu set etmiş oluyoruz ! 
            
        }
    }
}
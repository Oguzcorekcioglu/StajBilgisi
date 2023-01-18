using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace StajBilgisi
{
    public class girisYapEventArgs : EventArgs
    {
        private string mKullaniciAdi;
        private string mKullaniciSifre;

        public string KullaniciAdi { get=>mKullaniciAdi; set=>mKullaniciAdi=value; } //C# 7.0 Prop kullanımı.
        public string KullaniciSifre { get=>mKullaniciSifre; set=>mKullaniciSifre=value; }

        public girisYapEventArgs(string kullaniciadi,string kullanicisifre) : base() //Bilindiği üzere javada super class ve base class diye ayrılan ayrımı görüyoruz burada !
        {
            KullaniciAdi = kullaniciadi;
            KullaniciSifre = kullanicisifre;
        }

    }

    class dialog_giris_yap : DialogFragment
    {
        private EditText mEdtKullaniciAdi;
        private EditText mEdtKullaniciSifre;
        private Button mBtnGirisYap;
        public event EventHandler<girisYapEventArgs> mGirisYapTamam; //Event'in kolaylığından faydalanıyoruz ! 

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_giris_yap, container, false); //Ilgili layout'u çalıştırmak için yazıyoruz ve alt tarafta return ettiriyoruz ! 
            mEdtKullaniciAdi = view.FindViewById<EditText>(Resource.Id.edtKullaniciAdiGirisDialog); //Adını farklı tanımladık Kayıtla aynı olmasın diye.
            mEdtKullaniciSifre = view.FindViewById<EditText>(Resource.Id.edtKullaniciSifreGirisDialog);
            mBtnGirisYap = view.FindViewById<Button>(Resource.Id.btnDialogGirisYap);

            mBtnGirisYap.Click += MBtnGirisYap_Click;
            return view;
        }

        private void MBtnGirisYap_Click(object sender, EventArgs e)
        {
            mGirisYapTamam.Invoke(this, new girisYapEventArgs(mEdtKullaniciAdi.Text, mEdtKullaniciSifre.Text));
            this.Dismiss(); //Button'a basıldığında dialog'u kapatır ! 
        }
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);//Bu dialog fragment'in üst kısmını visible hale getirmek için bir enum, title bar'ı visible hale getirir !
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation2; //Görüldüğü üzere 2.Animasyonumuzu da set etmiş oluyoruz ! 
        }
    }
}
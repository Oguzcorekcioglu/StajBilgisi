using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace StajBilgisi
{
    [Activity(Label = "Silmek İçin Basılı Tut")]
    public class listeYorumGorSilActivity : Activity
    {
        //Bunlar global parametrelerimiz yani global handle edişlerimiz ! 
        private List<stajmodel> mStajmodels;
        private ListView mListView;
        private EditText mArama;
        private LinearLayout mContainer; //Bizim ana container'ımızı temsil ediyor grafik işlemleri için kullanacağız ! 
        private listeAdapter2 mAdapter;
        private bool mAnimasyonAsagi;
        private bool mAnimasyonOlurken;
        private int gelenidUye;

        SqlConnection con = BaglantiSingleton.Baglanti; //Static metotları bu şekilde kullanabiliyoruz,bu baglanti icin ! 
        
        private void listeguncelle() //Oluşturduğumuz sjatmodel nesnesini ana ListView'e atamak için burayı yazdık ! 
        {
            if (con.State == ConnectionState.Closed)
            {
                mStajmodels = new List<stajmodel>();
                SqlCommand cmd = new SqlCommand("select yorum_id,kurum_ad,kurum_departman,kurum_calisan,kurum_gorev,kurum_adres,kurum_yorum,tarih from tbl_icerik " + "where uye_id='" + gelenidUye + "'", con);
                con.Open(); //Aslında kontrol ettirmemiz gerekiyor ! 
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    mStajmodels.Add(new stajmodel() { Yorumid = (int)dr[0], Kurumad = (string)dr[1], KurumDepartman = (string)dr[2], CalisanSayisi = dr[3].ToString(), KurumGorev = (string)dr[4], KurumAdres = (string)dr[5], Yorum = (string)dr[6], YorumTarih = (DateTime)dr[7] });
                }
                dr.Close();
                con.Close();

                mAdapter = new listeAdapter2(this, Resource.Layout.listeyardimci, mStajmodels); //listeyardimci layout'u bu şekilde bizim datamızı gösteriyor ! 
                mListView.Adapter = mAdapter; //Burasıda alıyor listeyardımcıyı liste.axml'de gosteriyor ! 
            }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.liste); //Activity'mizin hangi layout'u temsil edeceği yazıyor ! 
            gelenidUye = Convert.ToInt32(Intent.GetIntExtra("idno", 0));
            mListView = FindViewById<ListView>(Resource.Id.listView); //Bu listeyi gösteren kısım 
            mArama = FindViewById<EditText>(Resource.Id.edtSearch); //Bu arama kısmı
            mContainer = FindViewById<LinearLayout>(Resource.Id.llContainer); //Aşağıdaki listView'ı gösterdiğimiz kısım hariç animasyon yapmak için kullanacağız !
            mArama.Alpha = 0; //Bu EditText'in invisible olmasını sağlar ! 
            mListView.ItemClick += MListView_ItemClick;
            mListView.ItemLongClick += MListView_ItemLongClick;
            mArama.TextChanged += MArama_TextChanged;
            listeguncelle();
        }


        private void MListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var position = e.Position;
            var gelenStajLine = mStajmodels[position];
            var kurumAd = gelenStajLine.Kurumad.ToString();
            var kurumTarih = gelenStajLine.YorumTarih.ToShortDateString().ToString();

            AlertDialog.Builder alert = new AlertDialog.Builder(this); //Ekrana Display Alert Yaptırmamız için AlertDialog Class'ını kullanıyoruz ! 
            alert.SetTitle("Silmeyi Onayla");
            alert.SetMessage(kurumAd+" Adlı "+kurumTarih+" tarihli kaydınız silinecektir, emin misiniz ?");
            alert.SetPositiveButton("Evet", (senderAlert, args) => {
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("delete from tbl_icerik where yorum_id='" + Convert.ToInt32(gelenStajLine.Yorumid.ToString()) + "'",con);
                        int check = cmd.ExecuteNonQuery();
                        
                        if (check!=0)
                        {
                            Toast.MakeText(this,kurumAd+" Adlı Kaydınız Silindi !", ToastLength.Short).Show();
                            Thread thread = new Thread(ActLikeRequest); //Yine thread kullandık ! 
                            thread.Start();
                        }
                    }
                }
                catch
                {
                    Toast.MakeText(this, "İnternet Bağlantısı Mevcut Değil !", ToastLength.Short).Show();
                }
                finally
                {
                    
                    con.Close();
                }
                
            });

            alert.SetNegativeButton("Hayır", (senderAlert, args) => {
                Toast.MakeText(this, "İptal Edildi !", ToastLength.Short).Show();
            });

            Dialog dialog = alert.Create();
            dialog.Show();
            
        }
        private void ActLikeRequest()
        {
            Thread.Sleep(1200); //Milisaniye cinsinden thread'i uyuttuk, progressbar için.
            RunOnUiThread(() => { listeguncelle(); }); //Bir user interface işlemi olduğu için bu şekilde güncelleme yapıyoruz ! 

        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var position = e.Position; //Item'ın class'da ilgili pozisyonun alıyor e.Position buda ItemClickEventArgs'dan geliyor.

            var gelenStajLine = mStajmodels[position];
            string Yorum = (gelenStajLine.Yorum).ToString();
            DateTime YorumTarih = (gelenStajLine.YorumTarih); //Veritabanında Datetime tipinde tutulduğu için burada da bu şekilde karşılamamız gerekiyor.
            string Adres = (gelenStajLine.KurumAdres).ToString();
            string Tarih = (YorumTarih.Day, YorumTarih.Month, YorumTarih.Year).ToString();
            var intent = new Intent(this, typeof(listeYorumActivity));
            intent.PutExtra("Yorum", Yorum);
            intent.PutExtra("Adres", Adres);
            intent.PutExtra("Tarih", Tarih);
            StartActivity(intent);
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
        }

        private void MArama_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            listeguncelle(); //StringComparasion için bir ExtMethod Yazdık ! 
            //Ve arama yaparken extension bir method kullanarak aramamızı case - sensitive olmayan hale getireceğiz ! 
            List<stajmodel> stajArama = (from liste in mStajmodels where liste.Yorumid.Equals(mArama.Text) || liste.Kurumad.Contains(mArama.Text, StringComparison.OrdinalIgnoreCase) || liste.KurumDepartman.Contains(mArama.Text, StringComparison.OrdinalIgnoreCase) || liste.CalisanSayisi.Contains(mArama.Text, StringComparison.OrdinalIgnoreCase) || liste.KurumGorev.Contains(mArama.Text, StringComparison.OrdinalIgnoreCase) || liste.KurumAdres.Contains(mArama.Text, StringComparison.OrdinalIgnoreCase) || liste.Yorum.Contains(mArama.Text, StringComparison.OrdinalIgnoreCase) || liste.YorumTarih.Equals(mArama.Text) select liste).ToList();//Bu bir linq yazımıdır liste dediğimiz bizim keyword'umuz gibidir ama özel bir anlamı vardır ! 
            //Yenilenmiş listView ve class'ımız Aramalara göre ! 
            mAdapter = new listeAdapter2(this, Resource.Layout.listeyardimci, stajArama);
            mListView.Adapter = mAdapter;
            if (mArama.Text != "")
            {
                mStajmodels = stajArama;
            }
            else
            {
                listeguncelle();
            }
        }

        public override void OnBackPressed()//Back tuşana basıldığında ne yapacağını yazıyoruz ! 
        {
            this.Finish();
        }
        public override bool OnCreateOptionsMenu(IMenu menu) //Aynısı javada da var androidle uğraşanlar bilir. ActionBar visible hale geldi ! 
        {
            MenuInflater.Inflate(Resource.Menu.actionbar2, menu); //ActionBar'ın gözükmesi için handle ettik ! 
            return base.OnCreateOptionsMenu(menu);
            //Ama burası hala search icon'ını çalıştırmaz başka bir methodu da override etmemiz gerekir !  
        }

        public override bool OnOptionsItemSelected(IMenuItem item) //Ilgılı action itemları kullanmak içi handle ediyoruz ! 
        {
            switch (item.ItemId)
            {
                case Resource.Id.search2: //Search'ın tıklandığı case olay durum .... Burada item'ın item id'sini kullanıyoruz ! 

                    if (mAnimasyonOlurken) //Bu animasyon olurken return true döndürecek ve aşağıdaki kodları çalıştımayacak ayrıca karmaşaya yol açamayıp karışıklığa neden olamayacak ! 
                    {
                        return true;
                    }

                    if (!mAnimasyonAsagi) //Ünlem işareti not operatörü oluyor, eğer animasyon aşağı hareket etmemişse...
                    {
                        //ListView Yukarıdayken ! 
                        MyAnimation anim = new MyAnimation(mListView, mListView.Height - mArama.Height); //Bu listView'i shrink edecek ve bize edittext'imizi gösterecektir !
                        anim.Duration = 600;
                        mListView.StartAnimation(anim);
                        anim.AnimationStart += Anim_AnimationStartDown;
                        //Bu aşağıdaki de bütün konteynır'ı hareket ettirmek için yazacağız ! 
                        anim.AnimationEnd += Anim_AnimationEndDown; //Animasyonun aşağıya inişi tamamlandığında 
                        mContainer.Animate().TranslationYBy(mArama.Height).SetDuration(600).Start(); //Container'ın yeteri kadar aşağı indiğinden emin olmak için yapıyoruz ! 
                    }
                    else
                    {
                        //ListView Aşağıdayken ! 
                        MyAnimation anim = new MyAnimation(mListView, mListView.Height + mArama.Height); //Bu listView'i shrink edecek ve bize edittext'imizi gösterecektir !
                        anim.Duration = 600;
                        mListView.StartAnimation(anim);
                        anim.AnimationStart += Anim_AnimationStartUp;
                        anim.AnimationEnd += Anim_AnimationEndUp; //Yani animasyonun yukarıya kaydırması bittiğinde ! 
                        //Bu aşağıdaki de bütün konteynır'ı hareket ettirmek için yazacağız ! 
                        mContainer.Animate().TranslationYBy(-mArama.Height).SetDuration(600).Start(); //Container'ın yeteri kadar aşağı indiğinden emin olmak için yapıyoruz ! 

                    }
                    mAnimasyonAsagi = !mAnimasyonAsagi; //Buna toggle etme işlemi deniyor true'yu false false'ı true yapıyor ! 
                    return true;
                default:
                    return base.OnOptionsItemSelected(item); //Return ile birşey döndürdüğümüz için break eklemiyoruz ! 


            } //Switch burada bitiyor dikkat ! 
        }

        private void Anim_AnimationEndUp(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
        {
            mAnimasyonOlurken = false;
            //Burada şimdiki yapacağımız listView'in yukarı çıkması tamamlandıktan sonra klavyeyi kapatmak ! 
            mArama.ClearFocus(); //Clear focus olunca aşağıdaki kodlarla ekrandaki input keyboard kapanıyor.
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService); //Buda bize bir inputmethodmanager nesnesi dönüyor ! 
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways); //Görüldüğü üzere klavyeyi her zaman olmayacak şekilde visible hale getirdik ! 
        }

        private void Anim_AnimationEndDown(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
        {
            mAnimasyonOlurken = false; //Animasyon Bittiği için false yaptık ! //Bunlara bayraklar yani flagsler de deniyor ! 
        }

        private void Anim_AnimationStartDown(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
        {
            mAnimasyonOlurken = true;
            mArama.Animate().AlphaBy(1.0f).SetDuration(600).Start(); //Alpha'yı 1'e getirerek tamamen EditText'i visible hale getiriyoruz ! Buna fade in deniyor ! 
        }
        private void Anim_AnimationStartUp(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
        {
            mAnimasyonOlurken = true;
            mArama.Animate().AlphaBy(-1.0f).SetDuration(240).Start(); //Alpha'yı tamamen değiştirerek invisible hale getiriyoruz !  Buna fade out deniyor !
        }
    }
}
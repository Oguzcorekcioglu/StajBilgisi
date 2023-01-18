using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;


namespace StajBilgisi
{
    [Activity(Label = "StajBilgisi", MainLauncher = true,Icon ="@drawable/pen2")]
    public class MainActivity : Activity
    {
        SqlConnection con = BaglantiSingleton.Baglanti;
        

        private Button mBtnKaydol;
        private Button mBtnGiris;
        private ProgressBar mProgressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Ana gösterilecek view'i aşağıda yazıyoruz ! 
            SetContentView(Resource.Layout.Main);
            
            //Aşağıda ise ana Main layout'umuzu ilgilendiren objeleri handle ettik !
            mBtnKaydol = FindViewById<Button>(Resource.Id.btnKayıtOl);
            mBtnGiris = FindViewById<Button>(Resource.Id.btnGiris);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            
            mBtnKaydol.Click += MBtnKaydol_Click;
            mBtnGiris.Click += MBtnGiris_Click;
        }

        private void MBtnGiris_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            dialog_giris_yap giris_dialog = new dialog_giris_yap(); //Ilgili Class'ımızı kullanıyoruz ! 
            giris_dialog.Show(transaction, "Dialog Fragment2"); //Dialog'u ekranda göstermemizi sağlar ! 

            giris_dialog.mGirisYapTamam += Giris_dialog_mGirisYapTamam; //Hani event yazmıştıkya onun sayesinde butona tıklandığında ilgili propların değerini alabiliyoruz ! 
        }

        private void Giris_dialog_mGirisYapTamam(object sender, girisYapEventArgs e)
        {
            //Buraları yapmadan önce istersek System.Sql.Client manage packet'ini kullanabiliriz.
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select uye_id from tbl_uye where kullanici_ad='" + e.KullaniciAdi.ToString() + "'and kullanici_sifre='" + e.KullaniciSifre.ToString() + " '", con);
                SqlDataAdapter sda = new SqlDataAdapter("select COUNT(*) from tbl_uye where kullanici_ad='" + e.KullaniciAdi.ToString() + "'and kullanici_sifre='" + e.KullaniciSifre.ToString() + " '", con);
                SqlDataAdapter ss = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                DataTable ds = new DataTable();
                ss.Fill(ds);
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    
                    var intent = new Intent(this, typeof(GirisActivity));
                    var id = Convert.ToInt32(ds.Rows[0][0]);
                    intent.PutExtra("id", id);
                    
                    StartActivity(intent); //Eğer açtığımız sayfaya verilerimizi göndermek istiyorsak bir intent yapısı kullanmamız gerekiyor diyebiliriz !
                    
                    dt.Clear();
                    ds.Clear();
                    Finish();
                    
                }
                else
                {
                    Toast.MakeText(this, "Kullanıcı adı veya şifre yanlış !", ToastLength.Short).Show();
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
        }

        //Evet görüldüğü üzere aşağıdaki mantık pull up the dialog işlemini yapmamızı sağlar !
        private void MBtnKaydol_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction(); //FragmentManager bizi static methodumuz ! 

            dialog_kayit_ol kayitoldialog = new dialog_kayit_ol();
            kayitoldialog.Show(transaction, "Dialog Fragment"); //Ilgılı class'ı dialogfragment'den düzelttiğimiz için kullanabiliyoruz ! 
            
            kayitoldialog.mKayitOlTamam += Kayitoldialog_mKayitOlTamam; //Hani event yazmıştıkya işte bu o ! 
        }

        private void Kayitoldialog_mKayitOlTamam(object sender, kayitOlEventArgs e)
        {
            SqlDataAdapter sda = new SqlDataAdapter("select COUNT(*) from tbl_uye where kullanici_ad='" + e.KullaniciAd.ToString() + "'and kullanici_sifre='" + e.KullaniciSifre.ToString() + " '", con);
            DataTable dtx = new DataTable();
            sda.Fill(dtx);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    if (e.KullaniciAd == "" || e.KullaniciSifre == "" || e.Ad == "" || e.Soyad == "" || e.Cinsiyet == "" || e.Il == "" || e.Ilce == "" || e.Bolum == "")
                    {

                        Toast.MakeText(this, "Bütün formu eksiksiz doldurunuz ! ", ToastLength.Short).Show();
                    }
                    
                    else if (dtx.Rows[0][0].ToString() != "1")
                    {
                        SqlCommand cmd1 = new SqlCommand("insert into tbl_uye (kullanici_ad,kullanici_sifre,uye_ad,uye_soyad,uye_cinsiyet,uye_il,uye_ilce,uye_bolum) values ('" + e.KullaniciAd.ToString() + "','" + e.KullaniciSifre.ToString() + "','" + e.Ad.ToString() + "','" + e.Soyad.ToString() + "','" + e.Cinsiyet.ToString() + "','" + e.Il.ToString() + "','" + e.Ilce.ToString() + "','" + e.Bolum.ToString() + "')", con);
                        cmd1.ExecuteNonQuery();
                        Toast.MakeText(this, "Üyeliğiniz Oluşturulmuştur", ToastLength.Short).Show();
                        mProgressBar.Visibility = Android.Views.ViewStates.Visible;
                        dtx.Clear();
                        Thread thread = new Thread(ActLikeRequest);
                        thread.Start();
                    }
                    else
                    {
                        Toast.MakeText(this, "Şifrenizi Değiştiriniz !", ToastLength.Short).Show();
                    }
                    
                }
            }
            catch
            {
                Toast.MakeText(this, "Internet bağlantısı mevcut değil ! ", ToastLength.Short).Show();
            }
            finally
            {
                con.Close();
            }

        }

        private void ActLikeRequest()
        {
            Thread.Sleep(4000); //Milisaniye cinsinden thread'i uyuttuk, progressbar için.
            RunOnUiThread(() =>{ mProgressBar.Visibility = Android.Views.ViewStates.Invisible; }); //Sonrada ProgressBar'ı invisible yapıyoruz.
            
        }
    }
}


using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.Data;
using System.Data.SqlClient;

namespace StajBilgisi
{
    [Activity(Label = "Kayıt Ekle")]
    public class kayitEkleActivity : Activity
    {
        SqlConnection con = BaglantiSingleton.Baglanti;
        EditText mKurumAd;
        EditText mKurumCalisanSayisi;
        EditText mKurumDepartman;
        EditText mKurumGorev;
        EditText mKurumAdres;
        AutoCompleteTextView mKurumYorum;
        Button btnKayitEkle;
        DateTime d;
        string zaman;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.kayitEkle);
            mKurumAd = FindViewById<EditText>(Resource.Id.edtKayitEkleKurumAdi);
            mKurumCalisanSayisi = FindViewById<EditText>(Resource.Id.edtKayitEkleCalisanSayisi);
            mKurumDepartman = FindViewById<EditText>(Resource.Id.edtKayitEkleKurumDepartman);
            mKurumGorev = FindViewById<EditText>(Resource.Id.edtKayitEkleKurumGorev);
            mKurumAdres = FindViewById<EditText>(Resource.Id.edtKayitEkleKurumAdres);
            mKurumYorum = FindViewById<AutoCompleteTextView>(Resource.Id.edtKayitEkleKurumYorum);
            btnKayitEkle = FindViewById<Button>(Resource.Id.btnKayitOlustur);
            d = DateTime.UtcNow;
            zaman = d.ToShortDateString();
            
            btnKayitEkle.Click += BtnKayitEkle_Click;
            
        }

        private void BtnKayitEkle_Click(object sender, EventArgs e)
        {
            
                    
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    if (mKurumAd.Text == "" || mKurumDepartman.Text == "" || mKurumCalisanSayisi.Text == "" || mKurumGorev.Text == "" || mKurumAdres.Text == "" || mKurumYorum.Text == "")
                    {
                        Toast.MakeText(this, "Bütün formu eksiksiz doldurunuz ! ", ToastLength.Short).Show();
                    }
                    else
                    {


                        string query = "insert into tbl_icerik (kurum_ad,kurum_departman,kurum_calisan,kurum_gorev,kurum_adres,kurum_yorum,uye_id,tarih)" + "values (@kurumad,@kurumdepartman,@kurumcalisan,@kurumgorev,@kurumadres,@kurumyorum,@uyeid,@tarih)";
                        SqlCommand cmd = new SqlCommand(query, con);

                        cmd.Parameters.Add("@kurumad", SqlDbType.NVarChar, 500).Value = mKurumAd.Text.ToString();
                        cmd.Parameters.Add("@kurumdepartman", SqlDbType.NVarChar, 500).Value = mKurumDepartman.Text.ToString();
                        cmd.Parameters.Add("@kurumcalisan", SqlDbType.Int).Value = Convert.ToInt32(mKurumCalisanSayisi.Text);
                        cmd.Parameters.Add("@kurumgorev", SqlDbType.NVarChar, 500).Value = mKurumGorev.Text.ToString();
                        cmd.Parameters.Add("@kurumadres", SqlDbType.NVarChar, 600).Value = mKurumAdres.Text.ToString();
                        cmd.Parameters.Add("@kurumyorum", SqlDbType.NVarChar, 1000).Value = mKurumYorum.Text.ToString();
                        cmd.Parameters.Add("@uyeid", SqlDbType.Int).Value = Convert.ToInt32(Intent.GetIntExtra("idno", 0));
                        cmd.Parameters.Add("@tarih", SqlDbType.DateTime).Value = zaman;
                        int check = cmd.ExecuteNonQuery();
                        if (check != 0)
                        {
                            Toast.MakeText(this, "Kayıt Başarılıdır...", ToastLength.Long).Show();
                        }
                    }

                }
            }
            catch
            {
                Toast.MakeText(this, "İnternet Bağlantısı Mevcut Değil !", ToastLength.Long).Show();
            }
            finally
            {
                con.Close();
            }
            


        }
        
    }
}
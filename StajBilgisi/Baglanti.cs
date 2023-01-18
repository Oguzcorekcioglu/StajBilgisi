using System.Data.SqlClient;

namespace StajBilgisi
{
    public class BaglantiSingleton //Adeta bir SingletonNesne olarak modelliyoruz ! 
    {
        private static SqlConnection baglanti;
        
        
        public static SqlConnection Baglanti
        {
            get
            {
                if(baglanti==null)
                    baglanti= new SqlConnection("Server=tcp:forstaj.database.windows.net,1433; Initial Catalog=stajazure; Persist Security Info=False;User ID=oguzDblU; Password=; MultipleActiveResultSets=False; Encrypt=False; Connection Timeout=60;");

                return baglanti; 
            }
        }
        
    }
}
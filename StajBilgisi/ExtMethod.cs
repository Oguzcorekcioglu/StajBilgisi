using System;

namespace StajBilgisi
{
    public static class ExtMethod
    {
        //Bu source bizim sınıf nesnlerimizi kapsayacak bir keyword gibidir. Extension metotlar bilindiği üzere c#'daki hazır gelen sınıflara bizim ürettiğimiz metotları eklememiz için bir yoldur. Extension metotlar sınıf adından ulaşmamız ve çalışma zamanında 1 kere yaratılması için static olarak tanımlanmıştır.

        public static bool Contains (this string source, string mKontrol,StringComparison comparasionType)
        {
            return (source.IndexOf(mKontrol, comparasionType) >= 0); //source'un içinde bulunan string'in index'i 0'dan büyük olanları değiştiriyor ! 

        }

    }
}
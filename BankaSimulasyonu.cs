using System;
using System.Collections.Generic;

namespace BankaSimulasyonu
{
    // 1. Temel Sınıf (Inheritance için Base Class)
    public abstract class BankaHesabi
    {
        public string HesapNo { get; set; }
        public string Sahip { get; set; }
        public decimal Bakiye { get; protected set; }

        public BankaHesabi(string hesapNo, string sahip, decimal ilkBakiye)
        {
            HesapNo = hesapNo;
            Sahip = sahip;
            Bakiye = ilkBakiye;
        }

        // Virtual Metot: Alt sınıflar bu metodu kendine göre özelleştirebilir (Polymorphism)
        public virtual void ParaYatir(decimal miktar)
        {
            Bakiye += miktar;
            Console.WriteLine($"{miktar} TL yatırıldı. Yeni bakiye: {Bakiye} TL");
        }

        public virtual bool ParaCek(decimal miktar)
        {
            if (miktar <= Bakiye)
            {
                Bakiye -= miktar;
                Console.WriteLine($"{miktar} TL çekildi. Kalan bakiye: {Bakiye} TL");
                return true;
            }
            Console.WriteLine("Yetersiz bakiye!");
            return false;
        }

        public abstract void HesapBilgisiGoster(); // Abstract metot: Alt sınıflar uygulamak zorunda
    }

    // 2. Vadesiz Hesap Sınıfı (Kalıtım)
    public class VadesizHesap : BankaHesabi
    {
        public VadesizHesap(string hesapNo, string sahip, decimal bakiye)
            : base(hesapNo, sahip, bakiye) { }

        public override void HesapBilgisiGoster()
        {
            Console.WriteLine($"[Vadesiz Hesap] No: {HesapNo} | Sahip: {Sahip} | Bakiye: {Bakiye} TL");
        }
    }

    // 3. Vadeli Hesap Sınıfı (Kalıtım ve Polymorphism)
    public class VadeliHesap : BankaHesabi
    {
        public double FaizOrani { get; set; }

        public VadeliHesap(string hesapNo, string sahip, decimal bakiye, double faizOrani)
            : base(hesapNo, sahip, bakiye)
        {
            FaizOrani = faizOrani;
        }

        // Para çekme metodu geçersiz kılındı (Override)
        public override bool ParaCek(decimal miktar)
        {
            Console.WriteLine("Uyarı: Vadeli hesaptan para çekildiğinde faiz getirisi etkilenebilir.");
            return base.ParaCek(miktar);
        }

        public override void HesapBilgisiGoster()
        {
            Console.WriteLine($"[Vadeli Hesap] No: {HesapNo} | Sahip: {Sahip} | Bakiye: {Bakiye} TL | Faiz: %{FaizOrani}");
        }
    }

    // 4. Ana Program
    class Program
    {
        static void Main(string[] args)
        {
            // Polymorphism örneği: BankaHesabi tipinde bir listede farklı hesap türlerini tutma
            List<BankaHesabi> hesaplar = new List<BankaHesabi>();

            hesaplar.Add(new VadesizHesap("TR001", "Arzu Güngör", 5000));
            hesaplar.Add(new VadeliHesap("TR002", "Eskişehir Teknik", 10000, 25.5));

            Console.WriteLine("--- Banka İşlem Simülasyonu ---\n");

            foreach (var hesap in hesaplar)
            {
                hesap.HesapBilgisiGoster();
                hesap.ParaYatir(1000);
                hesap.ParaCek(2000);
                Console.WriteLine("-------------------------------");
            }

            // Transfer İşlemi Örneği
            Console.WriteLine("\nTransfer İşlemi:");
            decimal transferMiktari = 1500;
            if (hesaplar[0].ParaCek(transferMiktari))
            {
                hesaplar[1].ParaYatir(transferMiktari);
                Console.WriteLine("Transfer başarıyla tamamlandı.");
            }

            Console.ReadLine();
        }
    }
}

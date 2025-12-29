using Microsoft.EntityFrameworkCore;
using UrunlerYonetim.Data;
using UrunlerYonetim.Models;

Console.OutputEncoding = System.Text.Encoding.UTF8;

//Veritabanı Nesnesi
using var context = new AppDbContext();

bool devamEt = true;

while (devamEt)
{
    Console.Clear();
    Console.WriteLine("-- ÜRÜN YÖNETİM SİSTEMİ --");
    Console.WriteLine("1. Kategori  İşlemleri");
    Console.WriteLine("2. Ürün  İşlemleri");
    Console.WriteLine("0. Çıkış");
    Console.Write("Seçiminiz: ");

    string? secim = Console.ReadLine();

    switch (secim)
    {
        case "1":
            KategoriMenusu(context);
            break;
        case "2":
            UrunMenusu(context);
            break;
        case "0":
            devamEt = false;
            break;
        default:
            Console.WriteLine("Geçersiz seçim. Devam etmek için bir tuşa basın...");
            Console.ReadKey();
            break;
    }

    void KategoriMenusu(AppDbContext db)
    {
        bool geriDon = false;

        while (!geriDon)
        {
            Console.Clear();
            Console.WriteLine("-- KATEGORİ İŞLEMLERİ --");
            Console.WriteLine("1. Listele");
            Console.WriteLine("2. Ekle");
            Console.WriteLine("3. Düzenle");
            Console.WriteLine("4. Sil");
            Console.WriteLine("0. Geri Dön");
            Console.Write("Seçiminiz: ");

            string? secim = Console.ReadLine();

            switch (secim)
            {
                case "1":
                    KategoriListele(db);
                    break;
                case "2":
                    KategoriEkle(db);
                    break;
                case "3":
                    KategoriDuzenle(db);
                    break;
                case "4":
                    KategoriSil(db);
                    break;
                case "0":
                    geriDon = true;
                    break;
            }
        }

        // Kategori işlemleri burada gerçekleştirilecek
    }

    void UrunMenusu(AppDbContext db)
    {
        Console.Clear();
        Console.WriteLine("-- ÜRÜN İŞLEMLERİ --");
        Console.WriteLine("1. Listele");
        Console.WriteLine("2. Ekle");
        Console.WriteLine("3. Düzenle");
        Console.WriteLine("4. Sil");
        Console.WriteLine("0. Geri Dön");
        Console.Write("Seçiminiz: ");

        string? secim = Console.ReadLine();

        switch (secim)
        {
            case "1":
                UrunListele(db);
                break;
            case "2":
                UrunEkle(db);
                break;
            case "3":
                // Ürün düzenleme işlemi
                break;
            case "4":
                // Ürün silme işlemi
                break;
            case "0":
                // Geri dönme işlemi
                break;
        }
    }

    void KategoriEkle(AppDbContext db)
    {
        Console.Clear();
        Console.WriteLine("-- YENİ KATEGORİ EKLEME --");
        Console.Write("Kategori Adı: ");

        string? ad = Console.ReadLine();

        if(!string.IsNullOrEmpty(ad))
        {
            var yeniKategori = new Kategori {Ad = ad, Durum=1};
            db.Kategoriler.Add(yeniKategori);
            db.SaveChanges();
            Console.WriteLine("Kategori başarıyla eklendi. Devam etmek için bir tuşa basın...");
        } else 
        {
            Console.WriteLine("Kategori adı boş olamaz. Devam etmek için bir tuşa basın...");
        }

        Console.ReadKey();
    }

    void KategoriListele(AppDbContext db)
    {
        Console.Clear();
        Console.WriteLine("-- KATEGORİ LİSTESİ --");

        var liste = db.Kategoriler.ToList();
        if (liste.Count == 0)
        {
            Console.WriteLine("Kayıtlı kategori bulunmamaktadır.");
        }
        else
        {
            Console.WriteLine("{0,-5} {1,-20}", "ID", "Kategori Adı");
            Console.WriteLine("----------------------------");

            foreach (var kategori in liste)
            {
                Console.WriteLine("{0,-5} {1, -20}", kategori.Id, kategori.Ad);
            }

            Console.WriteLine("Devam etmek için bir tuşa basın...");
        }

        Console.ReadKey();

    }

    void KategoriDuzenle(AppDbContext db)
    {
        Console.Clear();
        Console.WriteLine("-- KATEGORİ DÜZENLEME --");
        Console.Write("Düzenlemek istediğiniz kategorinin ID'sini girin: ");

        //Önce Kullanıcnın girdiği ID'yi tam sayıya çevirelim
        if(int.TryParse(Console.ReadLine(), out int kategoriId))
        {
            //1. Kaydı Bul
            var kategori = db.Kategoriler.Find(kategoriId);

            //Kategori boş degilse
            if(kategori != null)
            {
                Console.WriteLine($"Mevcut Adı: {kategori.Ad}");
                Console.Write("Yeni Adı: ");
                string? yeniAd = Console.ReadLine();

                //Yeni kategori adı bpoş değilse
                if(!string.IsNullOrEmpty(yeniAd))
                {
                    kategori.Ad = yeniAd;
                    db.SaveChanges();
                    Console.WriteLine("Kategori başarıyla güncellendi.");
                }
            } else
            {
                Console.WriteLine("Belirtilen ID'ye sahip kategori bulunamadı.");
            }
        } else 
        {
            Console.WriteLine("Geçersiz ID girdiniz.");
        }

        Console.WriteLine("Devam etmek için bir tuşa basın...");
        Console.ReadKey();
    }

    void KategoriSil(AppDbContext db)
    {
        Console.Clear();
        Console.WriteLine("-- KATEGORİ SİLME --");
        Console.Write("Silmek istediğiniz kategorinin ID'sini girin: ");

        //Önce Kullanıcnın girdiği ID'yi tam sayıya çevirelim
        if(int.TryParse(Console.ReadLine(), out int kategoriId))
        {
            //Kategorinin altındaki ürünlere de bakıyoruz
            var kategori = db.Kategoriler
                           .Include(k=> k.Urunler)
                           .FirstOrDefault(k => k.Id == kategoriId);
            
            if(kategori != null)
            {   
                //1. Kategori altında ürün var mı? Kontrol et.
                if(kategori.Urunler.Count > 0)
                {
                    Console.WriteLine($"Hata: '{kategori.Ad}' kategorisine bağlı {kategori.Urunler.Count} ürün var.");
                    Console.WriteLine("Önce bağlı ürünleri silmeli veya başka kategoriye taşımalısınız.");

                } else
                {
                  //2. Soft Delete için onay al
                  Console.Write($"'{kategori.Ad}' kategorisini silmek istediğinizden emin misiniz? (E/H);");
                  string? onay = Console.ReadLine();

                  if(onay != null && (onay.ToUpper() == "E"))
                  {
                    //Soft Delete: Durum alanını 3 yap
                    kategori.Durum = 3;
                    db.SaveChanges();
                    Console.WriteLine("Kategori başarıyla silindi.");
                  } else
                  {
                    Console.WriteLine("Silme işlemi iptal edildi.");
                  }
                  
                }
            } else
            {
                Console.WriteLine("Belirtilen ID'ye sahip kategori bulunamadı.");
            }

        } else
        {
            Console.WriteLine("Geçersiz ID girdiniz."); 
        }

        Console.WriteLine("Devam etmek için bir tuşa basın...");
        Console.ReadKey();
    }

    void UrunEkle(AppDbContext db)
    {
        Console.Clear();
        Console.WriteLine("-- YENİ ÜRÜN EKLEME --");

        //Önce Kullanıcı hangi kategoriyi seçeceğini görsün
        var kategoriler = db.Kategoriler.ToList();
        if(kategoriler.Count == 0)
        {
            Console.WriteLine("Önce kategori eklemelisiniz. Devam etmek için bir tuşa basın...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Mevcut Kategoriler:");
        foreach(var k in kategoriler)
        {
            Console.WriteLine($"{k.Id} - {k.Ad}");
        }

        Console.WriteLine("----------------------------");

        // 2. Kullanıcıdan verileri al
        Console.Write("Kategori ID Seçiniz: ");
        /*if (!int.TryParse(Console.ReadLine(), out int secilenKategoriId) || !kategoriler.Any(k => k.Id == secilenKategoriId))
        {
            Console.WriteLine("Geçersiz Kategori ID!");
            Console.ReadKey();
            return;
        }*/

        if(!int.TryParse(Console.ReadLine(), out int secilenKategoriId) || !kategoriler.Any(k => k.Id == secilenKategoriId)) 
        {
            Console.WriteLine("Geçersiz Kategori ID!");
            Console.ReadKey();
            return;
        }

        Console.Write("Ürün Adı: ");
        string? ad = Console.ReadLine();

        Console.Write("Fiyat: ");
        decimal.TryParse(Console.ReadLine(), out decimal fiyat);

        Console.Write("Stok Adedi: ");
        int.TryParse(Console.ReadLine(), out int stok);

        if(!string.IsNullOrEmpty(ad))
        {
           var yeniUrun = new Urun
           {
               Ad = ad,
               Fiyat = fiyat,
               Stok = stok,
               KategoriId = secilenKategoriId,
               Durum = 1
            };

            //Kaydet
            db.Urunler.Add(yeniUrun);
            db.SaveChanges();
            Console.WriteLine("Ürün başarıyla eklendi. Devam etmek için bir tuşa basın...");

        } else 
        {
            Console.WriteLine("Ürün adı boş olamaz. Devam etmek için bir tuşa basın...");
        }

        Console.ReadKey();
    }

    void UrunListele(AppDbContext db)
    {
        Console.Clear();
        Console.WriteLine("-- ÜRÜN LİSTESİ --");

        var Urunler = db.Urunler
                        .Include(u => u.Kategori)
                        .ToList();
        //Ürün var  mı?
        if(Urunler.Count == 0)
        {
            Console.WriteLine("Kayıtlı ürün bulunmamaktadır.");
        } else
        {
            Console.WriteLine("{0,-5} {1,-20} {2,-10:C2} {3,-8} {4,-15}", "ID", "Ürün Adı", "Fiyat", "Stok", "Kategori");
            Console.WriteLine("--------------------------------------------------------------");

            foreach (var u in Urunler)
            { 
                Console.WriteLine("{0,-5} {1,-20} {2,-10:C2} {3,-8:N0} {4,-15}", 
                u.Id, u.Ad, u.Fiyat.ToString("N2") + " TL", u.Stok, u.Kategori?.Ad ?? "Yok");            }

            Console.WriteLine("\nDevam etmek için bir tuşa basın...");   
        }
        Console.ReadKey();
    }
}   
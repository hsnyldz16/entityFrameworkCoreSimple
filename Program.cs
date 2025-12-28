using Microsoft.EntityFrameworkCore;
using UrunlerYonetim.Data;
using UrunlerYonetim.Models;

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

        // Ürün işlemleri burada gerçekleştirilecek
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
}   
# Ürün Yönetim Sistemi (Product Management System)

## Proje Açıklaması
Bu proje, C# ve .NET 9.0 kullanılarak geliştirilmiş basit bir konsol tabanlı Ürün Yönetim Sistemi uygulamasıdır. Entity Framework Core ile veritabanı etkileşimleri sağlanmaktadır. Uygulama, ürün ve kategorileri yönetmek için temel CRUD (Oluşturma, Okuma, Güncelleme, Silme) işlemlerini destekler. Kategori yönetimi tam olarak uygulanmış olup, ürün yönetimi için bir iskelet mevcuttur.

## Özellikler
- **Kategori Yönetimi:**
    - Kategori Ekleme
    - Kategorileri Listeleme
    - Kategori Düzenleme
    - Kategori Silme (Soft Delete ile: Durum=3 olarak işaretlenir)
- **Ürün Yönetimi:** (Mevcut durumda sadece menü iskeleti bulunmaktadır. Geliştirme aşamasındadır.)
- **Soft Delete (Yumuşak Silme):** Hem kategori hem de ürün kayıtları için `Durum` alanı kullanılarak yumuşak silme işlemi uygulanır. Silinen kayıtlar veritabanından tamamen kaldırılmaz, sadece `Durum` değeri `3` olarak güncellenir ve sorgularda otomatik olarak filtrelenir.

## Teknolojiler
- **C#**
- **.NET 9.0**
- **Entity Framework Core 9.0** (Veritabanı işlemleri için ORM)
- **SQL Server** (Veritabanı)

## Kurulum
1.  **Projeyi Klonlayın veya İndirin:**
    Projenin kaynak kodunu bilgisayarınıza indirin. Eğer bir Git deposundan klonluyorsanız:
    ```bash
    git clone https://github.com/yourusername/UrunlerYonetim.git # Bu kısmı kendi depo URL'nizle değiştirin
    cd UrunlerYonetim/UrunlerYonetim
    ```
    Eğer indirdiyseniz, proje dizinine (`UrunlerYonetim/UrunlerYonetim`) gidin.

2.  **Veritabanı Oluşturma ve Migrasyonlar:**
    Bu proje, Entity Framework Core Migrasyonlarını kullanır. Veritabanınızı oluşturmak ve güncel şemayı uygulamak için projenin kök dizininde (UrunlerYonetim.csproj dosyasının bulunduğu yer) aşağıdaki komutları çalıştırın:
    ```bash
    dotnet ef database update
    ```
    *Not: Bağlantı dizesi `Data/AppDbContext.cs` içerisinde `Server=.\SQLEXPRESS;Database=UrunlerDb;Trusted_Connection=True;TrustServerCertificate=True;` olarak yapılandırılmıştır. SQL Server Express veya benzeri bir yerel SQL Server kurulumunuzun olduğundan emin olun veya bağlantı dizesini kendi SQL Server ayarlarınıza göre güncelleyin.*

3.  **Bağımlılıkları Yükleme:**
    Projenin bağımlılıklarını yüklemek için aşağıdaki komutu çalıştırın:
    ```bash
    dotnet restore
    ```

## Uygulamayı Çalıştırma
Projeyi derlemek ve çalıştırmak için aşağıdaki komutu kullanın:
```bash
dotnet run
```

Bu komut uygulamayı başlatacak ve konsol menüsünü gösterecektir.

## Kullanım
Uygulama başlatıldıktan sonra, konsol arayüzü üzerinden Kategori ve Ürün işlemleri için seçenekler sunulacaktır. İlgili sayıları tuşlayarak menüler arasında gezinebilir ve işlemleri gerçekleştirebilirsiniz.

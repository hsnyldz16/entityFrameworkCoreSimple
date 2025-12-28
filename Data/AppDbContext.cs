using Microsoft.EntityFrameworkCore;
using UrunlerYonetim.Models;

namespace UrunlerYonetim.Data;

public class AppDbContext: DbContext
{
    
    //Veritabanındaki tabloları temsil edecek DbSet özellikleri
    public DbSet<Kategori> Kategoriler {get; set;}
    public DbSet<Urun> Urunler {get; set;}

    //Bağlantı sringi burada tanımlanır
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
               "Server=.\\SQLEXPRESS;Database=UrunlerDb;Trusted_Connection=True;TrustServerCertificate=True;"
            );
        }
    }

    // İlişki ve kuralları burada tanımlayabiliriz
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        //Soft Delete için global filter: Durum !=3 olanları göster
        modelBuilder.Entity<Kategori>().HasQueryFilter(k => k.Durum != 3);
        modelBuilder.Entity<Urun>().HasQueryFilter(u => u.Durum != 3);

        base.OnModelCreating(modelBuilder);
    }
}
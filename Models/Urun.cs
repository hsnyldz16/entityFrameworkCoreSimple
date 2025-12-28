namespace UrunlerYonetim.Models;


public class Urun
{
    public int Id {get; set;}
    public string Ad {get; set;} = string.Empty;
    public decimal Fiyat {get; set;}
    public int Stok {get; set;}

    //Durum: 1 = Aktif, 2 = Pasif, 3=Silinmiş
    public int Durum {get; set;} = 1;

    //Hangi kategoriye ait?
    public int KategoriId {get; set;}
    public Kategori? Kategori { get; set; }
}
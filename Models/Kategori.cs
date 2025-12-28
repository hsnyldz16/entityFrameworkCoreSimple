namespace UrunlerYonetim.Models;

public class Kategori
{
    public int Id {get; set;}
    public string Ad {get; set;} = string.Empty;

    //Durum: 1 = Aktif, 2 = Pasif, 3=Silinmiş
    public int Durum {get; set;} = 1;

    //Bir kategorinin birden fazla ürünü olabilir
    public List<Urun> Urunler {get; set;} = new List<Urun>();
}
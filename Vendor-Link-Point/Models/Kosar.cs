using System.Collections.Generic;
using System.Linq;

namespace Vendor_Link_Point.Models
{
    public class Kosar
    {
        public List<KosarTetel> Tetelek { get; set; } = new List<KosarTetel>();

        public void Hozzaad(Product termek, int mennyiseg)
        {
            var letezoTetel = Tetelek.FirstOrDefault(t => t.Termek.Id == termek.Id);
            if (letezoTetel == null)
                Tetelek.Add(new KosarTetel { Termek = termek, Mennyiseg = mennyiseg });
            else
                letezoTetel.Mennyiseg += mennyiseg;
        }

        public void Eltavolit(int termekId)
        {
            Tetelek.RemoveAll(t => t.Termek.Id == termekId);
        }

        public decimal OsszesitettAr() => Tetelek.Sum(t => t.Termek.Ar * t.Mennyiseg);
        public int OsszesDarabszam() => Tetelek.Sum(t => t.Mennyiseg);
    }
}
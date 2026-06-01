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

        public void Urit() => Tetelek.Clear();

        public void FrissitMennyiseg(int termekId, int valtozas)
        {
            var tetel = Tetelek.FirstOrDefault(t => t.Termek.Id == termekId);
            if (tetel != null)
            {
                int ujMennyiseg = tetel.Mennyiseg + valtozas;

                // Ne engedjük a raktárkészlet fölé
                if (ujMennyiseg > tetel.Termek.Raktarkeszlet)
                {
                    ujMennyiseg = tetel.Termek.Raktarkeszlet;
                }

                // Ha nullára vagy az alá csökkenti, akkor kivesszük a kosárból
                if (ujMennyiseg <= 0)
                {
                    Eltavolit(termekId);
                }
                else
                {
                    tetel.Mennyiseg = ujMennyiseg;
                }
            }
        }

        public decimal OsszesitettAr() => Tetelek.Sum(t => t.Termek.Ar * t.Mennyiseg);
        public int OsszesDarabszam() => Tetelek.Sum(t => t.Mennyiseg);
    }
}
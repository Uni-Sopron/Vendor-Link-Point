using Xunit;
using Vendor_Link_Point.Models;
using System.Collections.Generic;

namespace Vendor_Link_Point.Tests
{
    public class KosarTests
    {
        [Fact]
        public void OsszesitettAr_HelyesenSzamoljaKiAVegosszeget()
        {
            // Arrange (Előkészítés)
            var kosar = new Kosar();

            var tv = new TV { Id = 1, Nev = "Teszt TV", Ar = 100000 };
            var konyv = new Konyv { Id = 2, Nev = "Teszt Könyv", Ar = 5000 };

            // Beleteszünk 1 db TV-t (100.000) és 2 db Könyvet (2 * 5.000)
            kosar.Hozzaad(tv, 1);
            kosar.Hozzaad(konyv, 2);

            // Act (Cselekvés)
            decimal vegosszeg = kosar.OsszesitettAr();

            // Assert (Ellenőrzés)
            // Elvárt eredmény: 100.000 + 10.000 = 110.000
            Assert.Equal(110000, vegosszeg);
        }

        [Fact]
        public void UresKosar_OsszesitettAra_Nulla()
        {
            // Arrange
            var kosar = new Kosar();

            // Act
            decimal vegosszeg = kosar.OsszesitettAr();

            // Assert
            Assert.Equal(0, vegosszeg);
        }
    }
}
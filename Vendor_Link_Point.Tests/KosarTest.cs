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

            // TV létrehozása minden kötelező mezővel
            var tv = new TV
            {
                Id = 1,
                Nev = "Teszt TV",
                Ar = 100000,
                Gyarto = "Teszt Gyártó",
                KereskedoId = "VLP-TESZT",
                Kategoria = "Elektronika",
                Raktarkeszlet = 10,
                Kepatlo = 55,
                Felbontas = "4K"
            };

            // Könyv létrehozása minden kötelező mezővel
            var konyv = new Konyv
            {
                Id = 2,
                Nev = "Teszt Könyv",
                Ar = 5000,
                Gyarto = "Teszt Kiadó",
                KereskedoId = "VLP-TESZT",
                Kategoria = "Könyv",
                Raktarkeszlet = 10,
                Szerzo = "Teszt Szerző",
                Isbn = "123-456-789"
            };

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
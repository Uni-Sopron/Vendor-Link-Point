using Xunit;
using Vendor_Link_Point.Models;
using System.Linq;

namespace Vendor_Link_Point.Tests
{
    public class KosarTests
    {
        // Segédmetódus a teszt termékek gyors létrehozásához
        private TV CreateTestTV(int id = 1, decimal ar = 100000)
        {
            return new TV
            {
                Id = id,
                Nev = "Teszt TV",
                Ar = ar,
                Gyarto = "Teszt Gyártó",
                KereskedoId = "VLP-TESZT",
                Kategoria = "TV-k",
                Raktarkeszlet = 10,
                Kepatlo = 55,
                Felbontas = "4K"
            };
        }

        [Fact]
        public void OsszesitettAr_HelyesenSzamoljaKiAVegosszeget()
        {
            // Arrange
            var kosar = new Kosar();
            var tv1 = CreateTestTV(1, 100000);
            var tv2 = CreateTestTV(2, 50000);

            // Act
            kosar.Hozzaad(tv1, 1);
            kosar.Hozzaad(tv2, 2); // 2 db 50.000 Ft-os TV

            // Assert (100k + 2*50k = 200k)
            Assert.Equal(200000, kosar.OsszesitettAr());
        }

        [Fact]
        public void UresKosar_OsszesitettAra_Nulla()
        {
            var kosar = new Kosar();
            Assert.Equal(0, kosar.OsszesitettAr());
        }

        [Fact]
        public void Hozzaad_UgyanaztATermeketKetszer_NoveliAMennyiseget()
        {
            // Arrange
            var kosar = new Kosar();
            var tv = CreateTestTV(1, 100000);

            // Act: Kétszer adjuk hozzá UGYANAZT a TV-t
            kosar.Hozzaad(tv, 1);
            kosar.Hozzaad(tv, 2);

            // Assert: A kosárban csak 1 fajta termék lehet (1 tétel), de annak a mennyisége 3 kell hogy legyen
            Assert.Single(kosar.Tetelek);
            Assert.Equal(3, kosar.Tetelek.First().Mennyiseg);
        }

        [Fact]
        public void Torol_EltavolitjaAdottTermeketAKosarbol()
        {
            // Arrange
            var kosar = new Kosar();
            var tv1 = CreateTestTV(1, 100000);
            var tv2 = CreateTestTV(2, 50000);

            kosar.Hozzaad(tv1, 1);
            kosar.Hozzaad(tv2, 1);

            // Act: Töröljük az 1-es azonosítójú TV-t
            kosar.Eltavolit(tv1.Id);

            // Assert
            Assert.Single(kosar.Tetelek); // Már csak 1 tétel maradt
            Assert.Equal(2, kosar.Tetelek.First().Termek.Id); // És az a 2-es ID-jú TV
        }

        [Fact]
        public void Urit_MindenTeteletKitorol()
        {
            // Arrange
            var kosar = new Kosar();
            kosar.Hozzaad(CreateTestTV(1, 100000), 1);
            kosar.Hozzaad(CreateTestTV(2, 50000), 2);

            // Act
            kosar.Urit();

            // Assert
            Assert.Empty(kosar.Tetelek);
            Assert.Equal(0, kosar.OsszesitettAr());
        }
    }
}
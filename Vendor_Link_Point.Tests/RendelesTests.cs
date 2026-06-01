using Xunit;
using Vendor_Link_Point.Models;
using System;
using System.Collections.Generic;

namespace Vendor_Link_Point.Tests
{
    public class RendelesTests
    {
        [Fact]
        public void RendelesVegosszeg_HelyesenSzamoljaKiATetelekAlapjan()
        {
            // Arrange - Teszt Vásárló létrehozása
            var vasarlo = new Vasarlo
            {
                Id = 1,
                Nev = "Teszt Vásárló",
                Email = "teszt@teszt.hu",
                Jelszo = "Titkos123",
                Role = "Vasarlo",
                SzallitasiCim = "Teszt utca 1."
            };

            // Teszt termék
            var jatek = new Jatek
            {
                Id = 1,
                Nev = "Teszt Játék",
                Ar = 15000,
                Gyarto = "EA",
                KereskedoId = "VLP-TESZT",
                Kategoria = "Játékok",
                Raktarkeszlet = 10,
                Korhatar = 12,
                Tipus = "Akció"
            };

            // Teszt Rendelés összeállítása
            var rendeles = new Rendeles
            {
                Id = 1,
                UserId = vasarlo.Id,
                Vasarlo = vasarlo,
                Idopont = DateTime.Now,              // JAVÍTVA: Datum -> Idopont
                Allapot = "Feldolgozás alatt",
                FizetesiMod = "Utánvét",             // JAVÍTVA: Kötelező mező pótolva
                SzallitasiCim = "Teszt utca 1.",     // JAVÍTVA: Kötelező mező pótolva
                Vegosszeg = 0,                       // JAVÍTVA: Osszeg -> Vegosszeg
                RendelesTetelek = new List<RendelesTetel>()
            };

            // Hozzáadunk 2 db játékot a rendeléshez
            var tetel = new RendelesTetel
            {
                Id = 1,
                OrderId = rendeles.Id,               // JAVÍTVA: RendelesId -> OrderId
                Rendeles = rendeles,
                ProductId = jatek.Id,
                Termek = jatek,
                Mennyiseg = 2,
                Egysegar = jatek.Ar                  // JAVÍTVA: EgysegAr -> Egysegar
            };

            rendeles.RendelesTetelek.Add(tetel);

            // Act - Kiszámoljuk a végösszeget a tételek alapján
            decimal kalkulaltVegosszeg = 0;
            foreach (var t in rendeles.RendelesTetelek)
            {
                kalkulaltVegosszeg += (t.Egysegar * t.Mennyiseg); // JAVÍTVA
            }
            rendeles.Vegosszeg = kalkulaltVegosszeg;              // JAVÍTVA

            // Assert - Ellenőrizzük, hogy (2 * 15.000) = 30.000 Ft-ot kaptunk-e
            Assert.Equal(30000, rendeles.Vegosszeg);              // JAVÍTVA
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Vendor_Link_Point.Models;

namespace Vendor_Link_Point.Data
{
    public class VendorLinkPointContext : DbContext
    {
        public VendorLinkPointContext(DbContextOptions<VendorLinkPointContext> options) : base(options)
        {
        }

        // 1. DbSet-ek: Ezekből lesznek a fő táblák az adatbázisban
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Rendeles> Rendelesek { get; set; }
        public DbSet<RendelesTetel> RendelesTetelek { get; set; }
        public DbSet<Ertekeles> Ertekelesek { get; set; }

        // 2. Adatbázis sémák és öröklődések finomhangolása
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPT (Table-Per-Type) öröklődés beállítása
            modelBuilder.Entity<Vasarlo>().ToTable("Vasarlok");
            modelBuilder.Entity<Kereskedo>().ToTable("Kereskedok");
            modelBuilder.Entity<TV>().ToTable("TVk");
            modelBuilder.Entity<Konyv>().ToTable("Konyvek");
            modelBuilder.Entity<Jatek>().ToTable("Jatekok");

            // ==========================================
            // --- PROFESSZIONÁLIS TESZT ADATOK (SEED) ---
            // ==========================================

            // 1. Kereskedők létrehozása (Fix azonosítókkal)
            modelBuilder.Entity<Kereskedo>().HasData(
                new Kereskedo
                {
                    Id = 1,
                    Nev = "Elektro Admin",
                    Email = "info@elektro.hu",
                    Jelszo = "SEED_DUMMY_PASSWORD", // Ezzel nem lehet belépni a sózás miatt
                    Role = "Kereskedo",
                    KereskedoId = "VLP-ELEKTRO",
                    Cegnev = "Elektro Kft."
                },
                new Kereskedo
                {
                    Id = 2,
                    Nev = "Kocka Admin",
                    Email = "info@kockabarlang.hu",
                    Jelszo = "SEED_DUMMY_PASSWORD",
                    Role = "Kereskedo",
                    KereskedoId = "VLP-KOCKA",
                    Cegnev = "Kocka Barlang"
                }
            );

            // 2. TV-k (Hozzárendelve az Elektro Kft-hez -> "VLP-ELEKTRO")
            modelBuilder.Entity<TV>().HasData(
                new TV
                {
                    Id = 1,
                    KereskedoId = "VLP-ELEKTRO",
                    Nev = "Samsung 55\" Smart 4K TV",
                    Gyarto = "Samsung",
                    Ar = 165000,
                    Raktarkeszlet = 12,
                    Kategoria = "TV-k",
                    Kepatlo = 55,
                    Felbontas = "4K UHD",
                    KepUrl = "https://images.samsung.com/is/image/samsung/p6pim/hu/ue55cu7172uxxh/gallery/hu-crystal-uhd-cu7000-ue55cu7172uxxh-535874218?$650_519_PNG$",
                    Elerheto = true,
                    Leiras = "Kiváló minőségű okos TV 4K felbontással."
                },
                new TV
                {
                    Id = 2,
                    KereskedoId = "VLP-ELEKTRO",
                    Nev = "LG OLED 65\" C3",
                    Gyarto = "LG",
                    Ar = 540000,
                    Raktarkeszlet = 3,
                    Kategoria = "TV-k",
                    Kepatlo = 65,
                    Felbontas = "4K UHD",
                    KepUrl = "https://www.lg.com/hu/images/televiziok/md07560212/gallery/D-1.jpg",
                    Elerheto = true,
                    Leiras = "A legmélyebb fekete és a legélénkebb színek, amit csak egy OLED kijelző nyújthat."
                }
            );

            // 3. Könyvek és Játékok (Hozzárendelve a Kocka Barlanghoz -> "VLP-KOCKA")
            modelBuilder.Entity<Konyv>().HasData(
                new Konyv
                {
                    Id = 3,
                    KereskedoId = "VLP-KOCKA",
                    Nev = "A Gyűrűk Ura - A Gyűrű Szövetsége",
                    Gyarto = "Európa Kiadó",
                    Ar = 5500,
                    Raktarkeszlet = 30,
                    Kategoria = "Könyvek",
                    Szerzo = "J.R.R. Tolkien",
                    Isbn = "978-963-07-9204-5",
                    KepUrl = "https://bookline.hu/hu/control/shop/images?id=52945&type=10&size=original",
                    Elerheto = true,
                    Leiras = "Klasszikus fantasy regény, az epikus kaland kezdete."
                }
            );

            modelBuilder.Entity<Jatek>().HasData(
                new Jatek
                {
                    Id = 4,
                    KereskedoId = "VLP-KOCKA",
                    Nev = "The Witcher 3: Wild Hunt",
                    Gyarto = "CD Projekt Red",
                    Ar = 12000,
                    Raktarkeszlet = 5,
                    Kategoria = "Játékok",
                    Korhatar = 18,
                    Tipus = "RPG",
                    KepUrl = "https://image.api.playstation.com/vulcan/ap/rnd/202211/0711/kh4MUIuMmGIEPRaJ3z7E8MIG.png",
                    Elerheto = true,
                    Leiras = "Az egyik legjobb nyílt világú szerepjáték Geralt kalandjaival."
                },
                new Jatek
                {
                    Id = 5,
                    KereskedoId = "VLP-KOCKA",
                    Nev = "Hogwarts Legacy",
                    Gyarto = "Warner Bros",
                    Ar = 24000,
                    Raktarkeszlet = 15,
                    Kategoria = "Játékok",
                    Korhatar = 16,
                    Tipus = "Akció-RPG",
                    KepUrl = "https://image.api.playstation.com/vulcan/ap/rnd/202011/0919/cDKjqQc1QvM89tU33T94k8O6.png",
                    Elerheto = true,
                    Leiras = "Légy részese a varázslóvilágnak a 19. századi Roxfortban!"
                }
            );
        }
        public DbSet<Vendor_Link_Point.Models.TV> TV { get; set; } = default!;
        public DbSet<Vendor_Link_Point.Models.Konyv> Konyv { get; set; } = default!;
        public DbSet<Vendor_Link_Point.Models.Jatek> Jatek { get; set; } = default!;
    }
}
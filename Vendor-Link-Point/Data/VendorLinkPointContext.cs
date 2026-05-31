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

        // 2. Adatbázis sémák és öröklődések finomhangolása
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPT (Table-Per-Type) öröklődés beállítása a Felhasználóknál
            // Így a Users tábla mellett létrejön a Vasarlok és Kereskedok tábla is, a specifikus mezőkkel
            modelBuilder.Entity<Vasarlo>().ToTable("Vasarlok");
            modelBuilder.Entity<Kereskedo>().ToTable("Kereskedok");

            // TPT öröklődés beállítása a Termékeknél
            modelBuilder.Entity<TV>().ToTable("TVk");
            modelBuilder.Entity<Konyv>().ToTable("Konyvek");
            modelBuilder.Entity<Jatek>().ToTable("Jatekok");

            // --- TESZT ADATOK (SEEDING) ---
            modelBuilder.Entity<TV>().HasData(
                new TV
                {
                    Id = 1,
                    KereskedoId = "VLP-7OF08QF3",
                    Nev = "Samsung 55\" Smart 4K TV",
                    Gyarto = "Samsung",
                    Ar = 165000,
                    Raktarkeszlet = 12,
                    Kategoria = "TV-k",
                    Kepatlo = 55,
                    Felbontas = "4K UHD",
                    KepUrl = "https://dummyimage.com/400x300/282c34/fff.png&text=Samsung+TV",
                    Elerheto = true,
                    Leiras = "Kiváló minőségű okos TV 4K felbontással."
                }
            );

            modelBuilder.Entity<Konyv>().HasData(
                new Konyv
                {
                    Id = 2,
                    KereskedoId = "VLP-7OF08QF3",
                    Nev = "A Gyűrűk Ura - A Gyűrű Szövetsége",
                    Gyarto = "Európa Kiadó",
                    Ar = 5500,
                    Raktarkeszlet = 30,
                    Kategoria = "Könyvek",
                    Szerzo = "J.R.R. Tolkien",
                    Isbn = "978-963-07-9204-5",
                    KepUrl = "https://dummyimage.com/400x300/282c34/fff.png&text=Konyv",
                    Elerheto = true,
                    Leiras = "Klasszikus fantasy regény, az epikus kaland kezdete."
                }
            );

            modelBuilder.Entity<Jatek>().HasData(
                new Jatek
                {
                    Id = 3,
                    KereskedoId = "VLP-7OF08QF3",
                    Nev = "The Witcher 3: Wild Hunt",
                    Gyarto = "CD Projekt Red",
                    Ar = 12000,
                    Raktarkeszlet = 5,
                    Kategoria = "Játékok",
                    Korhatar = 18,
                    Tipus = "RPG",
                    KepUrl = "https://dummyimage.com/400x300/282c34/fff.png&text=Witcher+3",
                    Elerheto = true,
                    Leiras = "Az egyik legjobb nyílt világú szerepjáték Geralt kalandjaival."
                }
            );
        }
        public DbSet<Vendor_Link_Point.Models.TV> TV { get; set; } = default!;
        public DbSet<Vendor_Link_Point.Models.Konyv> Konyv { get; set; } = default!;
        public DbSet<Vendor_Link_Point.Models.Jatek> Jatek { get; set; } = default!;
    }
}

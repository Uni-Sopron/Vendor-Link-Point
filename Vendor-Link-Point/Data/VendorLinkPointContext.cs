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
        public DbSet<TV> TV { get; set; } = default!;
        public DbSet<Konyv> Konyv { get; set; } = default!;
        public DbSet<Jatek> Jatek { get; set; } = default!;

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
            // SEED DATA (ALAPADATOK FELTÖLTÉSE)
            // ==========================================

            // A kért biztonságos "Admin1234" jelszó hash-elve:
            string defaultPasswordHash = "7jqVtG5ypHGQXHK6YYkJWA==:Juoi47jOMwjH7dtqmA2zN6rRQXGZuoSJkewbDQoCcP8=";

            // 1. KERESKEDŐK FELTÖLTÉSE
            modelBuilder.Entity<Kereskedo>().HasData(
                new Kereskedo
                {
                    Id = 1,
                    Nev = "Kovács Elek",
                    Email = "elektro@vendor.hu",
                    Jelszo = defaultPasswordHash,
                    Role = "Kereskedo",
                    KereskedoId = "VLP-ELEKTRO",
                    Cegnev = "Elektro Kft."
                },
                new Kereskedo
                {
                    Id = 2,
                    Nev = "Nagy Károly",
                    Email = "kocka@vendor.hu",
                    Jelszo = defaultPasswordHash,
                    Role = "Kereskedo",
                    KereskedoId = "VLP-KOCKA",
                    Cegnev = "Kocka Barlang"
                }
            );

            // 2. VÁSÁRLÓ FELTÖLTÉSE
            modelBuilder.Entity<Vasarlo>().HasData(
                new Vasarlo
                {
                    Id = 3,
                    Nev = "Teszt Vásárló",
                    Email = "vevo@vendor.hu",
                    Jelszo = defaultPasswordHash,
                    Role = "Vasarlo",
                    SzallitasiCim = "1055 Budapest, Kossuth Lajos tér 1-3.",
                    Telefonszam = "+36301234567"
                }
            );

            // 3. TERMÉKEK - TV-k (Tulajdonos: VLP-ELEKTRO)
            modelBuilder.Entity<TV>().HasData(
                new TV
                {
                    Id = 1,
                    KereskedoId = "VLP-ELEKTRO",
                    Nev = "Samsung 55\" 4K Smart UHD TV",
                    Gyarto = "Samsung",
                    Ar = 165000,
                    Raktarkeszlet = 12,
                    Kategoria = "TV-k",
                    Kepatlo = 55,
                    Felbontas = "4K UHD",
                    KepUrl = "https://images.samsung.com/is/image/samsung/p6pim/hu/ue55cu7172uxxh/gallery/hu-crystal-uhd-cu7000-ue55cu7172uxxh-536306536?$650_519_PNG$",
                    Elerheto = true,
                    Leiras = "Lélegzetelállító 4K felbontás és kristálytiszta színek okos funkciókkal."
                },
                new TV
                {
                    Id = 2,
                    KereskedoId = "VLP-ELEKTRO",
                    Nev = "LG OLED 65\" C3",
                    Gyarto = "LG",
                    Ar = 540000,
                    Raktarkeszlet = 5,
                    Kategoria = "TV-k",
                    Kepatlo = 65,
                    Felbontas = "4K OLED",
                    KepUrl = "https://www.lg.com/hu/images/televiziok/md07593256/gallery/D-01.jpg",
                    Elerheto = true,
                    Leiras = "Tökéletes fekete és végtelen kontraszt az LG OLED technológiájával."
                }
            );

            // 4. TERMÉKEK - KÖNYVEK (Tulajdonos: VLP-KOCKA)
            modelBuilder.Entity<Konyv>().HasData(
                new Konyv
                {
                    Id = 3,
                    KereskedoId = "VLP-KOCKA",
                    Nev = "A Gyűrűk Ura - A Gyűrű Szövetsége",
                    Gyarto = "Európa Könyvkiadó",
                    Ar = 5500,
                    Raktarkeszlet = 20,
                    Kategoria = "Könyvek",
                    Szerzo = "J.R.R. Tolkien",
                    Isbn = "9789630798150",
                    KepUrl = "https://bookline.hu/zoom/bookline/092/10/92/27/0921092270.jpg",
                    Elerheto = true,
                    Leiras = "Minden idők leghíresebb fantasy regényének első része."
                }
            );

            // 5. TERMÉKEK - JÁTÉKOK (Tulajdonos: VLP-KOCKA)
            modelBuilder.Entity<Jatek>().HasData(
                new Jatek
                {
                    Id = 4,
                    KereskedoId = "VLP-KOCKA",
                    Nev = "The Witcher 3: Wild Hunt",
                    Gyarto = "CD Projekt Red",
                    Ar = 8500,
                    Raktarkeszlet = 30,
                    Kategoria = "Játékok",
                    Korhatar = 18,
                    Tipus = "RPG",
                    KepUrl = "https://image.api.playstation.com/vulcan/ap/rnd/202211/0711/kh4MUIuMmGIEPRaJ3z7E8MIG.png",
                    Elerheto = true,
                    Leiras = "Bújj Ríviai Geralt bőrébe ebben a hatalmas, nyílt világú kalandban."
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
                    Leiras = "Fedezd fel a varázslóvilágot a 19. században ebben a lenyűgöző játékban!"
                }
            );
        }
    }
}
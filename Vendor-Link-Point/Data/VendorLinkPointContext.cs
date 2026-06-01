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

            string defaultPasswordHash = "7jqVtG5ypHGQXHK6YYkJWA==:Juoi47jOMwjH7dtqmA2zN6rRQXGZuoSJkewbDQoCcP8=";

            // --- 1. KERESKEDŐK ---
            modelBuilder.Entity<Kereskedo>().HasData(
                new Kereskedo { Id = 1, Nev = "Kovács Elek", Email = "elektro@vendor.hu", Jelszo = defaultPasswordHash, Role = "Kereskedo", KereskedoId = "VLP-ELEKTRO", Cegnev = "Elektro Kft." },
                new Kereskedo { Id = 2, Nev = "Nagy Károly", Email = "kocka@vendor.hu", Jelszo = defaultPasswordHash, Role = "Kereskedo", KereskedoId = "VLP-KOCKA", Cegnev = "Kocka Barlang" },
                new Kereskedo { Id = 4, Nev = "Király Dávid", Email = "konyv@vendor.hu", Jelszo = defaultPasswordHash, Role = "Kereskedo", KereskedoId = "VLP-KIRALY", Cegnev = "Király Könyvesbolt" }
            );

            // --- 2. VÁSÁRLÓK ---
            modelBuilder.Entity<Vasarlo>().HasData(
                new Vasarlo { Id = 3, Nev = "Teszt Vásárló", Email = "vevo@vendor.hu", Jelszo = defaultPasswordHash, Role = "Vasarlo", SzallitasiCim = "1055 Budapest, Kossuth Lajos tér 1-3.", Telefonszam = "+36301234567" },
                new Vasarlo { Id = 5, Nev = "Nagy Anna", Email = "anna@vendor.hu", Jelszo = defaultPasswordHash, Role = "Vasarlo", SzallitasiCim = "9022 Győr, Kiss János utca 5.", Telefonszam = "+36209876543" }
            );

            // --- 3. TERMÉKEK (TV-K) ---
            modelBuilder.Entity<TV>().HasData(
                new TV { Id = 1, KereskedoId = "VLP-ELEKTRO", Nev = "Samsung 55\" 4K Smart UHD TV", Gyarto = "Samsung", Ar = 165000, Raktarkeszlet = 12, Kategoria = "TV-k", Kepatlo = 55, Felbontas = "4K UHD", KepUrl = "https://images.unsplash.com/photo-1593359677879-a4bb92f829d1?auto=format&fit=crop&w=800&q=80", Elerheto = true, Leiras = "Lélegzetelállító 4K felbontás és kristálytiszta színek okos funkciókkal." },
                new TV { Id = 2, KereskedoId = "VLP-ELEKTRO", Nev = "LG OLED 65\" C3", Gyarto = "LG", Ar = 540000, Raktarkeszlet = 5, Kategoria = "TV-k", Kepatlo = 65, Felbontas = "4K OLED", KepUrl = "https://images.unsplash.com/photo-1593784991095-a205069470b6?auto=format&fit=crop&w=800&q=80", Elerheto = true, Leiras = "Tökéletes fekete és végtelen kontraszt az LG OLED technológiájával." },
                new TV { Id = 6, KereskedoId = "VLP-ELEKTRO", Nev = "Sony Bravia 75\" XR", Gyarto = "Sony", Ar = 820000, Raktarkeszlet = 3, Kategoria = "TV-k", Kepatlo = 75, Felbontas = "8K UHD", KepUrl = "https://images.unsplash.com/photo-1552831388-6a0b35077328?auto=format&fit=crop&w=800&q=80", Elerheto = true, Leiras = "Hatalmas méret és prémium képminőség házimozihoz." },
                new TV { Id = 7, KereskedoId = "VLP-ELEKTRO", Nev = "Philips 50\" Ambilight", Gyarto = "Philips", Ar = 185000, Raktarkeszlet = 8, Kategoria = "TV-k", Kepatlo = 50, Felbontas = "4K UHD", KepUrl = "https://images.unsplash.com/photo-1601944177325-f8867652837f?auto=format&fit=crop&w=800&q=80", Elerheto = true, Leiras = "Különleges háttérvilágítással, amely követi a képernyő eseményeit." }
            );

            // --- 4. TERMÉKEK (KÖNYVEK) ---
            modelBuilder.Entity<Konyv>().HasData(
                new Konyv { Id = 3, KereskedoId = "VLP-KOCKA", Nev = "A Gyűrűk Ura - A Gyűrű Szövetsége", Gyarto = "Európa Könyvkiadó", Ar = 5500, Raktarkeszlet = 20, Kategoria = "Könyvek", Szerzo = "J.R.R. Tolkien", Isbn = "9789630798150", KepUrl = "https://images.unsplash.com/photo-1629196914594-5b481977e68e?auto=format&fit=crop&w=800&q=80", Elerheto = true, Leiras = "Minden idők leghíresebb fantasy regényének első része." },
                new Konyv { Id = 8, KereskedoId = "VLP-KIRALY", Nev = "Harry Potter és a bölcsek köve", Gyarto = "Animus Kiadó", Ar = 4800, Raktarkeszlet = 45, Kategoria = "Könyvek", Szerzo = "J.K. Rowling", Isbn = "9789633245453", KepUrl = "https://images.unsplash.com/photo-1618666012174-83b441c0bc76?auto=format&fit=crop&w=800&q=80", Elerheto = true, Leiras = "A történet, amellyel egy egész generáció szeretett bele az olvasásba." },
                new Konyv { Id = 9, KereskedoId = "VLP-KIRALY", Nev = "Tiszta kód (Clean Code)", Gyarto = "Kiskapu", Ar = 8900, Raktarkeszlet = 15, Kategoria = "Könyvek", Szerzo = "Robert C. Martin", Isbn = "9789639301980", KepUrl = "https://images.unsplash.com/photo-1555066931-4365d14bab8c?auto=format&fit=crop&w=800&q=80", Elerheto = true, Leiras = "Kötelező olvasmány minden szoftverfejlesztő számára." },
                new Konyv { Id = 10, KereskedoId = "VLP-KIRALY", Nev = "Dűne", Gyarto = "Gabo Kiadó", Ar = 5200, Raktarkeszlet = 30, Kategoria = "Könyvek", Szerzo = "Frank Herbert", Isbn = "9789634063216", KepUrl = "https://images.unsplash.com/photo-1541963463532-d68292c34b19?auto=format&fit=crop&w=800&q=80", Elerheto = true, Leiras = "Sci-fi klasszikus, amely alapjaiban határozta meg a műfajt." }
            );

            // --- 5. TERMÉKEK (JÁTÉKOK) ---
            modelBuilder.Entity<Jatek>().HasData(
                new Jatek { Id = 4, KereskedoId = "VLP-KOCKA", Nev = "The Witcher 3: Wild Hunt", Gyarto = "CD Projekt Red", Ar = 8500, Raktarkeszlet = 30, Kategoria = "Játékok", Korhatar = 18, Tipus = "RPG", KepUrl = "https://images.unsplash.com/photo-1550745165-9bc0b252726f?auto=format&fit=crop&w=800&q=80", Elerheto = true, Leiras = "Bújj Ríviai Geralt bőrébe ebben a hatalmas, nyílt világú kalandban." },
                new Jatek { Id = 5, KereskedoId = "VLP-KOCKA", Nev = "Hogwarts Legacy", Gyarto = "Warner Bros", Ar = 24000, Raktarkeszlet = 15, Kategoria = "Játékok", Korhatar = 16, Tipus = "Akció-RPG", KepUrl = "https://images.unsplash.com/photo-1511512578047-dfb367046420?auto=format&fit=crop&w=800&q=80", Elerheto = true, Leiras = "Fedezd fel a varázslóvilágot a 19. században ebben a lenyűgöző játékban!" },
                new Jatek { Id = 11, KereskedoId = "VLP-KOCKA", Nev = "Cyberpunk 2077", Gyarto = "CD Projekt Red", Ar = 12500, Raktarkeszlet = 40, Kategoria = "Játékok", Korhatar = 18, Tipus = "Akció-RPG", KepUrl = "https://images.unsplash.com/photo-1605806616949-1e87b487cb2a?auto=format&fit=crop&w=800&q=80", Elerheto = true, Leiras = "Éld át a jövő sötét és neonfényes városának izgalmait." },
                new Jatek { Id = 12, KereskedoId = "VLP-KOCKA", Nev = "EA Sports FC 24", Gyarto = "Electronic Arts", Ar = 19990, Raktarkeszlet = 50, Kategoria = "Játékok", Korhatar = 3, Tipus = "Sport", KepUrl = "https://images.unsplash.com/photo-1611158742626-663f73319be1?auto=format&fit=crop&w=800&q=80", Elerheto = true, Leiras = "A világ legnépszerűbb futballszimulátora új néven, de a régi minőséggel." }
            );
        }
    }
}
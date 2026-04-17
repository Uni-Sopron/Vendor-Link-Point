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

        }
    }
}

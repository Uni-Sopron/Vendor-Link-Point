using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vendor_Link_Point.Models
{
    public class Rendeles
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required int UserId { get; set; }

        [ForeignKey("UserId")]
        public required virtual User Vasarlo { get; set; } // Navigációs tulajdonság

        [Required]
        [Display(Name = "Rendelés időpontja")]
        public DateTime Idopont { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Végösszeg (Ft)")]
        public decimal Vegosszeg { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Rendelés állapota")]
        public required string Allapot { get; set; } // Pl.: "Feldolgozás alatt", "Kiszállítva"

        [Required]
        [StringLength(50)]
        [Display(Name = "Fizetési mód")]
        public required string FizetesiMod { get; set; }

        [Required(ErrorMessage = "A szállítási cím megadása kötelező!")]
        [StringLength(200)]
        [Display(Name = "Szállítási Cím")]
        public required string SzallitasiCim { get; set; }

        // Egy rendeléshez több tétel tartozik
        public virtual ICollection<RendelesTetel> RendelesTetelek { get; set; } = new List<RendelesTetel>();
    }

    public class RendelesTetel
    {
        // A kapcsolótábláknál érdemes saját Id-t is használni, vagy kompozit kulcsot beállítani
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public required virtual Rendeles Rendeles { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public required virtual Product Termek { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "A mennyiség 1 és 100 között kell legyen.")]
        [Display(Name = "Mennyiség (db)")]
        public required int Mennyiseg { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Egységár (Ft)")]
        public required decimal Egysegar { get; set; }
    }
}

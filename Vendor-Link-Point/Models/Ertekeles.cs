using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vendor_Link_Point.Models
{
    public class Ertekeles
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Termek { get; set; } = null!;

        [Required]
        public required int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User Vasarlo { get; set; } = null!;

        [Required]
        [Range(1, 5)]
        public required int Pontszam { get; set; } // 1-től 5 csillagig

        [StringLength(500)]
        public string? Szoveg { get; set; } // Szöveges vélemény (opcionális)

        public DateTime Datum { get; set; } = DateTime.Now;
    }
}
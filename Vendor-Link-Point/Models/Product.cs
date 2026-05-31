using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vendor_Link_Point.Models
{
    public abstract class Product
    {
        [Key]
        public int Id { get; set; }

        public string KereskedoId { get; set; }

        [Required(ErrorMessage = "A termék nevének megadása kötelező!")]
        [StringLength(100, ErrorMessage = "A név maximum 100 karakter lehet.")]
        [Display(Name = "Termék neve")]
        public string Nev { get; set; }

        [Required(ErrorMessage = "A gyártó megadása kötelező!")]
        [StringLength(50)]
        [Display(Name = "Gyártó")]
        public string Gyarto { get; set; }

        [Required(ErrorMessage = "Az ár megadása kötelező!")]
        [Range(1, 10000000, ErrorMessage = "Az árnak 1 és 10.000.000 Ft között kell lennie.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Ár (Ft)")]
        public decimal Ar { get; set; }

        [Required(ErrorMessage = "A raktárkészlet megadása kötelező!")]
        [Range(0, 10000, ErrorMessage = "A készlet nem lehet negatív!")]
        [Display(Name = "Raktárkészlet (db)")]
        public int Raktarkeszlet { get; set; }

        [Required(ErrorMessage = "A kategória kiválasztása kötelező!")]
        [StringLength(50)]
        [Display(Name = "Kategória")]
        public string Kategoria { get; set; }

        [Display(Name = "Termék leírása")]
        [StringLength(1000)]
        public string Leiras { get; set; }

        [Display(Name = "Kép elérési útja")]
        public string KepUrl { get; set; }

        [Display(Name = "Elérhető a webshopban?")]
        public bool Elerheto { get; set; } = true;
    }

    public class TV : Product
    {
        [Required(ErrorMessage = "A képátló megadása kötelező!")]
        [Range(10, 200, ErrorMessage = "Érvénytelen képátló (10-200 col).")]
        [Display(Name = "Képátló (col)")]
        public int Kepatlo { get; set; }

        [Required(ErrorMessage = "A felbontás megadása kötelező!")]
        [StringLength(20)]
        [Display(Name = "Felbontás (pl. 4K, 1080p)")]
        public string Felbontas { get; set; }
    }

    public class Konyv : Product
    {
        [Required(ErrorMessage = "A szerző megadása kötelező!")]
        [StringLength(100)]
        [Display(Name = "Szerző")]
        public string Szerzo { get; set; }

        [Required(ErrorMessage = "Az ISBN szám megadása kötelező!")]
        [StringLength(20)]
        [Display(Name = "ISBN szám")]
        public string Isbn { get; set; }
    }

    public class Jatek : Product
    {
        [Required(ErrorMessage = "A korhatár megadása kötelező!")]
        [Range(3, 18, ErrorMessage = "A korhatár 3 és 18 év között lehet.")]
        [Display(Name = "Korhatár (év)")]
        public int Korhatar { get; set; }

        [Required(ErrorMessage = "A típus megadása kötelező!")]
        [StringLength(50)]
        [Display(Name = "Játék típusa")]
        public string Tipus { get; set; }
    }
}

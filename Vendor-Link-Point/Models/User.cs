using System.ComponentModel.DataAnnotations;

namespace Vendor_Link_Point.Models
{
    public abstract class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A név megadása kötelező!")]
        [StringLength(100)]
        [Display(Name = "Teljes név")]
        public required string Nev { get; set; }

        [Required(ErrorMessage = "Az e-mail cím megadása kötelező!")]
        [EmailAddress(ErrorMessage = "Érvénytelen e-mail formátum!")]
        [Display(Name = "E-mail cím")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A jelszó megadása kötelező!")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A jelszónak legalább 6 karakternek kell lennie!")]
        [Display(Name = "Jelszó")]
        public required string Jelszo { get; set; }

        [Required]
        [StringLength(20)]
        public required string Role { get; set; } // Pl.: "Vasarlo" vagy "Kereskedo"
    }

    public class Vasarlo : User
    {
        [Required(ErrorMessage = "A szállítási cím megadása kötelező!")]
        [StringLength(200)]
        [Display(Name = "Szállítási cím")]
        public required string SzallitasiCim { get; set; }

        [Phone(ErrorMessage = "Érvénytelen telefonszám formátum!")]
        [Display(Name = "Telefonszám")]
        public string? Telefonszam { get; set; }
    }

    public class Kereskedo : User
    {
        [Required(ErrorMessage = "A Kereskedő ID megadása kötelező!")]
        [StringLength(50)]
        [Display(Name = "Kereskedő Azonosító")]
        public required string KereskedoId { get; set; }

        [Display(Name = "Cégnév / Bolt neve")]
        [StringLength(100)]
        public string? Cegnev { get; set; }
    }
}

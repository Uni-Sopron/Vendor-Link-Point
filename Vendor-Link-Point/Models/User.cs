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
        public string Nev { get; set; }

        [Required(ErrorMessage = "Az e-mail cím megadása kötelező!")]
        [EmailAddress(ErrorMessage = "Érvénytelen e-mail formátum!")]
        [Display(Name = "E-mail cím")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A jelszó megadása kötelező!")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A jelszónak legalább 6 karakternek kell lennie!")]
        [Display(Name = "Jelszó")]
        public string Jelszo { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; } // Pl.: "Vasarlo" vagy "Kereskedo"
    }

    public class Vasarlo : User
    {
        [Required(ErrorMessage = "A szállítási cím megadása kötelező!")]
        [StringLength(200)]
        [Display(Name = "Szállítási cím")]
        public string SzallitasiCim { get; set; }

        [Required(ErrorMessage = "A telefonszám megadása kötelező!")]
        [Phone(ErrorMessage = "Érvénytelen telefonszám formátum!")]
        [Display(Name = "Telefonszám")]
        public string Telefonszam { get; set; }
    }

    public class Kereskedo : User
    {
        [Required(ErrorMessage = "A Kereskedő ID megadása kötelező!")]
        [StringLength(50)]
        [Display(Name = "Kereskedő Azonosító")]
        public string KereskedoId { get; set; }

        [Display(Name = "Cégnév / Bolt neve")]
        [StringLength(100)]
        public string Cegnev { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Vendor_Link_Point.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Az e-mail cím megadása kötelező!")]
        [EmailAddress(ErrorMessage = "Érvénytelen e-mail formátum!")]
        [Display(Name = "E-mail cím")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A jelszó megadása kötelező!")]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public required string Jelszo { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "A név megadása kötelező!")]
        [Display(Name = "Teljes név")]
        public required string Nev { get; set; }

        [Required(ErrorMessage = "Az e-mail cím megadása kötelező!")]
        [EmailAddress(ErrorMessage = "Érvénytelen e-mail formátum!")]
        [Display(Name = "E-mail cím")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A jelszó megadása kötelező!")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "A jelszónak legalább 6 karakternek kell lennie!")]
        [Display(Name = "Jelszó")]
        public required string Jelszo { get; set; }

        [Required(ErrorMessage = "A jelszó megerősítése kötelező!")]
        [DataType(DataType.Password)]
        [Compare("Jelszo", ErrorMessage = "A két jelszó nem egyezik!")]
        [Display(Name = "Jelszó megerősítése")]
        public required string JelszoUjra { get; set; }

        [Required(ErrorMessage = "Válassz fióktípust!")]
        [Display(Name = "Fiók típusa")]
        public required string Role { get; set; } // Ide jön a "Vasarlo" vagy "Kereskedo"

        // --- Vásárló specifikus mezők (nullable, mert kereskedőnél üres lesz) ---
        [Display(Name = "Szállítási cím")]
        public required string SzallitasiCim { get; set; }

        [Display(Name = "Telefonszám")]
        public required string Telefonszam { get; set; }

        // --- Kereskedő specifikus mezők (nullable) ---
        [Display(Name = "Kereskedő Azonosító")]
        public required string KereskedoId { get; set; }

        [Display(Name = "Cégnév / Bolt neve")]
        public string? Cegnev { get; set; }
    }
}

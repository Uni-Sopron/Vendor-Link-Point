using System.ComponentModel.DataAnnotations;

namespace Vendor_Link_Point.ViewModels
{
    public class SettingsViewModel
    {
        public string Role { get; set; } = string.Empty;

        // --- VÁSÁRLÓ MEZŐI ---
        [Required(ErrorMessage = "A szállítási cím megadása kötelező!")]
        [Display(Name = "Szállítási cím")]
        [StringLength(200)]
        public string SzallitasiCim { get; set; } = string.Empty;

        [Display(Name = "Telefonszám")]
        [Phone(ErrorMessage = "Érvénytelen telefonszám formátum!")]
        public string? Telefonszam { get; set; }

        // --- KERESKEDŐ MEZŐI ---
        [Display(Name = "Cégnév / Bolt neve")]
        [StringLength(100)]
        public string? Cegnev { get; set; }
    }
}
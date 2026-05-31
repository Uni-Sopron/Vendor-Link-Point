using Vendor_Link_Point.Models;
using System.Collections.Generic;

namespace Vendor_Link_Point.ViewModels
{
    public class WebshopViewModel
    {
        // Maguk a megjelenítendő termékek
        public IEnumerable<Product> Termekek { get; set; } = new List<Product>();

        // Aktuális szűrő állapotok
        public string? CurrentCategory { get; set; }
        public int? MinAr { get; set; }
        public int? MaxAr { get; set; }
        public List<string> KivalasztottMeretek { get; set; } = new List<string>();

        // Kategória darabszámok
        public int TvCount { get; set; }
        public int KonyvCount { get; set; }
        public int JatekCount { get; set; }

        // TV Méret darabszámok
        public int TvMeretKicsiCount { get; set; }     // 45" alatt
        public int TvMeretKozepesCount { get; set; }   // 45" - 55"
        public int TvMeretNagyCount { get; set; }      // 56" - 65"
        public int TvMeretExtraCount { get; set; }     // 65" felett
    }
}
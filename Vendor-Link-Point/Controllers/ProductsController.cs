using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vendor_Link_Point.Data;
using Vendor_Link_Point.Models;
using Vendor_Link_Point.ViewModels;

namespace Vendor_Link_Point.Controllers
{
    public class ProductsController : Controller
    {
        private readonly VendorLinkPointContext _context;

        public ProductsController(VendorLinkPointContext context)
        {
            _context = context;
        }

        // GET: /Products
        public async Task<IActionResult> Index(string? kategoria, int? minAr, int? maxAr, List<string> meretek)
        {
            // 1. Alap lekérdezés: csak az elérhető termékek
            var baseQuery = _context.Products.Where(p => p.Elerheto);

            // 2. ViewModel előkészítése és globális kategória-darabszámok kiszámítása
            var vm = new WebshopViewModel
            {
                CurrentCategory = kategoria,
                MinAr = minAr,
                MaxAr = maxAr,
                KivalasztottMeretek = meretek ?? new List<string>(),

                TvCount = await baseQuery.CountAsync(p => p.Kategoria == "TV-k"),
                KonyvCount = await baseQuery.CountAsync(p => p.Kategoria == "Könyvek"),
                JatekCount = await baseQuery.CountAsync(p => p.Kategoria == "Játékok")
            };

            // 3. Kategória szűrés alkalmazása
            var filteredQuery = baseQuery.AsQueryable();
            if (!string.IsNullOrEmpty(kategoria))
            {
                filteredQuery = filteredQuery.Where(p => p.Kategoria == kategoria);
            }

            // 4. Ár szűrés alkalmazása
            if (minAr.HasValue) filteredQuery = filteredQuery.Where(p => p.Ar >= minAr.Value);
            if (maxAr.HasValue) filteredQuery = filteredQuery.Where(p => p.Ar <= maxAr.Value);

            // 5. Specifikus (TV) szűrések és darabszámok
            if (kategoria == "TV-k")
            {
                // Kiszámoljuk, miből mennyi van a JELENLEGI (árral már szűrt) listában
                var tvList = await filteredQuery.OfType<TV>().ToListAsync();

                vm.TvMeretKicsiCount = tvList.Count(t => t.Kepatlo < 45);
                vm.TvMeretKozepesCount = tvList.Count(t => t.Kepatlo >= 45 && t.Kepatlo <= 55);
                vm.TvMeretNagyCount = tvList.Count(t => t.Kepatlo >= 56 && t.Kepatlo <= 65);
                vm.TvMeretExtraCount = tvList.Count(t => t.Kepatlo > 65);

                // Méret szűrés alkalmazása, ha a user bepipált valamit
                if (vm.KivalasztottMeretek.Any())
                {
                    var validTvIds = tvList.Where(t =>
                        (vm.KivalasztottMeretek.Contains("kicsi") && t.Kepatlo < 45) ||
                        (vm.KivalasztottMeretek.Contains("kozepes") && t.Kepatlo >= 45 && t.Kepatlo <= 55) ||
                        (vm.KivalasztottMeretek.Contains("nagy") && t.Kepatlo >= 56 && t.Kepatlo <= 65) ||
                        (vm.KivalasztottMeretek.Contains("extra") && t.Kepatlo > 65)
                    ).Select(t => t.Id).ToList();

                    filteredQuery = filteredQuery.Where(p => validTvIds.Contains(p.Id));
                }
            }

            // 6. Végleges lista átadása
            vm.Termekek = await filteredQuery.ToListAsync();
            return View(vm);
        }

        // GET: /Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Lekérjük a terméket
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            // Megkeressük a hozzá tartozó Kereskedőt, hogy kiírhassuk a bolt nevét
            var kereskedo = await _context.Users.OfType<Kereskedo>()
                                          .FirstOrDefaultAsync(k => k.KereskedoId == product.KereskedoId);

            // Átadjuk a kereskedő cégnevét a Nézetnek (ha nincs, "Ismeretlen Bolt" lesz)
            ViewBag.Cegnev = kereskedo?.Cegnev ?? "Ismeretlen Bolt";

            return View(product);
        }
    }
}
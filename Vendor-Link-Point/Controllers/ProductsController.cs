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
        public async Task<IActionResult> Index(string? kategoria, int? minAr, int? maxAr, List<string> meretek, string? kereses)
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

            if (!string.IsNullOrEmpty(kereses))
            {
                // Keresünk a termék nevében VAGY a gyártó nevében
                filteredQuery = filteredQuery.Where(p => p.Nev.Contains(kereses) || p.Gyarto.Contains(kereses));
            }

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

            // Lekérjük a terméket és Hozzácsatoljuk (Include) az értékeléseket és az értékelőket is!
            var product = await _context.Products
                .Include(p => p.Ertekelesek)
                    .ThenInclude(e => e.Vasarlo)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            var kereskedo = await _context.Users.OfType<Kereskedo>()
                                          .FirstOrDefaultAsync(k => k.KereskedoId == product.KereskedoId);

            ViewBag.Cegnev = kereskedo?.Cegnev ?? "Ismeretlen Bolt";

            // --- ÉRTÉKELÉSI LOGIKA ÉS JOGOSULTSÁGVIZSGÁLAT ---
            bool canReview = false;
            bool hasReviewed = false;

            if (User.Identity?.IsAuthenticated == true && User.IsInRole("Vasarlo"))
            {
                var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdString, out int userId))
                {
                    // 1. Értékelte már?
                    hasReviewed = product.Ertekelesek.Any(e => e.UserId == userId);

                    // 2. Ha még nem, megvette és megkapta már?
                    if (!hasReviewed)
                    {
                        canReview = await _context.Rendelesek
                            .Include(r => r.RendelesTetelek)
                            .AnyAsync(r => r.UserId == userId &&
                                           r.Allapot == "Kiszállítva" &&
                                           r.RendelesTetelek.Any(rt => rt.ProductId == id));
                    }
                }
            }

            // Átadjuk az adatokat a nézetnek
            ViewBag.CanReview = canReview;
            ViewBag.HasReviewed = hasReviewed;
            ViewBag.AverageRating = product.Ertekelesek.Any() ? product.Ertekelesek.Average(e => e.Pontszam) : 0;

            return View(product);
        }

        // POST: /Products/AddReview (ÚJ METÓDUS AZ ÉRTÉKELÉS BEKÜLDÉSÉRE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int productId, int pontszam, string szoveg)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId)) return Unauthorized();

            // Háttér oldali biztonsági ellenőrzés (Hacker védelem)
            var hasPurchased = await _context.Rendelesek
                .Include(r => r.RendelesTetelek)
                .AnyAsync(r => r.UserId == userId && r.Allapot == "Kiszállítva" && r.RendelesTetelek.Any(rt => rt.ProductId == productId));

            var alreadyReviewed = await _context.Ertekelesek.AnyAsync(e => e.ProductId == productId && e.UserId == userId);

            if (hasPurchased && !alreadyReviewed)
            {
                var ertekeles = new Ertekeles
                {
                    ProductId = productId,
                    UserId = userId,
                    Pontszam = pontszam,
                    Szoveg = szoveg,
                    Datum = DateTime.Now
                };
                _context.Ertekelesek.Add(ertekeles);
                await _context.SaveChangesAsync();
            }

            // Visszadobjuk az adatlapra
            return RedirectToAction(nameof(Details), new { id = productId });
        }
    }
}
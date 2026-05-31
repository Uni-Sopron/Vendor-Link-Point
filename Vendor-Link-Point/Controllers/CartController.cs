using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vendor_Link_Point.Data;
using Vendor_Link_Point.Helpers;
using Vendor_Link_Point.Models;

namespace Vendor_Link_Point.Controllers
{
    public class CartController : Controller
    {
        private readonly VendorLinkPointContext _context;

        public CartController(VendorLinkPointContext context)
        {
            _context = context;
        }

        // Segédmetódus a kosár lekérésére
        private Kosar GetKosar()
        {
            return HttpContext.Session.Get<Kosar>("Kosar") ?? new Kosar();
        }

        // Segédmetódus a kosár mentésére
        private void SaveKosar(Kosar kosar)
        {
            HttpContext.Session.Set("Kosar", kosar);
        }

        // GET: /Cart (A Kosár tartalmának megjelenítése)
        public IActionResult Index()
        {
            var kosar = GetKosar();
            return View(kosar);
        }

        // POST: /Cart/Add/5 (Termék kosárba rakása)
        [HttpPost]
        public IActionResult Add(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                var kosar = GetKosar();
                kosar.Hozzaad(product, 1); // Alapból 1 db-ot adunk hozzá
                SaveKosar(kosar);
            }

            // Hozzáadás után visszadobjuk a kosár oldalra, hogy lássa a sikerességet
            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/Remove/5 (Termék törlése)
        [HttpPost]
        public IActionResult Remove(int id)
        {
            var kosar = GetKosar();
            kosar.Eltavolit(id);
            SaveKosar(kosar);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        public IActionResult UpdateQuantity(int id, int change)
        {
            var kosar = GetKosar();
            kosar.FrissitMennyiseg(id, change); // change lehet +1 vagy -1
            SaveKosar(kosar);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Cart/Checkout
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Checkout() // ++ Async lett! ++
        {
            var kosar = GetKosar();
            if (!kosar.Tetelek.Any()) return RedirectToAction(nameof(Index));

            // ++ LEKÉRJÜK A VÁSÁRLÓ ALAPÉRTELMEZETT CÍMÉT ++
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdString, out int userId))
            {
                var vasarlo = await _context.Users.OfType<Vasarlo>().FirstOrDefaultAsync(u => u.Id == userId);
                if (vasarlo != null)
                {
                    ViewBag.AlapCim = vasarlo.SzallitasiCim;
                }
            }

            return View(kosar);
        }

        // POST: /Cart/PlaceOrder
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // ++ MOST MÁR A SZÁLLÍTÁSI CÍMET IS VÁRJUK A PARAMÉTEREKBEN ++
        public async Task<IActionResult> PlaceOrder(string fizetesiMod, string szallitasiCim)
        {
            var kosar = GetKosar();
            if (!kosar.Tetelek.Any()) return RedirectToAction(nameof(Index));

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId)) return Unauthorized();

            // Ha valamiért üres címet küldene be, visszadobjuk egy hibával
            if (string.IsNullOrWhiteSpace(szallitasiCim))
            {
                ModelState.AddModelError("", "Kérjük, adj meg egy érvényes szállítási címet!");
                return View("Checkout", kosar);
            }

            var rendeles = new Rendeles
            {
                UserId = userId,
                Idopont = DateTime.Now,
                Vegosszeg = kosar.OsszesitettAr(),
                Allapot = "Feldolgozás alatt",
                FizetesiMod = fizetesiMod,
                SzallitasiCim = szallitasiCim, // ++ ITT ELMENTJÜK AZ ÚJ CÍMET ++
                RendelesTetelek = new List<RendelesTetel>()
            };

            // 2. Tételek átemelése és raktárkészlet csökkentése
            foreach (var item in kosar.Tetelek)
            {
                // Frissítjük a raktárkészletet az adatbázisban
                var termekDb = await _context.Products.FindAsync(item.Termek.Id);
                if (termekDb != null)
                {
                    termekDb.Raktarkeszlet -= item.Mennyiseg;
                    if (termekDb.Raktarkeszlet < 0) termekDb.Raktarkeszlet = 0; // Biztonsági limit

                    // Ha elfogyott, automatikusan rejtjük a vásárlók elől
                    if (termekDb.Raktarkeszlet == 0) termekDb.Elerheto = false;

                    _context.Update(termekDb);
                }

                // Rendelési tétel hozzáadása a rendeléshez
                rendeles.RendelesTetelek.Add(new RendelesTetel
                {
                    ProductId = item.Termek.Id,
                    Mennyiseg = item.Mennyiseg,
                    Egysegar = item.Termek.Ar
                });
            }

            // 3. Mentés az adatbázisba
            _context.Rendelesek.Add(rendeles);
            await _context.SaveChangesAsync();

            // 4. Kosár ürítése a memóriából (Sikeres fizetés)
            HttpContext.Session.Remove("Kosar");

            // 5. Átirányítás a Köszönjük oldalra
            return RedirectToAction(nameof(OrderSuccess), new { orderId = rendeles.Id });
        }

        // GET: /Cart/OrderSuccess
        public IActionResult OrderSuccess(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}
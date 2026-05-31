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

        // POST: /Cart/PlaceOrder (Rendelés véglegesítése és mentése)
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(string fizetesiMod, string szallitasiCim)
        {
            var kosar = GetKosar();
            if (!kosar.Tetelek.Any()) return RedirectToAction(nameof(Index));

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId)) return Unauthorized();

            if (string.IsNullOrWhiteSpace(szallitasiCim))
            {
                ModelState.AddModelError("", "Kérjük, adj meg egy érvényes szállítási címet!");
                return View("Checkout", kosar);
            }

            // ++ A ZSENIÁLIS LOGIKA: SZÉTBONTJUK A KOSARAT KERESKEDŐK SZERINT ++
            // Csoportosítjuk a kosárban lévő tételeket az eladó (KereskedoId) alapján
            var tetelekKereskedonkent = kosar.Tetelek.GroupBy(t => t.Termek.KereskedoId);

            foreach (var kereskedoCsoport in tetelekKereskedonkent)
            {
                // Minden egyes kereskedő kap egy SAJÁT, független rendelést
                var rendeles = new Rendeles
                {
                    UserId = userId,
                    Idopont = DateTime.Now,
                    // Csak az adott kereskedő termékeinek összegét számoljuk ki:
                    Vegosszeg = kereskedoCsoport.Sum(t => t.Termek.Ar * t.Mennyiseg),
                    Allapot = "Feldolgozás alatt",
                    FizetesiMod = fizetesiMod,
                    SzallitasiCim = szallitasiCim,
                    RendelesTetelek = new List<RendelesTetel>()
                };

                // Hozzáadjuk a kereskedőhöz tartozó termékeket
                foreach (var item in kereskedoCsoport)
                {
                    var termekDb = await _context.Products.FindAsync(item.Termek.Id);
                    if (termekDb != null)
                    {
                        termekDb.Raktarkeszlet -= item.Mennyiseg;
                        if (termekDb.Raktarkeszlet < 0) termekDb.Raktarkeszlet = 0;
                        if (termekDb.Raktarkeszlet == 0) termekDb.Elerheto = false;

                        _context.Update(termekDb);
                    }

                    rendeles.RendelesTetelek.Add(new RendelesTetel
                    {
                        ProductId = item.Termek.Id,
                        Mennyiseg = item.Mennyiseg,
                        Egysegar = item.Termek.Ar
                    });
                }

                // Mentjük a rész-rendelést
                _context.Rendelesek.Add(rendeles);
            }

            // Az összes (akár 3-4 db) rendelést egyszerre küldjük be az adatbázisba
            await _context.SaveChangesAsync();

            // Kosár ürítése a memóriából
            HttpContext.Session.Remove("Kosar");

            // Mivel most már nem egyetlen ID-nk van, hanem több is lehet, 
            // a sikeres fizetés után rögtön a "Saját rendeléseim" oldalra irányítjuk a vásárlót!
            return RedirectToAction("Index", "Orders");
        }

        // GET: /Cart/OrderSuccess
        public IActionResult OrderSuccess(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}
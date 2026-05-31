using Microsoft.AspNetCore.Mvc;
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
    }
}
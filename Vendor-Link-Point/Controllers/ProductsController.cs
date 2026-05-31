using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vendor_Link_Point.Data;
using Vendor_Link_Point.Models;

namespace Vendor_Link_Point.Controllers
{
    public class ProductsController : Controller
    {
        private readonly VendorLinkPointContext _context;

        public ProductsController(VendorLinkPointContext context)
        {
            _context = context;
        }

        // GET: /Products?kategoria=TV
        public async Task<IActionResult> Index(string kategoria)
        {
            // Eltároljuk az aktuális kategóriát, hogy a Nézet tudja, mit kell aktívként jelölni
            ViewData["CurrentCategory"] = kategoria;

            // Alap lekérdezés: minden, ami elérhető
            var query = _context.Products.Where(p => p.Elerheto);

            // Ha kaptunk kategóriát, rászűrünk
            if (!string.IsNullOrEmpty(kategoria))
            {
                // A kategória neveid a Seed adatok alapján: "TV-k", "Könyvek", "Játékok"
                query = query.Where(p => p.Kategoria == kategoria);
            }

            var termekek = await query.ToListAsync();
            return View(termekek);
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
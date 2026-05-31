using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vendor_Link_Point.Data;

namespace Vendor_Link_Point.Controllers
{
    [Authorize] // Szigorúan csak bejelentkezett felhasználóknak!
    public class OrdersController : Controller
    {
        private readonly VendorLinkPointContext _context;

        public OrdersController(VendorLinkPointContext context)
        {
            _context = context;
        }

        // GET: /Orders
        public async Task<IActionResult> Index()
        {
            // 1. Lekérjük a bejelentkezett vásárló ID-ját
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId)) return Unauthorized();

            // 2. Lekérjük a saját rendeléseit, Hozzá csatolva (Include) a tételeket és a termékeket is!
            var rendelesek = await _context.Rendelesek
                .Include(r => r.RendelesTetelek)
                    .ThenInclude(rt => rt.Termek)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Idopont) // Legújabbak legyenek elöl
                .ToListAsync();

            return View(rendelesek);
        }

        // GET: /Orders/Details/5 (Rendelés részletei)
        public async Task<IActionResult> Details(int id)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId)) return Unauthorized();

            // Lekérjük az adott rendelést, de csak ha a bejelentkezett vásárlóé!
            var rendeles = await _context.Rendelesek
                .Include(r => r.RendelesTetelek)
                    .ThenInclude(rt => rt.Termek)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (rendeles == null) return NotFound();

            return View(rendeles);
        }

        // POST: /Orders/Cancel/5 (Rendelés lemondása)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId)) return Unauthorized();

            var rendeles = await _context.Rendelesek
                .Include(r => r.RendelesTetelek)
                    .ThenInclude(rt => rt.Termek)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (rendeles == null) return NotFound();

            // Csak akkor engedjük lemondani, ha még "Feldolgozás alatt" van! 
            // (Ha már "Kiszállítva", akkor a futártól kell visszakérni)
            if (rendeles.Allapot == "Feldolgozás alatt")
            {
                rendeles.Allapot = "Lemondva"; // Állapot frissítése

                // Raktárkészlet visszapótlása a boltba!
                foreach (var tetel in rendeles.RendelesTetelek)
                {
                    if (tetel.Termek != null)
                    {
                        tetel.Termek.Raktarkeszlet += tetel.Mennyiseg; // Visszaadjuk a darabszámot
                        tetel.Termek.Elerheto = true; // Biztos ami biztos, láthatóvá tesszük a webshopban
                        _context.Update(tetel.Termek);
                    }
                }

                await _context.SaveChangesAsync();
            }

            // Visszadobjuk a részletek oldalra, hogy lássa az új (Lemondva) státuszt
            return RedirectToAction(nameof(Details), new { id = rendeles.Id });
        }
    }
}
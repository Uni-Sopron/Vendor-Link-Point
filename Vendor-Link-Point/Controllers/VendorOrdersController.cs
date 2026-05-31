using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vendor_Link_Point.Data;

namespace Vendor_Link_Point.Controllers
{
    // SZIGORÚ SZABÁLY: Ide csak Kereskedők léphetnek be!
    [Authorize(Roles = "Kereskedo")]
    public class VendorOrdersController : Controller
    {
        private readonly VendorLinkPointContext _context;

        public VendorOrdersController(VendorLinkPointContext context)
        {
            _context = context;
        }

        // GET: /VendorOrders
        public async Task<IActionResult> Index()
        {
            // Lekérjük a bejelentkezett kereskedő egyedi azonosítóját (pl. "VLP-ELEKTRO")
            var kereskedoId = User.FindFirst("KereskedoId")?.Value;

            // Lekérjük azokat a rendeléseket, amelyek tartalmaznak tőle származó terméket
            var rendelesek = await _context.Rendelesek
                .Include(r => r.Vasarlo) // Hogy lássuk, ki rendelt
                .Include(r => r.RendelesTetelek)
                    .ThenInclude(rt => rt.Termek)
                .Where(r => r.RendelesTetelek.Any(rt => rt.Termek.KereskedoId == kereskedoId))
                .OrderByDescending(r => r.Idopont)
                .ToListAsync();

            return View(rendelesek);
        }

        // POST: /VendorOrders/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string ujAllapot)
        {
            var kereskedoId = User.FindFirst("KereskedoId")?.Value;

            // Ellenőrizzük, hogy ez a rendelés tényleg az övé-e
            var rendeles = await _context.Rendelesek
                .Include(r => r.RendelesTetelek)
                    .ThenInclude(rt => rt.Termek)
                .FirstOrDefaultAsync(r => r.Id == id && r.RendelesTetelek.Any(rt => rt.Termek.KereskedoId == kereskedoId));

            if (rendeles == null) return NotFound();

            // Ha a kereskedő lemondja a rendelést (és eddig nem volt lemondva), visszaadjuk a készletet
            if (ujAllapot == "Lemondva" && rendeles.Allapot != "Lemondva")
            {
                foreach (var tetel in rendeles.RendelesTetelek)
                {
                    if (tetel.Termek != null)
                    {
                        tetel.Termek.Raktarkeszlet += tetel.Mennyiseg;
                        tetel.Termek.Elerheto = true;
                        _context.Update(tetel.Termek);
                    }
                }
            }

            // Ha viszont visszaállítja "Kiszállítva" vagy "Feldolgozás alatt" státuszra egy LEMONDOTT rendelést,
            // akkor újra le kell vonnunk a készletből (hogy ne lehessen vele csalni).
            if (rendeles.Allapot == "Lemondva" && ujAllapot != "Lemondva")
            {
                foreach (var tetel in rendeles.RendelesTetelek)
                {
                    if (tetel.Termek != null)
                    {
                        tetel.Termek.Raktarkeszlet -= tetel.Mennyiseg;
                        if (tetel.Termek.Raktarkeszlet < 0) tetel.Termek.Raktarkeszlet = 0;
                        if (tetel.Termek.Raktarkeszlet == 0) tetel.Termek.Elerheto = false;
                        _context.Update(tetel.Termek);
                    }
                }
            }

            rendeles.Allapot = ujAllapot;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
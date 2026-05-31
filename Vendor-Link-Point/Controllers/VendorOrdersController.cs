using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vendor_Link_Point.Data;

namespace Vendor_Link_Point.Controllers
{
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
            var kereskedoId = User.FindFirst("KereskedoId")?.Value;

            var rendelesek = await _context.Rendelesek
                .Include(r => r.Vasarlo)
                .Include(r => r.RendelesTetelek)
                    .ThenInclude(rt => rt.Termek)
                .Where(r => r.RendelesTetelek.Any(rt => rt.Termek.KereskedoId == kereskedoId))
                .OrderByDescending(r => r.Idopont)
                .ToListAsync();

            return View(rendelesek);
        }

        // GET: /VendorOrders/Details/5 (ÚJ METÓDUS!)
        public async Task<IActionResult> Details(int id)
        {
            var kereskedoId = User.FindFirst("KereskedoId")?.Value;

            var rendeles = await _context.Rendelesek
                .Include(r => r.Vasarlo)
                .Include(r => r.RendelesTetelek)
                    .ThenInclude(rt => rt.Termek)
                .FirstOrDefaultAsync(r => r.Id == id && r.RendelesTetelek.Any(rt => rt.Termek.KereskedoId == kereskedoId));

            if (rendeles == null) return NotFound();

            return View(rendeles);
        }

        // POST: /VendorOrders/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string ujAllapot)
        {
            var kereskedoId = User.FindFirst("KereskedoId")?.Value;

            var rendeles = await _context.Rendelesek
                .Include(r => r.RendelesTetelek)
                    .ThenInclude(rt => rt.Termek)
                .FirstOrDefaultAsync(r => r.Id == id && r.RendelesTetelek.Any(rt => rt.Termek.KereskedoId == kereskedoId));

            if (rendeles == null) return NotFound();

            // Ha lemondják, készlet visszatöltése
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
            // Ha egy lemondottat visszaállítanak aktívra, készlet levonása
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

            // Visszairányítjuk a részletek oldalra!
            return RedirectToAction(nameof(Details), new { id = rendeles.Id });
        }
    }
}
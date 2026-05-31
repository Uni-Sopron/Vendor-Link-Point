using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vendor_Link_Point.Data;
using Vendor_Link_Point.Models;

namespace Vendor_Link_Point.Controllers
{
    [Authorize(Roles = "Kereskedo")]
    public class KonyvsController : Controller
    {
        private readonly VendorLinkPointContext _context;

        public KonyvsController(VendorLinkPointContext context)
        {
            _context = context;
        }

        // GET: Konyvs
        public async Task<IActionResult> Index()
        {
            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;

            // 2. SZŰRÉS: Csak a bejelentkezett kereskedő könyveit kérjük le
            var myKonyvek = await _context.Konyv
                .Where(k => k.KereskedoId == myKereskedoId)
                .ToListAsync();

            return View(myKonyvek);
        }

        // GET: Konyvs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;

            // Csak akkor mutatjuk meg, ha az övé
            var konyv = await _context.Konyv
                .FirstOrDefaultAsync(m => m.Id == id && m.KereskedoId == myKereskedoId);

            if (konyv == null) return NotFound();

            return View(konyv);
        }

        // GET: Konyvs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Konyvs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Szerzo,Isbn,Id,Nev,Gyarto,Ar,Raktarkeszlet,Kategoria,Leiras,KepUrl,Elerheto")] Konyv konyv)
        {
            ModelState.Remove("KereskedoId");
            ModelState.Remove("Kategoria");

            if (ModelState.IsValid)
            {
                // 3. HOZZÁRENDELÉS: Mentés előtt rögzítjük, hogy ki a tulajdonos
                konyv.KereskedoId = User.FindFirst("KereskedoId")?.Value;
                konyv.Kategoria = "Könyvek"; // Biztosíték

                _context.Add(konyv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(konyv);
        }

        // GET: Konyvs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var konyv = await _context.Konyv.FindAsync(id);
            if (konyv == null) return NotFound();

            // 4. JOGOSULTSÁG ELLENŐRZÉS: Tényleg ő a tulajdonos?
            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;
            if (konyv.KereskedoId != myKereskedoId)
            {
                return Unauthorized(); // Ha másét akarja szerkeszteni, elutasítjuk!
            }

            return View(konyv);
        }

        // POST: Konyvs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Szerzo,Isbn,Id,Nev,Gyarto,Ar,Raktarkeszlet,Kategoria,Leiras,KepUrl,Elerheto")] Konyv konyv)
        {
            if (id != konyv.Id) return NotFound();

            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;

            ModelState.Remove("KereskedoId");
            ModelState.Remove("Kategoria");

            if (ModelState.IsValid)
            {
                try
                {
                    // Visszapótoljuk a rejtett azonosítót, nehogy elvesszen a frissítéskor
                    konyv.KereskedoId = myKereskedoId;
                    konyv.Kategoria = "Könyvek";

                    _context.Update(konyv);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KonyvExists(konyv.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(konyv);
        }

        // GET: Konyvs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;

            var konyv = await _context.Konyv
                .FirstOrDefaultAsync(m => m.Id == id && m.KereskedoId == myKereskedoId);

            if (konyv == null) return NotFound(); // Ha másé, úgy teszünk, mintha nem is létezne

            return View(konyv);
        }

        // POST: Konyvs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;
            var konyv = await _context.Konyv.FindAsync(id);

            // Törlés előtt is meggyőződünk róla, hogy az övé
            if (konyv != null && konyv.KereskedoId == myKereskedoId)
            {
                _context.Konyv.Remove(konyv);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KonyvExists(int id)
        {
            return _context.Konyv.Any(e => e.Id == id);
        }
    }
}

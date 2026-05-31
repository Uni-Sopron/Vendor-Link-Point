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
    public class JateksController : Controller
    {
        private readonly VendorLinkPointContext _context;

        public JateksController(VendorLinkPointContext context)
        {
            _context = context;
        }

        // GET: Jateks
        public async Task<IActionResult> Index()
        {
            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;

            // 2. SZŰRÉS: Csak a bejelentkezett kereskedő könyveit kérjük le
            var myJateks = await _context.Jatek
                .Where(j => j.KereskedoId == myKereskedoId)
                .ToListAsync();

            return View(myJateks);
        }

        // GET: Jateks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;

            // Csak akkor mutatjuk meg, ha az övé
            var jatek = await _context.Jatek
                .FirstOrDefaultAsync(m => m.Id == id && m.KereskedoId == myKereskedoId);

            if (jatek == null) return NotFound();

            return View(jatek);
        }

        // GET: Jateks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jateks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Korhatar,Tipus,Id,Nev,Gyarto,Ar,Raktarkeszlet,Kategoria,Leiras,KepUrl,Elerheto")] Jatek jatek)
        {
            ModelState.Remove("KereskedoId");
            ModelState.Remove("Kategoria");

            if (ModelState.IsValid)
            {
                // 3. HOZZÁRENDELÉS: Mentés előtt rögzítjük, hogy ki a tulajdonos
                jatek.KereskedoId = User.FindFirst("KereskedoId")?.Value;
                jatek.Kategoria = "Játék"; // Biztosíték

                _context.Add(jatek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jatek);
        }

        // GET: Jateks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var jatek = await _context.Jatek.FindAsync(id);
            if (jatek == null) return NotFound();

            // 4. JOGOSULTSÁG ELLENŐRZÉS: Tényleg ő a tulajdonos?
            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;
            if (jatek.KereskedoId != myKereskedoId)
            {
                return Unauthorized(); // Ha másét akarja szerkeszteni, elutasítjuk!
            }

            return View(jatek);
        }

        // POST: Jateks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Korhatar,Tipus,Id,Nev,Gyarto,Ar,Raktarkeszlet,Kategoria,Leiras,KepUrl,Elerheto")] Jatek jatek)
        {
            if (id != jatek.Id) return NotFound();

            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;

            ModelState.Remove("KereskedoId");
            ModelState.Remove("Kategoria");

            if (ModelState.IsValid)
            {
                try
                {
                    // Visszapótoljuk a rejtett azonosítót, nehogy elvesszen a frissítéskor
                    jatek.KereskedoId = myKereskedoId;
                    jatek.Kategoria = "Játék";

                    _context.Update(jatek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JatekExists(jatek.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jatek);
        }

        // GET: Jateks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;

            var jatek = await _context.Jatek
                .FirstOrDefaultAsync(m => m.Id == id && m.KereskedoId == myKereskedoId);

            if (jatek == null) return NotFound(); // Ha másé, úgy teszünk, mintha nem is létezne

            return View(jatek);
        }

        // POST: Jateks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;
            var jatek = await _context.Jatek.FindAsync(id);

            // Törlés előtt is meggyőződünk róla, hogy az övé
            if (jatek != null && jatek.KereskedoId == myKereskedoId)
            {
                _context.Jatek.Remove(jatek);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool JatekExists(int id)
        {
            return _context.Jatek.Any(e => e.Id == id);
        }
    }
}

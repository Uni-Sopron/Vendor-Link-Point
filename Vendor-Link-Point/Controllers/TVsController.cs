using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vendor_Link_Point.Data;
using Vendor_Link_Point.Models;

namespace Vendor_Link_Point.Controllers
{
    public class TVsController : Controller
    {
        private readonly VendorLinkPointContext _context;

        public TVsController(VendorLinkPointContext context)
        {
            _context = context;
        }

        // GET: TVs
        public async Task<IActionResult> Index()
        {
            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;

            // 2. SZŰRÉS: Csak a bejelentkezett kereskedő könyveit kérjük le
            var myTvs = await _context.TV
                .Where(t => t.KereskedoId == myKereskedoId)
                .ToListAsync();

            return View(myTvs);
        }

        // GET: TVs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;

            // Csak akkor mutatjuk meg, ha az övé
            var tv = await _context.TV
                .FirstOrDefaultAsync(m => m.Id == id && m.KereskedoId == myKereskedoId);

            if (tv == null) return NotFound();

            return View(tv);
        }

        // GET: TVs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TVs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Kepatlo,Felbontas,Id,Nev,Gyarto,Ar,Raktarkeszlet,Kategoria,Leiras,KepUrl,Elerheto")] TV tV)
        {
            if (ModelState.IsValid)
            {
                // 3. HOZZÁRENDELÉS: Mentés előtt rögzítjük, hogy ki a tulajdonos
                tV.KereskedoId = User.FindFirst("KereskedoId")?.Value;
                tV.Kategoria = "TV"; // Biztosíték

                _context.Add(tV);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tV);
        }

        // GET: TVs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tv = await _context.TV.FindAsync(id);
            if (tv == null) return NotFound();

            // 4. JOGOSULTSÁG ELLENŐRZÉS: Tényleg ő a tulajdonos?
            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;
            if (tv.KereskedoId != myKereskedoId)
            {
                return Unauthorized(); // Ha másét akarja szerkeszteni, elutasítjuk!
            }

            return View(tv);
        }

        // POST: TVs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Kepatlo,Felbontas,Id,Nev,Gyarto,Ar,Raktarkeszlet,Kategoria,Leiras,KepUrl,Elerheto")] TV tV)
        {
            if (id != tV.Id) return NotFound();

            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;

            if (ModelState.IsValid)
            {
                try
                {
                    // Visszapótoljuk a rejtett azonosítót, nehogy elvesszen a frissítéskor
                    tV.KereskedoId = myKereskedoId;
                    tV.Kategoria = "TV";

                    _context.Update(tV);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TVExists(tV.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tV);
        }

        // GET: TVs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;

            var tv = await _context.TV
                .FirstOrDefaultAsync(m => m.Id == id && m.KereskedoId == myKereskedoId);

            if (tv == null) return NotFound(); // Ha másé, úgy teszünk, mintha nem is létezne

            return View(tv);
        }

        // POST: TVs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myKereskedoId = User.FindFirst("KereskedoId")?.Value;
            var tv = await _context.TV.FindAsync(id);

            // Törlés előtt is meggyőződünk róla, hogy az övé
            if (tv != null && tv.KereskedoId == myKereskedoId)
            {
                _context.TV.Remove(tv);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TVExists(int id)
        {
            return _context.TV.Any(e => e.Id == id);
        }
    }
}

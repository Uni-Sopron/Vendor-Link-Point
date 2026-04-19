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
            return View(await _context.Jatek.ToListAsync());
        }

        // GET: Jateks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jatek = await _context.Jatek
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jatek == null)
            {
                return NotFound();
            }

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
            if (ModelState.IsValid)
            {
                _context.Add(jatek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jatek);
        }

        // GET: Jateks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jatek = await _context.Jatek.FindAsync(id);
            if (jatek == null)
            {
                return NotFound();
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
            if (id != jatek.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jatek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JatekExists(jatek.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jatek);
        }

        // GET: Jateks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jatek = await _context.Jatek
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jatek == null)
            {
                return NotFound();
            }

            return View(jatek);
        }

        // POST: Jateks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jatek = await _context.Jatek.FindAsync(id);
            if (jatek != null)
            {
                _context.Jatek.Remove(jatek);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JatekExists(int id)
        {
            return _context.Jatek.Any(e => e.Id == id);
        }
    }
}

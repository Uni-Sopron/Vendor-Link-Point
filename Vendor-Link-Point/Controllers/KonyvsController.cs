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
            return View(await _context.Konyv.ToListAsync());
        }

        // GET: Konyvs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konyv = await _context.Konyv
                .FirstOrDefaultAsync(m => m.Id == id);
            if (konyv == null)
            {
                return NotFound();
            }

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
            if (ModelState.IsValid)
            {
                _context.Add(konyv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(konyv);
        }

        // GET: Konyvs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konyv = await _context.Konyv.FindAsync(id);
            if (konyv == null)
            {
                return NotFound();
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
            if (id != konyv.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(konyv);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KonyvExists(konyv.Id))
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
            return View(konyv);
        }

        // GET: Konyvs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konyv = await _context.Konyv
                .FirstOrDefaultAsync(m => m.Id == id);
            if (konyv == null)
            {
                return NotFound();
            }

            return View(konyv);
        }

        // POST: Konyvs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var konyv = await _context.Konyv.FindAsync(id);
            if (konyv != null)
            {
                _context.Konyv.Remove(konyv);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KonyvExists(int id)
        {
            return _context.Konyv.Any(e => e.Id == id);
        }
    }
}

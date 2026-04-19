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
            return View(await _context.TV.ToListAsync());
        }

        // GET: TVs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tV = await _context.TV
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tV == null)
            {
                return NotFound();
            }

            return View(tV);
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
                _context.Add(tV);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tV);
        }

        // GET: TVs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tV = await _context.TV.FindAsync(id);
            if (tV == null)
            {
                return NotFound();
            }
            return View(tV);
        }

        // POST: TVs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Kepatlo,Felbontas,Id,Nev,Gyarto,Ar,Raktarkeszlet,Kategoria,Leiras,KepUrl,Elerheto")] TV tV)
        {
            if (id != tV.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tV);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TVExists(tV.Id))
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
            return View(tV);
        }

        // GET: TVs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tV = await _context.TV
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tV == null)
            {
                return NotFound();
            }

            return View(tV);
        }

        // POST: TVs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tV = await _context.TV.FindAsync(id);
            if (tV != null)
            {
                _context.TV.Remove(tV);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TVExists(int id)
        {
            return _context.TV.Any(e => e.Id == id);
        }
    }
}

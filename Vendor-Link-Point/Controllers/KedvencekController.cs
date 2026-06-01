using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vendor_Link_Point.Data;
using Vendor_Link_Point.Models;

namespace Vendor_Link_Point.Controllers
{
    [Authorize] // Csak bejelentkezett felhasználóknak!
    public class KedvencekController : Controller
    {
        private readonly VendorLinkPointContext _context;

        public KedvencekController(VendorLinkPointContext context)
        {
            _context = context;
        }

        // GET: /Kedvencek
        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId)) return RedirectToAction("Login", "Account");

            // Lekérjük a felhasználó kedvenc termékeit
            var kedvencek = await _context.Kedvencek
                .Include(k => k.Termek)
                .Where(k => k.UserId == userId)
                .ToListAsync();

            return View(kedvencek);
        }

        // POST: /Kedvencek/Toggle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int productId, string returnUrl)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId)) return RedirectToAction("Login", "Account");

            // Megnézzük, hogy a termék már a kedvencek között van-e
            var letezik = await _context.Kedvencek
                .FirstOrDefaultAsync(k => k.UserId == userId && k.ProductId == productId);

            if (letezik != null)
            {
                _context.Kedvencek.Remove(letezik);
                TempData["SuccessMessage"] = "Termék eltávolítva a kedvencek közül.";
            }
            else
            {
                _context.Kedvencek.Add(new Kedvenc { UserId = userId, ProductId = productId });
                TempData["SuccessMessage"] = "Termék hozzáadva a kedvencekhez!";
            }

            await _context.SaveChangesAsync();

            // Visszairányítjuk oda, ahonnan kattintott
            if (!string.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index", "Products");
        }
    }
}
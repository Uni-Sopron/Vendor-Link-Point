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
    }
}
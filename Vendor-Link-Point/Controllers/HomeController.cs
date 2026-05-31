using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Vendor_Link_Point.Data;
using Vendor_Link_Point.Models;

namespace Vendor_Link_Point.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VendorLinkPointContext _context;

        // Injektáljuk az adatbázis kontextust
        public HomeController(ILogger<HomeController> logger, VendorLinkPointContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Lekérünk 4 darab véletlenszerű, elérhető terméket az adatbázisból
            var trendingProducts = await _context.Products
                .Where(p => p.Elerheto)
                .OrderBy(x => Guid.NewGuid()) // Ez végzi a randomizálást
                .Take(4)                      // Csak az első 4-et vesszük
                .ToListAsync();

            // Átadjuk a termékeket a Nézetnek
            return View(trendingProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Vendor_Link_Point.Data;

namespace Vendor_Link_Point.Controllers
{
    public class ProductsController : Controller
    {
        private readonly VendorLinkPointContext _context;

        public ProductsController(VendorLinkPointContext context)
        {
            _context = context;
        }

        // GET: /Products
        public IActionResult Index()
        {
            // Lekérjük az összes terméket, ami elérhető (Elerheto == true)
            // Később ide jöhet a szűrési logika is!
            var termekek = _context.Products.Where(p => p.Elerheto).ToList();

            return View(termekek);
        }
    }
}

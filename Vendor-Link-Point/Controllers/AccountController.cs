using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vendor_Link_Point.Data;
using Vendor_Link_Point.Helpers;
using Vendor_Link_Point.Models;
using Vendor_Link_Point.ViewModels;

namespace Vendor_Link_Point.Controllers
{
    public class AccountController : Controller
    {
        private readonly VendorLinkPointContext _context;

        public AccountController(VendorLinkPointContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            // Alapértelmezetten legyen a Vásárló kiválasztva, de a Kereskedő ID már legyen bekészítve
            var model = new RegisterViewModel
            {
                Role = "Vasarlo",
                KereskedoId = StringHelper.GenerateRandomId() // Generáljuk az azonosítót
            };
            return View(model);
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Egyedi validáció: Ha vásárló, kell a cím, ha kereskedő, kell az ID.
            if (model.Role == "Vasarlo" && string.IsNullOrWhiteSpace(model.SzallitasiCim))
            {
                ModelState.AddModelError("SzallitasiCim", "Vásárló esetén a szállítási cím kötelező!");
            }
            if (model.Role == "Kereskedo" && string.IsNullOrWhiteSpace(model.KereskedoId))
            {
                ModelState.AddModelError("KereskedoId", "Kereskedő esetén az azonosító kötelező!");
            }

            if (ModelState.IsValid)
            {
                // Van már ilyen e-mail?
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Ez az e-mail cím már regisztrálva van!");
                    return View(model);
                }

                // Jelszó titkosítása
                string hashedPassword = PasswordHelper.HashPassword(model.Jelszo);

                if (model.Role == "Vasarlo")
                {
                    var vasarlo = new Vasarlo
                    {
                        Nev = model.Nev,
                        Email = model.Email,
                        Jelszo = hashedPassword,
                        Role = "Vasarlo",
                        SzallitasiCim = model.SzallitasiCim!,
                        Telefonszam = model.Telefonszam
                    };
                    _context.Users.Add(vasarlo);
                }
                else if (model.Role == "Kereskedo")
                {
                    // --- EGYEDISÉG BIZTOSÍTÁSA ---
                    // Megnézzük, van-e már ilyen KereskedoId az adatbázisban.
                    // Ha igen, addig generálunk újat, amíg egy teljesen egyedit nem kapunk!
                    while (_context.Users.OfType<Kereskedo>().Any(k => k.KereskedoId == model.KereskedoId))
                    {
                        model.KereskedoId = StringHelper.GenerateRandomId();
                    }
                    // -----------------------------

                    var kereskedo = new Kereskedo
                    {
                        Nev = model.Nev,
                        Email = model.Email,
                        Jelszo = hashedPassword,
                        Role = "Kereskedo",
                        KereskedoId = model.KereskedoId,
                        Cegnev = model.Cegnev
                    };
                    _context.Users.Add(kereskedo);
                }

                await _context.SaveChangesAsync();

                // Sikeres regisztráció után menjünk a bejelentkezésre
                TempData["SuccessMessage"] = "Sikeres regisztráció! Kérjük, jelentkezz be.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Keresés az adatbázisban CSAK e-mail alapján
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

                // 2. Ha megvan a felhasználó ÉS a jelszó is helyes a sózott ellenőrzés alapján
                if (user != null && PasswordHelper.VerifyPassword(model.Jelszo, user.Jelszo))
                {
                    // Ha megvan a felhasználó, létrehozzuk a "Claim"-eket (igazolványt)
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Nev),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    if (user is Kereskedo kereskedo)
                    {
                        claims.Add(new Claim("KereskedoId", kereskedo.KereskedoId));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // A Süti létrehozása és bejelentkeztetés
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    // Sikeres belépés után irány a főoldal
                    return RedirectToAction("Index", "Home");
                }

                // Ha nem talált ilyen e-mailt, VAGY rossz a jelszó
                ModelState.AddModelError(string.Empty, "Hibás e-mail cím vagy jelszó!");
            }

            return View(model);
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Settings
        [Authorize]
        public async Task<IActionResult> Settings()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId)) return RedirectToAction("Login");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            var model = new SettingsViewModel { Role = user.Role };

            // Adatok betöltése szerepkör alapján
            if (user is Vasarlo vasarlo)
            {
                model.SzallitasiCim = vasarlo.SzallitasiCim;
                model.Telefonszam = vasarlo.Telefonszam;
            }
            else if (user is Kereskedo kereskedo)
            {
                model.Cegnev = kereskedo.Cegnev;
            }

            return View(model);
        }

        // POST: /Account/Settings
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(SettingsViewModel model)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId)) return RedirectToAction("Login");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            if (ModelState.IsValid)
            {
                // Adatok mentése szerepkör alapján
                if (user is Vasarlo vasarlo)
                {
                    vasarlo.SzallitasiCim = model.SzallitasiCim;
                    vasarlo.Telefonszam = model.Telefonszam;
                }
                else if (user is Kereskedo kereskedo)
                {
                    kereskedo.Cegnev = model.Cegnev;
                }

                await _context.SaveChangesAsync();

                // Sikerüzenet beállítása
                TempData["SuccessMessage"] = "A beállítások sikeresen mentve!";
                return RedirectToAction("Settings");
            }

            model.Role = user.Role; // Ha hiba volt, visszaadjuk a formot
            return View(model);
        }
    }
}

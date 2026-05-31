using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Vendor_Link_Point.Data;

namespace Vendor_Link_Point
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // 30 perc inaktivitás után ürül a kosár
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Süti alapú hitelesítés beállítása
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login"; // Ide irányítja a usert, ha nincs bejelentkezve
                    options.LogoutPath = "/Account/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1); // 1 óráig maradjon bejelentkezve
                });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<VendorLinkPointContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            var app = builder.Build();

            // ++ INNEN KEZDŐDIK AZ ÚJ RÉSZ (Magyar lokalizáció beállítása) ++
            var supportedCultures = new[] { "hu-HU" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);
            // ++ ÚJ RÉSZ VÉGE ++

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseSession();

            app.UseAuthentication(); // Hitelesítés engedélyezése
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
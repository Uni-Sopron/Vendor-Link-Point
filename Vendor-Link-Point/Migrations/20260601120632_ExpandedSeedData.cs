using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Vendor_Link_Point.Migrations
{
    /// <inheritdoc />
    public partial class ExpandedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "KepUrl",
                value: "https://images.unsplash.com/photo-1593359677879-a4bb92f829d1?auto=format&fit=crop&w=800&q=80");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "KepUrl",
                value: "https://images.unsplash.com/photo-1593784991095-a205069470b6?auto=format&fit=crop&w=800&q=80");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "KepUrl",
                value: "https://images.unsplash.com/photo-1629196914594-5b481977e68e?auto=format&fit=crop&w=800&q=80");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "KepUrl",
                value: "https://images.unsplash.com/photo-1550745165-9bc0b252726f?auto=format&fit=crop&w=800&q=80");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "KepUrl",
                value: "https://images.unsplash.com/photo-1511512578047-dfb367046420?auto=format&fit=crop&w=800&q=80");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Ar", "Elerheto", "Gyarto", "Kategoria", "KepUrl", "KereskedoId", "Leiras", "Nev", "Raktarkeszlet" },
                values: new object[,]
                {
                    { 6, 820000m, true, "Sony", "TV-k", "https://images.unsplash.com/photo-1552831388-6a0b35077328?auto=format&fit=crop&w=800&q=80", "VLP-ELEKTRO", "Hatalmas méret és prémium képminőség házimozihoz.", "Sony Bravia 75\" XR", 3 },
                    { 7, 185000m, true, "Philips", "TV-k", "https://images.unsplash.com/photo-1601944177325-f8867652837f?auto=format&fit=crop&w=800&q=80", "VLP-ELEKTRO", "Különleges háttérvilágítással, amely követi a képernyő eseményeit.", "Philips 50\" Ambilight", 8 },
                    { 8, 4800m, true, "Animus Kiadó", "Könyvek", "https://images.unsplash.com/photo-1618666012174-83b441c0bc76?auto=format&fit=crop&w=800&q=80", "VLP-KIRALY", "A történet, amellyel egy egész generáció szeretett bele az olvasásba.", "Harry Potter és a bölcsek köve", 45 },
                    { 9, 8900m, true, "Kiskapu", "Könyvek", "https://images.unsplash.com/photo-1555066931-4365d14bab8c?auto=format&fit=crop&w=800&q=80", "VLP-KIRALY", "Kötelező olvasmány minden szoftverfejlesztő számára.", "Tiszta kód (Clean Code)", 15 },
                    { 10, 5200m, true, "Gabo Kiadó", "Könyvek", "https://images.unsplash.com/photo-1541963463532-d68292c34b19?auto=format&fit=crop&w=800&q=80", "VLP-KIRALY", "Sci-fi klasszikus, amely alapjaiban határozta meg a műfajt.", "Dűne", 30 },
                    { 11, 12500m, true, "CD Projekt Red", "Játékok", "https://images.unsplash.com/photo-1605806616949-1e87b487cb2a?auto=format&fit=crop&w=800&q=80", "VLP-KOCKA", "Éld át a jövő sötét és neonfényes városának izgalmait.", "Cyberpunk 2077", 40 },
                    { 12, 19990m, true, "Electronic Arts", "Játékok", "https://images.unsplash.com/photo-1611158742626-663f73319be1?auto=format&fit=crop&w=800&q=80", "VLP-KOCKA", "A világ legnépszerűbb futballszimulátora új néven, de a régi minőséggel.", "EA Sports FC 24", 50 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Jelszo", "Nev", "Role" },
                values: new object[,]
                {
                    { 4, "konyv@vendor.hu", "7jqVtG5ypHGQXHK6YYkJWA==:Juoi47jOMwjH7dtqmA2zN6rRQXGZuoSJkewbDQoCcP8=", "Király Dávid", "Kereskedo" },
                    { 5, "anna@vendor.hu", "7jqVtG5ypHGQXHK6YYkJWA==:Juoi47jOMwjH7dtqmA2zN6rRQXGZuoSJkewbDQoCcP8=", "Nagy Anna", "Vasarlo" }
                });

            migrationBuilder.InsertData(
                table: "Jatekok",
                columns: new[] { "Id", "Korhatar", "Tipus" },
                values: new object[,]
                {
                    { 11, 18, "Akció-RPG" },
                    { 12, 3, "Sport" }
                });

            migrationBuilder.InsertData(
                table: "Kereskedok",
                columns: new[] { "Id", "Cegnev", "KereskedoId" },
                values: new object[] { 4, "Király Könyvesbolt", "VLP-KIRALY" });

            migrationBuilder.InsertData(
                table: "Konyvek",
                columns: new[] { "Id", "Isbn", "Szerzo" },
                values: new object[,]
                {
                    { 8, "9789633245453", "J.K. Rowling" },
                    { 9, "9789639301980", "Robert C. Martin" },
                    { 10, "9789634063216", "Frank Herbert" }
                });

            migrationBuilder.InsertData(
                table: "TVk",
                columns: new[] { "Id", "Felbontas", "Kepatlo" },
                values: new object[,]
                {
                    { 6, "8K UHD", 75 },
                    { 7, "4K UHD", 50 }
                });

            migrationBuilder.InsertData(
                table: "Vasarlok",
                columns: new[] { "Id", "SzallitasiCim", "Telefonszam" },
                values: new object[] { 5, "9022 Győr, Kiss János utca 5.", "+36209876543" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jatekok",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Jatekok",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Kereskedok",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Konyvek",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Konyvek",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Konyvek",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TVk",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TVk",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Vasarlok",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "KepUrl",
                value: "https://images.samsung.com/is/image/samsung/p6pim/hu/ue55cu7172uxxh/gallery/hu-crystal-uhd-cu7000-ue55cu7172uxxh-536306536?$650_519_PNG$");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "KepUrl",
                value: "https://www.lg.com/hu/images/televiziok/md07593256/gallery/D-01.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "KepUrl",
                value: "https://bookline.hu/zoom/bookline/092/10/92/27/0921092270.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "KepUrl",
                value: "https://image.api.playstation.com/vulcan/ap/rnd/202211/0711/kh4MUIuMmGIEPRaJ3z7E8MIG.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "KepUrl",
                value: "https://image.api.playstation.com/vulcan/ap/rnd/202011/0919/cDKjqQc1QvM89tU33T94k8O6.png");
        }
    }
}

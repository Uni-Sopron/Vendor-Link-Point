using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Vendor_Link_Point.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jatekok",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Konyvek",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "Konyvek",
                columns: new[] { "Id", "Isbn", "Szerzo" },
                values: new object[] { 3, "978-963-07-9204-5", "J.R.R. Tolkien" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "KepUrl", "KereskedoId" },
                values: new object[] { "https://images.samsung.com/is/image/samsung/p6pim/hu/ue55cu7172uxxh/gallery/hu-crystal-uhd-cu7000-ue55cu7172uxxh-535874218?$650_519_PNG$", "VLP-ELEKTRO" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Ar", "Gyarto", "Kategoria", "KepUrl", "KereskedoId", "Leiras", "Nev", "Raktarkeszlet" },
                values: new object[] { 540000m, "LG", "TV-k", "https://www.lg.com/hu/images/televiziok/md07560212/gallery/D-1.jpg", "VLP-ELEKTRO", "A legmélyebb fekete és a legélénkebb színek, amit csak egy OLED kijelző nyújthat.", "LG OLED 65\" C3", 3 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Ar", "Gyarto", "Kategoria", "KepUrl", "KereskedoId", "Leiras", "Nev", "Raktarkeszlet" },
                values: new object[] { 5500m, "Európa Kiadó", "Könyvek", "https://bookline.hu/hu/control/shop/images?id=52945&type=10&size=original", "VLP-KOCKA", "Klasszikus fantasy regény, az epikus kaland kezdete.", "A Gyűrűk Ura - A Gyűrű Szövetsége", 30 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Ar", "Elerheto", "Gyarto", "Kategoria", "KepUrl", "KereskedoId", "Leiras", "Nev", "Raktarkeszlet" },
                values: new object[,]
                {
                    { 4, 12000m, true, "CD Projekt Red", "Játékok", "https://image.api.playstation.com/vulcan/ap/rnd/202211/0711/kh4MUIuMmGIEPRaJ3z7E8MIG.png", "VLP-KOCKA", "Az egyik legjobb nyílt világú szerepjáték Geralt kalandjaival.", "The Witcher 3: Wild Hunt", 5 },
                    { 5, 24000m, true, "Warner Bros", "Játékok", "https://image.api.playstation.com/vulcan/ap/rnd/202011/0919/cDKjqQc1QvM89tU33T94k8O6.png", "VLP-KOCKA", "Légy részese a varázslóvilágnak a 19. századi Roxfortban!", "Hogwarts Legacy", 15 }
                });

            migrationBuilder.InsertData(
                table: "TVk",
                columns: new[] { "Id", "Felbontas", "Kepatlo" },
                values: new object[] { 2, "4K UHD", 65 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Jelszo", "Nev", "Role" },
                values: new object[,]
                {
                    { 1, "info@elektro.hu", "SEED_DUMMY_PASSWORD", "Elektro Admin", "Kereskedo" },
                    { 2, "info@kockabarlang.hu", "SEED_DUMMY_PASSWORD", "Kocka Admin", "Kereskedo" }
                });

            migrationBuilder.InsertData(
                table: "Jatekok",
                columns: new[] { "Id", "Korhatar", "Tipus" },
                values: new object[,]
                {
                    { 4, 18, "RPG" },
                    { 5, 16, "Akció-RPG" }
                });

            migrationBuilder.InsertData(
                table: "Kereskedok",
                columns: new[] { "Id", "Cegnev", "KereskedoId" },
                values: new object[,]
                {
                    { 1, "Elektro Kft.", "VLP-ELEKTRO" },
                    { 2, "Kocka Barlang", "VLP-KOCKA" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jatekok",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Jatekok",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Kereskedok",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Kereskedok",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Konyvek",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TVk",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "Jatekok",
                columns: new[] { "Id", "Korhatar", "Tipus" },
                values: new object[] { 3, 18, "RPG" });

            migrationBuilder.InsertData(
                table: "Konyvek",
                columns: new[] { "Id", "Isbn", "Szerzo" },
                values: new object[] { 2, "978-963-07-9204-5", "J.R.R. Tolkien" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "KepUrl", "KereskedoId" },
                values: new object[] { "https://dummyimage.com/400x300/282c34/fff.png&text=Samsung+TV", "VLP-7OF08QF3" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Ar", "Gyarto", "Kategoria", "KepUrl", "KereskedoId", "Leiras", "Nev", "Raktarkeszlet" },
                values: new object[] { 5500m, "Európa Kiadó", "Könyvek", "https://dummyimage.com/400x300/282c34/fff.png&text=Konyv", "VLP-7OF08QF3", "Klasszikus fantasy regény, az epikus kaland kezdete.", "A Gyűrűk Ura - A Gyűrű Szövetsége", 30 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Ar", "Gyarto", "Kategoria", "KepUrl", "KereskedoId", "Leiras", "Nev", "Raktarkeszlet" },
                values: new object[] { 12000m, "CD Projekt Red", "Játékok", "https://dummyimage.com/400x300/282c34/fff.png&text=Witcher+3", "VLP-7OF08QF3", "Az egyik legjobb nyílt világú szerepjáték Geralt kalandjaival.", "The Witcher 3: Wild Hunt", 5 });
        }
    }
}

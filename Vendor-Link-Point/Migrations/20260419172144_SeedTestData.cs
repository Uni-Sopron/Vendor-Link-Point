using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Vendor_Link_Point.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Ar", "Elerheto", "Gyarto", "Kategoria", "KepUrl", "Leiras", "Nev", "Raktarkeszlet" },
                values: new object[,]
                {
                    { 1, 165000m, true, "Samsung", "TV-k", "https://dummyimage.com/400x300/282c34/fff.png&text=Samsung+TV", "Kiváló minőségű okos TV 4K felbontással.", "Samsung 55\" Smart 4K TV", 12 },
                    { 2, 5500m, true, "Európa Kiadó", "Könyvek", "https://dummyimage.com/400x300/282c34/fff.png&text=Konyv", "Klasszikus fantasy regény, az epikus kaland kezdete.", "A Gyűrűk Ura - A Gyűrű Szövetsége", 30 },
                    { 3, 12000m, true, "CD Projekt Red", "Játékok", "https://dummyimage.com/400x300/282c34/fff.png&text=Witcher+3", "Az egyik legjobb nyílt világú szerepjáték Geralt kalandjaival.", "The Witcher 3: Wild Hunt", 5 }
                });

            migrationBuilder.InsertData(
                table: "Jatekok",
                columns: new[] { "Id", "Korhatar", "Tipus" },
                values: new object[] { 3, 18, "RPG" });

            migrationBuilder.InsertData(
                table: "Konyvek",
                columns: new[] { "Id", "Isbn", "Szerzo" },
                values: new object[] { 2, "978-963-07-9204-5", "J.R.R. Tolkien" });

            migrationBuilder.InsertData(
                table: "TVk",
                columns: new[] { "Id", "Felbontas", "Kepatlo" },
                values: new object[] { 1, "4K UHD", 55 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jatekok",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Konyvek",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TVk",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}

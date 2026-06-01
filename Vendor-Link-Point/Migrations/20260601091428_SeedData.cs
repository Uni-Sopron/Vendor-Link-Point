using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendor_Link_Point.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Telefonszam",
                table: "Vasarlok",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Leiras",
                table: "Products",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "KepUrl",
                table: "Products",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Cegnev",
                table: "Kereskedok",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Szoveg",
                table: "Ertekelesek",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Konyvek",
                keyColumn: "Id",
                keyValue: 3,
                column: "Isbn",
                value: "9789630798150");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "KepUrl", "Leiras", "Nev" },
                values: new object[] { "https://images.samsung.com/is/image/samsung/p6pim/hu/ue55cu7172uxxh/gallery/hu-crystal-uhd-cu7000-ue55cu7172uxxh-536306536?$650_519_PNG$", "Lélegzetelállító 4K felbontás és kristálytiszta színek okos funkciókkal.", "Samsung 55\" 4K Smart UHD TV" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "KepUrl", "Leiras", "Raktarkeszlet" },
                values: new object[] { "https://www.lg.com/hu/images/televiziok/md07593256/gallery/D-01.jpg", "Tökéletes fekete és végtelen kontraszt az LG OLED technológiájával.", 5 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Gyarto", "KepUrl", "Leiras", "Raktarkeszlet" },
                values: new object[] { "Európa Könyvkiadó", "https://bookline.hu/zoom/bookline/092/10/92/27/0921092270.jpg", "Minden idők leghíresebb fantasy regényének első része.", 20 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Ar", "Leiras", "Raktarkeszlet" },
                values: new object[] { 8500m, "Bújj Ríviai Geralt bőrébe ebben a hatalmas, nyílt világú kalandban.", 30 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "Leiras",
                value: "Fedezd fel a varázslóvilágot a 19. században ebben a lenyűgöző játékban!");

            migrationBuilder.UpdateData(
                table: "TVk",
                keyColumn: "Id",
                keyValue: 2,
                column: "Felbontas",
                value: "4K OLED");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Jelszo", "Nev" },
                values: new object[] { "elektro@vendor.hu", "7jqVtG5ypHGQXHK6YYkJWA==:Juoi47jOMwjH7dtqmA2zN6rRQXGZuoSJkewbDQoCcP8=", "Kovács Elek" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "Jelszo", "Nev" },
                values: new object[] { "kocka@vendor.hu", "7jqVtG5ypHGQXHK6YYkJWA==:Juoi47jOMwjH7dtqmA2zN6rRQXGZuoSJkewbDQoCcP8=", "Nagy Károly" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Jelszo", "Nev", "Role" },
                values: new object[] { 3, "vevo@vendor.hu", "7jqVtG5ypHGQXHK6YYkJWA==:Juoi47jOMwjH7dtqmA2zN6rRQXGZuoSJkewbDQoCcP8=", "Teszt Vásárló", "Vasarlo" });

            migrationBuilder.InsertData(
                table: "Vasarlok",
                columns: new[] { "Id", "SzallitasiCim", "Telefonszam" },
                values: new object[] { 3, "1055 Budapest, Kossuth Lajos tér 1-3.", "+36301234567" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vasarlok",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Vasarlok",
                keyColumn: "Telefonszam",
                keyValue: null,
                column: "Telefonszam",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Telefonszam",
                table: "Vasarlok",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Leiras",
                keyValue: null,
                column: "Leiras",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Leiras",
                table: "Products",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "KepUrl",
                keyValue: null,
                column: "KepUrl",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "KepUrl",
                table: "Products",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Kereskedok",
                keyColumn: "Cegnev",
                keyValue: null,
                column: "Cegnev",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Cegnev",
                table: "Kereskedok",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Ertekelesek",
                keyColumn: "Szoveg",
                keyValue: null,
                column: "Szoveg",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Szoveg",
                table: "Ertekelesek",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Konyvek",
                keyColumn: "Id",
                keyValue: 3,
                column: "Isbn",
                value: "978-963-07-9204-5");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "KepUrl", "Leiras", "Nev" },
                values: new object[] { "https://images.samsung.com/is/image/samsung/p6pim/hu/ue55cu7172uxxh/gallery/hu-crystal-uhd-cu7000-ue55cu7172uxxh-535874218?$650_519_PNG$", "Kiváló minőségű okos TV 4K felbontással.", "Samsung 55\" Smart 4K TV" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "KepUrl", "Leiras", "Raktarkeszlet" },
                values: new object[] { "https://www.lg.com/hu/images/televiziok/md07560212/gallery/D-1.jpg", "A legmélyebb fekete és a legélénkebb színek, amit csak egy OLED kijelző nyújthat.", 3 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Gyarto", "KepUrl", "Leiras", "Raktarkeszlet" },
                values: new object[] { "Európa Kiadó", "https://bookline.hu/hu/control/shop/images?id=52945&type=10&size=original", "Klasszikus fantasy regény, az epikus kaland kezdete.", 30 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Ar", "Leiras", "Raktarkeszlet" },
                values: new object[] { 12000m, "Az egyik legjobb nyílt világú szerepjáték Geralt kalandjaival.", 5 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "Leiras",
                value: "Légy részese a varázslóvilágnak a 19. századi Roxfortban!");

            migrationBuilder.UpdateData(
                table: "TVk",
                keyColumn: "Id",
                keyValue: 2,
                column: "Felbontas",
                value: "4K UHD");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Jelszo", "Nev" },
                values: new object[] { "info@elektro.hu", "SEED_DUMMY_PASSWORD", "Elektro Admin" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "Jelszo", "Nev" },
                values: new object[] { "info@kockabarlang.hu", "SEED_DUMMY_PASSWORD", "Kocka Admin" });
        }
    }
}

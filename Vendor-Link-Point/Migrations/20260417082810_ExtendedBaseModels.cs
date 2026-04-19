using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendor_Link_Point.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedBaseModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Telefonszam",
                table: "Vasarlok",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Elerheto",
                table: "Products",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "KepUrl",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Leiras",
                table: "Products",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Cegnev",
                table: "Kereskedok",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefonszam",
                table: "Vasarlok");

            migrationBuilder.DropColumn(
                name: "Elerheto",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "KepUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Leiras",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Cegnev",
                table: "Kereskedok");
        }
    }
}

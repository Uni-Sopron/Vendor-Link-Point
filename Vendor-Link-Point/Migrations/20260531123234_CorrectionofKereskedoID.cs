using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendor_Link_Point.Migrations
{
    /// <inheritdoc />
    public partial class CorrectionofKereskedoID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "KereskedoId",
                value: "VLP-7OF08QF3");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "KereskedoId",
                value: "VLP-7OF08QF3");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "KereskedoId",
                value: "VLP-7OF08QF3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "KereskedoId",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "KereskedoId",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "KereskedoId",
                value: "1");
        }
    }
}

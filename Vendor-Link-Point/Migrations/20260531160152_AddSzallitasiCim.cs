using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendor_Link_Point.Migrations
{
    /// <inheritdoc />
    public partial class AddSzallitasiCim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SzallitasiCim",
                table: "Rendelesek",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SzallitasiCim",
                table: "Rendelesek");
        }
    }
}

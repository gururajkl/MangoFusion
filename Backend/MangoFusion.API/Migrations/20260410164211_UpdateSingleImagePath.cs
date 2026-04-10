using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangoFusion.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSingleImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "Image",
                value: "images/paneer pizza.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "Image",
                value: "");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webbankhoahoconline.Migrations
{
    /// <inheritdoc />
    public partial class addquantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sold",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Courses");
        }
    }
}

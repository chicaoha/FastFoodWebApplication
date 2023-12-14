using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class updateCart1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DishSize",
                table: "Cart");

            migrationBuilder.AddColumn<string>(
                name: "size",
                table: "Cart",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "size",
                table: "Cart");

            migrationBuilder.AddColumn<int>(
                name: "DishSize",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

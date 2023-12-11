using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class updateAllModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DishSize",
                table: "Dish");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DishSize",
                table: "Dish",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

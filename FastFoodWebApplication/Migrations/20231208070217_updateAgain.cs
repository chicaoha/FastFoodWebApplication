using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class updateAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "totalPayment",
                table: "Profile");

            migrationBuilder.AddColumn<decimal>(
                name: "UserSpend",
                table: "Profile",
                type: "decimal(18,3)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserSpend",
                table: "Profile");

            migrationBuilder.AddColumn<decimal>(
                name: "totalPayment",
                table: "Profile",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

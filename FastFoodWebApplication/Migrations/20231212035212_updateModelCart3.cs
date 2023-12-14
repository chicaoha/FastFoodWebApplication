using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class updateModelCart3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Profile_profileUserId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_profileUserId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "profileUserId",
                table: "Cart");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserId",
                table: "Cart",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_AspNetUsers_UserId",
                table: "Cart",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_AspNetUsers_UserId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_UserId",
                table: "Cart");

            migrationBuilder.AddColumn<int>(
                name: "profileUserId",
                table: "Cart",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cart_profileUserId",
                table: "Cart",
                column: "profileUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Profile_profileUserId",
                table: "Cart",
                column: "profileUserId",
                principalTable: "Profile",
                principalColumn: "UserId");
        }
    }
}

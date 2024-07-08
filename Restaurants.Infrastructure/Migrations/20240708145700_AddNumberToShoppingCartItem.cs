using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberToShoppingCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "ShoppingCartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "ShoppingCartItems");
        }
    }
}

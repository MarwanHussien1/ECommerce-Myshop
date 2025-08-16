using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Myshop.Web.Migrations
{
    /// <inheritdoc />
    public partial class SolveShoppingCArtIsuue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUseerId",
                table: "ShoppingCarts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUseerId",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

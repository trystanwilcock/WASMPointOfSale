using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WASMPointOfSale.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductTax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tax",
                table: "Products");
        }
    }
}

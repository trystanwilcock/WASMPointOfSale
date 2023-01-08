using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WASMPointOfSale.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class StockQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Stocks");
        }
    }
}

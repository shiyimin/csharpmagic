using Microsoft.EntityFrameworkCore.Migrations;

namespace tradebot_sqlserver.Migrations
{
    public partial class V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AskOrderId",
                table: "DbPlacedOrders");

            migrationBuilder.AddColumn<string>(
                name: "SellOrderId",
                table: "DbPlacedOrders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellOrderId",
                table: "DbPlacedOrders");

            migrationBuilder.AddColumn<string>(
                name: "AskOrderId",
                table: "DbPlacedOrders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

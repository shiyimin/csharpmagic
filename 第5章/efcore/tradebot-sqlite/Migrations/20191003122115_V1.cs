using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tradebot_sqlite.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbPlacedOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Coin = table.Column<string>(nullable: false),
                    BrokerSite = table.Column<string>(nullable: false),
                    AskOrderId = table.Column<string>(nullable: true),
                    AskQuote = table.Column<string>(nullable: true),
                    AskPrice = table.Column<decimal>(type: "decimal(16, 8)", nullable: true),
                    AskFillInfo = table.Column<int>(nullable: true),
                    BidOrderId = table.Column<string>(nullable: false),
                    BidQuote = table.Column<string>(nullable: false),
                    BidPrice = table.Column<decimal>(type: "decimal(16, 8)", nullable: false),
                    TradingVolume = table.Column<decimal>(type: "decimal(16, 8)", nullable: false),
                    BidTimestamp = table.Column<DateTime>(nullable: false),
                    AskTimestamp = table.Column<DateTime>(nullable: true),
                    BidFillInfo = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbPlacedOrders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbPlacedOrders");
        }
    }
}

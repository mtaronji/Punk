using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Punk.Migrations
{
    /// <inheritdoc />
    public partial class v0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialDate",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialDate", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "SectorSymbols",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorSymbols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    Ticker = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: true),
                    Country = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Ticker);
                });

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    Ticker = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Open = table.Column<double>(type: "REAL", nullable: false),
                    Close = table.Column<double>(type: "REAL", nullable: false),
                    AdjClose = table.Column<double>(type: "REAL", nullable: false),
                    Low = table.Column<double>(type: "REAL", nullable: false),
                    High = table.Column<double>(type: "REAL", nullable: false),
                    Volume = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_Price_FinancialDate_Date",
                        column: x => x.Date,
                        principalTable: "FinancialDate",
                        principalColumn: "Date",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Price_Stock_Ticker",
                        column: x => x.Ticker,
                        principalTable: "Stock",
                        principalColumn: "Ticker",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectorComponents",
                columns: table => new
                {
                    SectorSymbolId = table.Column<string>(type: "TEXT", nullable: false),
                    StockTicker = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorComponents", x => new { x.SectorSymbolId, x.StockTicker });
                    table.ForeignKey(
                        name: "FK_SectorComponents_SectorSymbols_SectorSymbolId",
                        column: x => x.SectorSymbolId,
                        principalTable: "SectorSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SectorComponents_Stock_StockTicker",
                        column: x => x.StockTicker,
                        principalTable: "Stock",
                        principalColumn: "Ticker",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Price_Date",
                table: "Price",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Price_Ticker_Date",
                table: "Price",
                columns: new[] { "Ticker", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SectorComponents_SectorSymbolId_StockTicker",
                table: "SectorComponents",
                columns: new[] { "SectorSymbolId", "StockTicker" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SectorComponents_StockTicker",
                table: "SectorComponents",
                column: "StockTicker");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "SectorComponents");

            migrationBuilder.DropTable(
                name: "FinancialDate");

            migrationBuilder.DropTable(
                name: "SectorSymbols");

            migrationBuilder.DropTable(
                name: "Stock");
        }
    }
}

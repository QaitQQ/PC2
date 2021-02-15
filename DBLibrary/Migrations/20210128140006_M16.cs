using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ServerCore.Migrations
{
    public partial class M16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_PriceСhangeHistory_HistoryIDId",
                table: "Item");

            migrationBuilder.DropTable(
                name: "PriceСhangeHistory");

            migrationBuilder.DropIndex(
                name: "IX_Item_HistoryIDId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "HistoryIDId",
                table: "Item");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HistoryIDId",
                table: "Item",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PriceСhangeHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateСhange = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ItemID = table.Column<int>(type: "integer", nullable: false),
                    PartnerIDId = table.Column<int>(type: "integer", nullable: true),
                    PriceDC = table.Column<double>(type: "double precision", nullable: false),
                    PriceRC = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceСhangeHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceСhangeHistory_Partner_PartnerIDId",
                        column: x => x.PartnerIDId,
                        principalTable: "Partner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_HistoryIDId",
                table: "Item",
                column: "HistoryIDId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceСhangeHistory_PartnerIDId",
                table: "PriceСhangeHistory",
                column: "PartnerIDId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_PriceСhangeHistory_HistoryIDId",
                table: "Item",
                column: "HistoryIDId",
                principalTable: "PriceСhangeHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ServerCore.Migrations
{
    public partial class M18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceСhangeHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemID = table.Column<int>(nullable: false),
                    DateСhange = table.Column<DateTime>(nullable: false),
                    PriceRC = table.Column<double>(nullable: false),
                    PriceDC = table.Column<double>(nullable: false),
                    SourceName = table.Column<string>(nullable: true),
                    PartnerIDId = table.Column<int>(nullable: true)
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
                name: "IX_PriceСhangeHistory_PartnerIDId",
                table: "PriceСhangeHistory",
                column: "PartnerIDId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceСhangeHistory");
        }
    }
}

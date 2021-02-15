using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerCore.Migrations
{
    public partial class M15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateСhange",
                table: "PriceСhangeHistory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ItemID",
                table: "PriceСhangeHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PartnerIDId",
                table: "PriceСhangeHistory",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PriceDC",
                table: "PriceСhangeHistory",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PriceRC",
                table: "PriceСhangeHistory",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_PriceСhangeHistory_PartnerIDId",
                table: "PriceСhangeHistory",
                column: "PartnerIDId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceСhangeHistory_Partner_PartnerIDId",
                table: "PriceСhangeHistory",
                column: "PartnerIDId",
                principalTable: "Partner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceСhangeHistory_Partner_PartnerIDId",
                table: "PriceСhangeHistory");

            migrationBuilder.DropIndex(
                name: "IX_PriceСhangeHistory_PartnerIDId",
                table: "PriceСhangeHistory");

            migrationBuilder.DropColumn(
                name: "DateСhange",
                table: "PriceСhangeHistory");

            migrationBuilder.DropColumn(
                name: "ItemID",
                table: "PriceСhangeHistory");

            migrationBuilder.DropColumn(
                name: "PartnerIDId",
                table: "PriceСhangeHistory");

            migrationBuilder.DropColumn(
                name: "PriceDC",
                table: "PriceСhangeHistory");

            migrationBuilder.DropColumn(
                name: "PriceRC",
                table: "PriceСhangeHistory");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ServerCore.Migrations
{
    public partial class M7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HistoryIDId",
                table: "Item",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PriceСhangeHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceСhangeHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_HistoryIDId",
                table: "Item",
                column: "HistoryIDId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_PriceСhangeHistory_HistoryIDId",
                table: "Item",
                column: "HistoryIDId",
                principalTable: "PriceСhangeHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}

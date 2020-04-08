using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerCore.Migrations
{
    public partial class M3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierID",
                table: "Warehouse");

            migrationBuilder.AddColumn<int>(
                name: "PartnerID",
                table: "Warehouse",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Item",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartnerID",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Item");

            migrationBuilder.AddColumn<int>(
                name: "SupplierID",
                table: "Warehouse",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

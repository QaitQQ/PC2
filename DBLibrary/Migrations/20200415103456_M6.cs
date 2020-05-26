using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerCore.Migrations
{
    public partial class M6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Storage_Warehouse_WarehouseIDId",
                table: "Storage");

            migrationBuilder.DropIndex(
                name: "IX_Storage_WarehouseIDId",
                table: "Storage");

            migrationBuilder.DropColumn(
                name: "PartnerID",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "WarehouseIDId",
                table: "Storage");

            migrationBuilder.AddColumn<int>(
                name: "PartnerIDId",
                table: "Warehouse",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Storage",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_PartnerIDId",
                table: "Warehouse",
                column: "PartnerIDId");

            migrationBuilder.CreateIndex(
                name: "IX_Storage_WarehouseId",
                table: "Storage",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Storage_Warehouse_WarehouseId",
                table: "Storage",
                column: "WarehouseId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouse_Partner_PartnerIDId",
                table: "Warehouse",
                column: "PartnerIDId",
                principalTable: "Partner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Storage_Warehouse_WarehouseId",
                table: "Storage");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouse_Partner_PartnerIDId",
                table: "Warehouse");

            migrationBuilder.DropIndex(
                name: "IX_Warehouse_PartnerIDId",
                table: "Warehouse");

            migrationBuilder.DropIndex(
                name: "IX_Storage_WarehouseId",
                table: "Storage");

            migrationBuilder.DropColumn(
                name: "PartnerIDId",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Storage");

            migrationBuilder.AddColumn<int>(
                name: "PartnerID",
                table: "Warehouse",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseIDId",
                table: "Storage",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Storage_WarehouseIDId",
                table: "Storage",
                column: "WarehouseIDId");

            migrationBuilder.AddForeignKey(
                name: "FK_Storage_Warehouse_WarehouseIDId",
                table: "Storage",
                column: "WarehouseIDId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

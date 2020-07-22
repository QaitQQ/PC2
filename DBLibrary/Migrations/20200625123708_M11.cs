using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerCore.Migrations
{
    public partial class M11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Storage_Warehouse_WarehouseId",
                table: "Storage");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "Storage",
                newName: "WarehouseID");

            migrationBuilder.RenameIndex(
                name: "IX_Storage_WarehouseId",
                table: "Storage",
                newName: "IX_Storage_WarehouseID");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseID",
                table: "Storage",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Storage_Warehouse_WarehouseID",
                table: "Storage",
                column: "WarehouseID",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Storage_Warehouse_WarehouseID",
                table: "Storage");

            migrationBuilder.RenameColumn(
                name: "WarehouseID",
                table: "Storage",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Storage_WarehouseID",
                table: "Storage",
                newName: "IX_Storage_WarehouseId");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "Storage",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Storage_Warehouse_WarehouseId",
                table: "Storage",
                column: "WarehouseId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

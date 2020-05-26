using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ServerCore.Migrations
{
    public partial class M5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Storage",
                table: "Storage");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Storage");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Storage");

            migrationBuilder.DropColumn(
                name: "WarehouseID",
                table: "Storage");

            migrationBuilder.DropColumn(
                name: "StorageID",
                table: "Item");

            migrationBuilder.AddColumn<int>(
                name: "StorageId",
                table: "Storage",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Storage",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Storage",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseIDId",
                table: "Storage",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Storage",
                table: "Storage",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_Storage_ItemId",
                table: "Storage",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Storage_WarehouseIDId",
                table: "Storage",
                column: "WarehouseIDId");

            migrationBuilder.AddForeignKey(
                name: "FK_Storage_Item_ItemId",
                table: "Storage",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Storage_Warehouse_WarehouseIDId",
                table: "Storage",
                column: "WarehouseIDId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Storage_Item_ItemId",
                table: "Storage");

            migrationBuilder.DropForeignKey(
                name: "FK_Storage_Warehouse_WarehouseIDId",
                table: "Storage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Storage",
                table: "Storage");

            migrationBuilder.DropIndex(
                name: "IX_Storage_ItemId",
                table: "Storage");

            migrationBuilder.DropIndex(
                name: "IX_Storage_WarehouseIDId",
                table: "Storage");

            migrationBuilder.DropColumn(
                name: "StorageId",
                table: "Storage");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Storage");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Storage");

            migrationBuilder.DropColumn(
                name: "WarehouseIDId",
                table: "Storage");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Storage",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Storage",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseID",
                table: "Storage",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StorageID",
                table: "Item",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Storage",
                table: "Storage",
                column: "Id");
        }
    }
}

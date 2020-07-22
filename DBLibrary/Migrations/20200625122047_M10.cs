using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerCore.Migrations
{
    public partial class M10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Storage_Item_ItemId",
                table: "Storage");

            migrationBuilder.DropIndex(
                name: "IX_Storage_ItemId",
                table: "Storage");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Storage",
                newName: "ItemID");

            migrationBuilder.AlterColumn<int>(
                name: "ItemID",
                table: "Storage",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<List<int>>(
                name: "StorageID",
                table: "Item",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageID",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "ItemID",
                table: "Storage",
                newName: "ItemId");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Storage",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Storage_ItemId",
                table: "Storage",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Storage_Item_ItemId",
                table: "Storage",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

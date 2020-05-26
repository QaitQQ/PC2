using Microsoft.EntityFrameworkCore.Migrations;

using System.Collections.Generic;

namespace ServerCore.Migrations
{
    public partial class M4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<List<string>>(
                name: "Tags",
                table: "Item",
                nullable: true);

        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
                name: "Tags",
                table: "Item");
    }
}

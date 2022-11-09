using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ServerCore.Migrations
{
    /// <inheritdoc />
    public partial class M20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Аddress",
                table: "Partner",
                newName: "Contact_2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateСhange",
                table: "Storage",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateСhange",
                table: "PriceСhangeHistory",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Partner",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact_1",
                table: "Partner",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SphereOfActivityId",
                table: "Partner",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateСhange",
                table: "Item",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateОccurred",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatePlanned",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SphereOfActivity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SphereOfActivity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partner_CityId",
                table: "Partner",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Partner_SphereOfActivityId",
                table: "Partner",
                column: "SphereOfActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Partner_City_CityId",
                table: "Partner",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Partner_SphereOfActivity_SphereOfActivityId",
                table: "Partner",
                column: "SphereOfActivityId",
                principalTable: "SphereOfActivity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partner_City_CityId",
                table: "Partner");

            migrationBuilder.DropForeignKey(
                name: "FK_Partner_SphereOfActivity_SphereOfActivityId",
                table: "Partner");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "SphereOfActivity");

            migrationBuilder.DropIndex(
                name: "IX_Partner_CityId",
                table: "Partner");

            migrationBuilder.DropIndex(
                name: "IX_Partner_SphereOfActivityId",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "Contact_1",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "SphereOfActivityId",
                table: "Partner");

            migrationBuilder.RenameColumn(
                name: "Contact_2",
                table: "Partner",
                newName: "Аddress");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateСhange",
                table: "Storage",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateСhange",
                table: "PriceСhangeHistory",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateСhange",
                table: "Item",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateОccurred",
                table: "Events",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatePlanned",
                table: "Events",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}

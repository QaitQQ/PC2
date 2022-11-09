using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerCore.Migrations
{
    /// <inheritdoc />
    public partial class M21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Partner",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "INN",
                table: "Partner",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "INN",
                table: "Partner");
        }
    }
}

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
            migrationBuilder.AddColumn<int>(
                name: "LeadManagerId",
                table: "Partner",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Partner_LeadManagerId",
                table: "Partner",
                column: "LeadManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Partner_User_LeadManagerId",
                table: "Partner",
                column: "LeadManagerId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partner_User_LeadManagerId",
                table: "Partner");

            migrationBuilder.DropIndex(
                name: "IX_Partner_LeadManagerId",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "LeadManagerId",
                table: "Partner");
        }
    }
}

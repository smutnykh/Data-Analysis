using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class TourUserRelationshipAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tours",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tours_UserId",
                table: "Tours",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_Users_UserId",
                table: "Tours",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tours_Users_UserId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_Tours_UserId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tours");
        }
    }
}

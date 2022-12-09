using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class AlbumsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sightseeings_Sightseeings_SightseeingId",
                table: "Sightseeings");

            migrationBuilder.RenameColumn(
                name: "SightseeingId",
                table: "Sightseeings",
                newName: "AlbumId");

            migrationBuilder.RenameIndex(
                name: "IX_Sightseeings_SightseeingId",
                table: "Sightseeings",
                newName: "IX_Sightseeings_AlbumId");

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_UserId",
                table: "Albums",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sightseeings_Albums_AlbumId",
                table: "Sightseeings",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sightseeings_Albums_AlbumId",
                table: "Sightseeings");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.RenameColumn(
                name: "AlbumId",
                table: "Sightseeings",
                newName: "SightseeingId");

            migrationBuilder.RenameIndex(
                name: "IX_Sightseeings_AlbumId",
                table: "Sightseeings",
                newName: "IX_Sightseeings_SightseeingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sightseeings_Sightseeings_SightseeingId",
                table: "Sightseeings",
                column: "SightseeingId",
                principalTable: "Sightseeings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

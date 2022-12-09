using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class EntitiesFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sightseeings_Albums_AlbumId",
                table: "Sightseeings");

            migrationBuilder.DropIndex(
                name: "IX_Sightseeings_AlbumId",
                table: "Sightseeings");

            migrationBuilder.DropColumn(
                name: "AlbumId",
                table: "Sightseeings");

            migrationBuilder.CreateTable(
                name: "AlbumSightseeing",
                columns: table => new
                {
                    AlbumsId = table.Column<int>(type: "int", nullable: false),
                    SightseeingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumSightseeing", x => new { x.AlbumsId, x.SightseeingsId });
                    table.ForeignKey(
                        name: "FK_AlbumSightseeing_Albums_AlbumsId",
                        column: x => x.AlbumsId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumSightseeing_Sightseeings_SightseeingsId",
                        column: x => x.SightseeingsId,
                        principalTable: "Sightseeings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumSightseeing_SightseeingsId",
                table: "AlbumSightseeing",
                column: "SightseeingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumSightseeing");

            migrationBuilder.AddColumn<int>(
                name: "AlbumId",
                table: "Sightseeings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sightseeings_AlbumId",
                table: "Sightseeings",
                column: "AlbumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sightseeings_Albums_AlbumId",
                table: "Sightseeings",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

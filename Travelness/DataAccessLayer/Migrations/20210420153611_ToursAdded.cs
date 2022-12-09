using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class ToursAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sightseeing",
                table: "Sightseeing");

            migrationBuilder.RenameTable(
                name: "Sightseeing",
                newName: "Sightseeings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sightseeings",
                table: "Sightseeings",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Tours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SightseeingTour",
                columns: table => new
                {
                    SightseeingsId = table.Column<int>(type: "int", nullable: false),
                    ToursId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SightseeingTour", x => new { x.SightseeingsId, x.ToursId });
                    table.ForeignKey(
                        name: "FK_SightseeingTour_Sightseeings_SightseeingsId",
                        column: x => x.SightseeingsId,
                        principalTable: "Sightseeings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SightseeingTour_Tours_ToursId",
                        column: x => x.ToursId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SightseeingTour_ToursId",
                table: "SightseeingTour",
                column: "ToursId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SightseeingTour");

            migrationBuilder.DropTable(
                name: "Tours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sightseeings",
                table: "Sightseeings");

            migrationBuilder.RenameTable(
                name: "Sightseeings",
                newName: "Sightseeing");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sightseeing",
                table: "Sightseeing",
                column: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class RatesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SightseeingId",
                table: "Sightseeings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<byte>(type: "tinyint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    SightseeingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rates_Sightseeings_SightseeingId",
                        column: x => x.SightseeingId,
                        principalTable: "Sightseeings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sightseeings_SightseeingId",
                table: "Sightseeings",
                column: "SightseeingId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_SightseeingId",
                table: "Rates",
                column: "SightseeingId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_UserId",
                table: "Rates",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sightseeings_Sightseeings_SightseeingId",
                table: "Sightseeings",
                column: "SightseeingId",
                principalTable: "Sightseeings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sightseeings_Sightseeings_SightseeingId",
                table: "Sightseeings");

            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Sightseeings_SightseeingId",
                table: "Sightseeings");

            migrationBuilder.DropColumn(
                name: "SightseeingId",
                table: "Sightseeings");
        }
    }
}

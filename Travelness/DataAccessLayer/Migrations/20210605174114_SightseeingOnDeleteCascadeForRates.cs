using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class SightseeingOnDeleteCascadeForRates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Sightseeings_SightseeingId",
                table: "Rates");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Sightseeings_SightseeingId",
                table: "Rates",
                column: "SightseeingId",
                principalTable: "Sightseeings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Sightseeings_SightseeingId",
                table: "Rates");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Sightseeings_SightseeingId",
                table: "Rates",
                column: "SightseeingId",
                principalTable: "Sightseeings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

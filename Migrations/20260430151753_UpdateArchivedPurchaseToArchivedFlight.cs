using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightManagementWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateArchivedPurchaseToArchivedFlight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchivedPurchases_Flights_FlightId",
                table: "ArchivedPurchases");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "ArchivedPurchases",
                newName: "ArchivedFlightId");

            migrationBuilder.RenameIndex(
                name: "IX_ArchivedPurchases_FlightId",
                table: "ArchivedPurchases",
                newName: "IX_ArchivedPurchases_ArchivedFlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArchivedPurchases_ArchivedFlights_ArchivedFlightId",
                table: "ArchivedPurchases",
                column: "ArchivedFlightId",
                principalTable: "ArchivedFlights",
                principalColumn: "ArchivedFlightId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchivedPurchases_ArchivedFlights_ArchivedFlightId",
                table: "ArchivedPurchases");

            migrationBuilder.RenameColumn(
                name: "ArchivedFlightId",
                table: "ArchivedPurchases",
                newName: "FlightId");

            migrationBuilder.RenameIndex(
                name: "IX_ArchivedPurchases_ArchivedFlightId",
                table: "ArchivedPurchases",
                newName: "IX_ArchivedPurchases_FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArchivedPurchases_Flights_FlightId",
                table: "ArchivedPurchases",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

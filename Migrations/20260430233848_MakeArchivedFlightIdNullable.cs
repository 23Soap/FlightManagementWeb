using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightManagementWeb.Migrations
{
    /// <inheritdoc />
    public partial class MakeArchivedFlightIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchivedPurchases_ArchivedFlights_ArchivedFlightId",
                table: "ArchivedPurchases");

            migrationBuilder.AlterColumn<int>(
                name: "ArchivedFlightId",
                table: "ArchivedPurchases",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_ArchivedPurchases_ArchivedFlights_ArchivedFlightId",
                table: "ArchivedPurchases",
                column: "ArchivedFlightId",
                principalTable: "ArchivedFlights",
                principalColumn: "ArchivedFlightId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchivedPurchases_ArchivedFlights_ArchivedFlightId",
                table: "ArchivedPurchases");

            migrationBuilder.AlterColumn<int>(
                name: "ArchivedFlightId",
                table: "ArchivedPurchases",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ArchivedPurchases_ArchivedFlights_ArchivedFlightId",
                table: "ArchivedPurchases",
                column: "ArchivedFlightId",
                principalTable: "ArchivedFlights",
                principalColumn: "ArchivedFlightId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

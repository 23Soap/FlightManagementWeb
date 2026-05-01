using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightManagementWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddAircraftInfoToArchivedFlight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AircraftModel",
                table: "ArchivedFlights",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AirlineName",
                table: "ArchivedFlights",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TailNumber",
                table: "ArchivedFlights",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AircraftModel",
                table: "ArchivedFlights");

            migrationBuilder.DropColumn(
                name: "AirlineName",
                table: "ArchivedFlights");

            migrationBuilder.DropColumn(
                name: "TailNumber",
                table: "ArchivedFlights");
        }
    }
}

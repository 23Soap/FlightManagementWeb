using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlightManagementWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFlightAndAircraftModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AirlineName",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "RemainingCapacity",
                table: "Flights",
                newName: "FlightDuration");

            migrationBuilder.RenameColumn(
                name: "NumberOfPassengers",
                table: "Flights",
                newName: "AircraftId");

            migrationBuilder.CreateTable(
                name: "Aircrafts",
                columns: table => new
                {
                    AircraftId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AircraftModel = table.Column<string>(type: "text", nullable: false),
                    AirlineName = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.AircraftId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AircraftId",
                table: "Flights",
                column: "AircraftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Aircrafts_AircraftId",
                table: "Flights",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "AircraftId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Aircrafts_AircraftId",
                table: "Flights");

            migrationBuilder.DropTable(
                name: "Aircrafts");

            migrationBuilder.DropIndex(
                name: "IX_Flights_AircraftId",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "FlightDuration",
                table: "Flights",
                newName: "RemainingCapacity");

            migrationBuilder.RenameColumn(
                name: "AircraftId",
                table: "Flights",
                newName: "NumberOfPassengers");

            migrationBuilder.AddColumn<string>(
                name: "AirlineName",
                table: "Flights",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Flights",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

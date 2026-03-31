using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightManagementWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddFlightToPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlightNumber",
                table: "Purchases",
                newName: "FlightId");

            migrationBuilder.AlterColumn<string>(
                name: "SaveInformations",
                table: "Purchases",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_FlightId",
                table: "Purchases",
                column: "FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Flights_FlightId",
                table: "Purchases",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Flights_FlightId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_FlightId",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "Purchases",
                newName: "FlightNumber");

            migrationBuilder.AlterColumn<string>(
                name: "SaveInformations",
                table: "Purchases",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}

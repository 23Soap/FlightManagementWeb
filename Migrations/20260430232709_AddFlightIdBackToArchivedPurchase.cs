using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightManagementWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddFlightIdBackToArchivedPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "ArchivedPurchases",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "ArchivedPurchases");
        }
    }
}

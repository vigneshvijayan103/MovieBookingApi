using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingAPI.Migrations
{
    /// <inheritdoc />
    public partial class ShowIdadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShowId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowId",
                table: "Bookings");
        }
    }
}

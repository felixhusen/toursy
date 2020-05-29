using Microsoft.EntityFrameworkCore.Migrations;

namespace localtour.Migrations
{
    public partial class Modify_Booking_Tour_Date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TourDateId",
                table: "Bookings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TourDateId",
                table: "Bookings",
                column: "TourDateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_TourDates_TourDateId",
                table: "Bookings",
                column: "TourDateId",
                principalTable: "TourDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_TourDates_TourDateId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TourDateId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TourDateId",
                table: "Bookings");
        }
    }
}

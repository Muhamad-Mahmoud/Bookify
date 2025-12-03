using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.DL.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelIdToReservationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Populate HotelId from existing relationships
            migrationBuilder.Sql(
                @"UPDATE Reservations
                  SET HotelId = h.Id
                  FROM Reservations r
                  INNER JOIN ReservedRooms rr ON r.Id = rr.ReservationId
                  INNER JOIN Rooms rm ON rr.RoomId = rm.Id
                  INNER JOIN RoomTypes rt ON rm.RoomTypeId = rt.Id
                  INNER JOIN Hotels h ON rt.HotelId = h.Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_HotelId",
                table: "Reservations",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Hotels_HotelId",
                table: "Reservations",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Hotels_HotelId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_HotelId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Reservations");
        }
    }
}

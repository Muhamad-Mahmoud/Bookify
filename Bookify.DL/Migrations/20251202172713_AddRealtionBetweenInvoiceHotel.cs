using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.DL.Migrations
{
    /// <inheritdoc />
    public partial class AddRealtionBetweenInvoiceHotel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Populate HotelId from existing relationships
            migrationBuilder.Sql(
                @"UPDATE Invoices
                  SET HotelId = h.Id
                  FROM Invoices i
                  INNER JOIN Reservations r ON i.ReservationId = r.Id
                  INNER JOIN ReservedRooms rr ON r.Id = rr.ReservationId
                  INNER JOIN Rooms rm ON rr.RoomId = rm.Id
                  INNER JOIN RoomTypes rt ON rm.RoomTypeId = rt.Id
                  INNER JOIN Hotels h ON rt.HotelId = h.Id");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_HotelId",
                table: "Invoices",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Hotels_HotelId",
                table: "Invoices",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Hotels_HotelId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_HotelId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Invoices");
        }
    }
}

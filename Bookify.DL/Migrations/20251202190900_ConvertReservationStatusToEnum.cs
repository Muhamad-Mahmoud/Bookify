using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.DL.Migrations
{
    /// <inheritdoc />
    public partial class ConvertReservationStatusToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add temporary column
            migrationBuilder.AddColumn<int>(
                name: "StatusTemp",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Convert existing string values to enum integers
            // Pending = 0, Confirmed = 1, Cancelled = 2, Completed = 3
            migrationBuilder.Sql(@"
                UPDATE Reservations 
                SET StatusTemp = CASE 
                    WHEN Status = 'Pending' THEN 0
                    WHEN Status = 'Confirmed' THEN 1
                    WHEN Status = 'Cancelled' THEN 2
                    WHEN Status = 'Completed' THEN 3
                    ELSE 0
                END");

            // Drop old Status column
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservations");

            // Rename temp column to Status
            migrationBuilder.RenameColumn(
                name: "StatusTemp",
                table: "Reservations",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add temporary string column
            migrationBuilder.AddColumn<string>(
                name: "StatusTemp",
                table: "Reservations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            // Convert enum integers back to strings
            migrationBuilder.Sql(@"
                UPDATE Reservations 
                SET StatusTemp = CASE 
                    WHEN Status = 0 THEN 'Pending'
                    WHEN Status = 1 THEN 'Confirmed'
                    WHEN Status = 2 THEN 'Cancelled'
                    WHEN Status = 3 THEN 'Completed'
                    ELSE 'Pending'
                END");

            // Drop enum Status column
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservations");

            // Rename temp column to Status
            migrationBuilder.RenameColumn(
                name: "StatusTemp",
                table: "Reservations",
                newName: "Status");
        }
    }
}

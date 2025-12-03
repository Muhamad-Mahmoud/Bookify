using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.DL.Migrations
{
    /// <inheritdoc />
    public partial class ConvertStatusToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add temporary integer column
            migrationBuilder.AddColumn<int>(
                name: "StatusTemp",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                UPDATE Reservations 
                SET StatusTemp = CASE 
                    WHEN Status = 'Pending' THEN 0
                    WHEN Status = 'Confirmed' THEN 1
                    WHEN Status = 'Cancelled' THEN 2
                    WHEN Status = 'Completed' THEN 3
                    ELSE 0
                END");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "StatusTemp",
                table: "Reservations",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "Invoices");

            migrationBuilder.AddColumn<string>(
                name: "StatusTemp",
                table: "Reservations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE Reservations 
                SET StatusTemp = CASE 
                    WHEN Status = 0 THEN 'Pending'
                    WHEN Status = 1 THEN 'Confirmed'
                    WHEN Status = 2 THEN 'Cancelled'
                    WHEN Status = 3 THEN 'Completed'
                    ELSE 'Pending'
                END");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "StatusTemp",
                table: "Reservations",
                newName: "Status");
            migrationBuilder.RenameColumn(
                name: "StatusTemp",
                table: "Reservations",
                newName: "Status");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.DL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHotelStausColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Hotels");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Hotels");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Hotels",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

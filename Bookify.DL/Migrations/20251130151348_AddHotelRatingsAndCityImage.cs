using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.DL.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelRatingsAndCityImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingCount",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Hotels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReviewCount",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StarRating",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "UserRating",
                table: "Hotels",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Cities",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingCount",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ReviewCount",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "StarRating",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "UserRating",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Cities");
        }
    }
}

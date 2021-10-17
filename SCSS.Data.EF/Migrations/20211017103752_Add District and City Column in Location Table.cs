using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddDistrictandCityColumninLocationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "Location");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Location",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Location",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Location");

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Location",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddScrapImageintoBookingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CollectorDealerId",
                table: "ServiceTransaction",
                newName: "CollectorId");

            migrationBuilder.AddColumn<string>(
                name: "ScrapImageUrl",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScrapImageUrl",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "CollectorId",
                table: "ServiceTransaction",
                newName: "CollectorDealerId");
        }
    }
}

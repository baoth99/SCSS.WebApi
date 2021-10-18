using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddsomecolumnsintoCollectingRequestConfigTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AvailableRadius",
                table: "CollectingRequestConfig",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "NearestDistanceOfAppointment",
                table: "CollectingRequestConfig",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PriorityRating",
                table: "CollectingRequestConfig",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableRadius",
                table: "CollectingRequestConfig");

            migrationBuilder.DropColumn(
                name: "NearestDistanceOfAppointment",
                table: "CollectingRequestConfig");

            migrationBuilder.DropColumn(
                name: "PriorityRating",
                table: "CollectingRequestConfig");
        }
    }
}

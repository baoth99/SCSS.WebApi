using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddRequestTypeintoCollectingRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Longtitude",
                table: "CollectorCoordinate");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "CollectorCoordinate",
                type: "decimal(8,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "CollectorCoordinate",
                type: "decimal(9,6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestType",
                table: "CollectingRequest",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "CollectorCoordinate");

            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "CollectingRequest");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "CollectorCoordinate",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,6)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longtitude",
                table: "CollectorCoordinate",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}

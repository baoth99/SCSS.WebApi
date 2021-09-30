using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddOperatingTimeFromandOperatingTimeTointoCRConfigTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActiveTimeTo",
                table: "CollectingRequestConfig",
                newName: "OperatingTimeTo");

            migrationBuilder.RenameColumn(
                name: "ActiveTimeFrom",
                table: "CollectingRequestConfig",
                newName: "OperatingTimeFrom");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OperatingTimeTo",
                table: "CollectingRequestConfig",
                newName: "ActiveTimeTo");

            migrationBuilder.RenameColumn(
                name: "OperatingTimeFrom",
                table: "CollectingRequestConfig",
                newName: "ActiveTimeFrom");
        }
    }
}

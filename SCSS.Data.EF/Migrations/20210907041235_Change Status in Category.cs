using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class ChangeStatusinCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ScrapCategoryDetail");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ScrapCategory");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ScrapCategoryDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ScrapCategory",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ScrapCategoryDetail");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ScrapCategory");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ScrapCategoryDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ScrapCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

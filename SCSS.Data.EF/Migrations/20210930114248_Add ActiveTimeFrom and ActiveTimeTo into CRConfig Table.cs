using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddActiveTimeFromandActiveTimeTointoCRConfigTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "ActiveTimeFrom",
                table: "CollectingRequestConfig",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ActiveTimeTo",
                table: "CollectingRequestConfig",
                type: "time",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveTimeFrom",
                table: "CollectingRequestConfig");

            migrationBuilder.DropColumn(
                name: "ActiveTimeTo",
                table: "CollectingRequestConfig");
        }
    }
}

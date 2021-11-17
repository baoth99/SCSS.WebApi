using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddTimePeriodintoServiceTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Period",
                table: "ServiceTransaction",
                newName: "DateTimeTo");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeFrom",
                table: "ServiceTransaction",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeFrom",
                table: "ServiceTransaction");

            migrationBuilder.RenameColumn(
                name: "DateTimeTo",
                table: "ServiceTransaction",
                newName: "Period");
        }
    }
}

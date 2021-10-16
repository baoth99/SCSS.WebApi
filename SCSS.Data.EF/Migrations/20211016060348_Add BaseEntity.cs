using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "DealerComplaint",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "DealerComplaint",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "DealerComplaint",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "DealerComplaint",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "CollectorComplaint",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "CollectorComplaint",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "CollectorComplaint",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "CollectorComplaint",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DealerComplaint");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "DealerComplaint");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "DealerComplaint");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "DealerComplaint");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CollectorComplaint");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "CollectorComplaint");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CollectorComplaint");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "CollectorComplaint");
        }
    }
}

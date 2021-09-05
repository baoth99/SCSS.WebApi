using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddManagedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountDealerId",
                table: "DealerInformation",
                newName: "DealerMemberId");

            migrationBuilder.AddColumn<Guid>(
                name: "DealerAccountId",
                table: "DealerInformation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "DealerImageUrl",
                table: "DealerInformation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManagedBy",
                table: "Account",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DealerAccountId",
                table: "DealerInformation");

            migrationBuilder.DropColumn(
                name: "DealerImageUrl",
                table: "DealerInformation");

            migrationBuilder.DropColumn(
                name: "ManagedBy",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "DealerMemberId",
                table: "DealerInformation",
                newName: "AccountDealerId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddAppliedObjectandIsActiveinTransactionAwardAmountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Active",
                table: "TransactionAwardAmount",
                newName: "IsActive");

            migrationBuilder.AddColumn<int>(
                name: "AppliedObject",
                table: "TransactionAwardAmount",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedObject",
                table: "TransactionAwardAmount");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "TransactionAwardAmount",
                newName: "Active");
        }
    }
}

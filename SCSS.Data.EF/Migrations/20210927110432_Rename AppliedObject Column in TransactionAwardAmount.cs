using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class RenameAppliedObjectColumninTransactionAwardAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppliedObject",
                table: "TransactionAwardAmount",
                newName: "TransactionType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "TransactionAwardAmount",
                newName: "AppliedObject");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class ModifyServiceTransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServiceTransaction_IsDeleted",
                table: "ServiceTransaction");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ServiceTransaction",
                newName: "IsFinished");

            migrationBuilder.AddColumn<string>(
                name: "Period",
                table: "ServiceTransaction",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Period",
                table: "ServiceTransaction");

            migrationBuilder.RenameColumn(
                name: "IsFinished",
                table: "ServiceTransaction",
                newName: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTransaction_IsDeleted",
                table: "ServiceTransaction",
                column: "IsDeleted");
        }
    }
}

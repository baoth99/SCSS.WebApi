using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddBaseEnittyandAppliedAmountinTransactionAwardAmountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionAwardAmount_IsDeleted",
                table: "TransactionAwardAmount");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "TransactionAwardAmount",
                newName: "Active");

            migrationBuilder.AddColumn<long>(
                name: "AppliedAmount",
                table: "TransactionAwardAmount",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "TransactionAwardAmount",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "TransactionAwardAmount",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedAmount",
                table: "TransactionAwardAmount");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TransactionAwardAmount");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "TransactionAwardAmount");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "TransactionAwardAmount",
                newName: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionAwardAmount_IsDeleted",
                table: "TransactionAwardAmount",
                column: "IsDeleted");
        }
    }
}

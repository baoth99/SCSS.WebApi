using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddStatusinTransactionFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionServiceFeePercent_IsDeleted",
                table: "TransactionServiceFeePercent");

            migrationBuilder.DropColumn(
                name: "CollectingAcountId",
                table: "SellCollectTransaction");

            migrationBuilder.DropColumn(
                name: "SellerAcountId",
                table: "SellCollectTransaction");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "TransactionServiceFeePercent",
                newName: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "TransactionServiceFeePercent",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<Guid>(
                name: "CollectingAcountId",
                table: "SellCollectTransaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SellerAcountId",
                table: "SellCollectTransaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionServiceFeePercent_IsDeleted",
                table: "TransactionServiceFeePercent",
                column: "IsDeleted");
        }
    }
}

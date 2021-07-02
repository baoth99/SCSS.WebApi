using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class RemoveUpdateTimeBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TransactionServiceFeePercent");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "TransactionServiceFeePercent");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TransactionAwardAmount");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "TransactionAwardAmount");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CollectDealTransactionPromotion");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "CollectDealTransactionPromotion");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CollectDealTransactionPromotion");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "CollectDealTransactionPromotion");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Account");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "TransactionServiceFeePercent",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "TransactionServiceFeePercent",
                type: "datetime2",
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

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "CollectDealTransactionPromotion",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "CollectDealTransactionPromotion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "CollectDealTransactionPromotion",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "CollectDealTransactionPromotion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Account",
                type: "decimal(8,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Account",
                type: "decimal(9,6)",
                nullable: true);
        }
    }
}

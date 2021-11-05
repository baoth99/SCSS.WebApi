using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class Add2columnsinto2transactiontables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CollectorCategoryInformationId",
                table: "SellCollectTransactionDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DealerCategoryInformationId",
                table: "CollectDealTransactionDetail",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectorCategoryInformationId",
                table: "SellCollectTransactionDetail");

            migrationBuilder.DropColumn(
                name: "DealerCategoryInformationId",
                table: "CollectDealTransactionDetail");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddColletorAccountIdandSellerAccountIdinSellCollectTransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectingAcountId",
                table: "SellCollectTransaction");

            migrationBuilder.DropColumn(
                name: "SellerAcountId",
                table: "SellCollectTransaction");
        }
    }
}

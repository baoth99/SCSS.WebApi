using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddPriceintoCollectDealandSellCollect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Price",
                table: "SellCollectTransactionDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Price",
                table: "CollectDealTransactionDetail",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "SellCollectTransactionDetail");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CollectDealTransactionDetail");
        }
    }
}

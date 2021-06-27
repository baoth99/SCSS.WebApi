using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddColumnTypeDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Longtitude",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "BonusMoney",
                table: "CollectDealTransactionDetail");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "CategoryAdmin",
                newName: "ImageName");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "AccountCategory",
                newName: "ImageName");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Account",
                newName: "ImageName");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "ServiceTransaction",
                type: "decimal(15,2)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "SellCollectTransactionDetail",
                type: "decimal(15,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "SellCollectTransaction",
                type: "decimal(15,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Location",
                type: "decimal(8,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Location",
                type: "decimal(9,6)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BonusAmount",
                table: "CollectDealTransactionPromotion",
                type: "decimal(15,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "CollectDealTransactionDetail",
                type: "decimal(15,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BonusAmount",
                table: "CollectDealTransactionDetail",
                type: "decimal(15,2)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "CollectDealTransaction",
                type: "decimal(15,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BonusAmount",
                table: "CollectDealTransaction",
                type: "decimal(15,2)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Account",
                type: "decimal(9,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Account",
                type: "decimal(8,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "BonusAmount",
                table: "CollectDealTransactionDetail");

            migrationBuilder.DropColumn(
                name: "BonusAmount",
                table: "CollectDealTransaction");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "CategoryAdmin",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "AccountCategory",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Account",
                newName: "Image");

            migrationBuilder.AlterColumn<float>(
                name: "Amount",
                table: "ServiceTransaction",
                type: "real",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "SellCollectTransactionDetail",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "SellCollectTransaction",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Location",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,6)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longtitude",
                table: "Location",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BonusAmount",
                table: "CollectDealTransactionPromotion",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "CollectDealTransactionDetail",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BonusMoney",
                table: "CollectDealTransactionDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "CollectDealTransaction",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Account",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Account",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,6)",
                oldNullable: true);
        }
    }
}

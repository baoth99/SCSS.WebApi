using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddDealerInformationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DealerName",
                table: "Account");

            migrationBuilder.CreateTable(
                name: "DealerInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    DealerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DealerPhone = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerInformation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealerInformation_IsDeleted",
                table: "DealerInformation",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealerInformation");

            migrationBuilder.AddColumn<string>(
                name: "DealerName",
                table: "Account",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}

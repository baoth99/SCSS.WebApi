using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddFeedbackToSystemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminReply",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "CollectingRequestId",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "SellingFeedback",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Feedback");

            migrationBuilder.CreateTable(
                name: "FeedbackToSystem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellingAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BuyingAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CollectDealTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CollectingRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SellingFeedback = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdminReply = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackToSystem", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackToSystem_IsDeleted",
                table: "FeedbackToSystem",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedbackToSystem");

            migrationBuilder.AddColumn<string>(
                name: "AdminReply",
                table: "Feedback",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CollectingRequestId",
                table: "Feedback",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellingFeedback",
                table: "Feedback",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Feedback",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Feedback",
                type: "int",
                nullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class AddSellerCollectorDealerComplaintTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollectorComplaint",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComplaintId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ComplaintedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CollectorAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ComplaintContent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdminReply = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectorComplaint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealerComplaint",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComplaintId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ComplaintedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DealerAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ComplaintContent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdminReply = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerComplaint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SellerComplaint",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComplaintId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ComplaintedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SellerAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ComplaintContent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdminReply = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerComplaint", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectorComplaint");

            migrationBuilder.DropTable(
                name: "DealerComplaint");

            migrationBuilder.DropTable(
                name: "SellerComplaint");
        }
    }
}

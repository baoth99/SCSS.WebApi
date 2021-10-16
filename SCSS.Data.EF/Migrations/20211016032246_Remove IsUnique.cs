using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class RemoveIsUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SellerComplaint_ComplaintId",
                table: "SellerComplaint");

            migrationBuilder.DropIndex(
                name: "IX_DealerComplaint_ComplaintId",
                table: "DealerComplaint");

            migrationBuilder.DropIndex(
                name: "IX_CollectorComplaint_ComplaintId",
                table: "CollectorComplaint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SellerComplaint_ComplaintId",
                table: "SellerComplaint",
                column: "ComplaintId",
                unique: true,
                filter: "[ComplaintId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DealerComplaint_ComplaintId",
                table: "DealerComplaint",
                column: "ComplaintId",
                unique: true,
                filter: "[ComplaintId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CollectorComplaint_ComplaintId",
                table: "CollectorComplaint",
                column: "ComplaintId",
                unique: true,
                filter: "[ComplaintId] IS NOT NULL");
        }
    }
}

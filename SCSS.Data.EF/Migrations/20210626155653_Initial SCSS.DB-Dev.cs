using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCSS.Data.EF.Migrations
{
    public partial class InitialSCSSDBDev : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdCard = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TotalPoint = table.Column<float>(type: "real", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", maxLength: 20, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeFrom = table.Column<TimeSpan>(type: "time", nullable: true),
                    TimeTo = table.Column<TimeSpan>(type: "time", nullable: true),
                    ItemTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SellerAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CollectorAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CancelReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryAdmin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: true),
                    LockedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryAdmin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CollectDealTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    DealerAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CollectorAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AwardPoint = table.Column<float>(type: "real", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectDealTransaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CollectDealTransactionDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    CollectDealTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DealerCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<float>(type: "real", nullable: true),
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BonusMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectDealTransactionDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CollectDealTransactionPromotion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CollectDealTransactionDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BonusAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectDealTransactionPromotion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    SellingAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BuyingAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SellCollectTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CollectDealTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rate = table.Column<float>(type: "real", nullable: true),
                    SellingReview = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BuyingReview = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longtitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DealerCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DealerAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AppliedQuantity = table.Column<float>(type: "real", nullable: true),
                    BonusAmount = table.Column<float>(type: "real", nullable: true),
                    FromTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Key = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SellCollectTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AwardPoint = table.Column<float>(type: "real", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellCollectTransaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SellCollectTransactionDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    SellCollectTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CollectorCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<float>(type: "real", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellCollectTransactionDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    CollectorDealerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<float>(type: "real", nullable: true),
                    Period = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTransaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionAwardAmount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Amount = table.Column<float>(type: "real", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionAwardAmount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionServiceFeePercent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    TransactionType = table.Column<int>(type: "int", nullable: true),
                    Percent = table.Column<float>(type: "real", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionServiceFeePercent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Acronym = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountCategory_IsDeleted",
                table: "AccountCategory",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_IsDeleted",
                table: "Booking",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAdmin_IsDeleted",
                table: "CategoryAdmin",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CollectDealTransaction_IsDeleted",
                table: "CollectDealTransaction",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CollectDealTransactionDetail_IsDeleted",
                table: "CollectDealTransactionDetail",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CollectDealTransactionPromotion_IsDeleted",
                table: "CollectDealTransactionPromotion",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_IsDeleted",
                table: "Feedback",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ItemType_IsDeleted",
                table: "ItemType",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Location_IsDeleted",
                table: "Location",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_IsDeleted",
                table: "Notification",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_IsDeleted",
                table: "Promotion",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Role_IsDeleted",
                table: "Role",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SellCollectTransaction_IsDeleted",
                table: "SellCollectTransaction",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SellCollectTransactionDetail_IsDeleted",
                table: "SellCollectTransactionDetail",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTransaction_IsDeleted",
                table: "ServiceTransaction",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionAwardAmount_IsDeleted",
                table: "TransactionAwardAmount",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionServiceFeePercent_IsDeleted",
                table: "TransactionServiceFeePercent",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_IsDeleted",
                table: "Unit",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "AccountCategory");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "CategoryAdmin");

            migrationBuilder.DropTable(
                name: "CollectDealTransaction");

            migrationBuilder.DropTable(
                name: "CollectDealTransactionDetail");

            migrationBuilder.DropTable(
                name: "CollectDealTransactionPromotion");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "ItemType");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "SellCollectTransaction");

            migrationBuilder.DropTable(
                name: "SellCollectTransactionDetail");

            migrationBuilder.DropTable(
                name: "ServiceTransaction");

            migrationBuilder.DropTable(
                name: "TransactionAwardAmount");

            migrationBuilder.DropTable(
                name: "TransactionServiceFeePercent");

            migrationBuilder.DropTable(
                name: "Unit");
        }
    }
}

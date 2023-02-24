using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF_Models.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountStandings",
                columns: table => new
                {
                    AccountStandingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountStandings", x => x.AccountStandingID);
                });

            migrationBuilder.CreateTable(
                name: "AccountTypes",
                columns: table => new
                {
                    AccountTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermLengthDays = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypes", x => x.AccountTypeID);
                });

            migrationBuilder.CreateTable(
                name: "FishTypes",
                columns: table => new
                {
                    FishTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScientificName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxSize = table.Column<double>(type: "float", nullable: true),
                    RecommendedTankSize = table.Column<int>(type: "int", nullable: true),
                    MinTemp = table.Column<double>(type: "float", nullable: true),
                    MaxTemp = table.Column<double>(type: "float", nullable: true),
                    PhMin = table.Column<double>(type: "float", nullable: true),
                    PhMax = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishTypes", x => x.FishTypeID);
                });

            migrationBuilder.CreateTable(
                name: "SiteType",
                columns: table => new
                {
                    SiteTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteType", x => x.SiteTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetAdress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateOrProvince = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountTypeID = table.Column<int>(type: "int", nullable: false),
                    AccountStandingID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountID);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountStandings_AccountStandingID",
                        column: x => x.AccountStandingID,
                        principalTable: "AccountStandings",
                        principalColumn: "AccountStandingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountTypes_AccountTypeID",
                        column: x => x.AccountTypeID,
                        principalTable: "AccountTypes",
                        principalColumn: "AccountTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Site",
                columns: table => new
                {
                    SiteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Site", x => x.SiteID);
                    table.ForeignKey(
                        name: "FK_Site_SiteType_SiteTypeID",
                        column: x => x.SiteTypeID,
                        principalTable: "SiteType",
                        principalColumn: "SiteTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_Payments_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fishs",
                columns: table => new
                {
                    FishID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameOrTag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FishTypeID = table.Column<int>(type: "int", nullable: false),
                    SiteID = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fishs", x => x.FishID);
                    table.ForeignKey(
                        name: "FK_Fishs_FishTypes_FishTypeID",
                        column: x => x.FishTypeID,
                        principalTable: "FishTypes",
                        principalColumn: "FishTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fishs_Site_SiteID",
                        column: x => x.SiteID,
                        principalTable: "Site",
                        principalColumn: "SiteID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SiteUsers",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    SiteID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteUsers", x => new { x.SiteID, x.UserID });
                    table.ForeignKey(
                        name: "FK_SiteUsers_Site_SiteID",
                        column: x => x.SiteID,
                        principalTable: "Site",
                        principalColumn: "SiteID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiteUsers_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    TestResultID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteID = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: true),
                    Ph = table.Column<float>(type: "real", nullable: true),
                    Sality = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Alkalinty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Calcium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Magnesium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Phosphate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Nitrate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Nitrite = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Ammonia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    SiteUserSiteID = table.Column<int>(type: "int", nullable: true),
                    SiteUserUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.TestResultID);
                    table.ForeignKey(
                        name: "FK_TestResults_Site_SiteID",
                        column: x => x.SiteID,
                        principalTable: "Site",
                        principalColumn: "SiteID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestResults_SiteUsers_SiteUserSiteID_SiteUserUserID",
                        columns: x => new { x.SiteUserSiteID, x.SiteUserUserID },
                        principalTable: "SiteUsers",
                        principalColumns: new[] { "SiteID", "UserID" });
                });

            migrationBuilder.InsertData(
                table: "AccountStandings",
                columns: new[] { "AccountStandingID", "Name" },
                values: new object[,]
                {
                    { 1, "Current" },
                    { 2, "NotCurrent" }
                });

            migrationBuilder.InsertData(
                table: "AccountTypes",
                columns: new[] { "AccountTypeID", "Cost", "Name", "TermLengthDays" },
                values: new object[,]
                {
                    { 1, 12.99m, "PremiumMonthly", 30 },
                    { 2, 129.99m, "PremiumYearly", 365 },
                    { 3, 0m, "Trail", 14 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountStandingID",
                table: "Accounts",
                column: "AccountStandingID");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountTypeID",
                table: "Accounts",
                column: "AccountTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Fishs_FishTypeID",
                table: "Fishs",
                column: "FishTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Fishs_SiteID",
                table: "Fishs",
                column: "SiteID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AccountID",
                table: "Payments",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Site_SiteTypeID",
                table: "Site",
                column: "SiteTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SiteUsers_UserID",
                table: "SiteUsers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_SiteID",
                table: "TestResults",
                column: "SiteID");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_SiteUserSiteID_SiteUserUserID",
                table: "TestResults",
                columns: new[] { "SiteUserSiteID", "SiteUserUserID" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccountID",
                table: "Users",
                column: "AccountID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fishs");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropTable(
                name: "FishTypes");

            migrationBuilder.DropTable(
                name: "SiteUsers");

            migrationBuilder.DropTable(
                name: "Site");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SiteType");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AccountStandings");

            migrationBuilder.DropTable(
                name: "AccountTypes");
        }
    }
}

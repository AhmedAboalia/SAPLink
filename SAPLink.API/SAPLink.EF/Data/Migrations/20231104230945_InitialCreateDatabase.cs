using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SAPLink.EF.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Document = table.Column<int>(type: "INTEGER", nullable: false),
                    DocumentName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnvironmentCode = table.Column<int>(type: "INTEGER", nullable: false),
                    EnvironmentName = table.Column<string>(type: "TEXT", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    PrismUserName = table.Column<string>(type: "TEXT", nullable: false),
                    PrismPassword = table.Column<string>(type: "TEXT", nullable: false),
                    BaseUri = table.Column<string>(type: "TEXT", nullable: false),
                    BackOfficeUri = table.Column<string>(type: "TEXT", nullable: false),
                    CommonUri = table.Column<string>(type: "TEXT", nullable: false),
                    RestUri = table.Column<string>(type: "TEXT", nullable: false),
                    AuthSession = table.Column<string>(type: "TEXT", nullable: false),
                    Origin = table.Column<string>(type: "TEXT", nullable: false),
                    Referer = table.Column<string>(type: "TEXT", nullable: false),
                    ServiceLayerUri = table.Column<string>(type: "TEXT", nullable: false),
                    Server = table.Column<string>(type: "TEXT", nullable: false),
                    ServerTypes = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyDb = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    DbUserName = table.Column<string>(type: "TEXT", nullable: false),
                    DbPassword = table.Column<string>(type: "TEXT", nullable: false),
                    AuthUserName = table.Column<string>(type: "TEXT", nullable: false),
                    AuthPassword = table.Column<string>(type: "TEXT", nullable: false),
                    Authorization = table.Column<string>(type: "TEXT", nullable: false),
                    Cookie = table.Column<string>(type: "TEXT", nullable: false),
                    IntegrationUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credentials_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recurrings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    Time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recurrings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recurrings_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subsidiaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SID = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    ActivePriceLevelid = table.Column<string>(type: "TEXT", nullable: false),
                    ActiveSeasonSid = table.Column<string>(type: "TEXT", nullable: false),
                    ActiveStoreSid = table.Column<string>(type: "TEXT", nullable: false),
                    ActiveTaxCode = table.Column<string>(type: "TEXT", nullable: false),
                    Clerksid = table.Column<string>(type: "TEXT", nullable: false),
                    CredentialId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subsidiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subsidiaries_Credentials_CredentialId",
                        column: x => x.CredentialId,
                        principalTable: "Credentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Active", "Name" },
                values: new object[,]
                {
                    { 1, false, "Al-Kaffary Subsidiary - SAP Live DB (KaffaryDB)" },
                    { 2, false, "Test Subsidiary - SAP Test DB (TESTDB)" },
                    { 3, true, "Fakeeh Vision Subsidiary - SAP Local DB (SBODemoGB)" }
                });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "Document", "DocumentName" },
                values: new object[,]
                {
                    { 1, 1, "Departments" },
                    { 2, 2, "Vendors" },
                    { 3, 3, "Items" },
                    { 4, 4, "GoodsReceiptPos" },
                    { 5, 5, "GoodsReceipts_Inbound" },
                    { 6, 6, "GoodsIssues_Inbound" },
                    { 7, 7, "SalesInvoices" },
                    { 8, 8, "ReturnInvoices" },
                    { 9, 9, "CustomerOrders" },
                    { 10, 10, "StockTransfers" },
                    { 11, 11, "InventoryPosting" },
                    { 12, 12, "GoodsReceipts_Outbound" },
                    { 13, 13, "GoodsIssues_Outbound" }
                });

            migrationBuilder.InsertData(
                table: "Credentials",
                columns: new[] { "Id", "Active", "AuthPassword", "AuthSession", "AuthUserName", "Authorization", "BackOfficeUri", "BaseUri", "ClientId", "CommonUri", "CompanyDb", "Cookie", "DbPassword", "DbUserName", "EnvironmentCode", "EnvironmentName", "IntegrationUrl", "Origin", "Password", "PrismPassword", "PrismUserName", "Referer", "RestUri", "Server", "ServerTypes", "ServiceLayerUri", "UserName" },
                values: new object[,]
                {
                    { 1, false, "", "", "{{\"UserName\" : \"manager\",\"CompanyDB\" : \"\"}}", "", "http://kaffaryretail.alkaffary.com:8080/api/backoffice", "http://kaffaryretail.alkaffary.com:8080", 1, "http://kaffaryretail.alkaffary.com:8080/v1/rest", "", "", "sap123456*", "sa", 1, "Production Environment", "https://localhost:44326", "http://kaffaryretail.alkaffary.com:8080", "", "RetailTec@123", "SAPLINK", "http://kaffaryretail.alkaffary.com:8080/prism.shtml", "http://kaffaryretail.alkaffary.com:8080/api/common", "SAP-TEST", 10, "https://sap-test:50000/b1s/v1/", "manager" },
                    { 2, false, "Ag123456*", "369B7B1BF58F469896B06B804BFBE272", "{{\"UserName\" : \"manager\",\"CompanyDB\" : \"TESTDB\"}}", "Basic eyJVc2VyTmFtZSI6ICJtYW5hZ2VyIiwgIkNvbXBhbnlEQiI6ICJURVNUREIifTpSczEyMzQ1Nio=", "http://postest.alkaffary.com:8080/api/backoffice", "http://postest.alkaffary.com:8080", 2, "http://postest.alkaffary.com:8080/v1/rest", "TESTDB", "", "sap123456*", "sa", 2, "Test Environment", "https://localhost:44326", "http://postest.alkaffary.com:8080", "Ag123456*", "sysadmin", "sysadmin", "http://postest.alkaffary.com:8080/prism.shtml", "http://postest.alkaffary.com:8080/api/common", "SAP-TEST", 10, "https://sap-test:50000/b1s/v1/", "manager" },
                    { 3, true, "manager", "F1726B4EC6304D969ED816D844617C02", "{{\"UserName\" : \"manager\",\"CompanyDB\" : \"SBODemoGB\"}}", "Basic eyJVc2VyTmFtZSI6ICJtYW5hZ2VyIiwgIkNvbXBhbnlEQiI6ICJTQk9EZW1vR0IifTptYW5hZ2Vy", "http://194.163.155.105/api/backoffice", "http://194.163.155.105", 3, "http://194.163.155.105/v1/rest", "SBODemoGB", "", "P@ssw0rd", "sa", 3, "Local Environment", "https://localhost:44326", "http://194.163.155.105", "manager", "sysadmin", "sysadmin", "http://194.163.155.105/prism.shtml", "http://194.163.155.105/api/common", "ABOALIA", 15, "https://Localhost:50000/b1s/v1/", "manager" }
                });

            migrationBuilder.InsertData(
                table: "Recurrings",
                columns: new[] { "Id", "Active", "ScheduleId", "Time", "TimeId" },
                values: new object[,]
                {
                    { 1, true, 3, new TimeOnly(7, 0, 0), 1 },
                    { 2, true, 3, new TimeOnly(12, 0, 0), 2 },
                    { 3, true, 3, new TimeOnly(17, 0, 0), 3 },
                    { 4, true, 4, new TimeOnly(7, 0, 0), 1 },
                    { 5, true, 4, new TimeOnly(12, 0, 0), 2 },
                    { 6, true, 4, new TimeOnly(17, 0, 0), 3 },
                    { 7, true, 7, new TimeOnly(13, 0, 0), 1 },
                    { 8, true, 7, new TimeOnly(18, 0, 0), 2 },
                    { 9, true, 7, new TimeOnly(0, 0, 0), 3 },
                    { 10, true, 8, new TimeOnly(13, 0, 0), 1 },
                    { 11, true, 8, new TimeOnly(18, 0, 0), 2 },
                    { 12, true, 8, new TimeOnly(0, 0, 0), 3 },
                    { 13, true, 10, new TimeOnly(13, 0, 0), 1 },
                    { 14, true, 10, new TimeOnly(18, 0, 0), 2 },
                    { 15, true, 10, new TimeOnly(0, 0, 0), 3 }
                });

            migrationBuilder.InsertData(
                table: "Subsidiaries",
                columns: new[] { "Id", "ActivePriceLevelid", "ActiveSeasonSid", "ActiveStoreSid", "ActiveTaxCode", "Clerksid", "CredentialId", "Name", "Number", "SID" },
                values: new object[,]
                {
                    { 1, "664651377000135721", "664651377000169734", "664651285000116261", "664651377000183746", "674955099100039866", 1, "AlKaffary - (Production)", 1, 664651285000113257L },
                    { 2, "663852140000113721", "663852140000143734", "674650601000132347", "663852140000157746", "674654182000171601", 2, "AlKaffary - (Test)", 1, 663852103000153257L },
                    { 3, "675951940000193772", "675951941000138785", "675951888000150261", "", "675951888000149260", 3, "Local Environment (Public API)", 1, 675951888000146257L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_ClientId",
                table: "Credentials",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Recurrings_ScheduleId",
                table: "Recurrings",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Subsidiaries_CredentialId",
                table: "Subsidiaries",
                column: "CredentialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recurrings");

            migrationBuilder.DropTable(
                name: "Subsidiaries");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}

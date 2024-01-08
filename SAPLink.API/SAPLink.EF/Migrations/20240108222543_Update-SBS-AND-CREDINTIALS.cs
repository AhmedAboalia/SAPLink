using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SAPLink.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSBSANDCREDINTIALS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sync",
                columns: table => new
                {
                    UpdateType = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sync", x => x.UpdateType);
                });

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Authorization", "PrismPassword", "PrismUserName" },
                values: new object[] { "Basic eyJVc2VyTmFtZSI6ICJtYW5hZ2VyIiwgIkNvbXBhbnlEQiI6ICJURVNUREIifTpBZzEyMzQ1Nio=", "RetailTec@123", "SAPLINK3" });

            migrationBuilder.UpdateData(
                table: "Subsidiaries",
                keyColumn: "Id",
                keyValue: 2,
                column: "Clerksid",
                value: "694703502045666230");

            migrationBuilder.InsertData(
                table: "Sync",
                columns: new[] { "UpdateType", "Date", "Name" },
                values: new object[,]
                {
                    { 0, new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "InitialDepartment" },
                    { 1, new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "SyncDepartment" },
                    { 2, new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "InitialVendors" },
                    { 3, new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "SyncVendors" },
                    { 4, new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "InitialItems" },
                    { 5, new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "SyncItems" },
                    { 6, new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "InitialGoodsReceiptPO" },
                    { 7, new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "SyncGoodsReceiptPO" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sync");

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Authorization", "PrismPassword", "PrismUserName" },
                values: new object[] { "Basic eyJVc2VyTmFtZSI6ICJtYW5hZ2VyIiwgIkNvbXBhbnlEQiI6ICJURVNUREIifTpSczEyMzQ1Nio=", "sysadmin", "sysadmin" });

            migrationBuilder.UpdateData(
                table: "Subsidiaries",
                keyColumn: "Id",
                keyValue: 2,
                column: "Clerksid",
                value: "674654182000171601");
        }
    }
}

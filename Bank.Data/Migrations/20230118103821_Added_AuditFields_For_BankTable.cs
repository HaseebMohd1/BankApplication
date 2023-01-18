using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedAuditFieldsForBankTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "BankDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "BankDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IsAtive",
                table: "BankDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BankDetails");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "BankDetails");

            migrationBuilder.DropColumn(
                name: "IsAtive",
                table: "BankDetails");
        }
    }
}

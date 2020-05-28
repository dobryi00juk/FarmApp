using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FarmApp.Infrastructure.Data.Migrations
{
    public partial class Sales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "SaleImportFiles",
                newName: "SaleImportFiles",
                newSchema: "tab");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                schema: "tab",
                table: "SaleImportFiles",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                schema: "tab",
                table: "SaleImportFiles",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                schema: "tab",
                table: "SaleImportFiles",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "SaleImportFiles",
                schema: "tab",
                newName: "SaleImportFiles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "SaleImportFiles",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "SaleImportFiles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "SaleImportFiles",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}

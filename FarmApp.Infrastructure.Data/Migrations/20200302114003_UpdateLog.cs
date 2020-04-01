using Microsoft.EntityFrameworkCore.Migrations;

namespace FarmApp.Infrastructure.Data.Migrations
{
    public partial class UpdateLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "log",
                table: "Logs",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                schema: "log",
                table: "Logs",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Header",
                schema: "log",
                table: "Logs",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderRequest",
                schema: "log",
                table: "Logs",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderResponse",
                schema: "log",
                table: "Logs",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "api",
                table: "ApiMethodRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                schema: "api",
                table: "ApiMethodRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                schema: "api",
                table: "ApiMethodRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                schema: "api",
                table: "ApiMethods",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsDeleted", "IsNeedAuthentication", "IsNotNullParam" },
                values: new object[] { false, false, true });

            migrationBuilder.UpdateData(
                schema: "api",
                table: "ApiMethods",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsDeleted", "IsNeedAuthentication", "IsNotNullParam" },
                values: new object[] { false, true, false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeaderRequest",
                schema: "log",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "HeaderResponse",
                schema: "log",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "log",
                table: "Logs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                schema: "log",
                table: "Logs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Header",
                schema: "log",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "api",
                table: "ApiMethodRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                schema: "api",
                table: "ApiMethodRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                schema: "api",
                table: "ApiMethodRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                schema: "api",
                table: "ApiMethods",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsDeleted", "IsNeedAuthentication", "IsNotNullParam" },
                values: new object[] { false, false, true });

            migrationBuilder.UpdateData(
                schema: "api",
                table: "ApiMethods",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsDeleted", "IsNeedAuthentication", "IsNotNullParam" },
                values: new object[] { false, true, false });
        }
    }
}

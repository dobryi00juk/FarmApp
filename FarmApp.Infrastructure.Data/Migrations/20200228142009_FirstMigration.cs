using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FarmApp.Infrastructure.Data.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "api");

            migrationBuilder.EnsureSchema(
                name: "dist");

            migrationBuilder.EnsureSchema(
                name: "tab");

            migrationBuilder.EnsureSchema(
                name: "log");

            migrationBuilder.CreateTable(
                name: "ApiMethods",
                schema: "api",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiMethodName = table.Column<string>(maxLength: 350, nullable: false),
                    StoredProcedureName = table.Column<string>(maxLength: 350, nullable: true),
                    PathUrl = table.Column<string>(maxLength: 350, nullable: false),
                    HttpMethod = table.Column<string>(maxLength: 350, nullable: false),
                    IsNotNullParam = table.Column<bool>(nullable: false, defaultValueSql: "((0))"),
                    IsNeedAuthentication = table.Column<bool>(nullable: false, defaultValueSql: "((0))"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeAthTypes",
                schema: "dist",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeAthId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    NameAth = table.Column<string>(maxLength: 350, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeAthTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeAthTypes_CodeAthTypes_CodeAthId",
                        column: x => x.CodeAthId,
                        principalSchema: "dist",
                        principalTable: "CodeAthTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegionTypes",
                schema: "dist",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionTypeName = table.Column<string>(maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "dist",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                schema: "dist",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorName = table.Column<string>(maxLength: 255, nullable: false),
                    IsDomestic = table.Column<bool>(nullable: false, defaultValueSql: "((0))"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                schema: "log",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(maxLength: 255, nullable: true),
                    RoleId = table.Column<string>(maxLength: 255, nullable: true),
                    HttpMethod = table.Column<string>(maxLength: 255, nullable: true),
                    PathUrl = table.Column<string>(maxLength: 255, nullable: true),
                    MethodRoute = table.Column<string>(maxLength: 255, nullable: true),
                    RequestTime = table.Column<DateTime>(nullable: true),
                    FactTime = table.Column<DateTime>(nullable: true),
                    Param = table.Column<string>(maxLength: 4000, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    ResponseId = table.Column<Guid>(nullable: true),
                    ResponseTime = table.Column<DateTime>(nullable: true),
                    Header = table.Column<string>(nullable: true),
                    Result = table.Column<string>(maxLength: 4000, nullable: true),
                    Exception = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                schema: "dist",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionId = table.Column<int>(nullable: true),
                    RegionTypeId = table.Column<int>(nullable: false),
                    RegionName = table.Column<string>(maxLength: 255, nullable: false),
                    Population = table.Column<int>(nullable: false, defaultValueSql: "0"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Regions_RegionId",
                        column: x => x.RegionId,
                        principalSchema: "dist",
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Regions_RegionTypes_RegionTypeId",
                        column: x => x.RegionTypeId,
                        principalSchema: "dist",
                        principalTable: "RegionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApiMethodRoles",
                schema: "api",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiMethodId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiMethodRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiMethodRoles_ApiMethods_ApiMethodId",
                        column: x => x.ApiMethodId,
                        principalSchema: "api",
                        principalTable: "ApiMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApiMethodRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dist",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dist",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(maxLength: 20, nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    UserName = table.Column<string>(maxLength: 255, nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dist",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Drugs",
                schema: "tab",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugName = table.Column<string>(maxLength: 255, nullable: false),
                    CodeAthTypeId = table.Column<int>(nullable: false),
                    VendorId = table.Column<int>(nullable: false),
                    IsGeneric = table.Column<bool>(nullable: false, defaultValueSql: "((0))"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drugs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drugs_CodeAthTypes_CodeAthTypeId",
                        column: x => x.CodeAthTypeId,
                        principalSchema: "dist",
                        principalTable: "CodeAthTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Drugs_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "dist",
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pharmacies",
                schema: "dist",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PharmacyId = table.Column<int>(nullable: true),
                    PharmacyName = table.Column<string>(nullable: true),
                    RegionId = table.Column<int>(nullable: false),
                    IsMode = table.Column<bool>(nullable: false, defaultValueSql: "((0))"),
                    IsType = table.Column<bool>(nullable: false, defaultValueSql: "((0))"),
                    IsNetwork = table.Column<bool>(nullable: false, defaultValueSql: "((0))"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalSchema: "dist",
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Regions_RegionId",
                        column: x => x.RegionId,
                        principalSchema: "dist",
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                schema: "tab",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugId = table.Column<int>(nullable: false),
                    PharmacyId = table.Column<int>(nullable: false),
                    SaleDate = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Price = table.Column<decimal>(type: "MONEY", nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    IsDiscount = table.Column<bool>(nullable: false, defaultValueSql: "((0))"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Drugs_DrugId",
                        column: x => x.DrugId,
                        principalSchema: "tab",
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sales_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalSchema: "dist",
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "api",
                table: "ApiMethods",
                columns: new[] { "Id", "ApiMethodName", "HttpMethod", "IsDeleted", "IsNeedAuthentication", "IsNotNullParam", "PathUrl", "StoredProcedureName" },
                values: new object[,]
                {
                    { 1, "GetToken", "POST", false, false, true, "/GetToken", null },
                    { 2, "GetUser", "GET", false, true, false, "/GetUser", null }
                });

            migrationBuilder.InsertData(
                schema: "dist",
                table: "RegionTypes",
                columns: new[] { "Id", "RegionTypeName" },
                values: new object[,]
                {
                    { 1, "Государство" },
                    { 2, "Субъект(регион)" },
                    { 3, "Город" },
                    { 4, "Сёла, деревни и др." },
                    { 5, "Микрорайон" }
                });

            migrationBuilder.InsertData(
                schema: "dist",
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "user" }
                });

            migrationBuilder.InsertData(
                schema: "api",
                table: "ApiMethodRoles",
                columns: new[] { "Id", "ApiMethodId", "IsDeleted", "RoleId" },
                values: new object[,]
                {
                    { 1, 1, false, 1 },
                    { 3, 2, false, 1 },
                    { 2, 1, false, 2 }
                });

            migrationBuilder.InsertData(
                schema: "dist",
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "RoleId", "UserName" },
                values: new object[,]
                {
                    { 1, "admin", "123456", 1, "Админ" },
                    { 2, "user", "123456", 2, "Пользователь" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiMethodRoles_ApiMethodId",
                schema: "api",
                table: "ApiMethodRoles",
                column: "ApiMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiMethodRoles_RoleId",
                schema: "api",
                table: "ApiMethodRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiMethods_ApiMethodName",
                schema: "api",
                table: "ApiMethods",
                column: "ApiMethodName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodeAthTypes_CodeAthId",
                schema: "dist",
                table: "CodeAthTypes",
                column: "CodeAthId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_PharmacyId",
                schema: "dist",
                table: "Pharmacies",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_RegionId",
                schema: "dist",
                table: "Pharmacies",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_RegionId",
                schema: "dist",
                table: "Regions",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_RegionTypeId",
                schema: "dist",
                table: "Regions",
                column: "RegionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                schema: "dist",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_CodeAthTypeId",
                schema: "tab",
                table: "Drugs",
                column: "CodeAthTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_VendorId",
                schema: "tab",
                table: "Drugs",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_DrugId",
                schema: "tab",
                table: "Sales",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_PharmacyId",
                schema: "tab",
                table: "Sales",
                column: "PharmacyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiMethodRoles",
                schema: "api");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dist");

            migrationBuilder.DropTable(
                name: "Logs",
                schema: "log");

            migrationBuilder.DropTable(
                name: "Sales",
                schema: "tab");

            migrationBuilder.DropTable(
                name: "ApiMethods",
                schema: "api");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "dist");

            migrationBuilder.DropTable(
                name: "Drugs",
                schema: "tab");

            migrationBuilder.DropTable(
                name: "Pharmacies",
                schema: "dist");

            migrationBuilder.DropTable(
                name: "CodeAthTypes",
                schema: "dist");

            migrationBuilder.DropTable(
                name: "Vendors",
                schema: "dist");

            migrationBuilder.DropTable(
                name: "Regions",
                schema: "dist");

            migrationBuilder.DropTable(
                name: "RegionTypes",
                schema: "dist");
        }
    }
}

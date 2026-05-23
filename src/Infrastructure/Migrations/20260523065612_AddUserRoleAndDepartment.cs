using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleAndDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationTypeFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationTypeId = table.Column<int>(type: "int", nullable: false),
                    FieldCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTypeFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CsvColumnMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationTypeId = table.Column<int>(type: "int", nullable: false),
                    CsvColumnIndex = table.Column<int>(type: "int", nullable: false),
                    CsvColumnName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TargetFieldCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CsvColumnMappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DemoItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessedBlobRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContainerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BlobName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BlobETag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RowCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedBlobRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DebitAccount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreditAccount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SourceBlobPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntraObjectId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Upn = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    RoleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTypeFields_ApplicationTypeId_FieldCode",
                table: "ApplicationTypeFields",
                columns: new[] { "ApplicationTypeId", "FieldCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTypes_Code",
                table: "ApplicationTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                table: "Departments",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedBlobRecords_ContainerName_BlobName",
                table: "ProcessedBlobRecords",
                columns: new[] { "ContainerName", "BlobName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_DepartmentId",
                table: "UserRoles",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_EntraObjectId",
                table: "UserRoles",
                column: "EntraObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationTypeFields");

            migrationBuilder.DropTable(
                name: "ApplicationTypes");

            migrationBuilder.DropTable(
                name: "CsvColumnMappings");

            migrationBuilder.DropTable(
                name: "DemoItems");

            migrationBuilder.DropTable(
                name: "ProcessedBlobRecords");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}

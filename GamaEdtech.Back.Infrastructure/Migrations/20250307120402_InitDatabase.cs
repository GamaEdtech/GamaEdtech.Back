using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Back.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitDatabase : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "FAQCategories",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                CategoryType = table.Column<byte>(type: "tinyint", nullable: false),
                TreePath = table.Column<string>(type: "hierarchyid", nullable: false),
                CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FAQCategories", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "FAQS",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                FAQCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FAQS", x => x.Id);
                table.ForeignKey(
                    name: "FK_FAQS_FAQCategories_FAQCategoryId",
                    column: x => x.FAQCategoryId,
                    principalTable: "FAQCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_FAQS_FAQCategoryId",
            table: "FAQS",
            column: "FAQCategoryId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FAQS");

        migrationBuilder.DropTable(
            name: "FAQCategories");
    }
}

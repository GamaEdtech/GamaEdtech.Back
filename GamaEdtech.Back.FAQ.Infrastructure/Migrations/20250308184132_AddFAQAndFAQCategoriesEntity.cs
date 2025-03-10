using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Back.FAQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFAQAndFAQCategoriesEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FAQS_FAQCategories_FAQCategoryId",
                table: "FAQS");

            migrationBuilder.DropIndex(
                name: "IX_FAQS_FAQCategoryId",
                table: "FAQS");

            migrationBuilder.RenameColumn(
                name: "Answer",
                table: "FAQS",
                newName: "SummaryOfQuestion");

            migrationBuilder.CreateTable(
                name: "FAQAndFAQCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    FAQId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FAQCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQAndFAQCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FAQAndFAQCategories_FAQCategories_FAQCategoryId",
                        column: x => x.FAQCategoryId,
                        principalTable: "FAQCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FAQAndFAQCategories_FAQS_FAQId",
                        column: x => x.FAQId,
                        principalTable: "FAQS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FAQAndFAQCategories_FAQCategoryId",
                table: "FAQAndFAQCategories",
                column: "FAQCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FAQAndFAQCategories_FAQId",
                table: "FAQAndFAQCategories",
                column: "FAQId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQAndFAQCategories");

            migrationBuilder.RenameColumn(
                name: "SummaryOfQuestion",
                table: "FAQS",
                newName: "Answer");

            migrationBuilder.CreateIndex(
                name: "IX_FAQS_FAQCategoryId",
                table: "FAQS",
                column: "FAQCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FAQS_FAQCategories_FAQCategoryId",
                table: "FAQS",
                column: "FAQCategoryId",
                principalTable: "FAQCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

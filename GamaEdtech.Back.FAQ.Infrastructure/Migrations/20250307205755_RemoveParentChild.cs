using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Back.FAQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveParentChild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FAQCategories_FAQCategories_ParentId",
                table: "FAQCategories");

            migrationBuilder.DropIndex(
                name: "IX_FAQCategories_ParentId",
                table: "FAQCategories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "FAQCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "FAQCategories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FAQCategories_ParentId",
                table: "FAQCategories",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_FAQCategories_FAQCategories_ParentId",
                table: "FAQCategories",
                column: "ParentId",
                principalTable: "FAQCategories",
                principalColumn: "Id");
        }
    }
}

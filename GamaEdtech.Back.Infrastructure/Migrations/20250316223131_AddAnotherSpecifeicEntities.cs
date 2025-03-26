using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.SqlServer.Types;
using NetTopologySuite.Geometries;

#nullable disable

namespace GamaEdtech.Back.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAnotherSpecifeicEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "Media",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "FAQS",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "FAQCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "FAQAndFAQCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LatinTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LocationType = table.Column<int>(type: "int", nullable: false),
                    Coordinates = table.Column<Point>(type: "geometry", nullable: false),
                    HierarchyPath = table.Column<SqlHierarchyId>(type: "hierarchyid", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    OsmId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LocalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SchoolType = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quarter = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FaxNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WebSite = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Facilities = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Score = table.Column<double>(type: "float", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BodyComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassesQualityRate = table.Column<double>(type: "float", nullable: false),
                    EducationRate = table.Column<double>(type: "float", nullable: false),
                    ITTrainingRate = table.Column<double>(type: "float", nullable: false),
                    SafetyAndHappinessRate = table.Column<double>(type: "float", nullable: false),
                    BehaviorRate = table.Column<double>(type: "float", nullable: false),
                    TuitionRatioRate = table.Column<double>(type: "float", nullable: false),
                    FacilitiesRate = table.Column<double>(type: "float", nullable: false),
                    ArtisticActivitiesRate = table.Column<double>(type: "float", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    DislikeCount = table.Column<int>(type: "int", nullable: false),
                    AverageRate = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolComments_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolImages_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntityVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleted = table.Column<bool>(type: "bit", nullable: false),
                    FAQAndFAQCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FAQCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FAQId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MediaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVersions_FAQAndFAQCategories_FAQAndFAQCategoryId",
                        column: x => x.FAQAndFAQCategoryId,
                        principalTable: "FAQAndFAQCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVersions_FAQCategories_FAQCategoryId",
                        column: x => x.FAQCategoryId,
                        principalTable: "FAQCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVersions_FAQS_FAQId",
                        column: x => x.FAQId,
                        principalTable: "FAQS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVersions_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVersions_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVersions_SchoolComments_SchoolCommentId",
                        column: x => x.SchoolCommentId,
                        principalTable: "SchoolComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVersions_SchoolImages_SchoolImageId",
                        column: x => x.SchoolImageId,
                        principalTable: "SchoolImages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVersions_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityVersions_FAQAndFAQCategoryId",
                table: "EntityVersions",
                column: "FAQAndFAQCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVersions_FAQCategoryId",
                table: "EntityVersions",
                column: "FAQCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVersions_FAQId",
                table: "EntityVersions",
                column: "FAQId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVersions_LocationId",
                table: "EntityVersions",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVersions_MediaId",
                table: "EntityVersions",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVersions_SchoolCommentId",
                table: "EntityVersions",
                column: "SchoolCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVersions_SchoolId",
                table: "EntityVersions",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVersions_SchoolImageId",
                table: "EntityVersions",
                column: "SchoolImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Code",
                table: "Locations",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_HierarchyPath",
                table: "Locations",
                column: "HierarchyPath");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationType",
                table: "Locations",
                column: "LocationType");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolComments_SchoolId",
                table: "SchoolComments",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolImages_SchoolId",
                table: "SchoolImages",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_LocationId",
                table: "Schools",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityVersions");

            migrationBuilder.DropTable(
                name: "SchoolComments");

            migrationBuilder.DropTable(
                name: "SchoolImages");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "FAQS");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "FAQCategories");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "FAQAndFAQCategories");
        }
    }
}

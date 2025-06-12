using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zora.Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "zora");

            migrationBuilder.CreateTable(
                name: "Attraction",
                schema: "zora",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attraction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Destination",
                schema: "zora",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destination", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                schema: "zora",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "zora",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tour",
                schema: "zora",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    ElevationGain = table.Column<double>(type: "float", nullable: false),
                    AvailableSpots = table.Column<int>(type: "int", nullable: false),
                    ScheduledAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    GuideId = table.Column<long>(type: "bigint", nullable: false),
                    DestinationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tour_Destination_DestinationId",
                        column: x => x.DestinationId,
                        principalSchema: "zora",
                        principalTable: "Destination",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tour_User_GuideId",
                        column: x => x.GuideId,
                        principalSchema: "zora",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CheckListItem",
                schema: "zora",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TourId = table.Column<long>(type: "bigint", nullable: false),
                    EquipmentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckListItem_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalSchema: "zora",
                        principalTable: "Equipment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CheckListItem_Tour_TourId",
                        column: x => x.TourId,
                        principalSchema: "zora",
                        principalTable: "Tour",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CheckListItem_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "zora",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiaryNote",
                schema: "zora",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TourId = table.Column<long>(type: "bigint", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiaryNote_Tour_TourId",
                        column: x => x.TourId,
                        principalSchema: "zora",
                        principalTable: "Tour",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiaryNote_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "zora",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourAttraction",
                schema: "zora",
                columns: table => new
                {
                    TourId = table.Column<long>(type: "bigint", nullable: false),
                    AttractionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourAttraction", x => new { x.TourId, x.AttractionId });
                    table.ForeignKey(
                        name: "FK_TourAttraction_Attraction_AttractionId",
                        column: x => x.AttractionId,
                        principalSchema: "zora",
                        principalTable: "Attraction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TourAttraction_Tour_TourId",
                        column: x => x.TourId,
                        principalSchema: "zora",
                        principalTable: "Tour",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TourEquipment",
                schema: "zora",
                columns: table => new
                {
                    TourId = table.Column<long>(type: "bigint", nullable: false),
                    EquipmentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourEquipment", x => new { x.TourId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_TourEquipment_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalSchema: "zora",
                        principalTable: "Equipment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TourEquipment_Tour_TourId",
                        column: x => x.TourId,
                        principalSchema: "zora",
                        principalTable: "Tour",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserTour",
                schema: "zora",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TourId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTour", x => new { x.UserId, x.TourId });
                    table.ForeignKey(
                        name: "FK_UserTour_Tour_TourId",
                        column: x => x.TourId,
                        principalSchema: "zora",
                        principalTable: "Tour",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserTour_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "zora",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_Name",
                schema: "zora",
                table: "Attraction",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckListItem_EquipmentId",
                schema: "zora",
                table: "CheckListItem",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListItem_TourId",
                schema: "zora",
                table: "CheckListItem",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListItem_UserId",
                schema: "zora",
                table: "CheckListItem",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Destination_Name",
                schema: "zora",
                table: "Destination",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiaryNote_TourId",
                schema: "zora",
                table: "DiaryNote",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_DiaryNote_UserId",
                schema: "zora",
                table: "DiaryNote",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_Name",
                schema: "zora",
                table: "Equipment",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tour_DestinationId",
                schema: "zora",
                table: "Tour",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_GuideId",
                schema: "zora",
                table: "Tour",
                column: "GuideId");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_Name",
                schema: "zora",
                table: "Tour",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourAttraction_AttractionId",
                schema: "zora",
                table: "TourAttraction",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_TourEquipment_EquipmentId",
                schema: "zora",
                table: "TourEquipment",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "zora",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTour_TourId",
                schema: "zora",
                table: "UserTour",
                column: "TourId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckListItem",
                schema: "zora");

            migrationBuilder.DropTable(
                name: "DiaryNote",
                schema: "zora");

            migrationBuilder.DropTable(
                name: "TourAttraction",
                schema: "zora");

            migrationBuilder.DropTable(
                name: "TourEquipment",
                schema: "zora");

            migrationBuilder.DropTable(
                name: "UserTour",
                schema: "zora");

            migrationBuilder.DropTable(
                name: "Attraction",
                schema: "zora");

            migrationBuilder.DropTable(
                name: "Equipment",
                schema: "zora");

            migrationBuilder.DropTable(
                name: "Tour",
                schema: "zora");

            migrationBuilder.DropTable(
                name: "Destination",
                schema: "zora");

            migrationBuilder.DropTable(
                name: "User",
                schema: "zora");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calanggo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShortenedUrls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    ShortCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortenedUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UrlStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShortenedUrlId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalClicks = table.Column<int>(type: "integer", nullable: false),
                    LastClickedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrlStatistics_ShortenedUrls_ShortenedUrlId",
                        column: x => x.ShortenedUrlId,
                        principalTable: "ShortenedUrls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClickEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UrlStatisticsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClickedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: false),
                    UserAgent = table.Column<string>(type: "text", nullable: false),
                    Referer = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    DeviceType = table.Column<string>(type: "text", nullable: false),
                    Browser = table.Column<string>(type: "text", nullable: false),
                    OperatingSystem = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClickEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClickEvent_UrlStatistics_UrlStatisticsId",
                        column: x => x.UrlStatisticsId,
                        principalTable: "UrlStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UrlStatisticsId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceType = table.Column<string>(type: "text", nullable: false),
                    Browser = table.Column<string>(type: "text", nullable: false),
                    Clicks = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceMetrics_UrlStatistics_UrlStatisticsId",
                        column: x => x.UrlStatisticsId,
                        principalTable: "UrlStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UrlStatisticsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Clicks = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationMetrics_UrlStatistics_UrlStatisticsId",
                        column: x => x.UrlStatisticsId,
                        principalTable: "UrlStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClickEvent_UrlStatisticsId",
                table: "ClickEvent",
                column: "UrlStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMetrics_UrlStatisticsId",
                table: "DeviceMetrics",
                column: "UrlStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationMetrics_UrlStatisticsId",
                table: "LocationMetrics",
                column: "UrlStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrls_CreatedAt",
                table: "ShortenedUrls",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrls_ShortCode",
                table: "ShortenedUrls",
                column: "ShortCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UrlStatistics_LastClickedAt",
                table: "UrlStatistics",
                column: "LastClickedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UrlStatistics_ShortenedUrlId",
                table: "UrlStatistics",
                column: "ShortenedUrlId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClickEvent");

            migrationBuilder.DropTable(
                name: "DeviceMetrics");

            migrationBuilder.DropTable(
                name: "LocationMetrics");

            migrationBuilder.DropTable(
                name: "UrlStatistics");

            migrationBuilder.DropTable(
                name: "ShortenedUrls");
        }
    }
}

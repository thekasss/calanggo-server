using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calango.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenderMigrations : Migration
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
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
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
                    TotalClicks = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LastClickedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FirstClickedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "UrlStatistics");

            migrationBuilder.DropTable(
                name: "ShortenedUrls");
        }
    }
}

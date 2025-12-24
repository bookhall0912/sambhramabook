using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SambhramaBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "service_category_id",
                table: "listings",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "service_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    display_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    icon_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    background_image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    theme_color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    display_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_categories", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_listings_service_category_id",
                table: "listings",
                column: "service_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_categories_code",
                table: "service_categories",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_service_categories_display_order",
                table: "service_categories",
                column: "display_order");

            migrationBuilder.CreateIndex(
                name: "IX_service_categories_is_active",
                table: "service_categories",
                column: "is_active");

            migrationBuilder.AddForeignKey(
                name: "FK_listings_service_categories_service_category_id",
                table: "listings",
                column: "service_category_id",
                principalTable: "service_categories",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            // Seed Service Categories with complete details
           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_listings_service_categories_service_category_id",
                table: "listings");

            migrationBuilder.DropTable(
                name: "service_categories");

            migrationBuilder.DropIndex(
                name: "IX_listings_service_category_id",
                table: "listings");

            migrationBuilder.DropColumn(
                name: "service_category_id",
                table: "listings");
        }
    }
}

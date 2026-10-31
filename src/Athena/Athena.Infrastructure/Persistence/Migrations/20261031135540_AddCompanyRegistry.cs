using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Athena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyRegistry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    registration_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_groups", x => x.id);
                    table.UniqueConstraint("ak_groups_id_group_id", x => new { x.id, x.group_id });
                });

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    legal_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    country_code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    tax_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    vat_number = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    registration_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    legal_form = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    pec = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    website = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    registered_address_street = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    registered_address_city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    registered_address_postal_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    registered_address_region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    registered_address_country_code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    registration_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    group_database_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_companies", x => x.id);
                    table.UniqueConstraint("ak_companies_id_company_id", x => new { x.id, x.company_id });
                    table.ForeignKey(
                        name: "FK_companies_groups_group_database_id_group_id",
                        columns: x => new { x.group_database_id, x.group_id },
                        principalTable: "groups",
                        principalColumns: new[] { "id", "group_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_companies_group_database_id_group_id",
                table: "companies",
                columns: new[] { "group_database_id", "group_id" });

            migrationBuilder.CreateIndex(
                name: "ux_companies_company_id",
                table: "companies",
                column: "company_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_companies_country_tax_code",
                table: "companies",
                columns: new[] { "country_code", "tax_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_companies_country_vat_number",
                table: "companies",
                columns: new[] { "country_code", "vat_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_companies_name",
                table: "companies",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_groups_group_id",
                table: "groups",
                column: "group_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_groups_name",
                table: "groups",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "groups");
        }
    }
}

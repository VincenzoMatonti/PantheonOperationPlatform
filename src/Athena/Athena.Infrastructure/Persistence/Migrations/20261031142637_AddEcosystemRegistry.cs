using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Athena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEcosystemRegistry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ecosystems",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ecosystem_id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    registration_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    company_database_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ecosystems", x => x.id);
                    table.UniqueConstraint("ak_ecosystems_id_ecosystem_id", x => new { x.id, x.ecosystem_id });
                    table.ForeignKey(
                        name: "FK_ecosystems_companies_company_database_id_company_id",
                        columns: x => new { x.company_database_id, x.company_id },
                        principalTable: "companies",
                        principalColumns: new[] { "id", "company_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_ecosystems_company_database_id_company_id",
                table: "ecosystems",
                columns: new[] { "company_database_id", "company_id" });

            migrationBuilder.CreateIndex(
                name: "ux_ecosystems_ecosystem_id",
                table: "ecosystems",
                column: "ecosystem_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_ecosystems_name",
                table: "ecosystems",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ecosystems");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Athena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialAthenaSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "systems",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    system_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    registration_status = table.Column<int>(type: "integer", nullable: false),
                    availability_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_systems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "system_capabilities",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    system_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_capabilities", x => x.id);
                    table.ForeignKey(
                        name: "FK_system_capabilities_systems_system_id",
                        column: x => x.system_id,
                        principalTable: "systems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "system_endpoints",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    base_address = table.Column<string>(type: "text", nullable: false),
                    system_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_endpoints", x => x.id);
                    table.ForeignKey(
                        name: "FK_system_endpoints_systems_system_id",
                        column: x => x.system_id,
                        principalTable: "systems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "system_lifecycle_operations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    started_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error_message = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    system_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_lifecycle_operations", x => x.id);
                    table.ForeignKey(
                        name: "FK_system_lifecycle_operations_systems_system_id",
                        column: x => x.system_id,
                        principalTable: "systems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "system_lifecycles",
                columns: table => new
                {
                    system_id = table.Column<int>(type: "integer", nullable: false),
                    desired_state = table.Column<int>(type: "integer", nullable: false),
                    operational_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_lifecycles", x => x.system_id);
                    table.ForeignKey(
                        name: "FK_system_lifecycles_systems_system_id",
                        column: x => x.system_id,
                        principalTable: "systems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_system_capabilities_system_id_name",
                table: "system_capabilities",
                columns: new[] { "system_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_system_endpoints_system_id_name",
                table: "system_endpoints",
                columns: new[] { "system_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_system_lifecycle_operations_system_created",
                table: "system_lifecycle_operations",
                columns: new[] { "system_id", "created_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_systems_name",
                table: "systems",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_systems_system_id",
                table: "systems",
                column: "system_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "system_capabilities");

            migrationBuilder.DropTable(
                name: "system_endpoints");

            migrationBuilder.DropTable(
                name: "system_lifecycle_operations");

            migrationBuilder.DropTable(
                name: "system_lifecycles");

            migrationBuilder.DropTable(
                name: "systems");
        }
    }
}

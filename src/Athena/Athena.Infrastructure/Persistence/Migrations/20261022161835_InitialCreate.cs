using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Athena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "orchestrations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    orchestration_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    registration_status = table.Column<int>(type: "integer", nullable: false),
                    availability_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orchestrations", x => x.id);
                    table.UniqueConstraint("ak_orchestrations_id_orchestration_id", x => new { x.id, x.orchestration_id });
                });

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
                    table.PrimaryKey("pk_systems", x => x.id);
                    table.UniqueConstraint("ak_systems_id_system_id", x => new { x.id, x.system_id });
                });

            migrationBuilder.CreateTable(
                name: "orchestration_lifecycle_operations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    orchestration_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    started_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error_message = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    orchestration_database_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orchestration_lifecycle_operations", x => x.id);
                    table.ForeignKey(
                        name: "FK_orchestration_lifecycle_operations_orchestrations_orchestra~",
                        columns: x => new { x.orchestration_database_id, x.orchestration_id },
                        principalTable: "orchestrations",
                        principalColumns: new[] { "id", "orchestration_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "orchestration_lifecycles",
                columns: table => new
                {
                    orchestration_database_id = table.Column<int>(type: "integer", nullable: false),
                    orchestration_id = table.Column<Guid>(type: "uuid", nullable: false),
                    desired_state = table.Column<int>(type: "integer", nullable: false),
                    operational_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orchestration_lifecycles", x => x.orchestration_database_id);
                    table.ForeignKey(
                        name: "FK_orchestration_lifecycles_orchestrations_orchestration_datab~",
                        columns: x => new { x.orchestration_database_id, x.orchestration_id },
                        principalTable: "orchestrations",
                        principalColumns: new[] { "id", "orchestration_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "orchestration_systems",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    association_id = table.Column<Guid>(type: "uuid", nullable: false),
                    orchestration_id = table.Column<Guid>(type: "uuid", nullable: false),
                    system_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    orchestration_database_id = table.Column<int>(type: "integer", nullable: false),
                    system_database_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orchestration_systems", x => x.id);
                    table.ForeignKey(
                        name: "FK_orchestration_systems_orchestrations_orchestration_database~",
                        columns: x => new { x.orchestration_database_id, x.orchestration_id },
                        principalTable: "orchestrations",
                        principalColumns: new[] { "id", "orchestration_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orchestration_systems_systems_system_database_id_system_id",
                        columns: x => new { x.system_database_id, x.system_id },
                        principalTable: "systems",
                        principalColumns: new[] { "id", "system_id" },
                        onDelete: ReferentialAction.Restrict);
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
                name: "system_lifecycles",
                columns: table => new
                {
                    system_database_id = table.Column<int>(type: "integer", nullable: false),
                    system_id = table.Column<Guid>(type: "uuid", nullable: false),
                    desired_state = table.Column<int>(type: "integer", nullable: false),
                    operational_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_system_lifecycles", x => x.system_database_id);
                    table.ForeignKey(
                        name: "FK_system_lifecycles_systems_system_database_id_system_id",
                        columns: x => new { x.system_database_id, x.system_id },
                        principalTable: "systems",
                        principalColumns: new[] { "id", "system_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "system_lifecycle_operations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    system_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    orchestration_lifecycle_operation_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    started_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error_message = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    system_database_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_system_lifecycle_operations", x => x.id);
                    table.ForeignKey(
                        name: "FK_system_lifecycle_operations_orchestration_lifecycle_operati~",
                        column: x => x.orchestration_lifecycle_operation_id,
                        principalTable: "orchestration_lifecycle_operations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_system_lifecycle_operations_systems_system_database_id_syst~",
                        columns: x => new { x.system_database_id, x.system_id },
                        principalTable: "systems",
                        principalColumns: new[] { "id", "system_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_orchestration_lifecycle_operations_orchestration_created",
                table: "orchestration_lifecycle_operations",
                columns: new[] { "orchestration_database_id", "created_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_orchestration_lifecycle_operations_orchestration_database_i~",
                table: "orchestration_lifecycle_operations",
                columns: new[] { "orchestration_database_id", "orchestration_id" });

            migrationBuilder.CreateIndex(
                name: "IX_orchestration_lifecycles_orchestration_database_id_orchestr~",
                table: "orchestration_lifecycles",
                columns: new[] { "orchestration_database_id", "orchestration_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_orchestration_systems_database_pair",
                table: "orchestration_systems",
                columns: new[] { "orchestration_database_id", "system_database_id" });

            migrationBuilder.CreateIndex(
                name: "ix_orchestration_systems_guid_pair",
                table: "orchestration_systems",
                columns: new[] { "orchestration_id", "system_id" });

            migrationBuilder.CreateIndex(
                name: "IX_orchestration_systems_orchestration_database_id_orchestrati~",
                table: "orchestration_systems",
                columns: new[] { "orchestration_database_id", "orchestration_id" });

            migrationBuilder.CreateIndex(
                name: "IX_orchestration_systems_system_database_id_system_id",
                table: "orchestration_systems",
                columns: new[] { "system_database_id", "system_id" });

            migrationBuilder.CreateIndex(
                name: "ux_orchestration_systems_association_id",
                table: "orchestration_systems",
                column: "association_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_orchestrations_name",
                table: "orchestrations",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_orchestrations_orchestration_id",
                table: "orchestrations",
                column: "orchestration_id",
                unique: true);

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
                name: "ix_system_lifecycle_operations_parent_operation",
                table: "system_lifecycle_operations",
                column: "orchestration_lifecycle_operation_id");

            migrationBuilder.CreateIndex(
                name: "ix_system_lifecycle_operations_system_created",
                table: "system_lifecycle_operations",
                columns: new[] { "system_database_id", "created_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_system_lifecycle_operations_system_database_id_system_id",
                table: "system_lifecycle_operations",
                columns: new[] { "system_database_id", "system_id" });

            migrationBuilder.CreateIndex(
                name: "IX_system_lifecycles_system_database_id_system_id",
                table: "system_lifecycles",
                columns: new[] { "system_database_id", "system_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_systems_name",
                table: "systems",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_systems_system_id",
                table: "systems",
                column: "system_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orchestration_lifecycles");

            migrationBuilder.DropTable(
                name: "orchestration_systems");

            migrationBuilder.DropTable(
                name: "system_capabilities");

            migrationBuilder.DropTable(
                name: "system_endpoints");

            migrationBuilder.DropTable(
                name: "system_lifecycle_operations");

            migrationBuilder.DropTable(
                name: "system_lifecycles");

            migrationBuilder.DropTable(
                name: "orchestration_lifecycle_operations");

            migrationBuilder.DropTable(
                name: "systems");

            migrationBuilder.DropTable(
                name: "orchestrations");
        }
    }
}

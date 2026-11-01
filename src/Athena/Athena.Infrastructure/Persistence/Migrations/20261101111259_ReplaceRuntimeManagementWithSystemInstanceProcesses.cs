using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Athena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceRuntimeManagementWithSystemInstanceProcesses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "orchestration_systems",
                newName: "orchestration_systems_legacy_archive");

            migrationBuilder.RenameTable(
                name: "system_lifecycle_operations",
                newName: "system_lifecycle_operations_legacy_archive");

            migrationBuilder.RenameTable(
                name: "system_lifecycles",
                newName: "system_lifecycles_legacy_archive");

            migrationBuilder.RenameTable(
                name: "system_runtime_instances",
                newName: "system_runtime_instances_legacy_archive");

            migrationBuilder.RenameColumn(
                name: "availability_status",
                table: "system_instances",
                newName: "legacy_availability_status");

            migrationBuilder.AddColumn<int>(
                name: "desired_state",
                table: "system_instances",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "operational_status",
                table: "system_instances",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "orchestration_system_instances",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    association_id = table.Column<Guid>(type: "uuid", nullable: false),
                    orchestration_id = table.Column<Guid>(type: "uuid", nullable: false),
                    system_instance_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    orchestration_database_id = table.Column<int>(type: "integer", nullable: false),
                    system_instance_database_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orchestration_system_instances", x => x.id);
                    table.ForeignKey(
                        name: "FK_orchestration_system_instances_orchestrations_orchestration~",
                        columns: x => new { x.orchestration_database_id, x.orchestration_id },
                        principalTable: "orchestrations",
                        principalColumns: new[] { "id", "orchestration_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orchestration_system_instances_system_instances_system_inst~",
                        columns: x => new { x.system_instance_database_id, x.system_instance_id },
                        principalTable: "system_instances",
                        principalColumns: new[] { "id", "system_instance_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "system_instance_operations",
                columns: table => new
                {
                    operation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    system_instance_id = table.Column<Guid>(type: "uuid", nullable: false),
                    operation_type = table.Column<int>(type: "integer", nullable: false),
                    operation_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    started_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error_message = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    system_instance_database_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_system_instance_operations", x => x.operation_id);
                    table.ForeignKey(
                        name: "FK_system_instance_operations_system_instances_system_instance~",
                        columns: x => new { x.system_instance_database_id, x.system_instance_id },
                        principalTable: "system_instances",
                        principalColumns: new[] { "id", "system_instance_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_orchestration_system_instances_database_pair",
                table: "orchestration_system_instances",
                columns: new[] { "orchestration_database_id", "system_instance_database_id" });

            migrationBuilder.CreateIndex(
                name: "ix_orchestration_system_instances_guid_pair",
                table: "orchestration_system_instances",
                columns: new[] { "orchestration_id", "system_instance_id" });

            migrationBuilder.CreateIndex(
                name: "IX_orchestration_system_instances_orchestration_database_id_or~",
                table: "orchestration_system_instances",
                columns: new[] { "orchestration_database_id", "orchestration_id" });

            migrationBuilder.CreateIndex(
                name: "IX_orchestration_system_instances_system_instance_database_id_~",
                table: "orchestration_system_instances",
                columns: new[] { "system_instance_database_id", "system_instance_id" });

            migrationBuilder.CreateIndex(
                name: "ux_orchestration_system_instances_association_id",
                table: "orchestration_system_instances",
                column: "association_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_system_instance_operations_instance_created",
                table: "system_instance_operations",
                columns: new[] { "system_instance_database_id", "system_instance_id", "created_at_utc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orchestration_system_instances");

            migrationBuilder.DropTable(
                name: "system_instance_operations");

            migrationBuilder.DropColumn(
                name: "desired_state",
                table: "system_instances");

            migrationBuilder.DropColumn(
                name: "operational_status",
                table: "system_instances");

            migrationBuilder.RenameColumn(
                name: "legacy_availability_status",
                table: "system_instances",
                newName: "availability_status");

            migrationBuilder.RenameTable(
                name: "system_runtime_instances_legacy_archive",
                newName: "system_runtime_instances");

            migrationBuilder.RenameTable(
                name: "system_lifecycles_legacy_archive",
                newName: "system_lifecycles");

            migrationBuilder.RenameTable(
                name: "system_lifecycle_operations_legacy_archive",
                newName: "system_lifecycle_operations");

            migrationBuilder.RenameTable(
                name: "orchestration_systems_legacy_archive",
                newName: "orchestration_systems");
        }
    }
}

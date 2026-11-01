using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddResourceManagementLifecycles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "block_lifecycles",
                columns: table => new
                {
                    block_database_id = table.Column<int>(type: "integer", nullable: false),
                    block_id = table.Column<Guid>(type: "uuid", nullable: false),
                    desired_state = table.Column<int>(type: "integer", nullable: false),
                    operational_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_block_lifecycles", x => x.block_database_id);
                    table.ForeignKey(
                        name: "FK_block_lifecycles_blocks_block_database_id_block_id",
                        columns: x => new { x.block_database_id, x.block_id },
                        principalTable: "blocks",
                        principalColumns: new[] { "id", "block_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "block_operations",
                columns: table => new
                {
                    operation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    block_id = table.Column<Guid>(type: "uuid", nullable: false),
                    operation_type = table.Column<int>(type: "integer", nullable: false),
                    operation_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    started_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error_message = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    block_database_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_block_operations", x => x.operation_id);
                    table.ForeignKey(
                        name: "FK_block_operations_blocks_block_database_id_block_id",
                        columns: x => new { x.block_database_id, x.block_id },
                        principalTable: "blocks",
                        principalColumns: new[] { "id", "block_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ecosystem_lifecycles",
                columns: table => new
                {
                    ecosystem_database_id = table.Column<int>(type: "integer", nullable: false),
                    ecosystem_id = table.Column<Guid>(type: "uuid", nullable: false),
                    desired_state = table.Column<int>(type: "integer", nullable: false),
                    operational_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ecosystem_lifecycles", x => x.ecosystem_database_id);
                    table.ForeignKey(
                        name: "FK_ecosystem_lifecycles_ecosystems_ecosystem_database_id_ecosy~",
                        columns: x => new { x.ecosystem_database_id, x.ecosystem_id },
                        principalTable: "ecosystems",
                        principalColumns: new[] { "id", "ecosystem_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ecosystem_operations",
                columns: table => new
                {
                    operation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ecosystem_id = table.Column<Guid>(type: "uuid", nullable: false),
                    operation_type = table.Column<int>(type: "integer", nullable: false),
                    operation_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    started_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error_message = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    ecosystem_database_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ecosystem_operations", x => x.operation_id);
                    table.ForeignKey(
                        name: "FK_ecosystem_operations_ecosystems_ecosystem_database_id_ecosy~",
                        columns: x => new { x.ecosystem_database_id, x.ecosystem_id },
                        principalTable: "ecosystems",
                        principalColumns: new[] { "id", "ecosystem_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "system_instance_lifecycles",
                columns: table => new
                {
                    system_instance_database_id = table.Column<int>(type: "integer", nullable: false),
                    system_instance_id = table.Column<Guid>(type: "uuid", nullable: false),
                    desired_state = table.Column<int>(type: "integer", nullable: false),
                    operational_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_system_instance_lifecycles", x => x.system_instance_database_id);
                    table.ForeignKey(
                        name: "FK_system_instance_lifecycles_system_instances_system_instance~",
                        columns: x => new { x.system_instance_database_id, x.system_instance_id },
                        principalTable: "system_instances",
                        principalColumns: new[] { "id", "system_instance_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "workload_lifecycles",
                columns: table => new
                {
                    workload_database_id = table.Column<int>(type: "integer", nullable: false),
                    workload_id = table.Column<Guid>(type: "uuid", nullable: false),
                    desired_state = table.Column<int>(type: "integer", nullable: false),
                    operational_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workload_lifecycles", x => x.workload_database_id);
                    table.ForeignKey(
                        name: "FK_workload_lifecycles_workloads_workload_database_id_workload~",
                        columns: x => new { x.workload_database_id, x.workload_id },
                        principalTable: "workloads",
                        principalColumns: new[] { "id", "workload_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "workload_operations",
                columns: table => new
                {
                    operation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    workload_id = table.Column<Guid>(type: "uuid", nullable: false),
                    operation_type = table.Column<int>(type: "integer", nullable: false),
                    operation_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    started_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error_message = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    workload_database_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workload_operations", x => x.operation_id);
                    table.ForeignKey(
                        name: "FK_workload_operations_workloads_workload_database_id_workload~",
                        columns: x => new { x.workload_database_id, x.workload_id },
                        principalTable: "workloads",
                        principalColumns: new[] { "id", "workload_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.Sql("""
                INSERT INTO system_instance_lifecycles
                    (system_instance_database_id, system_instance_id, desired_state, operational_status, created_at_utc, updated_at_utc, is_deleted)
                SELECT id, system_instance_id, desired_state, operational_status, created_at_utc, updated_at_utc, is_deleted
                FROM system_instances;

                INSERT INTO workload_lifecycles
                    (workload_database_id, workload_id, desired_state, operational_status, created_at_utc, updated_at_utc, is_deleted)
                SELECT id, workload_id, 0, 0, created_at_utc, updated_at_utc, is_deleted
                FROM workloads;

                INSERT INTO block_lifecycles
                    (block_database_id, block_id, desired_state, operational_status, created_at_utc, updated_at_utc, is_deleted)
                SELECT id, block_id, 0, 0, created_at_utc, updated_at_utc, is_deleted
                FROM blocks;

                INSERT INTO ecosystem_lifecycles
                    (ecosystem_database_id, ecosystem_id, desired_state, operational_status, created_at_utc, updated_at_utc, is_deleted)
                SELECT id, ecosystem_id, 0, 0, created_at_utc, updated_at_utc, is_deleted
                FROM ecosystems;
                """);

            migrationBuilder.DropColumn(
                name: "desired_state",
                table: "system_instances");

            migrationBuilder.DropColumn(
                name: "operational_status",
                table: "system_instances");

            migrationBuilder.CreateIndex(
                name: "IX_block_lifecycles_block_database_id_block_id",
                table: "block_lifecycles",
                columns: new[] { "block_database_id", "block_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_block_operations_block_created",
                table: "block_operations",
                columns: new[] { "block_database_id", "created_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_block_operations_block_database_id_block_id",
                table: "block_operations",
                columns: new[] { "block_database_id", "block_id" });

            migrationBuilder.CreateIndex(
                name: "IX_ecosystem_lifecycles_ecosystem_database_id_ecosystem_id",
                table: "ecosystem_lifecycles",
                columns: new[] { "ecosystem_database_id", "ecosystem_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_ecosystem_operations_ecosystem_created",
                table: "ecosystem_operations",
                columns: new[] { "ecosystem_database_id", "created_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_ecosystem_operations_ecosystem_database_id_ecosystem_id",
                table: "ecosystem_operations",
                columns: new[] { "ecosystem_database_id", "ecosystem_id" });

            migrationBuilder.CreateIndex(
                name: "IX_system_instance_lifecycles_system_instance_database_id_syst~",
                table: "system_instance_lifecycles",
                columns: new[] { "system_instance_database_id", "system_instance_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workload_lifecycles_workload_database_id_workload_id",
                table: "workload_lifecycles",
                columns: new[] { "workload_database_id", "workload_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_workload_operations_workload_created",
                table: "workload_operations",
                columns: new[] { "workload_database_id", "created_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_workload_operations_workload_database_id_workload_id",
                table: "workload_operations",
                columns: new[] { "workload_database_id", "workload_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.Sql("""
                UPDATE system_instances AS instance
                SET desired_state = lifecycle.desired_state,
                    operational_status = lifecycle.operational_status
                FROM system_instance_lifecycles AS lifecycle
                WHERE lifecycle.system_instance_database_id = instance.id;
                """);

            migrationBuilder.DropTable(
                name: "block_lifecycles");

            migrationBuilder.DropTable(
                name: "block_operations");

            migrationBuilder.DropTable(
                name: "ecosystem_lifecycles");

            migrationBuilder.DropTable(
                name: "ecosystem_operations");

            migrationBuilder.DropTable(
                name: "workload_lifecycles");

            migrationBuilder.DropTable(
                name: "workload_operations");

            migrationBuilder.DropTable(
                name: "system_instance_lifecycles");
        }
    }
}

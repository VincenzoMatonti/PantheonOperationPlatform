using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Phase3ReliableOperations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "agent_accepted_at_utc",
                table: "system_instance_operations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "agent_completed_at_utc",
                table: "system_instance_operations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "attempt_count",
                table: "system_instance_operations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "command_dispatched_at_utc",
                table: "system_instance_operations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_attempt_at_utc",
                table: "system_instance_operations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "lease_expires_at_utc",
                table: "system_instance_operations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lease_owner",
                table: "system_instance_operations",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "next_attempt_at_utc",
                table: "system_instance_operations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "version",
                table: "system_instance_operations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "observed_at_utc",
                table: "system_instance_lifecycles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.Sql("""
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM system_instance_operations
                        WHERE operation_status IN (0, 1, 4)
                        GROUP BY system_instance_database_id
                        HAVING COUNT(*) > 1
                    ) THEN
                        RAISE EXCEPTION 'Cannot create the active SystemInstance Operation constraint: duplicate active Operations must be reviewed and resolved without deleting history.';
                    END IF;
                END $$;
                """);

            migrationBuilder.CreateIndex(
                name: "ux_system_instance_operations_one_active",
                table: "system_instance_operations",
                column: "system_instance_database_id",
                unique: true,
                filter: "\"operation_status\" IN (0, 1, 4)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ux_system_instance_operations_one_active",
                table: "system_instance_operations");

            migrationBuilder.DropColumn(
                name: "agent_accepted_at_utc",
                table: "system_instance_operations");

            migrationBuilder.DropColumn(
                name: "agent_completed_at_utc",
                table: "system_instance_operations");

            migrationBuilder.DropColumn(
                name: "attempt_count",
                table: "system_instance_operations");

            migrationBuilder.DropColumn(
                name: "command_dispatched_at_utc",
                table: "system_instance_operations");

            migrationBuilder.DropColumn(
                name: "last_attempt_at_utc",
                table: "system_instance_operations");

            migrationBuilder.DropColumn(
                name: "lease_expires_at_utc",
                table: "system_instance_operations");

            migrationBuilder.DropColumn(
                name: "lease_owner",
                table: "system_instance_operations");

            migrationBuilder.DropColumn(
                name: "next_attempt_at_utc",
                table: "system_instance_operations");

            migrationBuilder.DropColumn(
                name: "version",
                table: "system_instance_operations");

            migrationBuilder.DropColumn(
                name: "observed_at_utc",
                table: "system_instance_lifecycles");
        }
    }
}

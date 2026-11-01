using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Athena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemInstanceRegistry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "system_instances",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    system_instance_id = table.Column<Guid>(type: "uuid", nullable: false),
                    system_id = table.Column<Guid>(type: "uuid", nullable: false),
                    workload_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    registration_status = table.Column<int>(type: "integer", nullable: false),
                    availability_status = table.Column<int>(type: "integer", nullable: false, defaultValue: 3),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    system_database_id = table.Column<int>(type: "integer", nullable: false),
                    workload_database_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_system_instances", x => x.id);
                    table.UniqueConstraint("ak_system_instances_id_system_instance_id", x => new { x.id, x.system_instance_id });
                    table.ForeignKey(
                        name: "FK_system_instances_systems_system_database_id_system_id",
                        columns: x => new { x.system_database_id, x.system_id },
                        principalTable: "systems",
                        principalColumns: new[] { "id", "system_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_system_instances_workloads_workload_database_id_workload_id",
                        columns: x => new { x.workload_database_id, x.workload_id },
                        principalTable: "workloads",
                        principalColumns: new[] { "id", "workload_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_system_instances_system_database_id_system_id",
                table: "system_instances",
                columns: new[] { "system_database_id", "system_id" });

            migrationBuilder.CreateIndex(
                name: "ix_system_instances_workload_database_id_workload_id",
                table: "system_instances",
                columns: new[] { "workload_database_id", "workload_id" });

            migrationBuilder.CreateIndex(
                name: "ux_system_instances_system_instance_id",
                table: "system_instances",
                column: "system_instance_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_system_instances_workload_name",
                table: "system_instances",
                columns: new[] { "workload_database_id", "workload_id", "name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "system_instances");
        }
    }
}

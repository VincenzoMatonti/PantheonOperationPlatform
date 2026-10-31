using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemRuntimeInstances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "system_runtime_instances",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    system_id = table.Column<Guid>(type: "uuid", nullable: false),
                    runtime_provider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    runtime_resource_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    runtime_resource_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    environment = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    system_database_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_system_runtime_instances", x => x.id);
                    table.ForeignKey(
                        name: "FK_system_runtime_instances_systems_system_database_id_system_~",
                        columns: x => new { x.system_database_id, x.system_id },
                        principalTable: "systems",
                        principalColumns: new[] { "id", "system_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_system_runtime_instances_system_database_id_system_id",
                table: "system_runtime_instances",
                columns: new[] { "system_database_id", "system_id" });

            migrationBuilder.CreateIndex(
                name: "ix_system_runtime_instances_system_environment",
                table: "system_runtime_instances",
                columns: new[] { "system_database_id", "environment" });

            migrationBuilder.CreateIndex(
                name: "ux_system_runtime_instances_resource",
                table: "system_runtime_instances",
                columns: new[] { "runtime_provider", "environment", "runtime_resource_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "system_runtime_instances");
        }
    }
}

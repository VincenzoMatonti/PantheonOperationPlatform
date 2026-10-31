using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Athena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkloadRegistry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "workloads",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    workload_id = table.Column<Guid>(type: "uuid", nullable: false),
                    block_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    registration_status = table.Column<int>(type: "integer", nullable: false),
                    availability_status = table.Column<int>(type: "integer", nullable: false, defaultValue: 3),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    block_database_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workloads", x => x.id);
                    table.UniqueConstraint("ak_workloads_id_workload_id", x => new { x.id, x.workload_id });
                    table.ForeignKey(
                        name: "FK_workloads_blocks_block_database_id_block_id",
                        columns: x => new { x.block_database_id, x.block_id },
                        principalTable: "blocks",
                        principalColumns: new[] { "id", "block_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_workloads_block_database_id_block_id",
                table: "workloads",
                columns: new[] { "block_database_id", "block_id" });

            migrationBuilder.CreateIndex(
                name: "ux_workloads_block_name",
                table: "workloads",
                columns: new[] { "block_database_id", "block_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_workloads_workload_id",
                table: "workloads",
                column: "workload_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "workloads");
        }
    }
}

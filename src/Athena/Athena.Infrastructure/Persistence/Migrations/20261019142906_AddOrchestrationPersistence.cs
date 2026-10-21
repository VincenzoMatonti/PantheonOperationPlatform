using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Athena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrchestrationPersistence : Migration
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
                    table.PrimaryKey("PK_orchestrations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orchestration_lifecycle_operations",
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
                    orchestration_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orchestration_lifecycle_operations", x => x.id);
                    table.ForeignKey(
                        name: "FK_orchestration_lifecycle_operations_orchestrations_orchestra~",
                        column: x => x.orchestration_id,
                        principalTable: "orchestrations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "orchestration_lifecycles",
                columns: table => new
                {
                    orchestration_id = table.Column<int>(type: "integer", nullable: false),
                    desired_state = table.Column<int>(type: "integer", nullable: false),
                    operational_status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orchestration_lifecycles", x => x.orchestration_id);
                    table.ForeignKey(
                        name: "FK_orchestration_lifecycles_orchestrations_orchestration_id",
                        column: x => x.orchestration_id,
                        principalTable: "orchestrations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_orchestration_lifecycle_operations_orchestration_created",
                table: "orchestration_lifecycle_operations",
                columns: new[] { "orchestration_id", "created_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_orchestrations_name",
                table: "orchestrations",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orchestrations_orchestration_id",
                table: "orchestrations",
                column: "orchestration_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orchestration_lifecycle_operations");

            migrationBuilder.DropTable(
                name: "orchestration_lifecycles");

            migrationBuilder.DropTable(
                name: "orchestrations");
        }
    }
}

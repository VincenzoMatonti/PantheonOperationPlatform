using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Athena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBlockRegistry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "blocks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    block_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ecosystem_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    registration_status = table.Column<int>(type: "integer", nullable: false),
                    availability_status = table.Column<int>(type: "integer", nullable: false, defaultValue: 3),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    ecosystem_database_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_blocks", x => x.id);
                    table.UniqueConstraint("ak_blocks_id_block_id", x => new { x.id, x.block_id });
                    table.ForeignKey(
                        name: "FK_blocks_ecosystems_ecosystem_database_id_ecosystem_id",
                        columns: x => new { x.ecosystem_database_id, x.ecosystem_id },
                        principalTable: "ecosystems",
                        principalColumns: new[] { "id", "ecosystem_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_blocks_ecosystem_database_id_ecosystem_id",
                table: "blocks",
                columns: new[] { "ecosystem_database_id", "ecosystem_id" });

            migrationBuilder.CreateIndex(
                name: "ux_blocks_block_id",
                table: "blocks",
                column: "block_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_blocks_name",
                table: "blocks",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blocks");
        }
    }
}

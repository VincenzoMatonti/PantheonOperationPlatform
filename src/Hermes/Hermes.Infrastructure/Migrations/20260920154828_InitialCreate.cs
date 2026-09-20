using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hermes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "endpoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_endpoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "execution_types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_execution_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "executions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_executions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operation_types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operation_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "execution_steps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExecutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_execution_steps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_execution_steps_executions_ExecutionId",
                        column: x => x.ExecutionId,
                        principalTable: "executions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExecutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_operations_executions_ExecutionId",
                        column: x => x.ExecutionId,
                        principalTable: "executions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "client_operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_client_operations_clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_client_operations_operation_types_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "operation_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "endpoint_operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EndpointId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_endpoint_operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_endpoint_operations_endpoints_EndpointId",
                        column: x => x.EndpointId,
                        principalTable: "endpoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_endpoint_operations_operation_types_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "operation_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EndpointId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_routes_clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_routes_endpoints_EndpointId",
                        column: x => x.EndpointId,
                        principalTable: "endpoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_routes_operation_types_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "operation_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "execution_step_types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExecutionStepId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExecutionTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_execution_step_types", x => x.Id);
                    table.ForeignKey(
                        name: "FK_execution_step_types_execution_steps_ExecutionStepId",
                        column: x => x.ExecutionStepId,
                        principalTable: "execution_steps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_execution_step_types_execution_types_ExecutionTypeId",
                        column: x => x.ExecutionTypeId,
                        principalTable: "execution_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "packages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_packages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_packages_operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "execution_step_routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExecutionStepId = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_execution_step_routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_execution_step_routes_execution_steps_ExecutionStepId",
                        column: x => x.ExecutionStepId,
                        principalTable: "execution_steps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_execution_step_routes_routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<byte[]>(type: "bytea", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_data_packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "headers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_headers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_headers_packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "metadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataId = table.Column<Guid>(type: "uuid", nullable: true),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_metadata_data_DataId",
                        column: x => x.DataId,
                        principalTable: "data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_metadata_packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_client_operations_ClientId_OperationTypeId",
                table: "client_operations",
                columns: new[] { "ClientId", "OperationTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_client_operations_OperationTypeId",
                table: "client_operations",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_clients_Code",
                table: "clients",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_data_PackageId",
                table: "data",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_data_PackageId_Sequence",
                table: "data",
                columns: new[] { "PackageId", "Sequence" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_endpoint_operations_EndpointId_OperationTypeId",
                table: "endpoint_operations",
                columns: new[] { "EndpointId", "OperationTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_endpoint_operations_OperationTypeId",
                table: "endpoint_operations",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_endpoints_Code",
                table: "endpoints",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_execution_step_routes_ExecutionStepId",
                table: "execution_step_routes",
                column: "ExecutionStepId");

            migrationBuilder.CreateIndex(
                name: "IX_execution_step_routes_ExecutionStepId_RouteId",
                table: "execution_step_routes",
                columns: new[] { "ExecutionStepId", "RouteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_execution_step_routes_RouteId",
                table: "execution_step_routes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_execution_step_types_ExecutionStepId",
                table: "execution_step_types",
                column: "ExecutionStepId");

            migrationBuilder.CreateIndex(
                name: "IX_execution_step_types_ExecutionStepId_ExecutionTypeId",
                table: "execution_step_types",
                columns: new[] { "ExecutionStepId", "ExecutionTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_execution_step_types_ExecutionTypeId",
                table: "execution_step_types",
                column: "ExecutionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_execution_steps_ExecutionId",
                table: "execution_steps",
                column: "ExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_execution_steps_ExecutionId_Sequence",
                table: "execution_steps",
                columns: new[] { "ExecutionId", "Sequence" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_execution_steps_Status",
                table: "execution_steps",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_execution_types_Code",
                table: "execution_types",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_executions_Status",
                table: "executions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_headers_PackageId",
                table: "headers",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_headers_PackageId_Key",
                table: "headers",
                columns: new[] { "PackageId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_metadata_DataId",
                table: "metadata",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_metadata_DataId_Key",
                table: "metadata",
                columns: new[] { "DataId", "Key" },
                unique: true,
                filter: "\"DataId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_metadata_PackageId",
                table: "metadata",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_metadata_PackageId_Key",
                table: "metadata",
                columns: new[] { "PackageId", "Key" },
                unique: true,
                filter: "\"DataId\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_operation_types_Code",
                table: "operation_types",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operations_CorrelationId",
                table: "operations",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_operations_ExecutionId",
                table: "operations",
                column: "ExecutionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operations_ExternalId",
                table: "operations",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operations_Status",
                table: "operations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_packages_OperationId",
                table: "packages",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_packages_OperationId_Sequence",
                table: "packages",
                columns: new[] { "OperationId", "Sequence" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_packages_Status",
                table: "packages",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_routes_ClientId_OperationTypeId_EndpointId",
                table: "routes",
                columns: new[] { "ClientId", "OperationTypeId", "EndpointId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_routes_EndpointId",
                table: "routes",
                column: "EndpointId");

            migrationBuilder.CreateIndex(
                name: "IX_routes_OperationTypeId",
                table: "routes",
                column: "OperationTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "client_operations");

            migrationBuilder.DropTable(
                name: "endpoint_operations");

            migrationBuilder.DropTable(
                name: "execution_step_routes");

            migrationBuilder.DropTable(
                name: "execution_step_types");

            migrationBuilder.DropTable(
                name: "headers");

            migrationBuilder.DropTable(
                name: "metadata");

            migrationBuilder.DropTable(
                name: "routes");

            migrationBuilder.DropTable(
                name: "execution_steps");

            migrationBuilder.DropTable(
                name: "execution_types");

            migrationBuilder.DropTable(
                name: "data");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "endpoints");

            migrationBuilder.DropTable(
                name: "operation_types");

            migrationBuilder.DropTable(
                name: "packages");

            migrationBuilder.DropTable(
                name: "operations");

            migrationBuilder.DropTable(
                name: "executions");
        }
    }
}

# Hermes.Infrastructure

> Hermes infrastructure layer: it implements PostgreSQL persistence and connects the domain model to technical details without contaminating the other layers.

## Table of contents

- [Overview](#overview)
- [Responsibilities](#responsibilities)
- [Structure](#structure)
- [PostgreSQL persistence](#postgresql-persistence)
- [DbContext and configurations](#dbcontext-and-configurations)
- [Repository](#repository)
- [Migration](#migration)
- [Local development](#local-development)
- [Principles](#principles)

---

## Overview

`Hermes.Infrastructure` is the technical boundary of Hermes. It implements the repository interfaces declared by `Hermes.Domain`, configures Entity Framework Core, and provides PostgreSQL access.

The layer does not define business rules and must not become the owner of use cases. Its role is to make the technical capabilities required by the application layer available.

## Responsibilities

- PostgreSQL access through EF Core and Npgsql;
- mapping of domain entities to the relational model;
- management of `HermesDbContext`;
- implementation of query and command repositories;
- schema versioning through migrations;
- tracking and saving entity state;
- configuration of the technical details required for local runtime.

## Structure

```text
Hermes.Infrastructure/
├── Migrations/                         # migrations and model snapshots
├── Persistence/
│   ├── Configurations/                 # Fluent API mapping by area
│   └── HermesDbContext.cs              # main DbContext
├── Repositories/
│   ├── Clients/
│   ├── Endpoints/
│   ├── Operations/
│   └── Routes/
└── Hermes.Infrastructure.csproj
```

## PostgreSQL persistence

The provider used is `Npgsql.EntityFrameworkCore.PostgreSQL`. Hermes receives the connection string through `ConnectionStrings__Hermes`:

```text
ConnectionStrings__Hermes=Host=postgres;Port=5432;Database=hermes;Username=<user>;Password=<password>
```

When the API runs on the Docker Compose network, `postgres` is the DNS name of the service and `5432` is the internal port. When the API runs directly on the host, use the PostgreSQL port published in `infra/local/.env`.

## DbContext and configurations

`HermesDbContext` exposes the `DbSet` objects for configuration, runtime, and payload areas. In `OnModelCreating`, it applies configurations from the assembly:

```csharp
modelBuilder.ApplyConfigurationsFromAssembly(
    typeof(HermesDbContext).Assembly);
```

Each configuration defines table names, keys, lengths, required fields, relationships, and conversions necessary to persist the domain objects correctly.

## Repository

Concrete repositories implement the domain interfaces and use `HermesDbContext` to read and modify entities. They are organized by area and separate query operations from command operations.

```text
Hermes.Domain repository abstraction
  → Hermes.Infrastructure repository implementation
  → HermesDbContext
  → PostgreSQL
```

This separation allows the application layer to use abstract repositories without knowing EF Core or SQL.

## Migration

Versioned migrations are located in `Migrations`. With PostgreSQL available and the connection string configured, the EF Core tools can be used as follows:

```bash
dotnet ef migrations list \
  --project src/Hermes/Hermes.Infrastructure/Hermes.Infrastructure.csproj \
  --startup-project src/Hermes/Hermes.Api/Hermes.Api.csproj

dotnet ef database update \
  --project src/Hermes/Hermes.Infrastructure/Hermes.Infrastructure.csproj \
  --startup-project src/Hermes/Hermes.Api/Hermes.Api.csproj
```

Migrations must be updated together with the persistent model. Do not modify an already applied migration manually: create a new migration representing the change.

## Local development

The full environment, including PostgreSQL, is started from the repository root with:

```bash
./scripts/pantheon.sh local up
```

To work from the Hermes Dev Container:

```bash
./scripts/pantheon.sh local db-up
./scripts/pantheon.sh local dev-up
```

Compose configuration and `.env.example` files are documented in [`infra/local/README.md`](../../../infra/local/README.md). The general Hermes README describes the relationship between this layer and the API, Application, and Domain layers: [`../README.md`](../README.md).

## Principles

1. **Infrastructure implements, it does not decide** — the rules remain in the domain and application layers.
2. **Dependencies on abstractions** — concrete repositories implement contracts defined internally.
3. **Centralized mapping** — EF Core configurations remain in `Persistence/Configurations`.
4. **Versioned schema** — every persistent change must be represented by a migration.
5. **Credentials out of source code** — use local `.env` files or a secret manager, never commit sensitive values.
6. **Database isolated** — PostgreSQL is a replaceable technical detail of the infrastructure layer.

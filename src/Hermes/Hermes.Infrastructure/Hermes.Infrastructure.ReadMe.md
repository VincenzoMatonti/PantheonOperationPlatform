# Hermes.Infrastructure

> Layer infrastrutturale di Hermes: implementa la persistenza PostgreSQL e collega il modello di dominio ai dettagli tecnici senza contaminare gli altri layer.

## Indice

- [Panoramica](#panoramica)
- [Responsabilità](#responsabilità)
- [Struttura](#struttura)
- [Persistenza PostgreSQL](#persistenza-postgresql)
- [DbContext e configurazioni](#dbcontext-e-configurazioni)
- [Repository](#repository)
- [Migration](#migration)
- [Sviluppo locale](#sviluppo-locale)
- [Principi](#principi)

## Panoramica

`Hermes.Infrastructure` è il boundary tecnico di Hermes. Implementa le interfacce repository dichiarate da `Hermes.Domain`, configura Entity Framework Core e fornisce l'accesso a PostgreSQL.

Il layer non definisce regole di business e non deve diventare il proprietario dei casi d'uso. Il suo compito è rendere disponibili le capacità tecniche richieste dall'application layer.

## Responsabilità

- accesso a PostgreSQL tramite EF Core e Npgsql;
- mapping delle entità del dominio nel modello relazionale;
- gestione di `HermesDbContext`;
- implementazione dei repository di query e command;
- versionamento dello schema tramite migration;
- gestione del tracking e del salvataggio delle entità;
- configurazione dei dettagli tecnici necessari al runtime locale.

## Struttura

```text
Hermes.Infrastructure/
├── Migrations/                         # migration e model snapshot
├── Persistence/
│   ├── Configurations/                 # mapping Fluent API per area
│   └── HermesDbContext.cs               # DbContext principale
├── Repositories/
│   ├── Clients/
│   ├── Endpoints/
│   ├── Operations/
│   └── Routes/
└── Hermes.Infrastructure.csproj
```

## Persistenza PostgreSQL

Il provider utilizzato è `Npgsql.EntityFrameworkCore.PostgreSQL`. Hermes riceve la connection string tramite `ConnectionStrings__Hermes`:

```text
ConnectionStrings__Hermes=Host=postgres;Port=5432;Database=hermes;Username=<user>;Password=<password>
```

Quando l'API gira nella rete Docker Compose, `postgres` è il nome DNS del servizio e `5432` è la porta interna. Quando l'API gira direttamente sull'host, usare la porta PostgreSQL pubblicata in `infra/local/.env`.

## DbContext e configurazioni

`HermesDbContext` espone i `DbSet` delle aree di configurazione, runtime e payload. In `OnModelCreating` applica le configurazioni dall'assembly:

```csharp
modelBuilder.ApplyConfigurationsFromAssembly(
    typeof(HermesDbContext).Assembly);
```

Ogni configurazione definisce nomi delle tabelle, chiavi, lunghezze, required fields, relazioni e conversioni necessarie per persistere correttamente gli oggetti del dominio.

## Repository

I repository concreti implementano le interfacce del dominio e usano `HermesDbContext` per leggere e modificare le entità. Sono organizzati per area e separano le operazioni di query da quelle di command.

```text
Hermes.Domain repository abstraction
  → Hermes.Infrastructure repository implementation
  → HermesDbContext
  → PostgreSQL
```

Questa separazione consente all'application layer di utilizzare repository astratti senza conoscere EF Core o SQL.

## Migration

Le migration versionate si trovano in `Migrations`. Con PostgreSQL disponibile e la connection string configurata, è possibile usare gli strumenti EF Core:

```bash
dotnet ef migrations list \
  --project src/Hermes/Hermes.Infrastructure/Hermes.Infrastructure.csproj \
  --startup-project src/Hermes/Hermes.Api/Hermes.Api.csproj

dotnet ef database update \
  --project src/Hermes/Hermes.Infrastructure/Hermes.Infrastructure.csproj \
  --startup-project src/Hermes/Hermes.Api/Hermes.Api.csproj
```

Le migration devono essere aggiornate insieme al modello persistente. Non modificare manualmente una migration già applicata: creare una nuova migration che rappresenti il cambiamento.

## Sviluppo locale

L'ambiente completo, incluso PostgreSQL, si avvia dalla root con:

```bash
./scripts/pantheon.sh local up
```

Per lavorare dal Dev Container di Hermes:

```bash
./scripts/pantheon.sh local db-up
./scripts/pantheon.sh local dev-up
```

La configurazione di Compose e gli `.env.example` sono documentati in [`infra/local/README.md`](../../../infra/local/README.md). Il README generale di Hermes descrive il rapporto tra questo layer e Api, Application e Domain: [`../README.md`](../README.md).

## Principi

1. **Infrastructure implementa, non decide** — le regole restano nel dominio e nell'application layer.
2. **Dipendenze verso le astrazioni** — i repository concreti implementano contratti definiti internamente.
3. **Mapping centralizzato** — le configurazioni EF Core restano in `Persistence/Configurations`.
4. **Schema versionato** — ogni cambiamento persistente deve essere rappresentato da una migration.
5. **Credenziali fuori dal codice** — usare `.env` locali o secret manager, mai valori sensibili committati.
6. **Database isolato** — PostgreSQL è un dettaglio sostituibile del layer infrastrutturale.

# Infrastructure locale

> Runtime locale containerizzato per PostgreSQL e per i servizi Pantheon.

## Configurazione

Dalla root del repository creare i file `.env` dagli esempi presenti in `infra/local` e nelle cartelle dei singoli servizi. Sostituire ogni valore `changeMe` con porte, database e credenziali locali.

```bash
cp infra/local/.env.example infra/local/.env
cp infra/local/postgres/.env.example infra/local/postgres/.env
cp infra/local/hermes/.env.example infra/local/hermes/.env
cp infra/local/hephaestus/.env.example infra/local/hephaestus/.env
cp infra/local/hephaestus-worker/.env.example infra/local/hephaestus-worker/.env
```

I file `.env` reali contengono configurazione locale e non devono essere committati.

## Runtime completo

Tutti i comandi sono pensati per essere eseguiti dalla root:

```bash
./scripts/pantheon.sh local up
./scripts/pantheon.sh local status
./scripts/pantheon.sh local logs
./scripts/pantheon.sh local down
```

Lo stack Compose avvia PostgreSQL, Hermes, Hephaestus e Hephaestus Worker nella rete `pantheon-local`. I servizi applicativi dipendono dal health check di PostgreSQL.

## Dev Container e debug

```bash
./scripts/pantheon.sh local dev-up
./scripts/pantheon.sh local dev-down
./scripts/pantheon.sh local debug-all
```

`dev-up` avvia i container di sviluppo con il repository montato in `/workspace`. `debug-all` verifica Docker, avvia lo stack e apre le finestre VS Code collegate ai container di Hermes, Hephaestus e Hephaestus Worker; richiede Docker, Docker Compose e il comando `code` disponibile nel `PATH`.

## PostgreSQL e persistenza

Il database usa il volume Docker `pantheon-postgres-data`, così i dati sopravvivono al normale riavvio dei container. Gli script SQL in `infra/local/postgres` inizializzano i database locali.

Per rimuovere anche il volume e tutti i dati locali:

```bash
docker compose -f infra/local/compose.yml down -v
```

Usare questo comando solo quando si desidera un reset completo dell'ambiente.

## Struttura

```text
infra/local/
├── compose.yml
├── .env.example
├── postgres/
├── hermes/
├── hephaestus/
└── hephaestus-worker/
```

I Dockerfile sotto `infra/local` producono le immagini runtime; quelli sotto `.devcontainer` producono gli ambienti interattivi per lo sviluppo e il debug.

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

## CLI locale e direnv
Pantheon utilizza `direnv` per rendere disponibile il comando `pant` all'interno della repository.

Dopo aver installato `direnv` e configurato il relativo hook per Bash:

```bash
direnv allow
```

Da quel momento il comando `pant` è disponibile dalla root e dalle sottodirectory.

Comandi principali:

```bash
pant help
pant local help
pant local up
pant local down
pant local status
pant local logs
pant local db-up
pant local db-down
pant local dev-up
pant local dev-down
pant local debug-all
```

Se non si vuole configurare `direnv`, il fallback equivalente è eseguire gli script dalla root del repository:

```bash
./scripts/pantheon.sh local up
./scripts/pantheon.sh local status
./scripts/pantheon.sh local logs
./scripts/pantheon.sh local down
./scripts/pantheon.sh local db-up
./scripts/pantheon.sh local db-down
./scripts/pantheon.sh local dev-up
./scripts/pantheon.sh local dev-down
./scripts/pantheon.sh local debug-all
```

## Runtime completo

Con `pant`:

```bash
pant local up
pant local status
pant local logs
pant local down
```

Oppure con lo script legacy dalla root:

```bash
./scripts/pantheon.sh local up
./scripts/pantheon.sh local status
./scripts/pantheon.sh local logs
./scripts/pantheon.sh local down
```

Lo stack Compose avvia PostgreSQL, Hermes, Hephaestus e Hephaestus Worker nella rete `pantheon-local`. I servizi applicativi dipendono dal health check di PostgreSQL.

## Dev Container e debug

Con `pant`:

```bash
pant local dev-up
pant local dev-down
pant local debug-all
```

Oppure con lo script legacy:

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

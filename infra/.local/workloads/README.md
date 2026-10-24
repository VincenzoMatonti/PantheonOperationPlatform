# Local Infrastructure

> Containerized local runtime for PostgreSQL and the Pantheon services.

## Configuration

From the repository root, create the `.env` files from the examples in `infra/local` and in the individual service directories. Replace every `changeMe` value with local ports, databases, and credentials.

```bash
cp infra/local/.env.example infra/local/.env
cp infra/local/postgres/.env.example infra/local/postgres/.env
cp infra/local/hermes/.env.example infra/local/hermes/.env
cp infra/local/hephaestus/.env.example infra/local/hephaestus/.env
cp infra/local/hephaestus-worker/.env.example infra/local/hephaestus-worker/.env
```

The actual `.env` files contain local configuration and must not be committed.

## Local CLI and direnv

Pantheon uses `direnv` to make the `pant` command available inside the repository.

### Ubuntu / Debian

```bash
sudo apt update
sudo apt install direnv

direnv --version

echo 'eval "$(direnv hook bash)"' >> ~/.bashrc
source ~/.bashrc
```

### macOS

```bash
brew install direnv

echo 'eval "$(direnv hook zsh)"' >> ~/.zshrc
source ~/.zshrc
```

### Windows

`direnv` is a Unix-like tool. On Windows, the recommended approach is to use WSL2 with Ubuntu or run the commands inside Git Bash/WSL, where `direnv` is properly supported. In native Windows environments, the simplest fallback is to run the repository scripts directly from a Unix-like shell.

### Enabling the project

From a shell with `direnv` installed:

```bash
cd ~/wa/solution/PantheonSolution
```

The repository contains an `.envrc` file that adds `scripts/` to the `PATH`:

```bash
export PATH="$PWD/scripts:$PATH"
```

The first time, authorize the file:

```bash
direnv allow
```

After authorization, every time you enter the project directory, `direnv` automatically loads `.envrc`.

```text
direnv: loading ~/wa/solution/PantheonSolution/.envrc
direnv: export ~PATH
```

The CLI is now available:

```bash
pant help
```

From then on, entering the project directory automatically makes `pant` available; leaving the directory causes `direnv` to remove `scripts/` from the `PATH`.

### Main commands

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

### Fallback without direnv

If you do not want to configure `direnv`, the equivalent fallback is to run the scripts from the repository root:

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

## Full runtime

With `pant`:

```bash
pant local up
pant local status
pant local logs
pant local down
```

Or with the legacy script from the repository root:

```bash
./scripts/pantheon.sh local up
./scripts/pantheon.sh local status
./scripts/pantheon.sh local logs
./scripts/pantheon.sh local down
```

The Compose stack starts PostgreSQL, Hermes, Hephaestus, and Hephaestus Worker on the `pantheon-local` network. The application services depend on PostgreSQL's health check.

## Dev Containers and debugging

With `pant`:

```bash
pant local dev-up
pant local dev-down
pant local debug-all
```

Or with the legacy script:

```bash
./scripts/pantheon.sh local dev-up
./scripts/pantheon.sh local dev-down
./scripts/pantheon.sh local debug-all
```

`dev-up` starts the development containers with the repository mounted at `/workspace`. `debug-all` verifies Docker, starts the stack, and opens the VS Code windows connected to the Hermes, Hephaestus, and Hephaestus Worker containers.

## PostgreSQL and persistence

The database uses the Docker volume `pantheon-postgres-data`, so data survives normal container restarts. The SQL scripts in `infra/local/postgres` initialize the local databases.

To remove the volume and all local data as well:

```bash
docker compose -f infra/local/compose.yml down -v
```

Use this command only when a complete environment reset is desired.

## Structure

```text
infra/local/
├── compose.yml
├── .env.example
├── postgres/
├── hermes/
├── hephaestus/
└── hephaestus-worker/
```

The Dockerfiles under `infra/local` produce the runtime images; those under `.devcontainer` produce the interactive development and debugging environments.

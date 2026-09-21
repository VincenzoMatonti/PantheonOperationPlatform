#!/usr/bin/env bash

set -Eeuo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

COMPOSE_FILE="$ROOT_DIR/infra/local/compose.yml"
DEV_COMPOSE_FILE="$ROOT_DIR/.devcontainer/compose.dev.yml"

HERMES_CONTAINER="pantheon-hermes-dev"
HEPHAESTUS_CONTAINER="pantheon-hephaestus-dev"
WORKER_CONTAINER="pantheon-hephaestus-worker-dev"

WORKSPACE="/workspace"

log() {
    echo
    echo "==> $1"
}

require_command() {
    if ! command -v "$1" > /dev/null 2>&1; then
        echo "Required command not found: $1"
        exit 1
    fi
}

container_running() {
    local container="$1"

    [[ "$(docker inspect -f '{{.State.Running}}' "$container" 2> /dev/null || true)" == "true" ]]
}

container_hex() {
    local container="$1"

    printf '%s' "$container" \
        | od -An -tx1 \
        | tr -d ' \n'
}

open_container() {
    local container="$1"

    local hex
    hex="$(container_hex "$container")"

    local uri
    uri="vscode-remote://attached-container+${hex}${WORKSPACE}"

    echo "Opening VS Code:"
    echo "  Container: $container"
    echo "  URI:       $uri"

    code --new-window --folder-uri="$uri"
}

show_status() {
    echo
    echo "Pantheon Dev Containers:"
    docker compose \
        -f "$COMPOSE_FILE" \
        -f "$DEV_COMPOSE_FILE" \
        ps \
        hermes \
        hephaestus \
        hephaestus-worker
}

require_command docker
require_command code

log "Starting Pantheon infrastructure"

docker compose \
    -f "$COMPOSE_FILE" \
    -f "$DEV_COMPOSE_FILE" \
    up -d \
    postgres \
    hermes \
    hephaestus \
    hephaestus-worker

log "Waiting for Dev Containers"

for container in \
    "$HERMES_CONTAINER" \
    "$HEPHAESTUS_CONTAINER" \
    "$WORKER_CONTAINER"; do
    if ! container_running "$container"; then
        echo "Container is not running: $container"
        show_status
        exit 1
    fi
done

log "All Dev Containers are running"

show_status

log "Opening Hermes Dev Container"

open_container "$HERMES_CONTAINER"

sleep 2

log "Opening Hephaestus Dev Container"

open_container "$HEPHAESTUS_CONTAINER"

sleep 2

log "Opening Hephaestus Worker Dev Container"

open_container "$WORKER_CONTAINER"

log "Pantheon Debug All started"

echo
echo "Three VS Code Dev Container windows have been requested."
echo
echo "  Hermes.Api"
echo "  Hephaestus.Api"
echo "  Hephaestus.Worker"
echo

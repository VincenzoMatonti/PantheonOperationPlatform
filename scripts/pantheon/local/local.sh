#!/usr/bin/env bash

set -Eeuo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
COMPOSE_FILE="$ROOT_DIR/infra/local/compose.yml"
DEV_COMPOSE_FILE="$ROOT_DIR/.devcontainer/compose.dev.yml"
HERMES_SERVICE="hermes"
HEPHAESTUS_SERVICE="hephaestus"
WORKER_SERVICE="hephaestus-worker"
POSTGRES_SERVICE="postgres"

show_help() {
    echo "Pantheon Local"
    echo
    echo "Usage:"
    echo "  pant local <command>"
    echo
    echo "Commands:"
    echo
    echo "  up         Start all local infrastructure"
    echo "  down       Stop local infrastructure"
    echo "  dev-up     Start all development containers"
    echo "  dev-down   Stop all development containers"
    echo "  db-up      Start only PostgreSQL"
    echo "  db-down    Stop only PostgreSQL"
    echo "  status     Show local infrastructure status"
    echo "  help       Show this help"
    echo
    echo "Examples:"
    echo
    echo "  pant local up"
    echo "  pant local down"
    echo "  pant local dev-up"
    echo "  pant local dev-down"
    echo "  pant local db-up"
    echo "  pant local status"
    echo
}

up() {
    echo "Starting Pantheon local infrastructure..."
    docker compose \
        -f "$COMPOSE_FILE" \
        up -d --build
}

down() {
    echo "Stopping Pantheon local infrastructure..."
    docker compose \
        -f "$COMPOSE_FILE" \
        down
}

dev_up() {
    echo "Starting Pantheon development containers..."
    docker compose \
        -f "$COMPOSE_FILE" \
        -f "$DEV_COMPOSE_FILE" \
        up -d \
        "$HERMES_SERVICE" \
        "$HEPHAESTUS_SERVICE" \
        "$WORKER_SERVICE"
}

dev_down() {
    echo "Stopping Pantheon development containers..."
    docker compose \
        -f "$COMPOSE_FILE" \
        -f "$DEV_COMPOSE_FILE" \
        stop \
        "$HERMES_SERVICE" \
        "$HEPHAESTUS_SERVICE" \
        "$WORKER_SERVICE"
}

db_up() {
    echo "Starting PostgreSQL..."
    docker compose \
        -f "$COMPOSE_FILE" \
        up -d "$POSTGRES_SERVICE"
}

db_down() {
    echo "Stopping PostgreSQL..."
    docker compose \
        -f "$COMPOSE_FILE" \
        stop "$POSTGRES_SERVICE"
}

status() {
    docker compose \
        -f "$COMPOSE_FILE" \
        ps
}

command="${1:-help}"

case "$command" in

    up)
        up
        ;;

    down)
        down
        ;;

    dev-up)
        dev_up
        ;;

    dev-down)
        dev_down
        ;;

    db-up)
        db_up
        ;;

    db-down)
        db_down
        ;;

    status)
        status
        ;;

    help)
        show_help
        ;;

    *)
        echo "Unknown local command: $command"
        echo
        show_help
        exit 1
        ;;

esac

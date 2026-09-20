#!/usr/bin/env bash

set -Eeuo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
COMPOSE_FILE="$ROOT_DIR/infra/local/compose.yml"

show_help() {
    echo "Pantheon"
    echo
    echo "Usage:"
    echo "  pantheon local <command>"
    echo
    echo "Commands:"
    echo
    echo "  up         Start all local infrastructure"
    echo "  down       Stop local infrastructure"
    echo "  dev-up     Start all development containers"
    echo "  dev-down   Stop all development containers"
    echo "  db-up      Start only PostgreSQL"
    echo "  db-down    Stop only PostgreSQL"
    echo "  debug-all  Launch all development containers and VS Code debug sessions"    
    echo "  status     Show local infrastructure status"
    echo "  logs       Show local infrastructure logs"
    echo "  help       Show this help"
    echo
}

if [[ $# -lt 1 ]]; then
    show_help
    exit 1
fi

environment="${1:-}"
command="${2:-help}"

case "$environment" in

    local)
        case "$command" in

            up)
                echo "Starting Pantheon local infrastructure..."
                docker compose -f "$COMPOSE_FILE" up -d --build
                ;;

            down)
                echo "Stopping Pantheon local infrastructure..."
                docker compose -f "$COMPOSE_FILE" down
                ;;

            dev-up)
                echo "Starting Pantheon development containers..."
                docker compose \
                    -f "$COMPOSE_FILE" \
                    -f "$ROOT_DIR/.devcontainer/compose.dev.yml" \
                    up -d \
                    hermes \
                    hephaestus \
                    hephaestus-worker
                ;;

            dev-down)
                echo "Stopping Pantheon development containers..."
                docker compose \
                    -f "$COMPOSE_FILE" \
                    -f "$ROOT_DIR/.devcontainer/compose.dev.yml" \
                    stop \
                    hermes \
                    hephaestus \
                    hephaestus-worker
                ;;

            db-up)
                echo "Starting PostgreSQL..."
                docker compose -f "$COMPOSE_FILE" up -d postgres
                ;;

            db-down)
                echo "Stopping PostgreSQL..."
                docker compose -f "$COMPOSE_FILE" stop postgres
                ;;

            debug-all)
                "$ROOT_DIR/scripts/pantheon-debug-all.sh"
                ;;

            status)
                docker compose -f "$COMPOSE_FILE" ps
                ;;

            logs)
                docker compose -f "$COMPOSE_FILE" logs -f
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
        ;;

    help)
        show_help
        ;;

    *)
        echo "Unknown environment: $environment"
        echo
        show_help
        exit 1
        ;;

esac


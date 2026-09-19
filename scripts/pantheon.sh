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
    echo "  up       Start all local infrastructure"
    echo "  db-up    Start only PostgreSQL"
    echo "  down     Stop local infrastructure"
    echo "  status   Show local infrastructure status"
    echo "  logs     Show local infrastructure logs"
    echo "  help     Show this help"
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

            db-up)
                echo "Starting PostgreSQL..."
                docker compose -f "$COMPOSE_FILE" up -d postgres
                ;;

            down)
                echo "Stopping Pantheon local infrastructure..."
                docker compose -f "$COMPOSE_FILE" down
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


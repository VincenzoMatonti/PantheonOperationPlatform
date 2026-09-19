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
    echo "  up"
    echo "  down"
    echo "  status"
    echo "  logs"
}

if [[ $# -lt 2 ]]; then
    show_help
    exit 1
fi

environment="$1"
command="$2"

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

            status)
                docker compose -f "$COMPOSE_FILE" ps
                ;;

            logs)
                docker compose -f "$COMPOSE_FILE" logs -f
                ;;

            *)
                echo "Unknown local command: $command"
                show_help
                exit 1
                ;;

        esac
        ;;

    *)
        echo "Unknown environment: $environment"
        show_help
        exit 1
        ;;

esac


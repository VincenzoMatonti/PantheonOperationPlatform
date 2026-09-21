#!/usr/bin/env bash

set -Eeuo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../../.." && pwd)"

COMPOSE_FILE="$ROOT_DIR/infra/local/compose.yml"

show_help() {
    echo "Pantheon Local Logs"
    echo
    echo "Usage:"
    echo "  pant local logs [service]"
    echo
    echo "Services:"
    echo
    echo "  all          All local services"
    echo "  hermes       Hermes API"
    echo "  hephaestus   Hephaestus API"
    echo "  worker       Hephaestus Worker"
    echo "  postgres     PostgreSQL"
    echo
    echo "Examples:"
    echo
    echo "  pant local logs"
    echo "  pant local logs hermes"
    echo "  pant local logs hephaestus"
    echo "  pant local logs worker"
    echo "  pant local logs postgres"
    echo
}

get_service() {
    local name="$1"

    case "$name" in

        hermes)
            echo "hermes"
            ;;

        hephaestus)
            echo "hephaestus"
            ;;

        worker)
            echo "hephaestus-worker"
            ;;

        postgres)
            echo "postgres"
            ;;

        *)
            echo "Unknown service: $name" >&2
            return 1
            ;;

    esac
}

logs() {
    local name="$1"

    if [[ "$name" == "all" ]]; then
        docker compose \
            -f "$COMPOSE_FILE" \
            logs -f

        return
    fi

    local service
    service="$(get_service "$name")"

    docker compose \
        -f "$COMPOSE_FILE" \
        logs -f "$service"
}

command="${1:-all}"

case "$command" in

    all | hermes | hephaestus | worker | postgres)
        logs "$command"
        ;;

    help)
        show_help
        ;;

    *)
        echo "Unknown logs target: $command"
        echo
        show_help
        exit 1
        ;;

esac

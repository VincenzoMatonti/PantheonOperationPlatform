#!/usr/bin/env bash

set -Eeuo pipefail

show_help() {
    echo "Pantheon Local Shell"
    echo
    echo "Usage:"
    echo "  pant local shell <container>"
    echo
    echo "Containers:"
    echo
    echo "  hermes             Hermes runtime container"
    echo "  hephaestus         Hephaestus API runtime container"
    echo "  worker             Hephaestus Worker runtime container"
    echo "  postgres           PostgreSQL container"
    echo
    echo "  hermes-dev         Hermes development container"
    echo "  hephaestus-dev     Hephaestus API development container"
    echo "  worker-dev         Hephaestus Worker development container"
    echo
    echo "Examples:"
    echo
    echo "  pant local shell hermes"
    echo "  pant local shell hephaestus"
    echo "  pant local shell worker"
    echo "  pant local shell hermes-dev"
    echo
}

get_container() {
    local name="$1"

    case "$name" in

        hermes)
            echo "pantheon-hermes"
            ;;

        hephaestus)
            echo "pantheon-hephaestus"
            ;;

        worker)
            echo "pantheon-hephaestus-worker"
            ;;

        postgres)
            echo "pantheon-postgres"
            ;;

        hermes-dev)
            echo "pantheon-hermes-dev"
            ;;

        hephaestus-dev)
            echo "pantheon-hephaestus-dev"
            ;;

        worker-dev)
            echo "pantheon-hephaestus-worker-dev"
            ;;

        *)
            echo "Unknown container: $name" >&2
            return 1
            ;;

    esac
}

shell() {
    local name="$1"
    local container

    container="$(get_container "$name")"

    docker exec -it "$container" bash
}

if [[ "${1:-help}" == "help" ]]; then
    show_help
    exit 0
fi

if [[ $# -lt 1 ]]; then
    show_help
    exit 1
fi

case "$1" in

    hermes | hephaestus | worker | postgres | hermes-dev | hephaestus-dev | worker-dev)
        shell "$1"
        ;;

    *)
        echo "Unknown shell target: $1"
        echo
        show_help
        exit 1
        ;;

esac

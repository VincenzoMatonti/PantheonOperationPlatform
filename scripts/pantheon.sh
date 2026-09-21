#!/usr/bin/env bash

set -Eeuo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

LOCAL_SCRIPT="$ROOT_DIR/scripts/pantheon/local/local.sh"
DB_SCRIPT="$ROOT_DIR/scripts/pantheon/local/db.sh"
SHELL_SCRIPT="$ROOT_DIR/scripts/pantheon/local/shell.sh"
LOGS_SCRIPT="$ROOT_DIR/scripts/pantheon/local/logs.sh"
DEBUG_SCRIPT="$ROOT_DIR/scripts/pantheon/debug/pantheon-debug-all.sh"

show_help() {
    echo "Pantheon"
    echo
    echo "Usage:"
    echo "  pant <command>"
    echo
    echo "Commands:"
    echo
    echo " local     Manage local infrastructure and development environment"
    echo " help      Show this help"
    echo
    echo "Additional Help:"
    echo
    echo " pant local help"
    echo "      Show local infrastructure and development commands"
    echo
    echo " pant local db help"
    echo "      Show local database commands"
    echo
    echo " pant local shell help"
    echo "      Show commands for entering local containers"
    echo
    echo " pant local logs help"
    echo "      Show local service logging commands"
    echo
}

if [[ $# -lt 1 ]]; then
    show_help
    exit 1
fi

command="${1:-help}"

case "$command" in

    local)
        subcommand="${2:-help}"

        case "$subcommand" in

            up | down | dev-up | dev-down | db-up | db-down | status)
                "$LOCAL_SCRIPT" "$subcommand"
                ;;

            debug-all)
                "$DEBUG_SCRIPT"
                ;;

            db)
                shift 2
                "$DB_SCRIPT" "$@"
                ;;

            shell)
                shift 2
                "$SHELL_SCRIPT" "$@"
                ;;

            logs)
                shift 2
                "$LOGS_SCRIPT" "$@"
                ;;

            help)
                "$LOCAL_SCRIPT" help
                ;;

            *)
                echo "Unknown local command: $subcommand"
                echo
                "$LOCAL_SCRIPT" help
                exit 1
                ;;

        esac
        ;;

    help)
        show_help
        ;;

    *)
        echo "Unknown command: $command"
        echo
        show_help
        exit 1
        ;;

esac

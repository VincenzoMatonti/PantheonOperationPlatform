#!/usr/bin/env bash

set -Eeuo pipefail

POSTGRES_CONTAINER="pantheon-postgres"
POSTGRES_USER="pantheon"
HERMES_DATABASE="hermes"
HEPHAESTUS_DATABASE="hephaestus"

show_help() {
    echo "Pantheon Local Database"
    echo
    echo "Usage:"
    echo "  pant local db <command>"
    echo
    echo "Commands:"
    echo
    echo "  shell                   Open PostgreSQL shell"
    echo "  connect <database>      Connect to a database"
    echo "  databases               List databases"
    echo "  tables <database>       List database tables"
    echo "  describe <db> <table>   Describe a database table"
    echo "  query <db> <sql>        Execute SQL query"
    echo "  status                  Show PostgreSQL container status"
    echo "  help                    Show this help"
    echo
    echo "Examples:"
    echo
    echo "  pant local db shell"
    echo "  pant local db connect hermes"
    echo "  pant local db tables hephaestus"
    echo "  pant local db describe hermes clients"
    echo '  pant local db query hermes "SELECT * FROM clients;"'
    echo
}

require_database() {
    local database="$1"

    case "$database" in
        "$HERMES_DATABASE" | "$HEPHAESTUS_DATABASE") ;;
        *)
            echo "Unknown database: $database"
            echo
            echo "Available databases:"
            echo "  $HERMES_DATABASE"
            echo "  $HEPHAESTUS_DATABASE"
            exit 1
            ;;
    esac
}

shell() {
    docker exec -it \
        "$POSTGRES_CONTAINER" \
        psql -U "$POSTGRES_USER"
}

connect() {
    local database="$1"

    require_database "$database"
    docker exec -it \
        "$POSTGRES_CONTAINER" \
        psql \
        -U "$POSTGRES_USER" \
        -d "$database"
}

databases() {
    docker exec -it \
        "$POSTGRES_CONTAINER" \
        psql \
        -U "$POSTGRES_USER" \
        -c '\l'
}

tables() {
    local database="$1"
    require_database "$database"
    docker exec -it \
        "$POSTGRES_CONTAINER" \
        psql \
        -U "$POSTGRES_USER" \
        -d "$database" \
        -c '\dt'
}

describe() {
    local database="$1"
    local table="$2"
    require_database "$database"
    docker exec -it \
        "$POSTGRES_CONTAINER" \
        psql \
        -U "$POSTGRES_USER" \
        -d "$database" \
        -c "\\d $table"
}

query() {
    local database="$1"
    local sql="$2"
    require_database "$database"
    docker exec -it \
        "$POSTGRES_CONTAINER" \
        psql \
        -U "$POSTGRES_USER" \
        -d "$database" \
        -c "$sql"
}

status() {
    docker inspect \
        -f '{{.State.Status}}' \
        "$POSTGRES_CONTAINER"
}

command="${1:-help}"

case "$command" in

    shell)
        shell
        ;;

    connect)
        [[ $# -ge 2 ]] || {
            echo "Usage: pant local db connect <database>"
            exit 1
        }

        connect "$2"
        ;;

    databases)
        databases
        ;;

    tables)
        [[ $# -ge 2 ]] || {
            echo "Usage: pant local db tables <database>"
            exit 1
        }

        tables "$2"
        ;;

    describe)
        [[ $# -ge 3 ]] || {
            echo "Usage: pant local db describe <database> <table>"
            exit 1
        }

        describe "$2" "$3"
        ;;

    query)
        [[ $# -ge 3 ]] || {
            echo "Usage: pant local db query <database> <sql>"
            exit 1
        }

        query "$2" "$3"
        ;;

    status)
        status
        ;;

    help)
        show_help
        ;;

    *)
        echo "Unknown database command: $command"
        echo
        show_help
        exit 1
        ;;

esac

#!/bin/bash

set -e

cd IssueTracker

docker compose -f docker-compose.prod.yml pull api

docker compose -f docker-compose.prod.yml up -d

for i in {1..10}; do

    if curl --fail http://localhost:9090/api/issue; then
        exit 0
    fi

    echo "Waiting for PROD API..."
    sleep 2

done

echo "PROD API did not become ready."
exit 1
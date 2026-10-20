
#!/bin/bash

# Stop the script if any command fails
set -e

# Go to the folder containing docker-compose.prod.yml
cd IssueTracker

# Pull the latest API image from GHCR
docker compose -f docker-compose.prod.yml pull api

# Start/update the PROD containers in detached mode
docker compose -f docker-compose.prod.yml up -d

# Wait for the PROD API health endpoint
for i in {1..10}; do
    if curl --fail http://localhost:9090/health; then
        echo "PROD API is ready!"
        exit 0
    fi

    echo "Waiting for PROD API..."
    sleep 2
done

echo "PROD API did not become ready."
exit 1

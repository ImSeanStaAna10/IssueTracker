
#!/bin/bash

# Stop the script if any command fails
set -e

# Go to the folder containing docker-compose.dev.yml
cd IssueTracker

# Pull the latest API image from GHCR
docker compose -f docker-compose.dev.yml pull api

# Start/update the DEV containers in detached mode
docker compose -f docker-compose.dev.yml up -d

# Wait for the API health endpoint
for i in {1..10}; do
    if curl --fail http://localhost:8080/health; then
        echo "API is ready!"
        exit 0
    fi

    echo "Waiting for API..."
    sleep 2
done

echo "API did not become ready."
exit 1
#!/bin/bash

# Stop the script if any command fails
set -e

# Pull the latest API image from GHCR
docker compose -f docker-compose.dev.yml pull api

# Start/update the DEV containers in detached mode
docker compose -f docker-compose.dev.yml up -d

# Check if the API is responding successfully
curl --fail http://localhost:8080/api/issue
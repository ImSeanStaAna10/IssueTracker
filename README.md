# 🐳 Docker — Complete Study Guide & Issue Tracker

> A personal Docker learning guide based on the Docker concepts and commands learned while containerizing the **Issue Tracker** ASP.NET Core + SQL Server application.

---

# 📚 Table of Contents

1. [What is Docker?](#1-what-is-docker)
2. [Image vs Container](#2-image-vs-container)
3. [Docker Hub](#3-docker-hub)
4. [Docker Installation & Basic Commands](#4-docker-installation--basic-commands)
5. [Running Containers](#5-running-containers)
6. [Container Management](#6-container-management)
7. [Container Logs](#7-container-logs)
8. [Entering a Container](#8-entering-a-container)
9. [Port Mapping](#9-port-mapping)
10. [Environment Variables](#10-environment-variables)
11. [Dockerfile](#11-dockerfile)
12. [Dockerfile Instructions](#12-dockerfile-instructions)
13. [Build Context](#13-build-context)
14. [Docker Build](#14-docker-build)
15. [Docker Layers](#15-docker-layers)
16. [Docker Cache](#16-docker-cache)
17. [Dockerignore](#17-dockerignore)
18. [Docker Compose](#18-docker-compose)
19. [Docker Compose Commands](#19-docker-compose-commands)
20. [Docker Compose Networking](#20-docker-compose-networking)
21. [SQL Server + Docker](#21-sql-server--docker)
22. [Persistent Volumes](#22-persistent-volumes)
23. [Environment Variables with `.env`](#23-environment-variables-with-env)
24. [ASP.NET Core Connection Strings in Docker](#24-aspnet-core-connection-strings-in-docker)
25. [Local Development vs Docker Development](#25-local-development-vs-docker-development)
26. [EF Core Migrations with Docker SQL Server](#26-ef-core-migrations-with-docker-sql-server)
27. [Git Bash + Docker Path Conversion](#27-git-bash--docker-path-conversion)
28. [Troubleshooting the SQL Server Login Error](#28-troubleshooting-the-sql-server-login-error)
29. [Testing the Dockerized API](#29-testing-the-dockerized-api)
30. [Docker Cleanup](#30-docker-cleanup)
31. [Useful Command Cheat Sheet](#31-useful-command-cheat-sheet)
32. [Issue Tracker Docker Architecture](#32-issue-tracker-docker-architecture)
33. [Docker Mental Model](#33-docker-mental-model)
34. [What We Learned](#34-what-we-learned)
35. [Next Step: CI/CD](#35-next-step-cicd)

---

# 1. What is Docker?

Docker is a platform used to **package and run applications inside containers**.

Instead of saying:

> "Works on my machine."

We want our application to have a predictable environment.

For our Issue Tracker:

```text
ASP.NET Core API
      +
.NET Runtime
      +
Application files
      +
SQL Server
      ↓
   Docker
```

Docker allows us to run these components in isolated environments.

---

# 2. Image vs Container

This is one of the most important Docker concepts.

## Image

An **Image** is like a:

> Blueprint / Template

It contains the things needed to create a container.

Example:

```text
ASP.NET Core Docker Image
        ↓
contains application
        ↓
can create containers
```

## Container

A **Container** is a running instance of an image.

```text
IMAGE
  ↓
docker run
  ↓
CONTAINER
```

### Simple analogy

Think of:

```text
Image     = Blueprint
Container = Actual building
```

One image can create multiple containers:

```text
             IMAGE
               │
       ┌───────┼───────┐
       ↓       ↓       ↓
 Container  Container  Container
    #1         #2        #3
```

---

# 3. Docker Hub

Docker Hub is a public container image registry.

It is similar to a repository where Docker images can be downloaded from.

For example:

```text
mcr.microsoft.com/mssql/server:2022-latest
```

is Microsoft's SQL Server Docker image.

When Docker needs an image that does not exist locally, it can pull the image from a registry.

---

# 4. Docker Installation & Basic Commands

## Check Docker version

```bash
docker --version
```

Purpose:

```text
Check if Docker is installed
and show the Docker version.
```

---

# 5. Running Containers

## `docker run`

```bash
docker run hello-world
```

`docker run` means:

> Create and start a NEW container from an image.

General flow:

```text
docker run
    ↓
Check local images
    ↓
Image exists?
    ├── YES → use image
    │
    └── NO
         ↓
      Pull image
         ↓
      Create container
         ↓
      Start container
```

---

## Container name

```bash
docker run --name my-test hello-world
```

`--name` allows us to give the container a custom name.

Without it:

```text
Docker generates a random name.
```

With it:

```text
my-test
```

---

# 6. Container Management

## Show running containers

```bash
docker ps
```

Only running containers are displayed.

---

## Show all containers

```bash
docker ps -a
```

This includes:

```text
Running
Stopped
Exited
```

---

## Show Docker images

```bash
docker images
```

or:

```bash
docker image ls
```

---

# Start vs Run

This distinction is VERY important.

## `docker run`

```bash
docker run nginx
```

Means:

```text
Create NEW container
        +
Start it
```

## `docker start`

```bash
docker start my-nginx
```

Means:

```text
Start EXISTING container
```

### Memory trick

```text
docker run
    = NEW

docker start
    = EXISTING
```

---

# Stop a container

```bash
docker stop my-nginx
```

Stops a running container.

The container still exists.

You can start it again:

```bash
docker start my-nginx
```

---

# Remove a container

```bash
docker rm my-nginx
```

This deletes the container.

Important:

```text
docker rm
    ↓
removes container
    ↓
does NOT remove the image
```

---

# 7. Container Logs

Use:

```bash
docker logs my-nginx
```

This displays the container's logs.

Useful for:

- debugging
- checking startup errors
- checking application output
- checking connection errors
- checking SQL Server errors

For our project:

```bash
docker logs issue-tracker-api
```

or:

```bash
docker logs issue-tracker-sqlserver
```

---

# 8. Entering a Container

Use:

```bash
docker exec -it my-nginx bash
```

Breakdown:

```text
docker exec
    ↓
Execute command inside container

-it
    ↓
Interactive terminal

my-nginx
    ↓
Target container

bash
    ↓
Open Bash shell
```

Inside the container you may see:

```text
root@container-id:/#
```

To leave:

```bash
exit
```

Important:

```text
exit
≠
docker stop
```

`exit` only leaves the interactive shell.

It does not necessarily stop the container.

---

# 9. Port Mapping

Containers have their own networking environment.

Suppose Nginx runs on:

```text
container port 80
```

We can expose it to our machine using:

```bash
docker run -p 8080:80 nginx
```

Syntax:

```text
-p HOST_PORT:CONTAINER_PORT
```

Therefore:

```text
-p 8080:80
     ↓    ↓
    Host Container
```

Flow:

```text
localhost:8080
      ↓
    Docker
      ↓
Container:80
      ↓
    Nginx
```

---

## Our Issue Tracker API

Our API container listens on:

```text
8080
```

We expose it:

```yaml
ports:
  - "8080:8080"
```

Therefore:

```text
localhost:8080
      ↓
Docker
      ↓
API container:8080
```

We access:

```text
http://localhost:8080
```

---

## SQL Server

SQL Server listens on:

```text
1433
```

We expose:

```yaml
ports:
  - "1433:1433"
```

Therefore:

```text
localhost:1433
      ↓
Docker
      ↓
SQL Server container:1433
```

---

# 10. Environment Variables

Environment variables allow applications to receive configuration from outside the application code.

Example:

```text
SQL_PASSWORD=IssueTracker.Sql123!
```

Instead of putting the password directly inside source code.

This is important because passwords should not be committed to Git.

---

# 11. Dockerfile

A `Dockerfile` is a:

> Recipe / set of instructions for creating a Docker Image.

Basic flow:

```text
Dockerfile
     ↓
 docker build
     ↓
   IMAGE
     ↓
 docker run
     ↓
 CONTAINER
```

---

# 12. Dockerfile Instructions

Our Dockerfile:

```dockerfile
# STAGE 1 BUILD
FROM mcr.microsoft.com/dotnet/sdk:10.0
WORKDIR /app

COPY ["IssueTracker.csproj", "."]
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

# STAGE 2 RUNTIME
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=0 /app/publish .

ENTRYPOINT ["dotnet", "IssueTracker.dll"]
```

Let's understand each instruction.

---

## FROM

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0
```

`FROM` defines the base image.

Think:

> "Saan tayo magsisimula?"

---

## WORKDIR

```dockerfile
WORKDIR /app
```

Defines the working directory inside the image/container.

Think:

> "Saan tayo magtatrabaho?"

Similar concept:

```bash
cd /app
```

Important:

```text
/app
≠
/
```

`/` is the root directory.

---

## COPY

```dockerfile
COPY . .
```

Copies files from the build context into the image.

Syntax:

```text
COPY SOURCE DESTINATION
```

Example:

```dockerfile
COPY . .
```

means:

```text
Local build context
       ↓
      /app
```

Memory:

```text
FROM    = saan magsisimula
WORKDIR = saan magtatrabaho
COPY    = anong files ang dadalhin
```

---

## RUN

Example:

```dockerfile
RUN dotnet restore
```

`RUN` executes a command during the image build.

Important:

```text
RUN
 ↓
BUILD TIME
```

Not runtime.

---

## ENTRYPOINT

Our application starts with:

```dockerfile
ENTRYPOINT ["dotnet", "IssueTracker.dll"]
```

Meaning:

> When the container starts, run the ASP.NET Core application.

---

# 13. Build Context

When we run:

```bash
docker build -t my-nginx-image .
```

the final `.` is important.

It means:

```text
Current directory
```

This directory becomes the Docker **build context**.

Example:

```text
IssueTracker/
├── Dockerfile
├── IssueTracker.csproj
├── Program.cs
├── Controllers/
└── Models/
```

Running:

```bash
docker build -t issue-tracker-api .
```

means:

```text
Docker build context
        ↓
IssueTracker/
```

Docker can access files from that build context during `COPY`.

---

# 14. Docker Build

Command:

```bash
docker build -t my-nginx-image .
```

Breakdown:

```text
docker build
    ↓
Create Docker image

-t
    ↓
Tag/name

my-nginx-image
    ↓
Image name

.
    ↓
Current directory/build context
```

You can also specify a version:

```bash
docker build -t my-nginx-image:1.0 .
```

If no version is specified:

```text
my-nginx-image:latest
```

---

# 15. Docker Layers

Each Dockerfile instruction can create a layer.

Example:

```dockerfile
FROM nginx
WORKDIR /app
COPY . .
RUN echo "Hello World"
```

Conceptually:

```text
FROM nginx
    ↓
Layer 1

WORKDIR /app
    ↓
Layer 2

COPY . .
    ↓
Layer 3

RUN echo "Hello World"
    ↓
Layer 4
```

Docker can reuse previous layers.

---

# 16. Docker Cache

Docker uses caching to avoid repeating work unnecessarily.

Example build output:

```text
CACHED [2/4] WORKDIR /app
```

This means Docker reused the existing layer.

---

## Cache rule

If a layer changes:

```text
Layer changes
      ↓
That layer rebuilds
      ↓
Following layers may rebuild
```

Example:

```text
WORKDIR /app
      ↓
   CACHED ✅

COPY . .
      ↓
  changed 🔴

RUN ...
      ↓
 rebuild 🔴
```

Therefore:

```text
Stable instructions
        ↓
put earlier

Frequently changing files
        ↓
put later
```

Goal:

> Maximize Docker cache and make builds faster.

---

# 17. `.dockerignore`

A `.dockerignore` file tells Docker which files should NOT be included in the build context.

Our example:

```text
bin/
obj/
.git/
.vs/
*.user
*.suo
appsettings.Development.json
```

Why?

We don't want unnecessary files inside our Docker build context.

Examples:

```text
bin/
obj/
```

are generated build files.

```text
.git/
```

contains Git repository information.

```text
.vs/
```

contains Visual Studio-specific files.

---

# 18. Docker Compose

Docker Compose allows us to define and run multiple related containers/services.

Instead of manually running:

```bash
docker run ...
docker run ...
docker network ...
docker ...
```

we can define everything in:

```text
docker-compose.yml
```

Our application has two main services:

```text
Docker Compose
      │
      ├── sqlserver
      │
      └── api
```

---

# 19. Our Docker Compose Configuration

Simplified version:

```yaml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: issue-tracker-sqlserver

    environment:
      ACCEPT_EULA: "Y"
      MSSQL_SA_PASSWORD: "${SQL_PASSWORD}"

    ports:
      - "1433:1433"

    volumes:
      - sqlserver_data:/var/opt/mssql

  api:
    build:
      context: .
      dockerfile: Dockerfile

    container_name: issue-tracker-api

    environment:
      ASPNETCORE_HTTP_PORTS: "8080"

      ConnectionStrings__DefaultConnection: "Server=sqlserver,1433;Database=IssueTrackerDb;User Id=sa;Password=${SQL_PASSWORD};TrustServerCertificate=True"

    ports:
      - "8080:8080"

    depends_on:
      - sqlserver

volumes:
  sqlserver_data:
```

---

# 20. Docker Compose Commands

## Start containers

```bash
docker compose up -d
```

Creates/starts the services.

---

## Start + rebuild

```bash
docker compose up -d --build
```

Use this when Dockerfile/application changes need to be included in a new image.

Flow:

```text
Dockerfile
    ↓
Build image
    ↓
Create/start containers
```

---

## Start existing stopped containers

```bash
docker compose start
```

Use this when containers already exist but were stopped.

---

## Stop containers

```bash
docker compose stop
```

This stops containers but keeps them.

Important:

```text
Containers = kept
Volumes    = kept
Database   = kept
```

This is what we used after finishing testing.

---

## Stop + remove containers

```bash
docker compose down
```

This removes:

```text
Containers
Network
```

But normally keeps named volumes.

---

## DO NOT casually use:

```bash
docker compose down -v
```

The `-v` removes the Compose volumes.

For our SQL Server:

```text
sqlserver_data
```

contains database data.

Therefore:

```text
docker compose down -v
```

can delete our persisted database storage.

---

## Check status

```bash
docker compose ps
```

Example:

```text
issue-tracker-api
issue-tracker-sqlserver
```

---

## View Compose logs

```bash
docker compose logs
```

Specific service:

```bash
docker compose logs api
```

or:

```bash
docker compose logs sqlserver
```

---

# 21. SQL Server + Docker

Our application uses:

```text
Microsoft SQL Server
```

Docker runs SQL Server inside:

```text
issue-tracker-sqlserver
```

Architecture:

```text
ASP.NET Core API
      │
      │ SQL connection
      ↓
SQL Server Container
      │
      ↓
IssueTrackerDb
```

---

# 22. Persistent Volumes

Containers are designed to be disposable.

But databases need persistent storage.

That's why we use:

```yaml
volumes:
  - sqlserver_data:/var/opt/mssql
```

Conceptually:

```text
SQL Server Container
        │
        ↓
/var/opt/mssql
        │
        ↓
sqlserver_data volume
```

If the container is recreated:

```text
Container deleted
      ↓
Volume remains
      ↓
Database data remains
```

This is why we normally use:

```bash
docker compose down
```

instead of:

```bash
docker compose down -v
```

when we want to preserve database data.

---

# 23. Environment Variables with `.env`

We created:

```text
.env
```

Example:

```env
SQL_PASSWORD=IssueTracker.Sql123!
```

This file contains our local Docker password.

---

## Why `.env`?

Instead of:

```yaml
MSSQL_SA_PASSWORD: "IssueTracker.Sql123!"
```

we use:

```yaml
MSSQL_SA_PASSWORD: "${SQL_PASSWORD}"
```

Docker Compose reads:

```text
.env
   ↓
SQL_PASSWORD
   ↓
${SQL_PASSWORD}
```

---

# IMPORTANT: `.env` is NOT the same as ASP.NET User Secrets

We have two separate configuration mechanisms.

## Local ASP.NET development

User Secrets:

```text
User Secrets
     ↓
ASP.NET Core
     ↓
SQL Server on Windows
```

Example connection:

```text
Server=localhost\SQLEXPRESS01;
```

---

## Docker Compose

`.env`:

```text
.env
 ↓
Docker Compose
 ↓
Containers
```

Example:

```text
SQL_PASSWORD=IssueTracker.Sql123!
```

---

# 24. ASP.NET Core Connection Strings in Docker

Inside the API container, we use:

```text
Server=sqlserver,1433
```

NOT:

```text
Server=localhost,1433
```

Why?

Because inside the API container:

```text
localhost
```

means:

> The API container itself.

It does NOT mean the SQL Server container.

Docker Compose provides service-to-service networking.

Our SQL Server service is named:

```yaml
sqlserver:
```

Therefore the API can connect using:

```text
Server=sqlserver,1433
```

Architecture:

```text
API Container
     │
     │ Server=sqlserver
     ↓
Docker network
     │
     ↓
SQL Server Container
```

---

# 25. Local Development vs Docker Development

We now have two possible environments.

## Local ASP.NET development

```text
dotnet run
     ↓
ASP.NET Core on Windows
     ↓
localhost\SQLEXPRESS01
     ↓
Windows SQL Server
```

---

## Docker development

```text
docker compose up -d
     ↓
API Container
     ↓
SQL Server Container
```

Connection string inside Docker:

```text
Server=sqlserver,1433;
```

---

## Important difference

When using Docker:

```text
API → sqlserver
```

When using host Windows SQL Server:

```text
API → localhost\SQLEXPRESS01
```

---

# 26. EF Core Migrations with Docker SQL Server

This part caused an important learning moment.

Our Docker SQL Server uses:

```text
localhost:1433
```

from the Windows host.

But inside Docker:

```text
sqlserver:1433
```

is used.

---

## Why?

When `dotnet ef` runs from Git Bash/Windows:

```text
Git Bash / Windows
      ↓
localhost:1433
      ↓
Docker SQL Server
```

But when the API container connects:

```text
API Container
      ↓
sqlserver:1433
      ↓
SQL Server Container
```

---

## Our EF connection string from host

```text
Server=localhost,1433;
Database=IssueTrackerDb;
User Id=sa;
Password=$SQL_PASSWORD;
TrustServerCertificate=True;
```

---

## Loading `.env` into Git Bash

Docker Compose automatically understands `.env`.

But:

```bash
dotnet ef
```

does NOT automatically treat `.env` as a shell environment file.

So we loaded it manually:

```bash
set -a
source .env
set +a
```

Then:

```bash
export ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=IssueTrackerDb;User Id=sa;Password=$SQL_PASSWORD;TrustServerCertificate=True;"
```

Then:

```bash
dotnet ef database update
```

---

## Important lesson

`.env`:

```text
Docker Compose
```

does not automatically mean:

```text
Git Bash
dotnet ef
ASP.NET Core CLI
```

Each tool has its own configuration mechanism.

---

# 27. Git Bash + Docker Path Conversion

We encountered this problem when running:

```bash
docker exec -it issue-tracker-sqlserver /opt/mssql-tools18/bin/sqlcmd ...
```

Git Bash converted:

```text
/opt/mssql-tools18/bin/sqlcmd
```

into something like:

```text
C:/Program Files/Git/opt/mssql-tools18/bin/sqlcmd
```

which is incorrect for the Linux container.

The solution:

```bash
MSYS_NO_PATHCONV=1
```

Example:

```bash
MSYS_NO_PATHCONV=1 docker exec -it issue-tracker-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'IssueTracker.Sql123!' -C -Q "SELECT 1"
```

---

## What does `MSYS_NO_PATHCONV=1` mean?

It tells Git Bash:

> Don't automatically convert Unix-style paths.

Useful when using Docker commands with Linux container paths.

---

# 28. Troubleshooting the SQL Server Login Error

We encountered:

```text
Login failed for user 'sa'.
```

SQL Server logs showed:

```text
Error: 18456
Login failed for user 'sa'.
Reason: Password did not match that for the login provided.
```

The key discovery was checking the actual environment variable inside the API container.

Command:

```bash
docker exec issue-tracker-api printenv ConnectionStrings__DefaultConnection
```

It showed:

```text
Password={SQL_PASSWORD}
```

That meant Docker was literally receiving:

```text
{SQL_PASSWORD}
```

as the password.

---

## Wrong

```yaml
Password={SQL_PASSWORD}
```

---

## Correct

```yaml
Password=${SQL_PASSWORD}
```

Docker Compose variable substitution requires:

```text
${VARIABLE}
```

not:

```text
{VARIABLE}
```

---

# Another problem: YAML indentation

We also encountered:

```text
go-yaml load error
did not find expected key
```

The cause was incorrect indentation in:

```yaml
environment:
```

YAML depends heavily on indentation.

Correct:

```yaml
environment:
  ASPNETCORE_HTTP_PORTS: "8080"
  ConnectionStrings__DefaultConnection: "..."
```

Incorrect indentation can prevent Compose from parsing the entire file.

---

# How we verified the fix

After fixing:

```yaml
Password=${SQL_PASSWORD}
```

we recreated the containers:

```bash
docker compose down
```

then:

```bash
docker compose up -d --build
```

Then verified:

```bash
docker exec issue-tracker-api printenv ConnectionStrings__DefaultConnection
```

The API container now received the actual environment value instead of:

```text
{SQL_PASSWORD}
```

---

# 29. Testing the Dockerized API

After Docker was working:

```text
API
↓
localhost:8080
```

We tested using Postman.

---

## Check Projects

```http
GET http://localhost:8080/api/project
```

Fresh database should initially return:

```json
[]
```

---

## Create Project

```http
POST http://localhost:8080/api/project
```

Body:

```json
{
  "name": "Issue Tracker",
  "description": "Practice project for Docker and CI/CD"
}
```

---

## Get Projects

```http
GET http://localhost:8080/api/project
```

---

## Create Issue

```http
POST http://localhost:8080/api/issue
```

Body:

```json
{
  "projectId": 1,
  "title": "Test Docker setup",
  "description": "Testing Issue Tracker inside Docker",
  "priority": "HIGH",
  "type": "TASK"
}
```

---

## Test invalid Project ID

```http
POST http://localhost:8080/api/issue
```

Body:

```json
{
  "projectId": 9999,
  "title": "Invalid project test",
  "description": "This should fail",
  "priority": "HIGH",
  "type": "BUG"
}
```

Expected:

```text
400 Bad Request
```

This verifies:

```text
Project validation
       ↓
ProjectNotFoundException
       ↓
GlobalExceptionHandler
       ↓
ProblemDetails
       ↓
400
```

---

# Test Issue Status Flow

Our Issue business rules are:

```text
OPEN
  ↓
IN_PROGRESS
  ↓
RESOLVED
  ↓
CLOSED
```

---

## Start

```http
POST http://localhost:8080/api/issue/1/start
```

Expected:

```text
204 No Content
```

---

## Resolve

```http
POST http://localhost:8080/api/issue/1/resolve
```

Expected:

```text
204 No Content
```

---

## Close

```http
POST http://localhost:8080/api/issue/1/close
```

Expected:

```text
204 No Content
```

---

# Invalid status transition

For example, trying to start an already closed issue should fail.

Expected:

```text
400 Bad Request
```

This verifies our OOP/business logic:

```text
Issue.Start()
Issue.Resolve()
Issue.Close()
```

---

# 30. Docker Cleanup

After testing, we used:

```bash
docker compose stop
```

This is useful when we are simply finished for the day.

It stops the containers but preserves them and their volumes.

---

## Start again tomorrow

Because the containers already exist:

```bash
docker compose start
```

---

## If Dockerfile changed

Use:

```bash
docker compose up -d --build
```

because we need to rebuild the image.

---

## If only stopping for the day

Use:

```bash
docker compose stop
```

---

## If removing containers but preserving named volumes

Use:

```bash
docker compose down
```

---

# 31. Useful Command Cheat Sheet

## Docker basics

```bash
docker --version
docker images
docker ps
docker ps -a
```

---

## Containers

```bash
docker run hello-world
docker run --name my-container nginx
docker start my-container
docker stop my-container
docker rm my-container
```

---

## Logs

```bash
docker logs my-container
```

---

## Enter container

```bash
docker exec -it my-container bash
```

---

## Port mapping

```bash
docker run -d -p 8080:80 nginx
```

Meaning:

```text
HOST:8080
   ↓
CONTAINER:80
```

---

## Dockerfile

```bash
docker build -t my-image .
docker images
docker run --name my-container -d -p 8080:80 my-image
```

---

## Compose

```bash
docker compose config
docker compose up -d
docker compose up -d --build
docker compose start
docker compose stop
docker compose down
docker compose ps
docker compose logs
```

---

## SQL Server

```bash
docker logs issue-tracker-sqlserver
```

Direct SQL Server test from Git Bash:

```bash
MSYS_NO_PATHCONV=1 docker exec -it issue-tracker-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'IssueTracker.Sql123!' -C -Q "SELECT 1"
```

---

## Check databases

```bash
MSYS_NO_PATHCONV=1 docker exec -it issue-tracker-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'IssueTracker.Sql123!' -C -Q "SELECT name FROM sys.databases"
```

---

## Check tables

```bash
MSYS_NO_PATHCONV=1 docker exec -it issue-tracker-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'IssueTracker.Sql123!' -C -d IssueTrackerDb -Q "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'"
```

---

## EF Core migration against Docker SQL Server

Load `.env`:

```bash
set -a
source .env
set +a
```

Set host connection string:

```bash
export ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=IssueTrackerDb;User Id=sa;Password=$SQL_PASSWORD;TrustServerCertificate=True;"
```

Apply migrations:

```bash
dotnet ef database update
```

---

# 32. Issue Tracker Docker Architecture

Our final development architecture:

```text
                     Windows Host
                          │
                          │
                ┌─────────┴─────────┐
                │                   │
                ▼                   ▼
          localhost:8080      localhost:1433
                │                   │
                ▼                   ▼
       ┌────────────────┐   ┌─────────────────────┐
       │ API Container   │   │ SQL Server Container│
       │                 │   │                     │
       │ ASP.NET Core    │──►│ SQL Server 2022     │
       │ Port 8080       │   │ Port 1433           │
       └────────────────┘   └──────────┬──────────┘
                                       │
                                       ▼
                               sqlserver_data
                                   volume
```

---

# 33. Docker Mental Model

Remember this hierarchy:

```text
Dockerfile
    │
    │ docker build
    ▼
  IMAGE
    │
    │ docker run
    ▼
CONTAINER
```

For multiple services:

```text
docker-compose.yml
        │
        ▼
 Docker Compose
        │
   ┌────┴────┐
   ▼         ▼
  API      SQL Server
Container  Container
   │         │
   └────┬────┘
        │
     Network
```

For persistent database data:

```text
SQL Server Container
        │
        ▼
 Docker Volume
        │
        ▼
Persistent Database Data
```

---

# 34. What We Learned

## Fundamental concepts

We learned:

```text
Image
Container
Docker Hub
Port mapping
Environment variables
Volumes
Dockerfile
Docker build
Docker cache
Docker Compose
Docker networking
```

---

## Commands

We learned:

```text
docker run
docker start
docker stop
docker rm
docker ps
docker ps -a
docker images
docker logs
docker exec
docker build
```

And:

```text
docker compose up
docker compose start
docker compose stop
docker compose down
docker compose ps
docker compose logs
docker compose config
```

---

## ASP.NET Core + Docker

We learned:

```text
ASP.NET Core
      ↓
Dockerfile
      ↓
Docker Image
      ↓
API Container
```

---

## ASP.NET Core + SQL Server

We learned:

```text
API Container
      ↓
Server=sqlserver,1433
      ↓
SQL Server Container
```

---

## Configuration

We learned that:

```text
User Secrets
```

and:

```text
.env
```

serve different purposes.

---

## Security

We learned:

```text
❌ Don't commit passwords
❌ Don't put secrets directly in docker-compose.yml
```

Instead:

```text
.env
  ↓
${SQL_PASSWORD}
  ↓
Docker Compose
```

And:

```gitignore
.env
```

---

## Troubleshooting

We learned how to investigate:

```text
Container status
      ↓
Container logs
      ↓
Environment variables
      ↓
Connection string
      ↓
Database connection
```

Instead of randomly rebuilding containers.

---

# 35. Next Step: CI/CD

Docker is now a stable milestone.

Our current flow:

```text
Developer
    ↓
Write ASP.NET Core code
    ↓
Test locally
    ↓
Docker Compose
    ↓
API Container
    ↓
SQL Server Container
```

The next step is to automate this.

---

## CI/CD target architecture

```text
                 GitHub
                    │
                    │ git push
                    ▼
            GitHub Actions
                    │
           ┌────────┴────────┐
           ▼                 ▼
        Restore             Test
           │                 │
           └────────┬────────┘
                    ▼
                  Build
                    │
                    ▼
              Docker Build
                    │
                    ▼
             Docker Image
                    │
                    ▼
               Registry
                    │
                    ▼
                Deploy
```

---

# 🧠 FINAL MEMORY CHEAT SHEET

If you forget everything else, remember these:

```text
Dockerfile
= Recipe for building an image

Image
= Blueprint/template

Container
= Running instance of an image

docker build
= Image

docker run
= Create + start NEW container

docker start
= Start EXISTING container

docker stop
= Stop container

docker rm
= Remove container

docker logs
= See what container is saying

docker exec -it
= Execute/enter container

-p
= Host port → Container port

Docker Compose
= Manage multiple related containers

docker compose up
= Create/start services

docker compose start
= Start existing stopped services

docker compose stop
= Stop services

docker compose down
= Remove containers/network

Volume
= Persistent storage

.env
= Local environment variables for Compose

${VARIABLE}
= Docker Compose variable substitution

sqlserver
= Docker Compose service name

localhost
= Current machine/container itself

```

---

# ⭐ Most Important Docker Flow

```text
              Dockerfile
                  │
                  │ docker build
                  ▼
                IMAGE
                  │
                  │ docker run
                  ▼
              CONTAINER
```

For our actual project:

```text
              Dockerfile
                  │
                  ▼
        IssueTracker API Image
                  │
                  ▼
          API Container
          localhost:8080
                  │
                  │ Docker Network
                  ▼
       SQL Server Container
          localhost:1433
                  │
                  ▼
          IssueTrackerDb
                  │
                  ▼
          sqlserver_data
              Volume
```

And eventually:

```text
              Developer
                  │
              git push
                  │
                  ▼
                GitHub
                  │
                  ▼
          GitHub Actions
                  │
          ┌───────┴───────┐
          ▼               ▼
        Build            Test
          │               │
          └───────┬───────┘
                  ▼
             Docker Build
                  │
                  ▼
             Docker Image
                  │
                  ▼
               Deploy
```

---

# 🏁 Current Project Milestone

```text
ASP.NET Core Backend          ✅
C# / OOP                      ✅
SQL Server                    ✅
EF Core                       ✅
CRUD                          ✅
DTOs                          ✅
Enums                         ✅
Dependency Injection          ✅
Async/Await                   ✅
Exception Handling            ✅
Project → Issue Relationship  ✅
Dockerfile                    ✅
Docker Image                  ✅
Docker Compose                ✅
SQL Server Container          ✅
Persistent Volume             ✅
.env Configuration            ✅
Docker Networking             ✅
EF Migration → Docker DB     ✅
Postman Testing               ✅
Git Commit                    ✅
GitHub Push                   ✅

CI/CD                         ⏳ NEXT
```

> **Docker milestone complete.**
>
> Next learning milestone: **GitHub Actions CI/CD** — starting with a simple CI pipeline that restores, builds, and tests the ASP.NET Core project before we add Docker image building and deployment.

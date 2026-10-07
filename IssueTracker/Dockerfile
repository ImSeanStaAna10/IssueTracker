# ==========================================
# STAGE 1: BUILD
# ==========================================

# Use the .NET SDK image.
# This contains the tools needed to restore and publish the app.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Set the working directory inside the container.
WORKDIR /app

# Copy the project file first.
# This helps Docker cache the restore step.
COPY ["IssueTracker.csproj", "."]

# Restore NuGet dependencies.
RUN dotnet restore

# Copy the rest of the application source code.
COPY . .

# Publish the application in Release mode.
# Published files will be placed in /app/publish.
RUN dotnet publish -c Release -o /app/publish


# ==========================================
# STAGE 2: MIGRATION
# ==========================================

FROM build AS migration

# Install the EF Core CLI tool.
RUN dotnet tool install --global dotnet-ef --version 10.0.12

# Make the global .NET tools available in the PATH.
ENV PATH="${PATH}:/root/.dotnet/tools"

# Run EF Core migrations when the migration container starts.
ENTRYPOINT ["dotnet", "ef", "database", "update"]


# ==========================================
# STAGE 3: RUNTIME
# ==========================================

# Use the ASP.NET Core runtime image.
# This stage is only used to run the finished application.
FROM mcr.microsoft.com/dotnet/aspnet:10.0

# Set the working directory.
WORKDIR /app

# Copy the published application from the build stage.
COPY --from=build /app/publish .

# Start the ASP.NET Core application.
ENTRYPOINT ["dotnet", "IssueTracker.dll"]
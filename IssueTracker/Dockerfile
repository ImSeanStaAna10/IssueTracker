# ==========================================
# STAGE 1: BUILD
# ==========================================

# Use the .NET SDK image.
# This contains the tools needed to restore and publish the app.
FROM mcr.microsoft.com/dotnet/sdk:10.0

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
# STAGE 2: RUNTIME
# ==========================================

# Use the ASP.NET Core runtime image.
# This stage is only used to run the finished application.
FROM mcr.microsoft.com/dotnet/aspnet:10.0

# Set the working directory.
WORKDIR /app

# Copy the published application from Stage 1.
COPY --from=0 /app/publish .

# Start the ASP.NET Core application.
ENTRYPOINT ["dotnet", "IssueTracker.dll"]
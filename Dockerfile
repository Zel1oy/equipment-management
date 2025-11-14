# Stage 1: Build the application
# Use the official .NET SDK image, which includes all tools needed to build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy the solution file and project files first
# This optimizes Docker's layer caching - dependencies are only restored if project files change
COPY *.sln .
COPY PstInventory.Core/*.csproj ./PstInventory.Core/
COPY PstInventory.Infrastructure/*.csproj ./PstInventory.Infrastructure/
COPY PstInventory.WebApp/*.csproj ./PstInventory.WebApp/

# Restore dependencies for all projects
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build and publish the WebApp in Release configuration
# The output will be placed in /app/publish
WORKDIR /source/PstInventory.WebApp
RUN dotnet publish -c Release -o /app/publish --no-restore


# Stage 2: Create the final runtime image
# Use the lightweight ASP.NET runtime image, which doesn't include the SDK
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Expose the standard ASP.NET Core ports used inside containers
# 8080 for HTTP, 8081 for HTTPS (these are defaults, your app might configure others)
EXPOSE 8080
EXPOSE 8081

# Set the entry point - command to run when the container starts
ENTRYPOINT ["dotnet", "PstInventory.WebApp.dll"]
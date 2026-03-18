# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy sln
COPY backend/TransformerMonitor.sln ./

# Copy all source folders
COPY backend/src/TransformerMonitor.Domain/ ./src/TransformerMonitor.Domain/
COPY backend/src/TransformerMonitor.Application/ ./src/TransformerMonitor.Application/
COPY backend/src/TransformerMonitor.Infrastructure/ ./src/TransformerMonitor.Infrastructure/
COPY backend/src/TransformerMonitor.Api/ ./src/TransformerMonitor.Api/

# Copy tests folder to satisfy solution requirements
COPY backend/tests/TransformerMonitor.Tests/ ./tests/TransformerMonitor.Tests/

# Restore
RUN dotnet restore TransformerMonitor.sln

# Build and publish
RUN dotnet publish src/TransformerMonitor.Api/TransformerMonitor.Api.csproj -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

# Shared config
COPY app-config.json /app-config.json

EXPOSE 61471
ENTRYPOINT ["dotnet", "TransformerMonitor.Api.dll"]

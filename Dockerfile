# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first for layer caching
COPY DesafioMirante.sln .
COPY Api/DesafioMirante.Api.csproj Api/
COPY Application/DesafioMirante.Application.csproj Application/
COPY Domain/DesafioMirante.Domain.csproj Domain/
COPY Infrastructure/DesafioMirante.Infrastructure.csproj Infrastructure/

RUN dotnet restore

# Copy remaining source and publish
COPY . .
RUN dotnet publish Api/DesafioMirante.Api.csproj -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create data directory for SQLite volume and set permissive permissions
# The base image may not provide a user named 'app', so set directory
# permissions instead of changing ownership to avoid build failures.
RUN mkdir -p /data && chmod 0777 /data

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "DesafioMirante.Api.dll"]

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
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
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Create data directory for SQLite volume and assign to built-in app user
RUN mkdir -p /data && chown app:app /data

USER app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "DesafioMirante.Api.dll"]

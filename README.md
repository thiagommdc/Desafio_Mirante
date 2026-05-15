# Desafio Mirante

API ASP.NET Core 8 organizada com Clean Architecture, foco em legibilidade, separacao de responsabilidades e facilidade de evolucao.

## Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- PostgreSQL ready
- Swagger
- FluentValidation
- AutoMapper

## Arquitetura

A estrutura completa e a responsabilidade de cada camada estao em [docs/ARQUITETURA.md](docs/ARQUITETURA.md).

## Comandos

```bash
dotnet restore DesafioMirante.sln
dotnet build DesafioMirante.sln
dotnet run --project Api/DesafioMirante.Api.csproj
```

## EF Core

```bash
dotnet tool restore
dotnet tool run dotnet-ef database update --project Infrastructure/DesafioMirante.Infrastructure.csproj
dotnet tool run dotnet-ef migrations add NomeDaMigration --project Infrastructure/DesafioMirante.Infrastructure.csproj --startup-project Api/DesafioMirante.Api.csproj
```

## Configuracao de provider

O provider de banco fica isolado na infraestrutura e pode ser alterado por configuracao:

- `Persistencia:Provedor = Sqlite`
- `Persistencia:Provedor = PostgreSql`

As regras de negocio em `Application` e `Domain` nao dependem diretamente de SQLite.

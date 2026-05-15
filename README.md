# Desafio Mirante

API ASP.NET Core 8 organizada em camadas, com foco em clareza, execução local simples e estrutura pronta para crescer sem overengineering.

## Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger
- FluentValidation
- AutoMapper

## Estrutura

```text
DesafioMirante.sln
|-- Api
|   |-- Controllers
|   |-- Middleware
|   |-- Properties
|-- Application
|   |-- Abstractions
|   |-- Common
|   |-- DTOs
|   |-- Mappings
|   |-- Services
|   |-- Validators
|-- Domain
|   |-- Common
|   |-- Entities
|-- Infrastructure
|   |-- Migrations
|   |-- Persistence
|   |-- Services
|-- dotnet-tools.json
```

## Arquitetura

- `Api`: ponto de entrada HTTP, controllers, middleware global, Swagger e composição de dependências.
- `Application`: regras de aplicação, DTOs, validações, mapeamentos e service layer.
- `Domain`: entidades e contratos de negócio mais estáveis.
- `Infrastructure`: EF Core, repositórios, seed, auditoria e implementação dos serviços técnicos.

## Funcionalidades base já prontas

- Repository Pattern
- Service Layer
- DTOs
- Middleware global de exceções
- Logging com providers nativos
- Paginação
- Filtros
- Soft delete
- Auditoria
- Migration inicial
- Seed inicial
- Async/await
- CancellationToken

## Entidade de exemplo

Foi criada a entidade `Product` para demonstrar a base da prova sem acoplar a estrutura a uma regra de negócio específica.

## Comandos

```bash
dotnet restore DesafioMirante.sln
dotnet build DesafioMirante.sln
dotnet run --project Api/DesafioMirante.Api.csproj
```

### EF Core

O repositório já inclui `dotnet-tools.json` com `dotnet-ef` 8.0.5.

```bash
dotnet tool restore
dotnet tool run dotnet-ef database update --project Infrastructure/DesafioMirante.Infrastructure.csproj
dotnet tool run dotnet-ef migrations add NomeDaMigration --project Infrastructure/DesafioMirante.Infrastructure.csproj --startup-project Api/DesafioMirante.Api.csproj
```

## Observação importante do ambiente

Para executar a API e os comandos do EF Core nesta máquina, é necessário ter o runtime ASP.NET Core 8 x64 instalado. A solution já compila em `net8.0`, mas a execução local falha sem esse runtime específico.

## Migração futura para PostgreSQL

A aplicação já está preparada em termos de separação de camadas. Na prática, a migração fica concentrada em:

- trocar o provider no `Infrastructure/DependencyInjection.cs`
- ajustar a connection string
- recriar as migrations com o provider PostgreSQL

O restante da aplicação pode permanecer praticamente igual.

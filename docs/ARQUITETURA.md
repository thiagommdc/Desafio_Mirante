# Arquitetura

## Visao Geral

A solution segue Clean Architecture com quatro projetos:

- `Api`: camada de entrada HTTP.
- `Application`: casos de uso, contratos, DTOs, validacoes e servicos de aplicacao.
- `Domain`: nucleo de negocio com entidades e tipos base do dominio.
- `Infrastructure`: persistencia, implementacoes tecnicas, seed e configuracao de provider.

## Estrutura

```text
DesafioMirante.sln
|-- Api
|   |-- Configuracoes
|   |-- Controllers
|   |   |-- V1
|   |-- Middlewares
|   |-- Properties
|
|-- Application
|   |-- Common
|   |   |-- Models
|   |-- DependencyInjection
|   |-- Features
|   |   |-- Produtos
|   |       |-- DTOs
|   |       |-- Mappings
|   |       |-- Services
|   |       |-- Validators
|   |-- Interfaces
|       |-- Persistence
|       |-- Services
|
|-- Domain
|   |-- Common
|   |-- Entities
|
|-- Infrastructure
|   |-- DependencyInjection
|   |-- Options
|   |-- Persistence
|   |   |-- Configurations
|   |   |-- Context
|   |   |-- Migrations
|   |   |-- Repositories
|   |   |-- Seed
|   |-- Services
```

## Responsabilidades por camada

### Api

- Expor controllers e contratos HTTP.
- Configurar Swagger, logging e pipeline.
- Aplicar middlewares transversais.
- Nao conter regra de negocio.

### Application

- Orquestrar casos de uso.
- Definir interfaces de servicos e persistencia.
- Centralizar DTOs, validadores e mapeamentos.
- Nao depender de EF Core, SQLite ou PostgreSql.

### Domain

- Representar entidades e tipos centrais do negocio.
- Conter abstractions estaveis do dominio, como auditoria.
- Permanecer isolada de detalhes tecnicos.

### Infrastructure

- Implementar persistencia e servicos tecnicos.
- Configurar `DbContext`, repositories, migrations e seed data.
- Escolher o provider de banco por configuracao.

## Preparacao para PostgreSql

A troca de provider fica isolada em `Infrastructure`:

- `Infrastructure/Options`
- `Infrastructure/DependencyInjection`
- `Infrastructure/Persistence`

Para migrar:

1. altere `Persistencia:Provedor` para `PostgreSql`
2. ajuste a connection string correspondente
3. gere migrations especificas do provider desejado

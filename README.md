# FCG API — FIAP Cloud Games (Fase 1)

API REST monolítica para cadastro de usuários, autenticação JWT e biblioteca de jogos adquiridos — Tech Challenge Fase 1 da FIAP.

## Objetivo

Esta solução implementa o MVP da **FIAP Cloud Games (FCG)** com:

- Cadastro e autenticação de usuários (JWT)
- Papéis `User` e `Administrator`
- CRUD de jogos (admin)
- Biblioteca de jogos adquiridos por usuário
- Persistência com **EF Core + PostgreSQL**
- Validação com **FluentValidation**
- Testes unitários das principais regras de negócio

## Arquitetura

```
FCG.Api                  → Controllers, Swagger, JWT, Serilog
FCG.Application          → DTOs, Validators, Services
FCG.Domain               → Entities, Result pattern, Repository interfaces
FCG.Infrastructure       → EF Core, Repositories, Mappings, Migrations
FCG.Infrastructure.IoC   → Dependency Injection
FCG.Tests                → Unit tests (xUnit + Moq)
```

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 18](https://www.postgresql.org/) (porta **5433** na configuração padrão)
- [EF Core CLI](https://learn.microsoft.com/ef/core/cli/dotnet): `dotnet tool install --global dotnet-ef`

## Configuração

Edite a connection string em `01- API/FCG.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "DbConnection": "Host=127.0.0.1;Port=5433;Database=fcg;Username=postgres;Password=postgres"
}
```

> Ajuste `Username` e `Password` conforme seu ambiente local.

### JWT (`TokenSettings` no appsettings)

Configure em `01- API/FCG.Api/appsettings.json`:

```json
"TokenSettings": {
  "Issuer": "FCG.Api",
  "Audience": "FCG.Client",
  "Key": "sua-chave-secreta-com-pelo-menos-32-caracteres",
  "DaysUntilExpires": 1
}
```

Para sobrescrever em desenvolvimento sem alterar o arquivo, use User Secrets:

```powershell
cd "01- API/FCG.Api"
dotnet user-secrets init
dotnet user-secrets set "TokenSettings:Key" "sua-chave-secreta-com-pelo-menos-32-caracteres"
```

## Migrations (manual)

A estrutura EF está pronta. Execute os comandos abaixo na raiz da solução:

```powershell
dotnet ef migrations add InitialCreate `
  --project "03- Infrastructure/FCG.Infrastructure/FCG.Infrastructure.csproj" `
  --startup-project "01- API/FCG.Api/FCG.Api.csproj"

dotnet ef database update `
  --project "03- Infrastructure/FCG.Infrastructure/FCG.Infrastructure.csproj" `
  --startup-project "01- API/FCG.Api/FCG.Api.csproj"
```

> A API também executa `Database.Migrate()` automaticamente no startup.

## Executar a API

```powershell
dotnet restore
dotnet run --project "01- API/FCG.Api/FCG.Api.csproj"
```

- Swagger: `https://localhost:7xxx/swagger` (porta exibida no console)
- Logs estruturados via **Serilog** no terminal

## Testes

```powershell
dotnet test
```

Cobertura principal:

| Teste | Regra |
|-------|-------|
| `RegisterUserDtoValidatorTests` | Senha forte, email válido |
| `UserServiceTests` | Login, email duplicado, role padrão |
| `UserRoleServiceTests` | Listagem dinâmica de roles |
| `UserGameServiceTests` | Compra e duplicidade |

## Primeiro usuário administrador

1. Registre um usuário: `POST /api/v1/users/register`
2. Consulte as roles: `GET /api/v1/users/roles`
3. No banco, atualize `UserRoleId` para `2` (`Administrator`) na tabela de usuários
4. Faça login: `POST /api/v1/users/logins` e use o token JWT no Swagger (Authorize)

## Endpoints

Base URL: `/api/v1`

### Users

| Método | Rota | Auth | Descrição |
|--------|------|------|-----------|
| GET | `/users/roles` | Anônimo | Listar roles |
| POST | `/users/register` | Anônimo | Cadastrar usuário |
| POST | `/users/logins` | Anônimo | Login (retorna JWT) |
| GET | `/users` | Admin | Listar usuários |
| GET | `/users/{id}` | Admin ou próprio | Detalhe do usuário |
| PUT | `/users/{id}` | Admin | Atualizar usuário |
| DELETE | `/users/{id}` | Admin | Excluir usuário (soft delete) |

### Games

| Método | Rota | Auth | Descrição |
|--------|------|------|-----------|
| POST | `/games` | Admin | Criar jogo |
| GET | `/games` | Autenticado | Listar jogos |
| GET | `/games/{id}` | Autenticado | Detalhe do jogo |
| PUT | `/games/{id}` | Admin | Atualizar jogo |
| DELETE | `/games/{id}` | Admin | Excluir jogo |

### User Games (biblioteca)

| Método | Rota | Auth | Descrição |
|--------|------|------|-----------|
| POST | `/user-games` | Autenticado | Adquirir jogo |
| GET | `/user-games` | Autenticado | Biblioteca do usuário |
| GET | `/user-games/{id}` | Autenticado | Detalhe da aquisição |

### Exemplo — Registro

```http
POST /api/v1/users/register
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "Abcdef1!"
}
```

### Exemplo — Login

```http
POST /api/v1/users/logins
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "Abcdef1!"
}
```

Use o token retornado no header: `Authorization: Bearer {token}`

## Entidades

| Entidade | Campos principais |
|----------|---------------------|
| `User` | Name, Email, Password, Salt, UserRoleId, CreatedAt |
| `UserRole` | Name (`User`, `Administrator`) — seed via EF |
| `Game` | Name, Description, Price, CreatedAt |
| `UserGame` | UserId, GameId, PurchasedAt |

## Entregáveis pendentes (grupo)

- Vídeo demonstrativo (até 15 min)
- Documentação DDD (Event Storming no Miro)
- Relatório PDF/TXT com links do repositório, documentação e vídeo

## Licença

Projeto acadêmico — FIAP Phase 1 Tech Challenge.

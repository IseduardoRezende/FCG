# FCG API — FIAP Cloud Games (Fase 1)

API REST para cadastro de usuários, autenticação JWT, catálogo de jogos e biblioteca de aquisições — Tech Challenge Fase 1 da FIAP.

## O que a API faz

- Cadastro e login de usuários com JWT
- Papéis `User` e `Administrator`
- CRUD de jogos (administrador)
- Compra e consulta da biblioteca de jogos por usuário
- Listagens paginadas com filtros
- Persistência com **EF Core + PostgreSQL**
- Validação com **FluentValidation**
- Testes unitários das principais regras de negócio

## Estrutura da solução

```
01- API/FCG.Api                  Controllers, Swagger, JWT, Serilog
02- Core/FCG.Application         DTOs, Validators, Services
02- Core/FCG.Domain              Entities, Filters, Result pattern, interfaces
03- Infrastructure/FCG.Infrastructure       EF Core, Repositories, Mappings
03- Infrastructure/FCG.Infrastructure.IoC Dependency Injection
05- Tests/FCG.Tests              Testes unitários (xUnit + Moq)
```

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/) em execução local

## Configuração rápida

### 1. Banco de dados

Crie o banco `fcg` no PostgreSQL e ajuste a connection string em `01- API/FCG.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "DbConnection": "Host=127.0.0.1;Port=5433;Database=fcg;Username=postgres;Password=SUA_SENHA"
}
```

> A API aplica as migrations automaticamente ao iniciar (`Database.Migrate()` no startup). Não é necessário rodar comandos manuais de migration.

### 2. JWT

As configurações ficam em `TokenSettings` no mesmo `appsettings.json`. Em produção, use User Secrets ou variáveis de ambiente para a chave:

```powershell
cd "01- API/FCG.Api"
dotnet user-secrets set "TokenSettings:Key" "sua-chave-secreta-com-pelo-menos-32-caracteres"
```

### 3. Executar

```powershell
dotnet restore
dotnet run --project "01- API/FCG.Api/FCG.Api.csproj"
```

| Ambiente | URL |
|----------|-----|
| Swagger (HTTPS) | https://localhost:7285/swagger |
| HTTP | http://localhost:5268 |

Os logs aparecem no terminal via **Serilog**.

## Usuário administrador padrão

Um admin já é criado via seed no banco:

| Campo | Valor |
|-------|-------|
| E-mail | `fcg@admin.com` |
| Senha | `Fcg@Admin2026!` |

Faça login em `POST /api/v1/users/logins` e use o token no Swagger (**Authorize** → `Bearer {token}`).

Para criar outros usuários comuns, use `POST /api/v1/users/register` (recebem a role `User` automaticamente).

## Autenticação

1. Login: `POST /api/v1/users/logins`
2. Copie o `token` da resposta
3. No Swagger, clique em **Authorize** e informe: `Bearer SEU_TOKEN`

Endpoints protegidos exigem o header:

```
Authorization: Bearer {token}
```

## Endpoints

Base: `/api/v1`

### Users

| Método | Rota | Acesso | Descrição |
|--------|------|--------|-----------|
| GET | `/users/roles` | Público | Listar roles |
| POST | `/users/register` | Público | Cadastrar usuário |
| POST | `/users/logins` | Público | Login (retorna JWT) |
| GET | `/users` | Admin | Listar usuários (paginado + filtros) |
| GET | `/users/{id}` | Admin ou próprio | Detalhe do usuário |
| PUT | `/users/{id}` | Admin | Atualizar usuário |
| DELETE | `/users/{id}` | Admin | Excluir usuário (soft delete) |

### Games

| Método | Rota | Acesso | Descrição |
|--------|------|--------|-----------|
| POST | `/games` | Admin | Criar jogo |
| GET | `/games` | Autenticado | Listar jogos (paginado + filtros) |
| GET | `/games/{id}` | Autenticado | Detalhe do jogo |
| PUT | `/games/{id}` | Admin | Atualizar jogo |
| DELETE | `/games/{id}` | Admin | Excluir jogo |

### User Games (biblioteca)

| Método | Rota | Acesso | Descrição |
|--------|------|--------|-----------|
| POST | `/user-games` | Autenticado | Adquirir jogo |
| GET | `/user-games` | Autenticado | Biblioteca (paginado + filtros) |
| GET | `/user-games/{id}` | Autenticado | Detalhe da aquisição |

## Filtros e paginação

As listagens (`GET /users`, `GET /games`, `GET /user-games`) aceitam query params de filtro. A resposta segue o formato `Pagination<T>` com `items`, `totalCount`, `page` e `pageSize`.

### Parâmetros comuns (`BaseFilter`)

| Parâmetro | Padrão | Descrição |
|-----------|--------|-----------|
| `value` | — | Busca textual |
| `currentPage` | `1` | Página atual |
| `pageSize` | `15` | Itens por página (máx. 100) |
| `orderField` | `Id` | Campo de ordenação |
| `orderType` | `Desc` | `Asc` ou `Desc` |

> O Swagger pode exibir os parâmetros em PascalCase (`Value`, `CurrentPage`...). Ambos os formatos funcionam no binding.

### Filtros específicos

| Endpoint | Parâmetros extras |
|----------|-------------------|
| `GET /users` | `userRoleId` |
| `GET /games` | `minPrice`, `maxPrice` |
| `GET /user-games` | `userId` (admin), `gameId`, `purchasedFrom`, `purchasedTo` |

Exemplo:

```
GET /api/v1/games?value=rpg&minPrice=10&maxPrice=100&currentPage=1&pageSize=15
```

## Exemplos de requisição

### Registro

```http
POST /api/v1/users/register
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "Abcdef1!"
}
```

Regras de senha: mínimo 6 caracteres, com letra, número e caractere especial.

### Login

```http
POST /api/v1/users/logins
Content-Type: application/json

{
  "email": "fcg@admin.com",
  "password": "Fcg@Admin2026!"
}
```

## Testes

```powershell
dotnet test "05- Tests/FCG.Tests/FCG.Tests.csproj"
```

Ou execute diretamente o binário de testes se o `dotnet test` não estiver disponível no SDK:

```powershell
& "05- Tests/FCG.Tests/bin/Debug/net10.0/FCG.Tests.exe"
```

## Entidades principais

| Entidade | Descrição |
|----------|-----------|
| `User` | Usuário com e-mail, senha (hash PBKDF2) e role |
| `UserRole` | `User` ou `Administrator` (seed) |
| `Game` | Jogo com nome, descrição e preço |
| `UserGame` | Vínculo de compra entre usuário e jogo |

Todas as entidades herdam soft delete via `IsDeleted`.

## Licença

Projeto acadêmico — FIAP Phase 1 Tech Challenge.

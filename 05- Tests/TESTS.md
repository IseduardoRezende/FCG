# Especificação dos Testes Unitários — FCG

Documentação de todos os testes do projeto `FCG.Tests`, descrevendo o objetivo, o cenário exercitado e o resultado esperado de cada um.

## Visão geral

| Item | Valor |
|------|-------|
| Projeto | `05- Tests/FCG.Tests` |
| Framework | xUnit v3 |
| Mocking | Moq |
| Validação (FluentValidation) | FluentValidation.TestHelper |
| Total de testes | 11 |

### Como executar

```powershell
dotnet test "05- Tests/FCG.Tests/FCG.Tests.csproj"
```

---

## 1. UserServiceTests

**Arquivo:** `Services/UserServiceTests.cs`  
**Classe testada:** `FCG.Application.Services.UserService`  
**Abordagem:** testes unitários com repositório, hasher de senha e gerador de token mockados.

| # | Nome do teste | Objetivo |
|---|---------------|----------|
| 1 | `LoginAsync_Should_Return_Token_When_Credentials_Are_Valid` | Garantir que o login retorna um token JWT quando as credenciais são válidas. |
| 2 | `LoginAsync_Should_Return_NotFound_When_Email_Does_Not_Exist` | Garantir que o login falha com `NotFound` quando o e-mail não está cadastrado. |
| 3 | `RegisterAsync_Should_Return_Conflict_When_Email_Already_Exists` | Garantir que o cadastro retorna `Conflict` quando o e-mail já existe. |
| 4 | `RegisterAsync_Should_Assign_User_Role_By_Default` | Garantir que um novo usuário recebe automaticamente a role `User`. |

### 1.1 LoginAsync_Should_Return_Token_When_Credentials_Are_Valid

**Cenário:** usuário com e-mail `user@test.com` existe no repositório e a verificação de senha retorna sucesso.

**Arranjo (Arrange):**
- `GetByEmailAsync` retorna um `User` com role `User`.
- `PasswordHasher.Verify` retorna `true`.
- `TokenService.Generate` retorna um `TokenDto` com token `"token"`.

**Ação (Act):** chama `LoginAsync` com e-mail e senha válidos.

**Asserções (Assert):**
- `result.Success` é `true`.
- `result.Value` não é nulo.
- `result.Value.Token` é igual a `"token"`.

---

### 1.2 LoginAsync_Should_Return_NotFound_When_Email_Does_Not_Exist

**Cenário:** tentativa de login com e-mail inexistente.

**Arranjo:** `GetByEmailAsync` retorna `null`.

**Ação:** chama `LoginAsync` com `missing@test.com`.

**Asserção:** o resultado é do tipo `NotFoundResult<TokenDto>`.

---

### 1.3 RegisterAsync_Should_Return_Conflict_When_Email_Already_Exists

**Cenário:** tentativa de cadastro com e-mail já registrado.

**Arranjo:** `ExistsByEmailAsync` retorna `true` para `user@test.com`.

**Ação:** chama `RegisterAsync` com nome, e-mail e senha válidos.

**Asserção:** o resultado é do tipo `ConflictResult<ReadUserDto>`.

---

### 1.4 RegisterAsync_Should_Assign_User_Role_By_Default

**Cenário:** cadastro bem-sucedido de um novo usuário.

**Arranjo:**
- `ExistsByEmailAsync` retorna `false`.
- `AddAsync` captura o `User` persistido.
- `GetByIdAsync` retorna o usuário criado com `UserRoleId = User`.

**Ação:** chama `RegisterAsync` com DTO válido.

**Asserções:**
- `result.Success` é `true`.
- O usuário capturado em `AddAsync` possui `UserRoleId` igual a `(long)UserRoles.User`.

---

## 2. UserGameServiceTests

**Arquivo:** `Services/UserGameServiceTests.cs`  
**Classe testada:** `FCG.Application.Services.UserGameService`  
**Abordagem:** testes unitários com repositórios e usuário autenticado mockados. O usuário logado tem `UserId = 1` e não é administrador.

| # | Nome do teste | Objetivo |
|---|---------------|----------|
| 5 | `PurchaseAsync_Should_Succeed_When_Game_Is_Available` | Garantir que a compra de um jogo disponível é concluída com sucesso. |
| 6 | `PurchaseAsync_Should_Return_Conflict_When_Game_Already_Purchased` | Garantir que não é possível comprar o mesmo jogo duas vezes. |

### 2.1 PurchaseAsync_Should_Succeed_When_Game_Is_Available

**Cenário:** usuário autenticado adquire um jogo que ainda não possui na biblioteca.

**Arranjo:**
- Usuário `Id = 1` existe.
- Jogo `Id = 10` existe (`Name = "Game"`, `Price = 99.9`).
- `ExistsAsync(1, 10)` retorna `false` (jogo ainda não adquirido).
- `GetByIdAsync` retorna o `UserGame` criado.

**Ação:** chama `PurchaseAsync` com `GameId = 10`.

**Asserções:**
- `result.Success` é `true`.
- `result.Value.GameName` é `"Game"`.

---

### 2.2 PurchaseAsync_Should_Return_Conflict_When_Game_Already_Purchased

**Cenário:** usuário tenta adquirir um jogo que já está na biblioteca.

**Arranjo:**
- Usuário e jogo existem.
- `ExistsAsync(1, 10)` retorna `true`.

**Ação:** chama `PurchaseAsync` com `GameId = 10`.

**Asserção:** o resultado é do tipo `ConflictResult<ReadUserGameDto>`.

---

## 3. UserRoleServiceTests

**Arquivo:** `Services/UserRoleServiceTests.cs`  
**Classe testada:** `FCG.Application.Services.UserRoleService`

| # | Nome do teste | Objetivo |
|---|---------------|----------|
| 7 | `GetAllAsync_Should_Return_Roles_From_Repository` | Garantir que a listagem de roles retorna os papéis do repositório. |

### 3.1 GetAllAsync_Should_Return_Roles_From_Repository

**Cenário:** consulta de todos os papéis do sistema.

**Arranjo:** `GetAllAsync` retorna duas roles — `User` (Id 1) e `Administrator` (Id 2).

**Ação:** chama `GetAllAsync`.

**Asserções:**
- `result.Success` é `true`.
- A lista contém 2 itens.
- O primeiro item tem `Name` igual a `"User"`.

---

## 4. RegisterUserDtoValidatorTests

**Arquivo:** `Validators/RegisterUserDtoValidatorTests.cs`  
**Classe testada:** `FCG.Application.Validators.RegisterUserDtoValidator`  
**Abordagem:** testes de validação com `FluentValidation.TestHelper` (sem dependências externas).

Regras validadas pelo `RegisterUserDtoValidator`:
- **Name:** obrigatório, máximo 100 caracteres.
- **Email:** obrigatório, formato válido, máximo 150 caracteres.
- **Password:** obrigatório, mínimo 8 caracteres, com letra, número e caractere especial.

| # | Nome do teste | Objetivo |
|---|---------------|----------|
| 8 | `Should_Have_Error_When_Name_Is_Empty` | Rejeitar cadastro com nome vazio. |
| 9 | `Should_Have_Error_When_Email_Is_Invalid` | Rejeitar cadastro com e-mail em formato inválido. |
| 10 | `Should_Have_Error_When_Password_Is_Weak` | Rejeitar cadastro com senha fraca. |
| 11 | `Should_Not_Have_Error_When_Dto_Is_Valid` | Aceitar cadastro com todos os campos válidos. |

### 4.1 Should_Have_Error_When_Name_Is_Empty

**Entrada:** `Name = ""`, e-mail e senha válidos.

**Asserção:** erro de validação no campo `Name`.

---

### 4.2 Should_Have_Error_When_Email_Is_Invalid

**Entrada:** `Email = "invalid"`, nome e senha válidos.

**Asserção:** erro de validação no campo `Email`.

---

### 4.3 Should_Have_Error_When_Password_Is_Weak

**Entrada:** `Password = "weak"`, nome e e-mail válidos.

**Asserção:** erro de validação no campo `Password`.

---

### 4.4 Should_Not_Have_Error_When_Dto_Is_Valid

**Entrada:** `Name = "User"`, `Email = "user@test.com"`, `Password = "Abcdef1!"`.

**Asserção:** nenhum erro de validação.

---

## Cobertura por módulo

| Módulo | Classe / componente | Testes | Regras cobertas |
|--------|---------------------|--------|-----------------|
| Autenticação | `UserService.LoginAsync` | 2 | Login com sucesso; e-mail inexistente |
| Cadastro | `UserService.RegisterAsync` | 2 | E-mail duplicado; role padrão `User` |
| Biblioteca | `UserGameService.PurchaseAsync` | 2 | Compra com sucesso; jogo já adquirido |
| Papéis | `UserRoleService.GetAllAsync` | 1 | Listagem de roles |
| Validação | `RegisterUserDtoValidator` | 4 | Nome, e-mail, senha e DTO válido |

## O que não está coberto por testes unitários

Os seguintes componentes e fluxos **não possuem** testes unitários neste projeto:

- `GameService` (CRUD de jogos)
- `TokenService` (geração de JWT)
- Controllers da API (`UsersController`, `GamesController`, `UserGamesController`)
- Repositórios e persistência (EF Core)
- Validadores: `LoginDtoValidator`, `CreateGameDtoValidator`, `UpdateGameDtoValidator`, `CreateUserGameDtoValidator`, `UpdateUserDtoValidator`

Esses fluxos podem ser validados manualmente via Swagger ou por testes de integração em evoluções futuras do projeto.

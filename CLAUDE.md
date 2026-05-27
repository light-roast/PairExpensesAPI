# CLAUDE.md

Guidance for Claude Code (or any future LLM session) working in this repository.

## Project overview

**PairExpensesAPI** is a small ASP.NET Core REST API that backs the "PairXpenses" web app — a two-couple expense ledger. Two independent couples (`pair1`, `pair2`) each have two members. Members of the same pair share full read/write access to their pair's payments and debts; the two pairs are isolated from each other.

- API: `https://pairxpenses.azurewebsites.net` (Azure App Service, Windows, F1 Free tier)
- Frontend (separate app, separate repo): `https://pairxpensesapp.azurewebsites.net`
- GitHub: `light-roast/PairExpensesAPI`
- Owner: Daniel Echeverri (`lightroast`)

## Tech stack

| Layer | Choice | Version |
|---|---|---|
| Runtime | .NET / ASP.NET Core | 10.0 |
| ORM | Entity Framework Core | 10.0 |
| Database | SQLite (file-based) | — |
| Auth | JWT bearer | `Microsoft.AspNetCore.Authentication.JwtBearer` 10 |
| Mapping | AutoMapper | 15.1.1 (CVE-2026-32933 patched) |
| Logging | Serilog (file sink) | `Serilog.AspNetCore` 9 |
| API docs | Built-in `Microsoft.AspNetCore.OpenApi` + Swashbuckle SwaggerUI shell | 10 / 7 |
| Hosting | Azure App Service Windows + IIS / AspNetCoreModuleV2 | — |

## Repository layout

```
Controllers/        — AccountController (login), Payment/Debt/User (CRUD)
Services/           — Service interfaces + implementations, DI scoped
Entities/           — Domain entities + *Req DTOs for inbound payloads
Mapper/             — AutoMapper Profile classes (*Req <-> Entity)
Data/               — DataContext (EF Core)
Migrations/         — EF Core migrations (do not regenerate without reason)
OpenApi/            — BearerSecuritySchemeTransformer for JWT scheme on /openapi/v1.json
Properties/         — launchSettings.json (dev-only, never deployed)
.github/workflows/  — Two YAMLs (master_pairxpenses.yml is the active one)
Program.cs          — Composition root
appsettings.json    — Includes Jwt:Key in plain text (smell — see Known issues)
```

## Domain model

- `User { Id, Name, Username, Password, PairRole, Debts[], Payments[] }`
- `Payment { Id, Name, Value (long), CreateDate, UpdateDate, UserId }`
- `Debt { Id, Name, Value (long), CreateDate, UpdateDate, UserId }`
- `*Req` DTOs (`PaymentReq`, `DebtReq`, `UserReq`) carry inbound update payloads
- `Value` is `long` (storing minor currency units, e.g. `908000` = $908.000 in COP-style formatting on the frontend)

## Authentication & authorization

- `AccountController.Login` is the only anonymous endpoint. Issues JWT with claims: `sub` (username), `name`, `role` (`pair1` or `pair2`), `exp`, `iss`, `aud`.
- All other controllers carry `[Authorize(Roles="pair1, pair2")]` at the class level. No anonymous reads or writes.
- Pair boundary is enforced inside Payment + Debt actions via `IsAllowedForUser(int userId)`. It resolves the caller's pair from the JWT role claim, loads the target user, and compares `User.PairRole`. Mismatch → `403 Forbid()`.
- Two members of the *same* pair can read/write each other's data — intentional. Do not narrow this.
- Dates (`CreateDate`, `UpdateDate`) are client-supplied via JSON body. Treated as feature (used for corrections and historical entry). Don't lock them down without asking.
- `Jwt:Key` is checked into `appsettings.json` plain — see Known issues.

## Database

- SQLite file `Database.db`.
- **Production path:** `C:\home\data\Database.db` (persistent across Azure deploys — outside wwwroot on purpose).
- **Connection string override:** Azure App Service → Configuration → Connection Strings → `Database = Data Source=C:\home\data\Database.db`. This sets env var `CUSTOMCONNSTR_Database`, which maps to `ConnectionStrings:Database` in `IConfiguration` automatically.
- **Local fallback:** `Program.cs` defaults to relative `Data Source=Database.db` if no env var. Creates a fresh DB on first run.
- `Database.db` is currently committed to the repo (legacy). New deploys do **not** ship it (no `<Content Include>` in csproj), so the prod DB at `C:\home\data\` is unaffected by deploys. Consider `.gitignore` cleanup at some point.
- Two migrations exist (`InitialCreate`, `ChangesToUsers`). They are applied manually — no `Database.Migrate()` call in `Program.cs`.

## Local development

```bash
dotnet restore
dotnet run                # uses Properties/launchSettings.json
                          # → ASPNETCORE_ENVIRONMENT=Development
                          # → http://localhost:5002, https://localhost:7234
```

- `IsDevelopment()` is the env switch for Swagger UI + CORS:
  - Swagger UI mounted at `/swagger` (backed by `/openapi/v1.json`) — **dev only**.
  - CORS adds `SetIsOriginAllowed(o => new Uri(o).Host == "localhost")` so any localhost port works.
- In production, both Swagger and localhost CORS are off. Only allowed origin is `https://pairxpensesapp.azurewebsites.net`.

## Deployment

- Trigger: `git push origin master`.
- Workflow: `.github/workflows/master_pairxpenses.yml` (the other YAML `PairXpenses.yml` is a leftover and uses an older `webapps-deploy@v2` — the v3 one in `master_pairxpenses.yml` is the live deploy).
- Build runs on `windows-latest` with `DOTNET_CORE_VERSION: 10.0.x`.
- `dotnet publish` artifact → uploaded → `azure/webapps-deploy@v3` does **OneDeploy with `clean=true`**, which **wipes `C:\home\site\wwwroot` before extracting**. This is why the DB lives in `C:\home\data\` and not wwwroot — anything in wwwroot not included in the publish artifact gets deleted on every deploy.
- Azure prerequisites that must remain ON for the deploy to succeed (have bitten us before):
  - **SCM Basic Auth Publishing Credentials** (Configuración general)
  - **FTP Basic Auth Publishing Credentials** (Configuración general)
  - **Pila en tiempo de ejecución** = `.NET 10 (LTS)`
- Publish profile is stored as GitHub secret `PairXpenses_FAC7`. If you ever see `Unauthorized (CODE: 401)` from `webapps-deploy`, the fix is almost always: re-download publish profile from Azure portal → update the secret.

## Health and observability

- Serilog writes to `logs\log.txt` relative to content root. On Azure: `C:\home\site\wwwroot\logs\log.txt`. **This dies on every deploy** because it lives in wwwroot.
- Application Insights connection string is set as env var (`APPLICATIONINSIGHTS_CONNECTION_STRING`) on the App Service but is not wired into Program.cs. Insights agent is loaded as a site extension instead.
- Quick health probe: `curl -i https://pairxpenses.azurewebsites.net/api/User` → expected `401` with `WWW-Authenticate: Bearer` if app is up. `/openapi/v1.json` should `404` in prod (proves `IsDevelopment()` is false).

## Coding conventions

- Tabs for indentation in C# files. Don't reformat.
- Service layer is thin — controllers do auth checks, services do EF work. Pair-boundary checks live in the **controller** layer, not in services.
- `*Req` DTOs for inbound bodies on `PATCH` endpoints. Top-level `Entity` types for `POST` bodies (legacy — `Value`, `UserId`, dates all settable from JSON; this is intentional).
- AutoMapper profiles use explicit `ForMember` even for properties that would auto-map. Don't simplify.
- Controllers inject `ILogger<AccountController>` even outside `AccountController` (legacy quirk — not worth touching).
- The codebase mixes `_field`, `this._field`, and bare `field` access. Don't crusade.

## Known issues / smells (leave unless explicitly asked to fix)

1. **`Jwt:Key` plain in `appsettings.json`** — anyone with repo access can forge tokens. Should move to env var or Key Vault.
2. **`Jwt:Issuer` and `Jwt:Audience` are `localhost` URLs** even in production. Validation works only because it's a string equality check. Confusing but functional.
3. **`AccountController` has 7 pre-existing nullable warnings** (`Username` / `Password` / `Encoding.GetBytes` / `Claim` ctor). Real `NullReferenceException` paths if a malformed login body is posted. Not security-critical, fix when in there for other reasons.
4. **`Database.db` is committed to git** — local dev runs mutate it, creating noisy diffs. Production is decoupled (lives at `C:\home\data\`), so this is cosmetic.
5. **Two workflow YAMLs** in `.github/workflows/`: `PairXpenses.yml` (old, `webapps-deploy@v2`) and `master_pairxpenses.yml` (live, v3). Both trigger on push to master, so both run; the v2 one is effectively dead weight. Safe to delete.
6. **Free F1 SKU** → 32-bit worker, no Always-On, 1 GB memory. Cold starts are visible. Don't enable Always-On — it requires Basic+ and will fail silently on F1.
7. **`Microsoft.AspNetCore.Mvc.DataAnnotations 2.2.0`** is no longer referenced — was removed during the .NET 10 upgrade. The framework's built-in validation handles it.

## History (most recent first)

- **2026-05-26**: Big day.
  - Upgraded `net8.0` → `net10.0`.
  - Patched AutoMapper `13.0.1` → `15.1.1` (CVE-2026-32933, high severity).
  - Replaced `Swashbuckle.AspNetCore` with built-in `Microsoft.AspNetCore.OpenApi` + `Swashbuckle.AspNetCore.SwaggerUI` shell for the UI.
  - Removed `Microsoft.AspNetCore.Mvc.DataAnnotations 2.2.0` (ASP.NET 2.x leftover).
  - Updated `Microsoft.OpenApi` to 2.x — types moved out of `.Models` sub-namespace, `OpenApiDocument.SecurityRequirements` renamed to `Security`. `BearerSecuritySchemeTransformer` was written against this new API.
  - AutoMapper 15 removed the `AddAutoMapper(params Assembly[])` overload. All overloads now require an `Action<IMapperConfigurationExpression>` argument. Current call site: `builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);`.
  - CORS rewritten: prod allows only the frontend, dev allows any localhost port via predicate.
  - DB moved out of `wwwroot` to `C:\home\data\Database.db` (persistent storage).
  - Added class-level `[Authorize(Roles="pair1, pair2")]` to Payment/Debt/User controllers (previously most endpoints were anonymous — significant hole closed).
  - Added cross-pair boundary check (`IsAllowedForUser`) on every per-user / per-entity action in Payment + Debt controllers.

## Things not to do

- Don't add `Database.EnsureCreated()` or `Database.Migrate()` to Program.cs on startup — migrations are applied out-of-band.
- Don't move the SQLite file back inside wwwroot.
- Don't relax CORS to include `localhost` in production.
- Don't remove the `[Authorize]` class-level attributes.
- Don't switch back to Swashbuckle (`AddSwaggerGen` / `UseSwagger`) — the project uses built-in OpenAPI now.
- Don't change date semantics on POST to "server-side only" — corrections workflow depends on client-supplied dates.
- Don't enable Always-On on the App Service — requires Basic+, breaks on F1 Free.

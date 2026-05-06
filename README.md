# Study .NET

This folder contains a small ASP.NET Core project built from the beginner training plan in `/Users/long/Downloads/training-plan-dotnet-beginner.md`.

The goal is not to be a production API. The goal is to make the code easy to read while mirroring the flow used by larger .NET projects:

```text
Request -> Controller -> Service -> UnitOfWork -> Repository -> StudyDbContext -> EF InMemory Database
                ^
             Middleware
```

## Run

```bash
cd /Users/long/Code/personal/dotnet-progress-tracker/study-dotnet
dotnet run --project StudyDotnet.Api
```

The app prints the local URL, usually `http://localhost:5xxx`.

Swagger UI is available at:

```text
http://localhost:5000/swagger
```

## Try The API

Most v1 endpoints require the API key header:

```bash
curl -H "X-Api-Key: study-key" -H "X-Tenant: demo" http://localhost:5000/api/v1/companies
curl -H "X-Api-Key: study-key" -H "X-Tenant: lab" http://localhost:5000/api/v1/companies/count
```

Search devices:

```bash
curl -X POST http://localhost:5000/api/v1/devices/search \
  -H "Content-Type: application/json" \
  -H "X-Api-Key: study-key" \
  -H "X-Tenant: demo" \
  -d '{"keyword":"hik","supplier":"HIKvision","page":1,"pageSize":10}'
```

Login demo:

```bash
curl -X POST http://localhost:5000/api/v2/auth/login \
  -H "Content-Type: application/json" \
  -d '{"userName":"admin","password":"study"}'
```

Protected endpoint with `[Authorize]`, `ClaimsPrincipal`, and claims:

```bash
TOKEN="paste-token-from-login"

curl http://localhost:5000/api/v1/profile/me \
  -H "X-Api-Key: study-key" \
  -H "Authorization: Bearer $TOKEN"
```

Status code examples:

```bash
curl http://localhost:5000/api/v1/companies
curl -H "X-Api-Key: study-key" http://localhost:5000/api/v1/companies
curl -H "X-Api-Key: study-key" -H "X-Tenant: demo" http://localhost:5000/missing
curl -H "X-Api-Key: study-key" -H "X-Tenant: demo" http://localhost:5000/api/v1/debug/error
```

## What To Read

- `Program.cs`: app bootstrapping and `UseStartup<Startup>()`.
- `Startup.cs`: DI, Swagger, CORS, error handling, status-code pages, middleware, and routing.
- `Auth/DemoBearerAuthenticationHandler.cs`: reads `Authorization: Bearer ...` and creates claims.
- `Tenancy/`: request-scoped tenant context populated from `X-Tenant` or the bearer token claim.
- `Controllers/`: API routes and model binding.
- `Services/`: async/await, LINQ, DTO mapping, business logic.
- `Data/StudyDbContext.cs`: EF Core `DbContext`, `DbSet`, entity mapping, seed data, audit handling.
- `Repositories/`: EF-backed generic repository and unit of work pattern.
- `Domain/`: classes, interfaces, inheritance, enum, audit base class.
- `Middleware/ApiKeyMiddleware.cs`: custom middleware that blocks requests without `X-Api-Key`.

## Phase 5: Entity Framework Core & Database

This study project uses EF Core with the in-memory provider so you can read EF-style code without setting up PostgreSQL.

Read in this order:

1. `Data/StudyDbContext.cs`
2. `Repositories/EfRepository.cs`
3. `Repositories/UnitOfWork.cs`
4. `Services/CompanyService.cs`
5. `Services/DeviceService.cs`

What each topic looks like here:

- DbContext and DbSet: `StudyDbContext` exposes `Companies` and `Devices`.
- Entity mapping: `OnModelCreating` configures keys, required fields, indexes, enum conversion, and relationships.
- Seed data: `HasData(...)` creates demo tenants, companies, and devices.
- Repository pattern: `EfRepository<TEntity, TKey>` wraps `DbSet<TEntity>`.
- Unit of Work: `UnitOfWork.SaveChangesAsync(...)` calls `StudyDbContext.SaveChangesAsync(...)`.
- Querying: services use `Where`, `OrderBy`, `Skip`, `Take`, `Select`, `CountAsync`, and `ToListAsync`.
- Include: `DeviceService` uses `.Include(device => device.Company)` to load related company data.
- Auditing: `StudyDbContext.SaveChangesAsync(userName)` fills `CreatedAt`, `CreatedBy`, `UpdatedAt`, and `UpdatedBy`.
- Migrations: not generated because the in-memory provider has no real schema. In a PostgreSQL/SQL Server project, migrations are created from the same entity mappings in `OnModelCreating`.
- Transactions: also skipped for the in-memory provider. With a relational provider, transaction methods usually live beside `UnitOfWork.SaveChangesAsync`.

### Migrations

The project includes a PostgreSQL migration setup because real EF migrations require a relational provider.

Migration files live in:

```text
StudyDotnet.Api/Data/Migrations
```

Important files:

- `Data/StudyDbContextFactory.cs`: design-time factory used by `dotnet ef`.
- `Data/Migrations/*_InitialCreate.cs`: creates `Companies`, `Devices`, indexes, foreign keys, and seed data.
- `Data/Migrations/StudyDbContextModelSnapshot.cs`: EF's current model snapshot.

Create a new migration after changing entities or mappings:

```bash
dotnet ef migrations add AddSomething \
  --project StudyDotnet.Api \
  --startup-project StudyDotnet.Api \
  --context StudyDbContext \
  --output-dir Data/Migrations
```

Apply migrations to PostgreSQL:

```bash
dotnet ef database update \
  --project StudyDotnet.Api \
  --startup-project StudyDotnet.Api \
  --context StudyDbContext
```

Generate SQL without touching a database:

```bash
dotnet ef migrations script \
  --project StudyDotnet.Api \
  --startup-project StudyDotnet.Api \
  --context StudyDbContext \
  --idempotent
```

By default, `appsettings.json` uses `"Database:Provider": "InMemory"` so the API runs without PostgreSQL. Change it to `"Postgres"` to make the app use the `Postgres` connection string and apply migrations at startup.

## Training Plan Mapping

- Week 1: `Domain`, `Dtos`, LINQ in services.
- Week 2: `async` service methods and DI in `Startup.cs`.
- Week 3: controllers, routing, DTO request/response objects, and Swagger/OpenAPI.
- Week 4: `[Authorize]`, claims, `ClaimsPrincipal`, CORS policy, error handling, status codes, custom API key middleware, and fake login flow.
- Week 5: EF Core `DbContext`, `DbSet`, mapping, repository, unit of work, `Include`, pagination, and auditing.
- Week 6: full request trace from controller to data store, including tenant isolation.

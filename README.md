# eFitness

A gym management platform with three roles — Admin, Trainer and Client — covering
memberships, training sessions, workout plans, a shop, payments, messaging and
progress tracking.

## Stack

- **Backend**: ASP.NET Core 8 Web API, Clean Architecture (Domain / Application /
  Infrastructure / API), CQRS with MediatR, EF Core with MySQL (Pomelo provider),
  FluentValidation, JWT authentication with rotating httpOnly refresh tokens.
- **Frontend**: Angular 18 with Angular Material, NgModule-based feature modules
  with lazy loading, role-based route guards, HTTP interceptors for auth and
  token refresh.
- **Database**: MySQL 8.

## Project layout

```
backend/
  eFitness.sln
  src/
    eFitness.Domain/          entities, enums
    eFitness.Application/     CQRS commands/queries, DTOs, validators
    eFitness.Infrastructure/  EF Core DbContext, migrations, JWT/password services
    eFitness.API/             controllers, Program.cs, appsettings
frontend/
  src/app/
    core/                     services, guards, interceptors, models
    shared/                   Material re-exports, shared chat component
    layout/                   sidebar/topbar shell
    features/
      landing/ auth/ admin/ trainer/ client/
docker-compose.yml           local MySQL for development
```

## Prerequisites

- .NET 8 SDK
- Node.js 20+ and npm
- MySQL 8 (via Docker, or a local install)

## 1. Start the database

```bash
docker compose up -d
```

This starts MySQL 8 on `localhost:3306` with database `efitness`, user
`efitness_user` / password `efitness_password` (override via a `.env` file —
see `.env.example`).

If you'd rather use a local MySQL install instead of Docker, just create a
database and user yourself and point the connection string at it.

## 2. Configure the backend

Secrets and environment-specific values are never committed — use
`dotnet user-secrets` for local development:

```bash
cd backend/src/eFitness.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" \
  "Server=localhost;Port=3306;Database=efitness;User=efitness_user;Password=efitness_password;"
dotnet user-secrets set "Jwt:Secret" "a-long-random-string-at-least-32-characters"
```

## 3. Apply migrations

```bash
cd backend
dotnet tool install --global dotnet-ef   # first time only
export PATH="$PATH:$HOME/.dotnet/tools"

dotnet ef database update \
  --project src/eFitness.Infrastructure/eFitness.Infrastructure.csproj \
  --startup-project src/eFitness.API/eFitness.API.csproj
```

## 4. Run the backend

```bash
dotnet run --project backend/src/eFitness.API/eFitness.API.csproj
```

Swagger UI is at `http://localhost:5234/swagger` in development.

## 5. Run the frontend

```bash
cd frontend
npm install
npm start
```

Open `http://localhost:4200`. Register a client account from the landing page,
or promote a user to `Admin`/`Trainer` directly in the database to explore
those roles (there's no seed data yet — see "Known gaps" below).

## Running tests

```bash
cd backend
dotnet test
```

## Architecture notes

- **Auth**: access tokens are short-lived JWTs kept in memory on the frontend
  (never in `localStorage`); refresh tokens are rotated on every use and stored
  in an httpOnly, `SameSite=Strict` cookie set by the API.
- **Authorization**: every mutating endpoint that acts "on behalf of" a trainer
  or client resolves the trainer/member id from the JWT server-side — a
  client-supplied id is never trusted for identifying the acting user.
- **Paging & filtering**: every listing endpoint accepts `pageNumber`/`pageSize`
  plus entity-specific filters and returns a consistent `PaginatedList<T>` shape.
- **Config**: connection strings, JWT secret and CORS origins all live in
  `appsettings.json` / user-secrets / environment variables — nothing is
  hardcoded in source.

## Known gaps

- No seed data — the first user must self-register (as a Client); promote to
  Admin/Trainer via direct DB access for local testing.
- No automated frontend tests yet.
- Advanced features from the course rubric (Stripe payments, i18n, CAPTCHA,
  file uploads, background jobs, PDF reports) are not implemented — payments
  are simulated/recorded directly rather than processed through a real gateway.

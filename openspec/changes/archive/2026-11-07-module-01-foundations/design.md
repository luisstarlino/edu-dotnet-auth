## Context

Module 00 shipped a clean Minimal API (`src/EduAuth.Api`, `net10.0`) with a health/home endpoint and OpenAPI, but no persistence. The course now needs somewhere to store users before teaching passwords (module 02) and identity (module 03). This module adds the data layer. It must stay beginner-friendly: no database server to install, migrations visible and explainable, and the whole wiring readable in `Program.cs`.

## Goals / Non-Goals

**Goals:**
- EF Core + SQLite wired into the Minimal API with a single `AppDbContext`.
- A first `User` entity (`Id`, `Email`, `CreatedAt`) and its schema via an initial migration.
- Database exists on first run without manual steps (migrations applied at startup).
- One read endpoint (`GET /users`) so data flow is visible end to end.
- Teaching notes explaining ORM, EF Core, SQLite, and migrations.

**Non-Goals:**
- No password hashing or credential fields on `User` (module 02).
- No registration/login, user store, or ASP.NET Core Identity (module 03).
- No authentication, authorization, or write/validation endpoints.
- No production database or repository/service abstraction layer.

## Decisions

- **SQLite provider.** Zero classroom setup — a single file, no server. Connection string `Data Source=eduauth.db` in `appsettings.json` under `ConnectionStrings:DefaultConnection`. Alternative (in-memory provider) rejected: it hides migrations, which are a teaching goal; (SQL Server/Postgres) rejected: install friction.

- **Single `AppDbContext` in `Data/`, entity in `Entities/`.** Matches the folder story the README already advertises (`Data/`, `Entities/`). One context keeps the surface small. `DbSet<User> Users` is the only set this module.

- **`User.Id` as `Guid`.** Non-guessable identifier that carries into auth modules without exposing a sequential count. Generated in app (`Guid.NewGuid()`) so the entity is valid before it reaches the database. Alternative (int identity) rejected: leaks row counts and is a weaker fit for auth.

- **No credential fields yet.** `User` is deliberately dumb this module. Module 02 adds a password hash to it; keeping it minimal now makes that diff the teaching moment.

- **Apply migrations at startup via `db.Database.Migrate()`.** Students run the app and the DB just works. `shortcut: Migrate()-at-startup is a teaching convenience, not production-safe (races under multiple instances, runs migrations with app privileges); switch to a controlled migration step before any real deployment.`

- **`GET /users` reads directly from `AppDbContext`.** No repository/service layer — the course favors a visible request path in Minimal APIs. Abstractions get introduced only when a module needs them.

## Risks / Trade-offs

- [SQLite file committed by accident] → add the `*.db` (and `-shm`/`-wal`) pattern to `.gitignore`.
- [`Migrate()` at startup is not production-safe] → accepted for a local teaching run; documented as a shortcut and called out in `docs/module-01.md`'s "teaching vs production" note.
- [EF Core design-time tools needed for `dotnet ef`] → add `Microsoft.EntityFrameworkCore.Design` as a package reference so `dotnet ef migrations add` works without a global tool dependency; `dotnet-ef` CLI may still need installing on the student's machine (documented in the run steps).
- [No automated test yet] → tests arrive in module 09; module 01 is verified by `dotnet build`, applying the migration, and a manual `GET /users` returning `200 []`.

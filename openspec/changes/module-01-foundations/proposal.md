## Why

Module 00 left a clean Minimal API with no persistence. Before any auth concept (passwords, identity, tokens) can be taught, the course needs a place to store users. This module introduces the data layer — Entity Framework Core over SQLite — so later modules have a database to build on. SQLite is chosen for zero classroom setup: a single file, no server to install.

## What Changes

- Add EF Core with the SQLite provider and the design-time tools (migrations).
- Add an `AppDbContext` exposing a `Users` set.
- Add a first entity: `User` with `Id` (Guid), `Email`, and `CreatedAt`. No password or auth fields yet — those arrive in modules 02 and 03.
- Add a SQLite connection string to configuration and register `AppDbContext` in `Program.cs`.
- Add the initial EF Core migration and apply it at startup so the database exists on first run (teaching convenience).
- Expose `GET /users` returning the persisted users as JSON, so students see data flow end to end.
- Add teaching material: `docs/module-01.md` (what an ORM is, why EF Core + SQLite, migrations, and how to run).

Out of scope (deferred): password hashing (module 02), registration/login and a real user store (module 03), and any authentication or authorization.

## Capabilities

### New Capabilities
- `data-layer`: Relational persistence for the API — an EF Core `DbContext` over SQLite, the `User` entity and its schema via migrations, automatic schema application at startup, and a read endpoint that lists persisted users.

### Modified Capabilities
<!-- None: the api-foundation capability's requirements are unchanged; this change only adds persistence and one new endpoint. -->

## Impact

- `src/EduAuth.Api/EduAuth.Api.csproj`: add `Microsoft.EntityFrameworkCore.Sqlite` and `Microsoft.EntityFrameworkCore.Design`.
- `src/EduAuth.Api/Entities/User.cs`: new entity.
- `src/EduAuth.Api/Data/AppDbContext.cs`: new `DbContext` with `DbSet<User> Users`.
- `src/EduAuth.Api/Program.cs`: register `AppDbContext` with SQLite, apply migrations at startup, map `GET /users`.
- `src/EduAuth.Api/appsettings.json`: add the `DefaultConnection` SQLite connection string.
- `src/EduAuth.Api/Migrations/`: initial migration (generated).
- `src/EduAuth.Api/EduAuth.Api.http`: add a `GET /users` request.
- `docs/module-01.md`: new teaching notes.
- Target framework stays `net10.0`. The generated SQLite file (e.g. `eduauth.db`) is ignored by git.

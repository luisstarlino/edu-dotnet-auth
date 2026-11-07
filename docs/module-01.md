# Module 01 — Foundations: EF Core & SQLite

Module 00 gave us a running API with no memory — restart it and everything is
gone. Before we can talk about users, passwords or tokens, we need somewhere to
**store** data. This module adds the data layer.

## What is an ORM, and why EF Core?

An **ORM** (Object-Relational Mapper) lets you work with database rows as plain
C# objects instead of writing SQL by hand. You describe a `User` class; the ORM
figures out the `Users` table, the `INSERT`, the `SELECT`.

**Entity Framework Core** is the ORM that ships with .NET. We pair it with
**SQLite** — a database that is just a single file on disk. No server to
install, no credentials, nothing to configure. Perfect for a classroom: clone,
run, done.

| Piece | Role |
|-------|------|
| **Entity** (`User`) | A C# class that maps to a table |
| **DbContext** (`AppDbContext`) | The session with the database; exposes `DbSet<User> Users` |
| **Provider** (SQLite) | Translates EF Core calls into SQL for a specific database |
| **Migration** | A versioned script that creates/changes the schema |

## The first entity

```csharp
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

Deliberately minimal — **no password, no auth fields yet**. We use a `Guid` id
(not a sequential `int`) so identifiers don't leak how many users exist and
carry cleanly into the auth modules. Passwords arrive in **module 02**, real
registration and login in **module 03**.

## Migrations

A migration is a snapshot of your schema turned into code. We created the first
one with:

```bash
dotnet ef migrations add InitialCreate --project src/EduAuth.Api
```

That generated the `Migrations/` folder describing a `Users` table. The app
applies pending migrations **at startup**, so the SQLite file (`eduauth.db`) is
created automatically the first time you run it.

## Seeding

An empty database is a boring demo. **Seeding** means putting some initial data
in place so the app has something to work with. We do it at startup, right after
the migration runs:

```csharp
db.Database.Migrate();

if (!db.Users.Any())
{
    db.Users.AddRange(
        new User { Email = "luis@edu.dev" },
        new User { Email = "starlino@edu.dev" });
    db.SaveChanges();
}
```

The `if (!db.Users.Any())` check is what makes this **idempotent**: it only
inserts when the table is empty, so restarting the app never creates duplicates.
That's the whole idea of a seed — *inject the rows that aren't there yet, and
leave the rest alone*.

> There are other ways to seed (EF Core's `HasData` bakes the data into a
> migration; EF Core 9+ has a `UseSeeding` hook). We use the plain startup check
> because it reads top-to-bottom and also shows `AddRange` + `SaveChanges`.

## What this module ships

- EF Core + the SQLite provider wired into the Minimal API.
- An `AppDbContext` with a `Users` set.
- The `User` entity and its initial migration.
- A startup **seed** that adds two users to an empty database.
- `GET /users` → `200 OK` with a JSON array of the seeded users.

## Run it

```bash
# (once) install the EF Core CLI if you don't have it
dotnet tool install --global dotnet-ef

dotnet run --project src/EduAuth.Api
```

The database is created and seeded on startup. Then list the users:

```bash
curl http://localhost:5163/users
# [{"id":"...","email":"luis@edu.dev","createdAt":"..."},
#  {"id":"...","email":"starlino@edu.dev","createdAt":"..."}]
```

Real registration (users creating themselves) starts in module 03. Prefer the
editor? Open `src/EduAuth.Api/EduAuth.Api.http` and send the `GET /users`
request.

## Teaching vs production

> [!WARNING]
> We apply migrations at **startup** (`db.Database.Migrate()`) purely for
> classroom convenience. In production this is risky: multiple instances can
> race, and it runs schema changes with the app's privileges. Real systems run
> migrations as a separate, controlled deployment step. SQLite itself is great
> for learning but you'd use a server database (PostgreSQL, SQL Server) in
> production.

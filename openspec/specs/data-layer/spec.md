# data-layer Specification

## Purpose
TBD - created by archiving change module-01-foundations. Update Purpose after archive.
## Requirements
### Requirement: EF Core over SQLite

The API SHALL use Entity Framework Core with the SQLite provider for persistence. An `AppDbContext` MUST be registered in dependency injection and MUST read its connection string from configuration (`ConnectionStrings:DefaultConnection`).

#### Scenario: DbContext resolves at runtime

- **WHEN** the application starts
- **THEN** `AppDbContext` can be resolved from the service provider and is configured to use the SQLite connection string from configuration

### Requirement: User entity persisted

The data layer SHALL define a `User` entity with an `Id` (Guid), an `Email` (string), a `PasswordHash` (string), and a `CreatedAt` timestamp, exposed through `AppDbContext.Users`. `PasswordHash` MUST be a required column holding a BCrypt hash, and `Email` MUST have a unique index so no two users share an email. Plaintext passwords MUST NOT be stored.

#### Scenario: Users set is available

- **WHEN** code queries `AppDbContext.Users`
- **THEN** it returns `User` records with `Id`, `Email`, `PasswordHash`, and `CreatedAt`

#### Scenario: Email is unique

- **WHEN** a second user is persisted with an email that already exists
- **THEN** the database rejects it via the unique index on `Email`

### Requirement: Schema created via migration at startup

The database schema SHALL be defined by an EF Core migration, and the application SHALL apply pending migrations at startup so the SQLite database exists on first run.

#### Scenario: Database ready on first run

- **WHEN** the application starts against a database with pending migrations
- **THEN** the migrations are applied and the `Users` table exists before any request is served

### Requirement: List users endpoint

The API SHALL expose `GET /users` that returns HTTP 200 with a JSON array of the persisted users, and requires no authentication.

#### Scenario: Listing users

- **WHEN** a client sends `GET /users`
- **THEN** the API responds with HTTP 200 and a JSON array of users read from the database

#### Scenario: Empty database

- **WHEN** a client sends `GET /users` and no users exist
- **THEN** the API responds with HTTP 200 and an empty JSON array


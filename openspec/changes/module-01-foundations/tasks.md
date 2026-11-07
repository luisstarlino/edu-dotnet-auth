## 1. Add EF Core dependencies

- [ ] 1.1 Add `Microsoft.EntityFrameworkCore.Sqlite` package to `src/EduAuth.Api/EduAuth.Api.csproj`
- [ ] 1.2 Add `Microsoft.EntityFrameworkCore.Design` package (needed for `dotnet ef`)
- [ ] 1.3 Add `*.db`, `*.db-shm`, `*.db-wal` to `.gitignore`

## 2. Model the data layer

- [ ] 2.1 Create `src/EduAuth.Api/Entities/User.cs` with `Id` (Guid), `Email` (string), `CreatedAt` (DateTime)
- [ ] 2.2 Create `src/EduAuth.Api/Data/AppDbContext.cs` with `DbSet<User> Users`
- [ ] 2.3 Add `ConnectionStrings:DefaultConnection` = `Data Source=eduauth.db` to `appsettings.json`

## 3. Wire it into the app

- [ ] 3.1 In `Program.cs`, register `AppDbContext` with `UseSqlite(...)` reading `DefaultConnection`
- [ ] 3.2 Apply pending migrations at startup (`db.Database.Migrate()` inside a scope)
- [ ] 3.3 Map `GET /users` returning the users from `AppDbContext` as JSON

## 4. Create the initial migration

- [ ] 4.1 Run `dotnet ef migrations add InitialCreate --project src/EduAuth.Api` to generate `Migrations/`
- [ ] 4.2 Confirm the migration creates the `Users` table with the three columns

## 5. Teaching material

- [ ] 5.1 Create `docs/module-01.md`: what an ORM is, why EF Core + SQLite, migrations, how to run, and a teaching-vs-production note on startup migration

## 6. Verify

- [ ] 6.1 `dotnet build edu-dotnet-auth.slnx` succeeds with 0 warnings/errors
- [ ] 6.2 Run the API; confirm the migration applies and `eduauth.db` is created
- [ ] 6.3 `GET /users` returns HTTP 200 with `[]` on an empty database
- [ ] 6.4 Update `src/EduAuth.Api/EduAuth.Api.http` with a `GET /users` request

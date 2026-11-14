## 1. Persist the credential

- [x] 1.1 Add a required `PasswordHash` (string) property to `src/EduAuth.Api/Entities/User.cs`
- [x] 1.2 Configure a unique index on `User.Email` in `src/EduAuth.Api/Data/AppDbContext.cs` (`OnModelCreating`)
- [x] 1.3 Add an EF Core migration for the `PasswordHash` column and the unique email index

## 2. Keep the seed loginable

- [x] 2.1 Update the startup seed in `Program.cs` to set each demo user's `PasswordHash` via `IPasswordHasher.Hash` with a fixed, documented password

## 3. Registration endpoint

- [x] 3.1 Add `RegisterRequest(string Email, string Password)` DTO
- [x] 3.2 Map `POST /auth/register`: reject with `409` if the email already exists, otherwise persist a `User` with the hashed password and return `200` with `{ id, email }` (never the hash)

## 4. Login endpoint

- [x] 4.1 Add `LoginRequest(string Email, string Password)` DTO
- [x] 4.2 Map `POST /auth/login`: look up by email and `Verify` the password; return `200 { authenticated = true }` on success, `401` for both unknown email and wrong password

## 5. Teaching material

- [x] 5.1 Create `docs/module-03.md`: how register/login reuse the hasher, why the hash never leaves the server, why login returns an identical `401` for unknown email vs wrong password, the demo seed password, and the teaching-vs-production note (no token yet, no validation/rate limiting)

## 6. Verify

- [x] 6.1 `dotnet build edu-dotnet-auth.slnx` succeeds with 0 warnings/errors
- [x] 6.2 `POST /auth/register` with a new email returns `200` with `id`/`email` and no hash; the same email again returns `409`
- [x] 6.3 `POST /auth/login` returns `200` for a correct password, `401` for a wrong password, and `401` for an unknown email
- [x] 6.4 Update `src/EduAuth.Api/EduAuth.Api.http` with the register and login requests

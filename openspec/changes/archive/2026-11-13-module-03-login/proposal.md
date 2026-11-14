## Why

Modules 01 and 02 built the pieces in isolation: a `User` store (no credentials) and a reusable `IPasswordHasher` (no persistence). Module 03 joins them into the first real authentication flow a student can use end to end — register an account, then log in. This is where "hash a password" stops being a demo and becomes how an account is actually created and checked.

## What Changes

- Add a required `PasswordHash` field to the `User` entity and a migration for the new column. The hash is written on registration and never returned in any response.
- Enforce one account per email with a unique index on `User.Email`.
- Update the startup seed so the two demo users have a known password (hashed via `IPasswordHasher`), making login demoable on first run.
- Add an `authentication` capability with two endpoints, both reusing the module 02 `IPasswordHasher`:
  - `POST /auth/register` → create a user with a hashed password; `409` if the email is already taken.
  - `POST /auth/login` → verify the password against the stored hash; `200` on success, `401` on bad credentials.
- Add teaching material: `docs/module-03.md` (how register/login use the hasher, why login returns the same `401` for unknown email and wrong password, why the hash never leaves the server, and why there is still no session/token yet).

Out of scope (deferred): sessions, cookies, or JWT tokens — login here only confirms the credentials are correct; issuing a token that keeps the user logged in is the next module. Also deferred: email format / password strength validation (a later validation module) and rate limiting / lockout (module 07).

## Capabilities

### New Capabilities
- `authentication`: Account registration and login. `POST /auth/register` persists a new user with a BCrypt-hashed password; `POST /auth/login` checks a submitted password against the stored hash and reports whether the credentials are valid. No token is issued yet.

### Modified Capabilities
- `data-layer`: The `User` entity gains a required `PasswordHash` field and a unique index on `Email`. Credentials are now persisted; the prior "no password fields" guarantee from module 01 is lifted.

## Impact

- `src/EduAuth.Api/Entities/User.cs`: add `PasswordHash`.
- `src/EduAuth.Api/Data/AppDbContext.cs`: configure the unique index on `Email`.
- New EF Core migration for the `PasswordHash` column and unique index.
- `src/EduAuth.Api/Program.cs`: seed a hashed password for demo users; map `POST /auth/register` and `POST /auth/login`; add register/login request DTOs.
- `src/EduAuth.Api/EduAuth.Api.http`: add register and login requests.
- `docs/module-03.md`: new teaching notes.
- Reuses the existing `IPasswordHasher` from module 02 unchanged. Target framework stays `net10.0`.

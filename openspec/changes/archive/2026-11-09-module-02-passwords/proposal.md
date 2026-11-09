## Why

Storing passwords as plain text is the classic catastrophic mistake: one database leak exposes every user's actual password, which they likely reuse elsewhere. Before the course builds registration and login (module 03), students need to understand how passwords are stored safely — hashed, salted, and never reversible. This module teaches that concept with a small, reusable hashing service.

## What Changes

- Add the `BCrypt.Net-Next` package.
- Add an `IPasswordHasher` abstraction with a BCrypt-backed implementation (`Hash` and `Verify`), registered in DI.
- Expose two demo endpoints so students can see the behavior end to end:
  - `POST /passwords/hash` → returns the BCrypt hash of a password.
  - `POST /passwords/verify` → returns whether a password matches a given hash.
- Add teaching material: `docs/module-02.md` (why never plain text, hashing vs encryption, what salting is, why the same password yields different hashes, and work factor).

Out of scope (deferred): persisting a password on `User`, registration, login, and the real user store — all of that is module 03. This module does not modify the `User` entity, the database schema, or the seed.

## Capabilities

### New Capabilities
- `password-hashing`: A reusable password hashing service — a BCrypt-backed `IPasswordHasher` with `Hash`/`Verify`, plus demo endpoints that hash a password and verify an attempt against a hash. Later modules use this to store and check user credentials.

### Modified Capabilities
<!-- None: data-layer and api-foundation requirements are unchanged. This change only adds a new capability. -->

## Impact

- `src/EduAuth.Api/EduAuth.Api.csproj`: add `BCrypt.Net-Next`.
- `src/EduAuth.Api/Services/IPasswordHasher.cs` and `BcryptPasswordHasher.cs`: new service.
- `src/EduAuth.Api/Program.cs`: register `IPasswordHasher`, map `POST /passwords/hash` and `POST /passwords/verify`.
- `src/EduAuth.Api/EduAuth.Api.http`: add the two password requests.
- `docs/module-02.md`: new teaching notes.
- No changes to `User`, `AppDbContext`, migrations, or the seed. Target framework stays `net10.0`.

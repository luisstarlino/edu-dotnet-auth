## Context

The course has a running Minimal API (module 00), an EF Core + SQLite `User` store (module 01), and a reusable `IPasswordHasher` (module 02). Each was built deliberately on its own. Module 03 is the payoff: the first authentication flow that uses all three — persist a hashed credential on register, read it back and verify on login. It stays intentionally short of sessions/tokens so the lesson is exactly "how is a credential stored and checked," not "how is a session kept."

## Goals / Non-Goals

**Goals:**
- Persist a password credential on `User` safely (hash only, written on register, never returned).
- `POST /auth/register` and `POST /auth/login` built on the module 02 `IPasswordHasher`.
- Teach the security reflexes: identical `401` for unknown email vs wrong password, one account per email, the hash never leaves the server.
- Keep the seeded demo users loginable so the flow works on first run.

**Non-Goals:**
- No session, cookie, or JWT token — login only reports validity (next module).
- No email-format or password-strength validation (later validation module).
- No rate limiting / account lockout (module 07).
- No password reset or email verification.

## Decisions

- **Reuse `IPasswordHasher` unchanged.** Register calls `Hash`, login calls `Verify`. This is the whole point of extracting the service in module 02 — module 03 adds no crypto code of its own.

- **`PasswordHash` is a required (`non-null`) column on `User`.** Every account has a credential; there is no "passwordless" user in this course yet. The migration adds the column. Because existing rows (the seed) must have a value, the seed is updated in the same change rather than back-filling.

- **Login returns an opaque `401` for both unknown email and wrong password.** Never reveal which half failed — that leaks whether an email is registered. Documented in `docs/module-03.md` as a deliberate security choice, not an oversight.

- **No token yet — login responds `200 { "authenticated": true }`.** The module teaches credential checking; keeping a user logged in (session/JWT) is the next module. Returning a boolean result keeps the seam obvious for what comes next.

- **One account per email via a unique index on `User.Email`.** The register handler also checks first and returns `409 Conflict` for a friendly error, but the index is the real guarantee against a concurrent duplicate. `shortcut: emails are matched case-sensitively as stored; add normalization (lower-casing) when a real signup flow needs it.`

- **Seed users get a known password.** The two demo users are seeded with a fixed password (hashed through `IPasswordHasher`) and the password is stated in `docs/module-03.md`, so `POST /auth/login` works immediately on a fresh database.

- **Request bodies as small records.** `RegisterRequest(string Email, string Password)` and `LoginRequest(string Email, string Password)`, matching the course's explicit Minimal API style.

## Risks / Trade-offs

- [No input validation — empty or junk email/password is accepted] → acceptable for a teaching flow; format/strength validation is a dedicated later module, and `docs/module-03.md` flags it. Register still requires the fields to be present.
- [Demo seed password is public in the docs] → intentional, it is a local teaching database; real deployments never ship known credentials, noted in the teaching-vs-production section.
- [No automated test yet] → verified by `dotnet build`, then: register a new email (expect `200`, no hash in the body), register the same email again (expect `409`), log in with the right password (`200`), wrong password and unknown email (both `401`). Automated tests arrive in module 09.

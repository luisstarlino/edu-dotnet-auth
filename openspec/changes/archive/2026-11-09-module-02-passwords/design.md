## Context

The course has a running Minimal API (module 00) with an EF Core + SQLite data layer and a `User` entity (module 01). The next big step is real authentication, but before storing any credential the student must understand safe password storage. Module 02 is a focused, self-contained lesson: a reusable hashing service plus two demo endpoints. It deliberately does not persist anything — module 03 owns registration, login, and the user store.

## Goals / Non-Goals

**Goals:**
- Teach why plaintext is never stored, and what hashing + salting buy you.
- A small `IPasswordHasher` service (`Hash`/`Verify`) students can reuse in module 03.
- Two demo endpoints that make the behavior tangible (hash, verify, and "same password → different hash").
- Teaching notes: hashing vs encryption, salt, work factor.

**Non-Goals:**
- No `PasswordHash` field on `User`, no migration, no seed change (module 03).
- No registration, login, tokens, or authentication.
- No account lockout / rate limiting (module 07).

## Decisions

- **BCrypt via `BCrypt.Net-Next`.** Adaptive hash with a tunable work factor and the salt embedded in the output string — so `Verify` needs only the stored hash, nothing else. Very teachable. Alternatives: ASP.NET Core Identity's `PasswordHasher<T>` (rejected here to keep module 02 independent of Identity, which is module 03's topic); hand-rolled PBKDF2 (rejected: more code and easy to get wrong — the point is the concept, not a DIY crypto implementation).

- **`IPasswordHasher` abstraction over the concrete hasher.** A one-method-pair interface keeps the call sites (and module 03) decoupled from BCrypt, so the algorithm can change later without touching callers. Registered as a singleton — the hasher is stateless.

- **Work factor 12.** A sensible default that is clearly "slow on purpose" without making the demo sluggish. Documented in the teaching notes as the knob you raise as hardware gets faster. `shortcut: work factor is hard-coded at 12; make it configurable if a real deployment needs to tune cost.`

- **Two demo endpoints, no persistence.** `POST /passwords/hash` and `POST /passwords/verify` let students poke the behavior from the `.http` file or curl and literally see two different hashes for the same input. Keeping them stateless avoids pulling in the user store early.

- **Request bodies as small records.** Minimal API handlers bind simple DTOs (e.g. `HashRequest(string Password)`), matching the course's readable, explicit style.

## Risks / Trade-offs

- [Demo endpoints that hash arbitrary input could be abused as a CPU sink] → acceptable for a local teaching API; real hashing lives behind auth + rate limiting from module 07. Noted in the teaching-vs-production section.
- [Students might think the demo endpoints are "the auth system"] → `docs/module-02.md` states explicitly these are illustrations; real register/login arrives in module 03.
- [No automated test yet] → verified by `dotnet build`, then calling both endpoints: hash a password, confirm two calls differ, verify matching and non-matching attempts. Automated tests arrive in module 09.

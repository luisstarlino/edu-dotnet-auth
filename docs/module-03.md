# Module 03 — Registration & Login

Module 01 gave us a user store; module 02 gave us a safe way to hash a password.
Module 03 joins them: an account is **created** (register) and **checked**
(login). This is the first real authentication flow in the course.

## The credential lives on the user

`User` gains one new field: `PasswordHash`. On registration we hash the password
with the module 02 `IPasswordHasher` and store *only the hash* — the plaintext is
never written, never logged, never returned. A new EF Core migration adds the
column (and the unique email index below).

## Register

`POST /auth/register` takes an email and password:

1. If the email is already taken → `409 Conflict`.
2. Otherwise hash the password, persist the user, return `200` with `{ id, email }`.

The response deliberately contains **no hash** — the client never needs it, so it
never leaves the server.

### One account per email

We enforce uniqueness two ways. The handler checks first so it can return a
friendly `409`, but the real guarantee is a **unique index on `Email`** in the
database: even if two registrations race, the index rejects the duplicate. App
checks are convenience; the constraint is correctness.

## Login

`POST /auth/login` takes an email and password, looks the user up, and uses
`IPasswordHasher.Verify` to compare the attempt against the stored hash:

- Match → `200 { "authenticated": true }`
- No match → `401`

### Why the same 401 for "unknown email" and "wrong password"

Both failures return the *identical* `401`. If "unknown email" looked different
from "wrong password", an attacker could probe which emails are registered — a
privacy and security leak. Same response, no information given away. This is a
deliberate choice, not an oversight.

## No token yet

Login here only answers *"are these credentials valid?"*. It does **not** keep
you logged in — there's no session or token, so the next request doesn't know who
you are. Issuing something that persists the login (a JWT / session) is the next
module. Keeping that out makes this module exactly one idea: checking a stored
credential.

## What this module ships

- `PasswordHash` on `User` + a migration and a unique `Email` index.
- `POST /auth/register` → `{ id, email }` (or `409`).
- `POST /auth/login` → `{ authenticated: true }` (or `401`).
- The two seeded demo users now have a password so login works on first run.

## Try it

```bash
dotnet run --project src/EduAuth.Api
```

Register a new account:

```bash
curl -X POST http://localhost:5163/auth/register \
  -H "Content-Type: application/json" -d '{"email":"new@edu.dev","password":"s3cret"}'
# {"id":"...","email":"new@edu.dev"}
```

Log in as a **seeded** user — the demo password is `password123`:

```bash
curl -X POST http://localhost:5163/auth/login \
  -H "Content-Type: application/json" -d '{"email":"luis@edu.dev","password":"password123"}'
# {"authenticated":true}   (wrong password or unknown email → 401)
```

Or open `src/EduAuth.Api/EduAuth.Api.http` (or `/scalar`) and send the register
and login requests.

## Teaching vs production

> [!WARNING]
> The seeded users ship with a known password (`password123`) so the lesson works
> out of the box — a real system never ships known credentials. This module also
> has **no input validation** (any string is an acceptable email/password) and
> **no rate limiting**: both let an attacker hammer `/auth/login`. Validation
> comes in a later module and rate limiting / lockout in module 07. Emails are
> matched exactly as stored (case-sensitive); a real signup normalizes them first.

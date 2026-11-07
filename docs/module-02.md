# Module 02 — Passwords: Hashing & Salting

Module 01 gave us a place to store users. Now the single most important rule
before we ever store a credential: **never store a password as plain text.**

## Why never plain text?

If your database leaks — and databases do leak — plain-text passwords hand the
attacker every account instantly. Worse, people reuse passwords, so you've also
handed over their email, bank, and everything else. The fix: store something you
can *check against* but never *read back*.

## Hashing ≠ encryption

| | Encryption | Hashing |
|---|---|---|
| **Reversible?** | Yes, with the key | **No** — one-way |
| **Use for passwords?** | No | **Yes** |
| **Goal** | Get the data back later | Prove a guess matches, nothing more |

We never need the original password back. When a user logs in, we hash their
*attempt* and compare it to the stored hash. That's why hashing — a one-way
function — is the right tool.

## Salting: why the same password gives different hashes

A plain hash has a weakness: identical passwords produce identical hashes, so an
attacker can precompute hashes of common passwords (a "rainbow table") and match
them in bulk. A **salt** is random data mixed in before hashing, unique per
password. Same password, different salt → different hash. Rainbow tables stop
working.

BCrypt generates the salt for you and **embeds it in the hash string**, so to
verify later you only need the stored hash — the salt travels with it.

## Work factor

BCrypt is *deliberately slow*. The **work factor** (we use `12`) sets how many
rounds it runs — each step up doubles the cost. Slow is good here: a legitimate
login barely notices, but brute-forcing millions of guesses becomes impractical.
As hardware gets faster, you raise the number.

## What this module ships

- An `IPasswordHasher` service with a BCrypt implementation (`Hash` / `Verify`).
- `POST /passwords/hash` → `{ "hash": "..." }`
- `POST /passwords/verify` → `{ "valid": true | false }`
- **Scalar**, an interactive API UI at `/scalar` (development only).

Nothing is persisted — these endpoints exist so you can *see* the behavior. Real
registration and login (storing a user's hash, checking it at sign-in) arrive in
module 03.

## An interface for the API: Scalar

Typing `curl` gets old. **Scalar** reads the OpenAPI document the API already
produces and turns it into a browser UI where you can see every endpoint and
call it with a form. Run the API and open:

```
http://localhost:5163/scalar
```

Try the password endpoints there: hash `s3cret`, copy the hash, paste it into
`verify`, and watch `valid` flip between `true` and `false`. It's wired up only
in development — production APIs don't expose their docs UI by default.

## Try it

```bash
dotnet run --project src/EduAuth.Api
```

Hash a password — run it twice and notice the two hashes differ (that's the
salt):

```bash
curl -X POST http://localhost:5163/passwords/hash \
  -H "Content-Type: application/json" -d '{"password":"s3cret"}'
# {"hash":"$2a$12$...."}
```

Then verify an attempt against a hash you got above:

```bash
curl -X POST http://localhost:5163/passwords/verify \
  -H "Content-Type: application/json" \
  -d '{"password":"s3cret","hash":"$2a$12$...."}'
# {"valid":true}   ("wrong" → {"valid":false})
```

Or open `src/EduAuth.Api/EduAuth.Api.http` and send the two requests.

## Teaching vs production

> [!WARNING]
> These demo endpoints hash arbitrary input with no authentication or rate
> limiting — fine for a local lesson, but in production, hashing only happens
> behind register/login, and those endpoints are rate-limited (module 07) to
> stop abuse. The work factor is hard-coded at `12` here; a real app makes it
> configurable so it can be raised over time.

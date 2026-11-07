# Module 00 — Presentation & First Run

Welcome. Before we touch a single password or token, this module sets the stage:
what the course is about, and a clean API that actually runs.

## Authentication vs Authorization

Two words that get mixed up constantly. They are different questions:

| | Authentication | Authorization |
|---|---|---|
| **Question** | *Who are you?* | *What are you allowed to do?* |
| **Happens** | First | After authentication |
| **Fails with** | `401 Unauthorized` | `403 Forbidden` |
| **Analogy** | Showing ID at the door | Checking if your ticket is VIP |

A request is first **authenticated** (we figure out who you are), then
**authorized** (we decide if you may do the thing). Keep that order in your head —
the whole course is built on it.

## The course roadmap

Each module lives in its own branch and builds on the previous one.

| # | Module | What you learn |
|:-:|--------|----------------|
| 00 | Presentation | Concepts, roadmap, first run *(you are here)* |
| 01 | Foundations | Minimal APIs, EF Core, SQLite |
| 02 | Passwords | Hashing, salting, why never plain text |
| 03 | Identity | Register, login, user store |
| 04 | JWT | Token anatomy, signing, validation |
| 05 | Refresh Tokens | Rotation, expiration, revocation |
| 06 | Roles & Policies | Roles, claims, custom policies |
| 07 | Security Hardening | Rate limiting, lockout, brute-force, timing attacks |
| 08 | External Login | OAuth 2.0 vs OIDC with Google / GitHub |
| 09 | Testing | WebApplicationFactory, integration tests, faking auth |

`main` holds the final version with everything together.

## What this module ships

We start from the default ASP.NET template and strip it down to a clean
**Minimal API** with one endpoint — no controllers, no database, no auth yet:

- `GET /health` → `200 OK` with `{ "status": "ok" }`

That's on purpose. A tiny surface means the next module can introduce the data
layer without fighting leftover scaffolding.

## Run it

```bash
dotnet run --project src/EduAuth.Api
```

Then check the health endpoint:

```bash
curl http://localhost:5163/health
# {"status":"ok"}
```

In development you also get the OpenAPI document at `/openapi/v1.json`.
Prefer the editor? Open `src/EduAuth.Api/EduAuth.Api.http` and send the request.

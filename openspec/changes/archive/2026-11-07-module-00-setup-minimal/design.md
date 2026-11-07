## Context

The repo starts from the default `dotnet new webapi` scaffold: controller-based, with the Weather template. The course teaches auth step by step with one module per branch; module 00 is the "presentation + first run" module. It must leave a clean Minimal API baseline that module 01 (EF Core + SQLite) builds on. Target framework is `net10.0`, project at `src/EduAuth.Api`.

## Goals / Non-Goals

**Goals:**
- A clean Minimal API host that builds and runs.
- One trivial, unauthenticated endpoint (`GET /health`) to prove the API is up.
- OpenAPI retained for development.
- Short teaching material: authentication vs authorization, the roadmap, how to run.

**Non-Goals:**
- No EF Core, SQLite, or entities (module 01).
- No Scalar UI, auth logic, or extra endpoints.
- No new NuGet packages.

## Decisions

- **Minimal APIs over controllers.** The course favors clarity and small, readable surface area; Minimal APIs keep the whole request path visible in `Program.cs`. Alternative (keep controllers) rejected: more ceremony, hides the pipeline from beginners.
- **`GET /health` as the single endpoint.** A conventional, self-explanatory "is it up?" probe returning `{ "status": "ok" }`. Alternative (`GET /` hello) rejected: `/health` reads as intentional and carries into later modules.
- **Keep `AddOpenApi()`/`MapOpenApi()`, dev-only.** Already present in the scaffold; no reason to remove. Scalar is deferred to avoid adding a dependency this early.
- **Teaching notes in `docs/`.** Markdown next to the code so students read concepts in the same checkout as the branch. Keeps `main`'s "all presentations" goal working as branches accumulate.

## Risks / Trade-offs

- [Removing the Weather template leaves `Controllers/` empty] → delete the folder so the project tree matches the Minimal API story.
- [`UseHttpsRedirection` with the dev HTTP profile can warn/redirect oddly] → acceptable for a local teaching run; revisit when HTTPS is taught. `shortcut: HTTPS behavior not hardened in module 00, revisit when TLS/token transport is taught`.
- [No automated test yet; tests arrive in module 09] → module 00 is verified by `dotnet build` + a manual `GET /health`. Acceptable for an intro module.

## Why

This is the opening module of a hands-on course that teaches authentication and authorization in .NET. Before any auth concept can be taught, students need a clean, running API and a shared mental model of what the course is about. The default `dotnet new webapi` scaffold ships a controller-based Weather template that is noise for this course — it must be removed so module 01 can start the data layer from a clear baseline.

## What Changes

- **BREAKING** Remove the Weather template: delete `WeatherForecastController` and the `WeatherForecast` model.
- Convert the API from controller-based (`AddControllers`/`MapControllers`) to **Minimal APIs**.
- Expose a single trivial endpoint (`GET /health`) returning a static status payload, so students can confirm the API runs.
- Keep OpenAPI document generation (`AddOpenApi`/`MapOpenApi` in development).
- Add teaching material for the module: authentication vs authorization, the course roadmap, and how to run the empty API.

Out of scope (deferred): EF Core, SQLite, entities, Scalar UI, and any auth logic. Those begin in module 01.

## Capabilities

### New Capabilities
- `api-foundation`: The minimal running web API surface — Minimal API host, a health endpoint, and OpenAPI document generation in development. This is the baseline every later module builds on.

### Modified Capabilities
<!-- None: no existing specs in openspec/specs/. -->

## Impact

- `src/EduAuth.Api/Program.cs`: rewritten for Minimal APIs (remove `AddControllers`/`MapControllers`, add the `GET /health` endpoint).
- `src/EduAuth.Api/Controllers/WeatherForecastController.cs`: deleted (and the now-empty `Controllers/` folder).
- `src/EduAuth.Api/WeatherForecast.cs`: deleted.
- `src/EduAuth.Api/EduAuth.Api.http`: updated to call `GET /health` instead of the weather route.
- `docs/`: new module-00 teaching notes (authn vs authz, roadmap, run instructions).
- No new NuGet dependencies; no database; target framework stays `net10.0`.

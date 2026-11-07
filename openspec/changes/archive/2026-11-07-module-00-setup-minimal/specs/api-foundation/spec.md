## ADDED Requirements

### Requirement: Minimal API host

The API SHALL be hosted using ASP.NET Core Minimal APIs. It MUST NOT register MVC controllers (`AddControllers`/`MapControllers`), and the Weather template (`WeatherForecastController`, `WeatherForecast`) MUST be removed.

#### Scenario: Application starts without controllers

- **WHEN** the application starts
- **THEN** it builds and runs with no controller services registered and no Weather endpoints exposed

### Requirement: Health endpoint

The API SHALL expose `GET /health` that returns HTTP 200 with a JSON body containing a static status field (e.g. `{ "status": "ok" }`) and requires no authentication.

#### Scenario: Health check returns ok

- **WHEN** a client sends `GET /health`
- **THEN** the API responds with HTTP 200 and a JSON body whose `status` is `"ok"`

### Requirement: OpenAPI document in development

The API SHALL generate an OpenAPI document and expose it only when running in the Development environment.

#### Scenario: OpenAPI available in development

- **WHEN** the API runs with environment Development
- **THEN** the OpenAPI document endpoint is reachable

#### Scenario: OpenAPI hidden outside development

- **WHEN** the API runs with a non-Development environment
- **THEN** the OpenAPI document endpoint is not mapped

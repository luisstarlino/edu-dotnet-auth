## 1. Remove the Weather template

- [x] 1.1 Delete `src/EduAuth.Api/Controllers/WeatherForecastController.cs` and the now-empty `Controllers/` folder
- [x] 1.2 Delete `src/EduAuth.Api/WeatherForecast.cs`

## 2. Convert to Minimal APIs

- [x] 2.1 In `Program.cs`, remove `AddControllers()` and `MapControllers()`
- [x] 2.2 Map `GET /health` returning `{ "status": "ok" }` with HTTP 200
- [x] 2.3 Keep `AddOpenApi()` and the dev-only `MapOpenApi()`

## 3. Teaching material

- [x] 3.1 Create `docs/module-00.md`: authentication vs authorization, the course roadmap, and how to run the API

## 4. Verify

- [x] 4.1 `dotnet build edu-dotnet-auth.slnx` succeeds with 0 warnings/errors
- [x] 4.2 Run the API and confirm `GET /health` returns 200 `{ "status": "ok" }`
- [x] 4.3 Update `src/EduAuth.Api/EduAuth.Api.http` to call `GET /health`

## 1. Add the BCrypt dependency

- [x] 1.1 Add `BCrypt.Net-Next` package to `src/EduAuth.Api/EduAuth.Api.csproj`

## 2. Build the hashing service

- [x] 2.1 Create `src/EduAuth.Api/Services/IPasswordHasher.cs` with `string Hash(string password)` and `bool Verify(string password, string hash)`
- [x] 2.2 Create `src/EduAuth.Api/Services/BcryptPasswordHasher.cs` using `BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12)` and `Verify`
- [x] 2.3 Register `IPasswordHasher` as a singleton in `Program.cs`

## 3. Demo endpoints

- [x] 3.1 Add request DTOs (e.g. `HashRequest(string Password)`, `VerifyRequest(string Password, string Hash)`)
- [x] 3.2 Map `POST /passwords/hash` returning `{ "hash": ... }`
- [x] 3.3 Map `POST /passwords/verify` returning `{ "valid": ... }`

## 4. Teaching material

- [x] 4.1 Create `docs/module-02.md`: why never plain text, hashing vs encryption, salting (same password → different hash), work factor, and a teaching-vs-production note

## 5. Verify

- [x] 5.1 `dotnet build edu-dotnet-auth.slnx` succeeds with 0 warnings/errors
- [x] 5.2 `POST /passwords/hash` with a password returns a hash that differs from the input; calling it twice yields two different hashes
- [x] 5.3 `POST /passwords/verify` returns `valid: true` for the matching password and `valid: false` for a wrong one
- [x] 5.4 Update `src/EduAuth.Api/EduAuth.Api.http` with the two password requests

## 6. Scalar API UI (added during implementation)

- [x] 6.1 Add `Scalar.AspNetCore` package
- [x] 6.2 Map `MapScalarApiReference()` in the development pipeline (serves `/scalar` from the OpenAPI document)
- [x] 6.3 Document the Scalar UI in `docs/module-02.md`

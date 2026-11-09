# password-hashing Specification

## Purpose
TBD - created by archiving change module-02-passwords. Update Purpose after archive.
## Requirements
### Requirement: Password hashing service

The API SHALL provide an `IPasswordHasher` service, registered in dependency injection, with a `Hash(password)` operation that returns a salted BCrypt hash and a `Verify(password, hash)` operation that returns whether the password matches the hash. The service MUST NOT store or log plaintext passwords.

#### Scenario: Hashing a password

- **WHEN** `Hash` is called with a password
- **THEN** it returns a BCrypt hash string that is not equal to the plaintext password

#### Scenario: Salt makes equal passwords hash differently

- **WHEN** `Hash` is called twice with the same password
- **THEN** the two resulting hashes are different, yet each verifies against that password

### Requirement: Hash endpoint

The API SHALL expose `POST /passwords/hash` that accepts a JSON body with a `password` field and returns HTTP 200 with the computed hash. It requires no authentication.

#### Scenario: Hash a password over HTTP

- **WHEN** a client sends `POST /passwords/hash` with `{ "password": "s3cret" }`
- **THEN** the API responds with HTTP 200 and a JSON body containing a non-empty `hash` that differs from the submitted password

### Requirement: Verify endpoint

The API SHALL expose `POST /passwords/verify` that accepts a JSON body with `password` and `hash` fields and returns HTTP 200 with a boolean result indicating whether the password matches the hash. It requires no authentication.

#### Scenario: Correct password verifies

- **WHEN** a client sends `POST /passwords/verify` with a password and its matching hash
- **THEN** the API responds with HTTP 200 and `valid` is `true`

#### Scenario: Wrong password fails

- **WHEN** a client sends `POST /passwords/verify` with a password that does not match the hash
- **THEN** the API responds with HTTP 200 and `valid` is `false`

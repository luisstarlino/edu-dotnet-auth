## MODIFIED Requirements

### Requirement: User entity persisted

The data layer SHALL define a `User` entity with an `Id` (Guid), an `Email` (string), a `PasswordHash` (string), and a `CreatedAt` timestamp, exposed through `AppDbContext.Users`. `PasswordHash` MUST be a required column holding a BCrypt hash, and `Email` MUST have a unique index so no two users share an email. Plaintext passwords MUST NOT be stored.

#### Scenario: Users set is available

- **WHEN** code queries `AppDbContext.Users`
- **THEN** it returns `User` records with `Id`, `Email`, `PasswordHash`, and `CreatedAt`

#### Scenario: Email is unique

- **WHEN** a second user is persisted with an email that already exists
- **THEN** the database rejects it via the unique index on `Email`

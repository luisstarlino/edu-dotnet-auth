## ADDED Requirements

### Requirement: User registration

The API SHALL expose `POST /auth/register` that accepts a JSON body with `email` and `password` fields, creates a new user with the password stored as a BCrypt hash (via `IPasswordHasher`), and returns HTTP 200 with the created user's `id` and `email`. The response MUST NOT include the password or its hash. If a user with the same email already exists, the API SHALL return HTTP 409 and create nothing.

#### Scenario: Register a new account

- **WHEN** a client sends `POST /auth/register` with an email not already in use and a password
- **THEN** the API responds with HTTP 200, a body containing the new user's `id` and `email`, and no password or hash field, and the user is persisted with a stored hash that is not the plaintext password

#### Scenario: Duplicate email is rejected

- **WHEN** a client sends `POST /auth/register` with an email that already belongs to a user
- **THEN** the API responds with HTTP 409 and no new user is created

### Requirement: User login

The API SHALL expose `POST /auth/login` that accepts a JSON body with `email` and `password` fields, looks up the user by email, and verifies the submitted password against the stored hash using `IPasswordHasher`. On a match the API SHALL return HTTP 200 indicating success. If the email is unknown or the password does not match, the API SHALL return HTTP 401 with the same response in both cases, revealing nothing about which part failed. No session or token is issued.

#### Scenario: Successful login

- **WHEN** a client sends `POST /auth/login` with a registered email and its correct password
- **THEN** the API responds with HTTP 200 indicating the credentials are valid

#### Scenario: Wrong password

- **WHEN** a client sends `POST /auth/login` with a registered email and an incorrect password
- **THEN** the API responds with HTTP 401

#### Scenario: Unknown email

- **WHEN** a client sends `POST /auth/login` with an email that is not registered
- **THEN** the API responds with HTTP 401, identical to the wrong-password response

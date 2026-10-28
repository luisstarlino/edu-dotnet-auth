<div align="center">

# 🔐 edu-dotnet-auth - Luis Starlino

### Learn authentication and authorization in .NET, step by step

From password hashing to JWT, refresh tokens and role-based access,
built as a hands-on course with one module per branch.

<br>

![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-13-239120?style=for-the-badge&logo=csharp&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-Auth-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-F7B93E?style=for-the-badge)
![Purpose](https://img.shields.io/badge/Purpose-Teaching-FF6B6B?style=for-the-badge&logo=bookstack&logoColor=white)

<br>

[📚 What You'll Learn](#-what-youll-learn) •
[🗺️ Learning Path](#️-learning-path) •
[🚀 Getting Started](#-getting-started) •
[🧠 Key Concepts](#-key-concepts) •
[🏆 Challenges](#-challenges)

</div>

<br>

---

## 📚 What You'll Learn

> **Authentication** answers *"who are you?"*
> **Authorization** answers *"what are you allowed to do?"*
> This repository teaches both, the right way.

<table>
<tr>
<td width="50%" valign="top">

### 🔑 Authentication
- Secure password hashing
- ASP.NET Core Identity
- Issuing and validating **JWT**
- Refresh token rotation
- Logout and token revocation

</td>
<td width="50%" valign="top">

### 🛡️ Authorization
- `[Authorize]` and `[AllowAnonymous]`
- Role-based access control (Rules)
- Claims and policies
- Custom authorization handlers
- Protecting Minimal API endpoints

</td>
</tr>
</table>

---

## 🗺️ Learning Path

Each module lives in its own branch, so students can follow the evolution of the code, compare changes and jump to any point of the course.

| # | Module | Branch | Topics |
|:-:|--------|--------|--------|
| 00 | 🧱 **Presentation** | `module-00-setup-minimal` | Project setup, Minimal APIs|
| 01 | 🧱 **Foundations** | `module-01-foundations` | Project setup, Minimal APIs, EF Core |
| 02 | 🔒 **Passwords** | `module-02-passwords` | Hashing, salting, why never plain text |
| 03 | 👤 **Identity** | `module-03-identity` | Register, login, user store |
| 04 | 🎫 **JWT** | `module-04-jwt` | Token anatomy, signing, validation |
| 05 | 🔄 **Refresh Tokens** | `module-05-refresh-tokens` | Rotation, expiration, revocation |
| 06 | 👑 **Roles & Policies** | `module-06-authorization` | Roles, claims, custom policies |
| 07 | 🌐 **External Login** | `module-07-external-login` | OAuth 2.0 with Google / GitHub |
| ✅ | 🎓 **Final Version** | `main` | Everything together |

```bash
# Jump to any module
git checkout module-04-jwt

# See what changed between two modules
git diff module-03-identity module-04-jwt
```

---

## 🔄 How It Works

### The authentication flow

```mermaid
sequenceDiagram
    autonumber
    actor U as 👤 User
    participant A as 🌐 API
    participant DB as 🗄️ Database

    U->>A: POST /auth/login (email + password)
    A->>DB: Find user and verify password hash
    DB-->>A: ✅ Valid user
    A-->>U: Access Token (15 min) + Refresh Token (7 days)

    U->>A: GET /orders (Bearer access token)
    A-->>U: 200 OK 📦

    Note over U,A: ⏰ Access token expires...

    U->>A: POST /auth/refresh (refresh token)
    A->>DB: Validate, revoke old, store new
    A-->>U: 🔁 New Access Token + New Refresh Token
```

### Inside the request pipeline

```mermaid
flowchart LR
    R["📨 Request"] --> AN{"🔑 Authentication<br/>Who are you?"}
    AN -- "No / invalid token" --> E401["❌ 401 Unauthorized"]
    AN -- "Valid token" --> AZ{"🛡️ Authorization<br/>Can you do this?"}
    AZ -- "Missing role/policy" --> E403["🚫 403 Forbidden"]
    AZ -- "Allowed" --> OK["✅ Endpoint runs"]

    style E401 fill:#ffe0e0,stroke:#e74c3c,color:#000
    style E403 fill:#fff1d6,stroke:#f39c12,color:#000
    style OK fill:#dcf7e3,stroke:#27ae60,color:#000
```

---

## 🧰 Tech Stack

| Tool | Purpose |
|------|---------|
| ![](https://img.shields.io/badge/-ASP.NET%20Core-512BD4?logo=dotnet&logoColor=white) | Web API with Minimal APIs |
| ![](https://img.shields.io/badge/-Identity-512BD4?logo=dotnet&logoColor=white) | User management and password hashing |
| ![](https://img.shields.io/badge/-EF%20Core-512BD4?logo=dotnet&logoColor=white) | Data access and migrations |
| ![](https://img.shields.io/badge/-SQLite-003B57?logo=sqlite&logoColor=white) | Zero-setup database for the classroom |
| ![](https://img.shields.io/badge/-JWT%20Bearer-000000?logo=jsonwebtokens&logoColor=white) | Token-based authentication |
| ![](https://img.shields.io/badge/-Scalar-1A1A1A?logo=openapiinitiative&logoColor=white) | Interactive API docs |
| ![](https://img.shields.io/badge/-xUnit-5E1F87?logo=dotnet&logoColor=white) | Automated tests |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- An editor: [VS Code](https://code.visualstudio.com/), [Visual Studio](https://visualstudio.microsoft.com/) or [Rider](https://www.jetbrains.com/rider/)
- *(optional)* [Git](https://git-scm.com/) to switch between modules

### Run in 3 steps

```bash
# 1. Clone the repository
git clone https://github.com/your-username/edu-dotnet-auth.git
cd edu-dotnet-auth

# 2. Apply the database migrations
dotnet ef database update --project src/EduAuth.Api

# 3. Run the API
dotnet run --project src/EduAuth.Api
```

Then open **`https://localhost:5001/scalar`** to explore and test every endpoint. 🎉

> [!TIP]
> Prefer testing from the editor? Open `requests.http` in VS Code (REST Client extension) or Visual Studio and click **Send Request**.

---

## 📁 Project Structure

```
edu-dotnet-auth/
├── 📂 src/
│   └── 📂 EduAuth.Api/
│       ├── 📂 Auth/            # Login, register, refresh endpoints
│       ├── 📂 Data/            # DbContext and migrations
│       ├── 📂 Entities/        # User, RefreshToken
│       ├── 📂 Services/        # TokenService, PasswordService
│       ├── 📂 Policies/        # Custom authorization handlers
│       ├── 📄 Program.cs       # Pipeline configuration
│       └── 📄 appsettings.json
├── 📂 tests/
│   └── 📂 EduAuth.Tests/       # Unit and integration tests
├── 📂 docs/                    # Slides, diagrams, extra material
├── 📄 requests.http            # Ready-to-run API requests
└── 📄 README.md
```

---

## 🌐 API Endpoints

| Method | Endpoint | Auth | Description |
|:------:|----------|:----:|-------------|
| ![POST](https://img.shields.io/badge/POST-49CC90?style=flat-square) | `/auth/register` | 🔓 | Create a new account |
| ![POST](https://img.shields.io/badge/POST-49CC90?style=flat-square) | `/auth/login` | 🔓 | Get access + refresh tokens |
| ![POST](https://img.shields.io/badge/POST-49CC90?style=flat-square) | `/auth/refresh` | 🔓 | Exchange a refresh token for new tokens |
| ![POST](https://img.shields.io/badge/POST-49CC90?style=flat-square) | `/auth/logout` | 🔒 | Revoke the current refresh token |
| ![GET](https://img.shields.io/badge/GET-61AFFE?style=flat-square) | `/me` | 🔒 | Current user's profile and claims |
| ![GET](https://img.shields.io/badge/GET-61AFFE?style=flat-square) | `/admin/users` | 👑 | List users (**Admin** role only) |

<sub>🔓 Public &nbsp;•&nbsp; 🔒 Authenticated &nbsp;•&nbsp; 👑 Admin only</sub>

---

## 🧠 Key Concepts

<details>
<summary><b>🔑 Authentication vs Authorization</b></summary>
<br>

| | Authentication | Authorization |
|---|---|---|
| **Question** | Who are you? | What can you do? |
| **Happens** | First | After authentication |
| **Failure** | `401 Unauthorized` | `403 Forbidden` |
| **Example** | Checking your ID at the door | Checking if you have VIP access |

</details>

<details>
<summary><b>🎫 Anatomy of a JWT</b></summary>
<br>

A JWT has three parts separated by dots: `header.payload.signature`

```
eyJhbGciOiJIUzI1NiJ9 . eyJzdWIiOiIxMjMiLCJyb2xlIjoiQWRtaW4ifQ . SflKxwRJSMeKKF2QT4fw
└──── Header ────┘     └────────────── Payload ──────────────┘    └── Signature ──┘
  algorithm, type         claims: user id, role, expiration        proves integrity
```

⚠️ The payload is **encoded, not encrypted**. Anyone can read it, so never put secrets in a JWT.

</details>

<details>
<summary><b>🔄 Why refresh tokens?</b></summary>
<br>

Short-lived access tokens limit the damage if one is stolen. Refresh tokens let users stay logged in without typing their password again. With **rotation**, every refresh issues a new refresh token and revokes the old one, so a stolen token can only be used once.

</details>

<details>
<summary><b>🔒 Why hash passwords?</b></summary>
<br>

If the database leaks, hashed passwords can't simply be read back. Identity uses **PBKDF2** with a unique salt per user, so identical passwords produce different hashes and precomputed attacks become impractical.

</details>

---

## 🏆 Challenges

Finished a module? Put your knowledge to the test:

- [ ] 🟢 Add a `/auth/change-password` endpoint
- [ ] 🟢 Lock the account after 5 failed login attempts
- [ ] 🟡 Create a policy that only allows users over 18
- [ ] 🟡 Store refresh tokens in an `HttpOnly` cookie
- [ ] 🔴 Implement email confirmation on register
- [ ] 🔴 Add two-factor authentication (TOTP)

<sub>🟢 Beginner &nbsp;•&nbsp; 🟡 Intermediate &nbsp;•&nbsp; 🔴 Advanced</sub>

---

## ⚠️ Teaching vs Production

> [!WARNING]
> This project is built for **learning**. Some choices favor clarity over production readiness. Before shipping real software:
> - Store secrets in **User Secrets**, environment variables or a vault, never in `appsettings.json`
> - Use a production database instead of SQLite
> - Enable HTTPS everywhere and configure CORS carefully
> - Add rate limiting to the login endpoint
> - Consider a dedicated identity provider for larger systems

---

## 🤝 Contributing

Found a bug, a typo, or have an idea for a new module? Contributions are welcome!

1. Fork the repository
2. Create a branch: `git checkout -b fix/my-improvement`
3. Commit your changes: `git commit -m "fix: explain refresh token rotation better"`
4. Open a Pull Request 🚀

---

## 📄 License

Distributed under the **MIT License**. Use it freely in your own classes, courses and study groups. See [`LICENSE`](LICENSE) for details.

<br>

<div align="center">

### 👨‍🏫 Made with ❤️ for teaching

If this project helped you learn, consider giving it a ⭐

**[Your Name](https://github.com/your-username)** •
[LinkedIn](https://linkedin.com/in/your-profile) •
[YouTube](https://youtube.com/@your-channel)

</div>
using EduAuth.Api.Data;
using EduAuth.Api.Entities;
using EduAuth.Api.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Module 01: EF Core over SQLite. The connection string lives in appsettings.json.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Module 02: the password hashing service. Stateless, so a singleton is fine.
builder.Services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

var app = builder.Build();

// Teaching convenience: apply pending migrations at startup so the SQLite file
// exists on first run. Not production-safe — see docs/module-01.md.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Module 01: seed a couple of users when the database is empty, so GET /users
    // has something to show. Idempotent — the check means re-running never
    // duplicates them.
    // Module 03: seed a known password (hashed) so POST /auth/login works on first
    // run. The demo password is documented in docs/module-03.md — never ship known
    // credentials in a real deployment.
    if (!db.Users.Any())
    {
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var seedHash = hasher.Hash("edu-test");
        db.Users.AddRange(
            new User { Email = "luis@edu.dev", PasswordHash = seedHash },
            new User { Email = "starlino@edu.dev", PasswordHash = seedHash });
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Module 02: Scalar serves an interactive API UI at /scalar from the
    // OpenAPI document, so endpoints can be explored and called in the browser.
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Module 00: a single, unauthenticated endpoint to prove the API runs.
app.MapGet("/home", () => new { status = "ok", message = "API is running! Welcome my friend!!!" });

// Module 01: list the persisted users so the data layer is visible end to end.
app.MapGet("/users", async (AppDbContext db) => await db.Users.ToListAsync());

// Module 02: demo endpoints showing safe password storage. Hash a password,
// then verify an attempt against a hash. Nothing is persisted here — the real
// user store arrives in module 03.
app.MapPost("/passwords/hash", (HashRequest req, IPasswordHasher hasher) =>
    new { hash = hasher.Hash(req.Password) });

app.MapPost("/passwords/verify", (VerifyRequest req, IPasswordHasher hasher) =>
    new { valid = hasher.Verify(req.Password, req.Hash) });

// Module 03: registration. Hash the password, persist the user, return no hash.
// 409 if the email is already taken — the unique index is the real guard.
app.MapPost("/auth/register", async (RegisterRequest req, AppDbContext db, IPasswordHasher hasher) =>
{
    if (await db.Users.AnyAsync(u => u.Email == req.Email))
        return Results.Conflict(new { error = "Email already registered." });

    var user = new User { Email = req.Email, PasswordHash = hasher.Hash(req.Password) };
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Ok(new { id = user.Id, email = user.Email });
});

// Module 03: login. Verify the password against the stored hash. Unknown email
// and wrong password return the same 401 — never reveal which half failed.
app.MapPost("/auth/login", async (LoginRequest req, AppDbContext db, IPasswordHasher hasher) =>
{
    var user = await db.Users.SingleOrDefaultAsync(u => u.Email == req.Email);
    if (user is null || !hasher.Verify(req.Password, user.PasswordHash))
        return Results.Unauthorized();

    return Results.Ok(new { authenticated = true });
});

app.Run();

// Request DTOs: records give value equality and a concise constructor for binding JSON bodies.
record HashRequest(string Password);
record VerifyRequest(string Password, string Hash);
record RegisterRequest(string Email, string Password);
record LoginRequest(string Email, string Password);

// Exposed so integration tests can boot the app via WebApplicationFactory<Program>.
public partial class Program;

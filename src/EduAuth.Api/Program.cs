using EduAuth.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Module 01: EF Core over SQLite. The connection string lives in appsettings.json.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Teaching convenience: apply pending migrations at startup so the SQLite file
// exists on first run. Not production-safe — see docs/module-01.md.
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Module 00: a single, unauthenticated endpoint to prove the API runs.
app.MapGet("/home", () => new { status = "ok", message = "API is running! Welcome my friend!!!" });

// Module 01: list the persisted users so the data layer is visible end to end.
app.MapGet("/users", async (AppDbContext db) => await db.Users.ToListAsync());

app.Run();

// Exposed so integration tests can boot the app via WebApplicationFactory<Program>.
public partial class Program;

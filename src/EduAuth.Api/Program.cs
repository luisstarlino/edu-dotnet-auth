var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Module 00: a single, unauthenticated endpoint to prove the API runs.
app.MapGet("/home", () => new { status = "ok", message = "API is running! Welcome my friend!!!" });

app.Run();

// Exposed so integration tests can boot the app via WebApplicationFactory<Program>.
public partial class Program;

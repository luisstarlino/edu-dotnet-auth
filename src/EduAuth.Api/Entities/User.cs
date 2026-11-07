namespace EduAuth.Api.Entities;

// Module 01: a deliberately minimal user. No password or auth fields yet —
// those arrive in module 02 (passwords) and module 03 (identity).
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

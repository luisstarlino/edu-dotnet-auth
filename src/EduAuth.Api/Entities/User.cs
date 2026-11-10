namespace EduAuth.Api.Entities;

// Module 01: a minimal user. Module 03 adds the credential: PasswordHash holds
// a BCrypt hash (never the plaintext), written on registration.
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

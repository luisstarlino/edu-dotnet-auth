namespace EduAuth.Api.Services;

// Abstraction over the hashing algorithm so call sites (and module 03's user
// store) don't depend on BCrypt directly.
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}

namespace EduAuth.Api.Services;

// BCrypt generates a random salt per call and embeds it in the hash string,
// so Verify needs only the stored hash. Work factor 12 makes hashing
// deliberately slow to resist brute force.
public class BcryptPasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12;

    public string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);

    public bool Verify(string password, string hash) =>
        BCrypt.Net.BCrypt.Verify(password, hash);
}

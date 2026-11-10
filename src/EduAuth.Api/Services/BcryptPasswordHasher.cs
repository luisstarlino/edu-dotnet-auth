namespace EduAuth.Api.Services;

// BCrypt generates a random salt per call and embeds it in the hash string,
// so Verify needs only the stored hash. Work factor 12 makes hashing
// deliberately slow to resist brute force.
public class BcryptPasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12;

    // Example output for Hash("password123"):
    //   $2a$12$eImiTXuWVxfM37uY4JANjQ.k3Rz3ZbKqXwq7Yx1yJ3K8pQ2n5dC6e
    //    |  |  |                     |
    //    |  |  |                     +-- 31 chars: salt (first 22) + hash
    //    |  |  +-- cost / work factor (12)
    //    |  +-- algorithm version (2a)
    //    +-- bcrypt marker ($)
    // The salt is embedded, so every call produces a different string for the same password.
    public string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);

    public bool Verify(string password, string hash) =>
        BCrypt.Net.BCrypt.Verify(password, hash);
}

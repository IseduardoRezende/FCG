using System.Security.Cryptography;
using System.Text;
using FCG.Domain.Security;

namespace FCG.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;

    public string GenerateSalt()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(SaltSize));
    }

    public string Hash(string password, string salt)
    {
        var saltBytes = Encoding.UTF8.GetBytes(salt);
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        var hash = Rfc2898DeriveBytes.Pbkdf2(passwordBytes, saltBytes, Iterations, HashAlgorithmName.SHA256, KeySize);
        return Convert.ToBase64String(hash);
    }

    public bool Verify(string hashedPassword, string password, string salt)
    {
        var computedHash = Hash(password, salt);
        var savedHash = Convert.FromBase64String(hashedPassword);
        var newHash = Convert.FromBase64String(computedHash);
        return CryptographicOperations.FixedTimeEquals(savedHash, newHash);
    }
}

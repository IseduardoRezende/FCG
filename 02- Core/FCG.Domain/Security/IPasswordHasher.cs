namespace FCG.Domain.Security;

public interface IPasswordHasher
{
    string GenerateSalt();

    string Hash(string password, string salt);

    bool Verify(string hashedPassword, string password, string salt);
}

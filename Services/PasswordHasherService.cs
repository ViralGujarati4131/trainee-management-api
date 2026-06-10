using System.Security.Cryptography;
using PasswordHasher.Service.Interface;

namespace PasswordHasher.Service;

public class PasswordHasherService : IPasswordHasherService
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password can not be empty");
        }
        using var rng = RandomNumberGenerator.Create();
        byte[] salt = new byte[SaltSize];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        byte[] key = pbkdf2.GetBytes(KeySize);

        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
    }
    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
            return false;

        var parts = hashedPassword.Split('.');
        if (parts.Length != 3)
            return false;

        int iterations = Convert.ToInt32(parts[0]);
        byte[] salt = Convert.FromBase64String(parts[1]);
        byte[] key = Convert.FromBase64String(parts[2]);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        byte[] keyToCheck = pbkdf2.GetBytes(KeySize);

        return CryptographicOperations.FixedTimeEquals(key, keyToCheck);
    }
}
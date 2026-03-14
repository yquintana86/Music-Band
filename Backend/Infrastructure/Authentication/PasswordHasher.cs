using Application.Abstractions.Authentication;
using System.Security.Cryptography;

namespace Infrastructure.Authentication;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;       //must be at least 16 bits (128 bytes)
    private const int HashSize = 32;       //must be at least 32 bits (256 bytes)
    private const int Iterations = 100000; //must be at least 32 bits (256 bytes)

    private readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password,salt,Iterations,Algorithm,HashSize);

        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    public bool Verify(string password, string passwordHashed)
    {
        string[] passwordSplited = passwordHashed.Split('-');
        byte[] hash = Convert.FromHexString(passwordSplited[0]);
        byte[] salt = Convert.FromHexString(passwordSplited[1]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password,salt,Iterations,Algorithm,HashSize);

        //return hash.SequenceEqual(inputHash);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}

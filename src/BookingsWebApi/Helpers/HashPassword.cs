using System.Security.Cryptography;
using System.Text;

namespace BookingsWebApi.Helpers;

/// <summary>
///     Provides methods for encrypting and verifying passwords using the SHA256 hashing algorithm.
/// </summary>
public static class HashPassword
{
    /// <summary>
    ///     Encrypts a password using SHA256 hashing algorithm and a generated salt.
    /// </summary>
    /// <param name="password">The password to be encrypted.</param>
    /// <param name="salt">An out parameter to receive the generated salt.</param>
    /// <returns>The Base64-encoded string representing the encrypted password.</returns>
    public static string EncryptPassword(string? password, out string salt)
    {
        salt = GenerateSalt();
        var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password + salt);
        var hashBytes = sha256.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    ///     Generates a random salt for password encryption.
    /// </summary>
    /// <returns>A Base64-encoded string representing the generated salt.</returns>
    private static string GenerateSalt()
    {
        var saltBytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    /// <summary>
    ///     Verifies if a provided password matches its encrypted version.
    /// </summary>
    /// <param name="passwordTyped">The password entered by the user for verification.</param>
    /// <param name="passwordHashed">The stored encrypted password to compare against.</param>
    /// <param name="salt">The salt associated with the stored password.</param>
    /// <returns>True if the provided password matches the stored encrypted password; otherwise, false.</returns>
    public static bool VerifyPassword(string? passwordTyped, string passwordHashed, string salt)
    {
        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(passwordTyped + salt);
        var hashBytes = sha256.ComputeHash(passwordBytes);
        var passwordTypedHashed = Convert.ToBase64String(hashBytes);
        return passwordTypedHashed == passwordHashed;
    }
}

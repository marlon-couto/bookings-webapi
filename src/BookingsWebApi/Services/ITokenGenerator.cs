using BookingsWebApi.Models;

namespace BookingsWebApi.Services;

/// <summary>
///     Generates tokens for authenticated users.
/// </summary>
public interface ITokenGenerator
{
    /// <summary>
    ///     Generates a new token.
    /// </summary>
    /// <param name="user">The user data used to generate a new token.</param>
    /// <returns>A string representing the newly generated token.</returns>
    public string Generate(User user);
}
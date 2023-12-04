using System.Security.Claims;

namespace BookingsWebApi.Helpers;

/// <summary>
///     A class to help with user authorization and authentication.
/// </summary>
public abstract class AuthHelper
{
    /// <summary>
    ///     Retrieves the authenticated user's email.
    /// </summary>
    /// <param name="identity">The primary identity of the authenticated user.</param>
    /// <returns>Returns the email retrieved from the authenticated user.</returns>
    /// <exception cref="UnauthorizedAccessException">
    ///     Thrown if  identity is null or user's email is null.
    /// </exception>
    public static string GetLoggedUserEmail(ClaimsIdentity? identity)
    {
        if (identity == null)
        {
            throw new UnauthorizedAccessException("You must provide a token to proceed with this operation.");
        }

        string? userEmail = identity?.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Email)?.Value;

        return userEmail ?? throw new UnauthorizedAccessException("The token provided is invalid.");
    }
}
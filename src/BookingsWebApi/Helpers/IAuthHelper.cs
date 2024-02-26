using System.Security.Claims;

namespace BookingsWebApi.Helpers;

/// <summary>
///     A class to help with user authorization and authentication.
/// </summary>
public interface IAuthHelper
{
    /// <summary>
    ///     Retrieves the authenticated user's email.
    /// </summary>
    /// <param name="identity">The primary identity of the authenticated user.</param>
    /// <returns>Returns the email retrieved from the authenticated user.</returns>
    /// <exception cref="UnauthorizedAccessException">
    ///     Thrown if  identity is null or user's email is null.
    /// </exception>
    string GetLoggedUserEmail(ClaimsIdentity? identity);
}

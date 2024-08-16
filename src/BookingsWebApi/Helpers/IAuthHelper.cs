using System.Security.Claims;
using BookingsWebApi.Exceptions;

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
    /// <exception cref="UnauthorizedException">
    ///     Thrown if  identity is null or user's email is null.
    /// </exception>
    public string GetLoggedUserEmail(ClaimsIdentity? identity);

    /// <summary>
    ///     Checks if the authenticated user is an admin.
    /// </summary>
    /// <param name="identity">The primary identity of the authenticated user.</param>
    /// <returns>Returns true if the user is an admin, otherwise returns false.</returns>
    public bool IsAdmin(ClaimsIdentity? identity);
}

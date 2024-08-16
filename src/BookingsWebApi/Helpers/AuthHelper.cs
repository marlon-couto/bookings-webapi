using System.Security.Claims;
using BookingsWebApi.Exceptions;

namespace BookingsWebApi.Helpers;

public sealed class AuthHelper : IAuthHelper
{
    public string GetLoggedUserEmail(ClaimsIdentity? identity)
    {
        if (identity == null)
        {
            throw new UnauthorizedException(
                "You must provide a token to proceed with this operation."
            );
        }

        var userEmail = identity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        return userEmail ?? throw new UnauthorizedException("The token provided is invalid.");
    }

    public bool IsAdmin(ClaimsIdentity? identity)
    {
        return identity!.Claims.Any(x => x is { Type: ClaimTypes.Role, Value: "Admin" });
    }
}

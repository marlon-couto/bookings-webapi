using System.Security.Claims;

namespace BookingsWebApi.Helpers;

public class AuthHelper : IAuthHelper
{
    public virtual string GetLoggedUserEmail(ClaimsIdentity? identity)
    {
        if (identity == null)
        {
            throw new UnauthorizedAccessException("You must provide a token to proceed with this operation.");
        }

        string? userEmail = identity?.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Email)?.Value;
        return userEmail ?? throw new UnauthorizedAccessException("The token provided is invalid.");
    }
}
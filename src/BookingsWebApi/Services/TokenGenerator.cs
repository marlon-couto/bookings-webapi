using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookingsWebApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace BookingsWebApi.Services;

/// <summary>
///     Generates tokens for authenticated users.
/// </summary>
public class TokenGenerator
{
    private readonly TokenOptions _tokenOptions;

    public TokenGenerator(IConfiguration configuration)
    {
        _tokenOptions = new TokenOptions
        {
            Secret = configuration["Token:Secret"]!,
            ExpiresDay = int.Parse(configuration["Token:ExpiresDay"]!)
        };
    }

    /// <summary>
    ///     Generates a new token.
    /// </summary>
    /// <param name="user">The user data used to generate a new token.</param>
    /// <returns>A string representing the newly generated token.</returns>
    public string Generate(User user)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        SecurityTokenDescriptor tokenDescriptor =
            new()
            {
                Subject = AddClaims(user),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenOptions.Secret)),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Expires = DateTime.Now.AddDays(_tokenOptions.ExpiresDay)
            };

        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static ClaimsIdentity AddClaims(User user)
    {
        ClaimsIdentity claims = new();
        claims.AddClaim(new Claim(ClaimTypes.Email, user.Email!));
        if (user.Role == "Admin")
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
        }

        return claims;
    }
}

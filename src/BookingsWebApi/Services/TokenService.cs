using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using BookingsWebApi.Models;

using Microsoft.IdentityModel.Tokens;

namespace BookingsWebApi.Services;

/// <summary>
///     Generates tokens for authenticated users.
/// </summary>
public class TokenService
{
    private readonly TokenModel _tokenModel;

    public TokenService(IConfiguration configuration)
    {
        _tokenModel = new TokenModel
        {
            Secret = configuration["Token:Secret"]!, ExpiresDay = int.Parse(configuration["Token:ExpiresDay"]!)
        };
    }

    /// <summary>
    ///     Generates a new token.
    /// </summary>
    /// <param name="user">The user data used to generate a new token.</param>
    /// <returns>A string representing the newly generated token.</returns>
    public string Generate(UserModel user)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        SecurityTokenDescriptor tokenDescriptor =
            new()
            {
                Subject = AddClaims(user),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenModel.Secret)),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Expires = DateTime.Now.AddDays(_tokenModel.ExpiresDay)
            };

        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private static ClaimsIdentity AddClaims(UserModel user)
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
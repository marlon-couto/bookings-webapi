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
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenModel.Secret));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = AddClaims(user),
            SigningCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256Signature
            ),
            Expires = DateTime.Now.AddDays(_tokenModel.ExpiresDay)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static ClaimsIdentity AddClaims(UserModel user)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        if (user.Role == "Admin")
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
        }

        return claims;
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookingsWebApi.Models;
using dotenv.net.Utilities;
using Microsoft.IdentityModel.Tokens;

namespace BookingsWebApi.Services;

/// <summary>
///     Generates tokens for authenticated users.
/// </summary>
public class TokenService
{
    private readonly TokenModel _tokenModel;

    public TokenService()
    {
        _tokenModel = new TokenModel
        {
            ExpireDay = EnvReader.GetIntValue("TOKEN_EXPIRE_DAY"),
            Secret = EnvReader.GetStringValue("TOKEN_SECRET")
        };
    }

    public TokenService(TokenModel tokenModel)
    {
        _tokenModel = tokenModel;
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
            Expires = DateTime.Now.AddDays(_tokenModel.ExpireDay)
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
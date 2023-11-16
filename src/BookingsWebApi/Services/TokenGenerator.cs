using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using BookingsWebApi.Models;

using Microsoft.IdentityModel.Tokens;

namespace BookingsWebApi.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly TokenOptions _tokenOptions;
        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
            _tokenOptions = new TokenOptions
            {
                Secret = _configuration["Token:Secret"]!,
                ExpiresDay = int.Parse(_configuration["Token:ExpiresDay"]!)
            };
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

        public string Generate(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = AddClaims(user),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenOptions.Secret)),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Expires = DateTime.Now.AddDays(_tokenOptions.ExpiresDay)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

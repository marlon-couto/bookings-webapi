using BookingsWebApi.Models;
using BookingsWebApi.Services;
using BookingsWebApi.Test.Helpers.Builders;
using FluentAssertions;
using Xunit;

namespace BookingsWebApi.Test.Unit.Services;

public class TokenServiceTest
{
    private readonly TokenService _service;

    public TokenServiceTest()
    {
        var tokenModel = new TokenModel { ExpireDay = 7, Secret = "super_secret_key" };
        _service = new TokenService(tokenModel);
    }

    [Fact(DisplayName = "Generate should generate token")]
    public void Generate_ShouldGenerateToken()
    {
        var user = UserBuilder.New().Build();
        var token = _service.Generate(user);
        token.Should().NotBeNull();
    }
}

using System.Collections.Generic;

using BookingsWebApi.Models;
using BookingsWebApi.Services;
using BookingsWebApi.Test.Helpers.Builders;

using FluentAssertions;

using Microsoft.Extensions.Configuration;

using Xunit;

namespace BookingsWebApi.Test.Unit.Services;

public class TokenServiceTest
{
    private readonly TokenService _service;

    public TokenServiceTest()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    { "Token:Secret", "6ce1a0e05b576372b1fc569425a1e0f5e72adad7b318bb6420a6c307b6f2ca41" },
                    { "Token:ExpiresDay", "1" }
                }
            )
            .Build();

        _service = new TokenService(configuration);
    }

    [Fact(DisplayName = "Generate should generate token")]
    public void Generate_ShouldGenerateToken()
    {
        User user = UserBuilder.New().Build();
        string token = _service.Generate(user);

        token.Should().NotBeNull();
    }
}
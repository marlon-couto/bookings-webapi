using BookingsWebApi.Models;
using BookingsWebApi.Services;
using BookingsWebApi.Test.Helpers.Builders;
using FluentAssertions;
using Moq;
using Xunit;

namespace BookingsWebApi.Test.Unit.Services;

public class TokenServiceTest
{
    private readonly TokenService _service;

    public TokenServiceTest()
    {
        var tokenModelMock = new Mock<TokenModel>();
        tokenModelMock.Setup(x => x.Secret).Returns("mock_secret_key");
        tokenModelMock.Setup(x => x.ExpireDay).Returns(7);
        _service = new TokenService(tokenModelMock.Object);
    }

    [Fact(DisplayName = "Generate should generate token")]
    public void Generate_ShouldGenerateToken()
    {
        var user = UserBuilder.New().Build();
        var token = _service.Generate(user);
        token.Should().NotBeNull();
    }
}
using BookingsWebApi.Helpers;

using FluentAssertions;

using Xunit;
using Xunit.Abstractions;

namespace BookingsWebApi.Test.Unit.Helpers;

public class HashPasswordTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public HashPasswordTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact(DisplayName = "EncryptPassword should encrypt password")]
    public void EncryptPassword_ShouldEncryptPassword()
    {
        const string password = "Pass@12345";
        string passwordHashed = HashPassword.EncryptPassword(password, out string salt);

        passwordHashed.Should().NotBe(password);
        _testOutputHelper.WriteLine($"Result: password hashed: {passwordHashed}, salt: {salt}");
    }

    [Fact(DisplayName = "VerifyPassword should verify password")]
    public void VerifyPassword_ShouldVerifyPassword()
    {
        const string password = "Pass@12345";
        string passwordHashed = HashPassword.EncryptPassword(password, out string salt);

        bool isCorrectPassword = HashPassword.VerifyPassword(password, passwordHashed, salt);

        isCorrectPassword.Should().BeTrue();
    }
}
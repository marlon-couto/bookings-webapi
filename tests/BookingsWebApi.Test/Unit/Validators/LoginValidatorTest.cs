using BookingsWebApi.Test.Helpers.Builders;
using BookingsWebApi.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace BookingsWebApi.Test.Unit.Validators;

public class LoginValidatorTest
{
    private readonly LoginValidator _validator = new();

    [Fact(DisplayName = "LoginValidator should not have errors when data is invalid")]
    public void LoginValidator_ShouldNotHaveErrors_WhenDataIsInvalid()
    {
        var dto = UserBuilder.New().BuildAsLoginDto();
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "LoginValidator should have error when email is invalid")]
    [InlineData("")]
    [InlineData("invalidEmail")]
    public void LoginValidator_ShouldHaveError_WhenEmailIsInvalid(string email)
    {
        var dto = UserBuilder.New().WithEmail(email).BuildAsLoginDto();
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory(DisplayName = "LoginValidator should have error when password is invalid")]
    [InlineData("")]
    public void LoginValidator_ShouldHaveError_WhenPasswordIsInvalid(string password)
    {
        var dto = UserBuilder.New().WithPassword(password).BuildAsLoginDto();
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}

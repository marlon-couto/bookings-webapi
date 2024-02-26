using BookingsWebApi.DTOs;
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
        LoginInsertDto dto = UserBuilder.New().BuildAsLoginDto();
        TestValidationResult<LoginInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "LoginValidator should have error when email is invalid")]
    [InlineData("")]
    [InlineData("invalidEmail")]
    public void LoginValidator_ShouldHaveError_WhenEmailIsInvalid(string email)
    {
        LoginInsertDto dto = UserBuilder.New().WithEmail(email).BuildAsLoginDto();
        TestValidationResult<LoginInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(l => l.Email);
    }

    [Theory(DisplayName = "LoginValidator should have error when password is invalid")]
    [InlineData("")]
    public void LoginValidator_ShouldHaveError_WhenPasswordIsInvalid(string password)
    {
        LoginInsertDto dto = UserBuilder.New().WithPassword(password).BuildAsLoginDto();
        TestValidationResult<LoginInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(l => l.Password);
    }
}

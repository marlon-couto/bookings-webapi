using BookingsWebApi.Test.Helpers.Builders;
using BookingsWebApi.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace BookingsWebApi.Test.Unit.Validators;

public class UserValidatorTest
{
    private readonly UserValidator _validator = new();

    [Fact(DisplayName = "UserValidator should not have errors when data is valid")]
    public void UserValidator_ShouldNotHaveErrors_WhenDataIsValid()
    {
        var dto = UserBuilder.New().BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "UserValidator should have error when name is invalid")]
    [InlineData("")]
    [InlineData("x")]
    [InlineData("Lorem ipsum dolor sit amet consectetur adipiscing elit.")]
    public void UserValidator_ShouldHaveError_WhenNameIsInvalid(string name)
    {
        var dto = UserBuilder.New().WithName(name).BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(u => u.Name);
    }

    [Theory(DisplayName = "UserValidator should have error when email is invalid")]
    [InlineData("")]
    [InlineData("invalidEmail")]
    public void UserValidator_ShouldHaveError_WhenEmailsInvalid(string email)
    {
        var dto = UserBuilder.New().WithEmail(email).BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(u => u.Email);
    }

    [Theory(DisplayName = "UserValidator should have error when password is invalid")]
    [InlineData("")]
    [InlineData("12345678")]
    [InlineData("invalidPassword")]
    [InlineData("Pass@12")]
    public void UserValidator_ShouldHaveError_WhenPasswordIsInvalid(string password)
    {
        var dto = UserBuilder.New().WithPassword(password).BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(u => u.Password);
    }
}
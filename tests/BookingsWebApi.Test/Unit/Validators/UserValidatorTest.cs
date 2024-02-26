using BookingsWebApi.DTOs;
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
        UserInsertDto dto = UserBuilder.New().BuildAsInsertDto();
        TestValidationResult<UserInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "UserValidator should have error when name is invalid")]
    [InlineData("")]
    [InlineData("x")]
    [InlineData("Lorem ipsum dolor sit amet consectetur adipiscing elit.")]
    public void UserValidator_ShouldHaveError_WhenNameIsInvalid(string name)
    {
        UserInsertDto dto = UserBuilder.New().WithName(name).BuildAsInsertDto();
        TestValidationResult<UserInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(u => u.Name);
    }

    [Theory(DisplayName = "UserValidator should have error when email is invalid")]
    [InlineData("")]
    [InlineData("invalidEmail")]
    public void UserValidator_ShouldHaveError_WhenEmailsInvalid(string email)
    {
        UserInsertDto dto = UserBuilder.New().WithEmail(email).BuildAsInsertDto();
        TestValidationResult<UserInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(u => u.Email);
    }

    [Theory(DisplayName = "UserValidator should have error when password is invalid")]
    [InlineData("")]
    [InlineData("12345678")]
    [InlineData("invalidPassword")]
    [InlineData("Pass@12")]
    public void UserValidator_ShouldHaveError_WhenPasswordIsInvalid(string password)
    {
        UserInsertDto dto = UserBuilder.New().WithPassword(password).BuildAsInsertDto();
        TestValidationResult<UserInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(u => u.Password);
    }
}

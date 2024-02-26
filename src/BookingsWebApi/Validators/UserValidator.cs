using BookingsWebApi.DTOs;
using FluentValidation;

namespace BookingsWebApi.Validators;

public class UserValidator : AbstractValidator<UserInsertDto>
{
    public UserValidator()
    {
        RuleFor(u => u.Name).NotEmpty().Length(2, 25);
        RuleFor(u => u.Email).NotEmpty().EmailAddress();
        RuleFor(u => u.Password)
            .NotEmpty()
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,25}$")
            .WithMessage(
                "'Password' must be between 8-25 characters long, must include an lowercase letter and a uppercase letter, must have at least one number and one special character (#?!@$%^&*-)."
            );
    }
}

public class LoginValidator : AbstractValidator<LoginInsertDto>
{
    public LoginValidator()
    {
        RuleFor(u => u.Email).NotEmpty().EmailAddress();
        RuleFor(u => u.Password).NotEmpty().MinimumLength(1);
    }
}

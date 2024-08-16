using BookingsWebApi.DTOs;
using FluentValidation;

namespace BookingsWebApi.Validators;

public class UserValidator : AbstractValidator<UserInsertDto>
{
    public UserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, 25);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password)
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
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(1);
    }
}

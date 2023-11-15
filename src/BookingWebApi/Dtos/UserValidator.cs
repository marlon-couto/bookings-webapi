using FluentValidation;

namespace BookingWebApi.Dtos
{
    public class UserInsertValidator : AbstractValidator<UserInsertDto>
    {
        public UserInsertValidator()
        {
            RuleFor(b => b.Name).NotEmpty().MinimumLength(2);
            RuleFor(b => b.Email).NotEmpty().EmailAddress();
            RuleFor(b => b.Password)
                .NotEmpty()
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
                .WithMessage("'Password' must be at least 8 character long, must include an lowercase letter, and uppercase letter, at least one number and one special character (#?!@$%^&*-)");
        }
    }
}

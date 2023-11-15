using FluentValidation;

namespace BookingsWebApi.Dtos
{
    public class UserInsertValidator : AbstractValidator<UserInsertDto>
    {
        public UserInsertValidator()
        {
            RuleFor(u => u.Name).NotEmpty().MinimumLength(2);
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Password)
                .NotEmpty()
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
                .WithMessage("'Password' must be at least 8 character long, must include an lowercase letter and a uppercase letter, must have at least one number and one special character (#?!@$%^&*-)");
        }
    }
}

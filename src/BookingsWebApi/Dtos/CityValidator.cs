using FluentValidation;

namespace BookingsWebApi.Dtos
{
    public class CityInsertValidator : AbstractValidator<CityInsertDto>
    {
        public CityInsertValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(2);
            RuleFor(c => c.State).NotEmpty().MinimumLength(2);
        }
    }
}

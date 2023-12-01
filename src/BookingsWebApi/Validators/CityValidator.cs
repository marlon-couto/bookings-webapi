using BookingsWebApi.DTOs;

using FluentValidation;

namespace BookingsWebApi.Validators;

public class CityValidator : AbstractValidator<CityInsertDto>
{
    public CityValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(2);
        RuleFor(c => c.State).NotEmpty().MinimumLength(2);
    }
}
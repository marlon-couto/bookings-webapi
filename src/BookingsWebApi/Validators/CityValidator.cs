using BookingsWebApi.DTOs;
using FluentValidation;

namespace BookingsWebApi.Validators;

public class CityValidator : AbstractValidator<CityInsertDto>
{
    public CityValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, 50);
        RuleFor(x => x.State).NotEmpty().Length(2, 50);
    }
}

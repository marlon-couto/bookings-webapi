using BookingsWebApi.DTOs;
using FluentValidation;

namespace BookingsWebApi.Validators;

public class CityValidator : AbstractValidator<CityInsertDto>
{
    public CityValidator()
    {
        RuleFor(c => c.Name).NotEmpty().Length(2, 50);
        RuleFor(c => c.State).NotEmpty().Length(2, 50);
    }
}
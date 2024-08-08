using BookingsWebApi.DTOs;
using FluentValidation;

namespace BookingsWebApi.Validators;

public class HotelValidator : AbstractValidator<HotelInsertDto>
{
    public HotelValidator()
    {
        RuleFor(h => h.Name).NotEmpty().Length(2, 50);
        RuleFor(h => h.Address).NotEmpty().Length(5, 100);
        RuleFor(h => h.CityId).NotEmpty().MinimumLength(1);
    }
}
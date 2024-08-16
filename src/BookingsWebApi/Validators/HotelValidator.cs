using BookingsWebApi.DTOs;
using FluentValidation;

namespace BookingsWebApi.Validators;

public class HotelValidator : AbstractValidator<HotelInsertDto>
{
    public HotelValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, 50);
        RuleFor(x => x.Address).NotEmpty().Length(5, 100);
        RuleFor(x => x.CityId).NotEmpty();
    }
}

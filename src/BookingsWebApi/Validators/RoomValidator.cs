using BookingsWebApi.DTOs;
using FluentValidation;

namespace BookingsWebApi.Validators;

public class RoomValidator : AbstractValidator<RoomInsertDto>
{
    public RoomValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, 50);
        RuleFor(x => x.Capacity).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Image).NotEmpty().MinimumLength(1);
        RuleFor(x => x.HotelId).NotEmpty();
    }
}

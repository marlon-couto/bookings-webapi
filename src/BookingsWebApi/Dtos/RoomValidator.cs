using FluentValidation;

namespace BookingsWebApi.Dtos
{
    public class RoomInsertValidator : AbstractValidator<RoomInsertDto>
    {
        public RoomInsertValidator()
        {
            RuleFor(r => r.HotelId).NotEmpty().MinimumLength(1);
            RuleFor(r => r.Name).NotEmpty().Length(2, 25);
            RuleFor(r => r.Capacity).NotEmpty().GreaterThan(0);
            RuleFor(r => r.Image).NotEmpty().MinimumLength(1);
        }
    }
}

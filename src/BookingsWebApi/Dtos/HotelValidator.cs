using FluentValidation;

namespace BookingsWebApi.Dtos
{
    public class HotelInsertValidator : AbstractValidator<HotelInsertDto>
    {
        public HotelInsertValidator()
        {
            RuleFor(h => h.Address).NotEmpty().Length(5, 50);
            RuleFor(h => h.Name).NotEmpty().Length(2, 25);
            RuleFor(h => h.CityId).NotEmpty().MinimumLength(1);
        }
    }
}

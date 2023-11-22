using BookingsWebApi.DTOs;

using FluentValidation;

namespace BookingsWebApi.Validators;

public class BookingInsertValidator : AbstractValidator<BookingInsertDto>
{
    public BookingInsertValidator()
    {
        RuleFor(b => b.CheckIn).NotEmpty().Must(IsValidDate).WithMessage("'Check In' must be a valid date.");
        RuleFor(b => b.CheckOut).NotEmpty().Must(IsValidDate).WithMessage("'Check Out' must be a valid date.");
        RuleFor(b => b.GuestQuantity).NotEmpty().GreaterThan(0);
        RuleFor(b => b.RoomId).NotEmpty().MinimumLength(1);
    }

    private static bool IsValidDate(string dateString)
    {
        return DateTime.TryParse(dateString, out _);
    }
}
using System.Globalization;
using BookingsWebApi.DTOs;
using FluentValidation;

namespace BookingsWebApi.Validators;

public class BookingValidator : AbstractValidator<BookingInsertDto>
{
    public BookingValidator()
    {
        RuleFor(x => x.CheckIn)
            .NotEmpty()
            .Must(IsValidDate)
            .WithMessage("'Check In' must be a valid date.");
        RuleFor(x => x.CheckOut)
            .NotEmpty()
            .Must(IsValidDate)
            .WithMessage("'Check Out' must be a valid date.");
        RuleFor(x => x.GuestQuantity).NotEmpty().GreaterThan(0);
        RuleFor(x => x.RoomId).NotEmpty();
    }

    private static bool IsValidDate(string? dateString)
    {
        return DateTime.TryParseExact(
            dateString,
            "dd/MM/yyyy HH:mm:ss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _
        );
    }
}

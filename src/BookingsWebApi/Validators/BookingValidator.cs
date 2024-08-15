using System.Globalization;
using BookingsWebApi.DTOs;
using FluentValidation;

namespace BookingsWebApi.Validators;

public class BookingValidator : AbstractValidator<BookingInsertDto>
{
    public BookingValidator()
    {
        RuleFor(b => b.CheckIn)
            .NotEmpty()
            .Must(IsValidDate)
            .WithMessage("'Check In' must be a valid date.");
        RuleFor(b => b.CheckOut)
            .NotEmpty()
            .Must(IsValidDate)
            .WithMessage("'Check Out' must be a valid date.");
        RuleFor(b => b.GuestQuantity).NotEmpty().GreaterThan(0);
        RuleFor(b => b.RoomId).NotEmpty();
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
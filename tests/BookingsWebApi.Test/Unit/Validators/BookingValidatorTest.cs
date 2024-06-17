using BookingsWebApi.DTOs;
using BookingsWebApi.Test.Helpers.Builders;
using BookingsWebApi.Validators;

using FluentValidation.TestHelper;

using Xunit;

namespace BookingsWebApi.Test.Unit.Validators;

public class BookingValidatorTest
{
    private readonly BookingValidator _validator = new();

    [Fact(DisplayName = "BookingValidator should not have any errors when data is valid")]
    public void BookingValidator_ShouldNotHaveAnyErrors_WhenDataIsValid()
    {
        BookingInsertDto dto = BookingBuilder.New().BuildAsInsertDto();
        TestValidationResult<BookingInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "BookingValidator should have error when check in date is invalid")]
    [InlineData("")]
    [InlineData("01")]
    [InlineData("32/12/23")]
    [InlineData("01/13/23")]
    public void BookingValidator_ShouldHaveError_WhenCheckInDateIsInvalid(string checkOutStr)
    {
        BookingInsertDto dto = BookingBuilder.New().WithCheckIn(checkOutStr).BuildAsInsertDto();
        TestValidationResult<BookingInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(b => b.CheckIn);
    }

    [Theory(DisplayName = "BookingValidator should have error when check out date is invalid")]
    [InlineData("")]
    [InlineData("01")]
    [InlineData("32/12/23")]
    [InlineData("01/13/23")]
    public void BookingValidator_ShouldHaveError_WhenCheckOutIsInvalid(string checkOutStr)
    {
        BookingInsertDto dto = BookingBuilder.New().WithCheckOut(checkOutStr).BuildAsInsertDto();
        TestValidationResult<BookingInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(b => b.CheckOut);
    }

    [Theory(DisplayName = "BookingValidator should have error when guest quantity is invalid")]
    [InlineData(0)]
    [InlineData(-1)]
    public void BookingValidator_ShouldHaveError_WhenGuestQuantityIsInvalid(int guestQuantity)
    {
        BookingInsertDto dto = BookingBuilder
            .New()
            .WithGuestQuantity(guestQuantity)
            .BuildAsInsertDto();

        TestValidationResult<BookingInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(b => b.GuestQuantity);
    }

    [Theory(DisplayName = "BookingValidator should have error when room ID is invalid")]
    [InlineData("")]
    public void BookingValidator_ShouldHaveError_WhenRoomIdIsInvalid(string roomId)
    {
        BookingInsertDto dto = BookingBuilder.New().WithRoomId(roomId).BuildAsInsertDto();
        TestValidationResult<BookingInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(b => b.RoomId);
    }
}
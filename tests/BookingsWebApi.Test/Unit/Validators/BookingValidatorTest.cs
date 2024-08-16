using System;
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
        var dto = BookingBuilder.New().BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "BookingValidator should have error when check in date is invalid")]
    [InlineData("")]
    [InlineData("01")]
    [InlineData("32/12/23")]
    [InlineData("01/13/23")]
    public void BookingValidator_ShouldHaveError_WhenCheckInDateIsInvalid(string checkOutStr)
    {
        var dto = BookingBuilder.New().WithCheckIn(checkOutStr).BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.CheckIn);
    }

    [Theory(DisplayName = "BookingValidator should have error when check out date is invalid")]
    [InlineData("")]
    [InlineData("01")]
    [InlineData("32/12/23")]
    [InlineData("01/13/23")]
    public void BookingValidator_ShouldHaveError_WhenCheckOutIsInvalid(string checkOutStr)
    {
        var dto = BookingBuilder.New().WithCheckOut(checkOutStr).BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.CheckOut);
    }

    [Theory(DisplayName = "BookingValidator should have error when guest quantity is invalid")]
    [InlineData(0)]
    [InlineData(-1)]
    public void BookingValidator_ShouldHaveError_WhenGuestQuantityIsInvalid(int guestQuantity)
    {
        var dto = BookingBuilder.New().WithGuestQuantity(guestQuantity).BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.GuestQuantity);
    }

    [Theory(DisplayName = "BookingValidator should have error when room ID is invalid")]
    [InlineData(null)]
    public void BookingValidator_ShouldHaveError_WhenRoomIdIsInvalid(Guid? roomId)
    {
        var dto = BookingBuilder.New().WithRoomId(roomId).BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.RoomId);
    }
}

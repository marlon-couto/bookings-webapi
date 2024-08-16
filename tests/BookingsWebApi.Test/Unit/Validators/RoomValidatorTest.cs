using System;
using BookingsWebApi.Test.Helpers.Builders;
using BookingsWebApi.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace BookingsWebApi.Test.Unit.Validators;

public class RoomValidatorTest
{
    private readonly RoomValidator _validator = new();

    [Fact(DisplayName = "RoomValidator should not have errors when data is valid")]
    public void RoomValidator_ShouldNotHaveErrors_WhenDataIsValid()
    {
        var dto = RoomBuilder.New().BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "RoomValidator should have error when name is invalid")]
    [InlineData("")]
    [InlineData("x")]
    [InlineData("Lorem ipsum dolor sit amet consectetur adipiscing elit.")]
    public void RoomValidator_ShouldHaveError_WhenNameIsInvalid(string name)
    {
        var dto = RoomBuilder.New().WithName(name).BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Theory(DisplayName = "RoomValidator should have error when hotel ID is invalid")]
    [InlineData(null)]
    public void RoomValidator_ShouldHaveError_WhenHotelIdIsInvalid(Guid? hotelId)
    {
        var dto = RoomBuilder.New().WithHotelId(hotelId).BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.HotelId);
    }

    [Theory(DisplayName = "RoomValidator should have error when image is invalid")]
    [InlineData("")]
    public void RoomValidator_ShouldHaveError_WhenImageIsInvalid(string image)
    {
        var dto = RoomBuilder.New().WithImage(image).BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Image);
    }

    [Theory(DisplayName = "RoomValidator should have error when capacity is invalid")]
    [InlineData(0)]
    [InlineData(-1)]
    public void RoomValidator_ShouldHaveError_WhenCapacityIsInvalid(int capacity)
    {
        var dto = RoomBuilder.New().WithCapacity(capacity).BuildAsInsertDto();
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Capacity);
    }
}

using BookingsWebApi.DTOs;
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
        RoomInsertDto dto = RoomBuilder.New().BuildAsInsertDto();
        TestValidationResult<RoomInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "RoomValidator should have error when name is invalid")]
    [InlineData("")]
    [InlineData("x")]
    [InlineData("Lorem ipsum dolor sit amet consectetur adipiscing elit.")]
    public void RoomValidator_ShouldHaveError_WhenNameIsInvalid(string name)
    {
        RoomInsertDto dto = RoomBuilder.New().WithName(name).BuildAsInsertDto();
        TestValidationResult<RoomInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(r => r.Name);
    }

    [Theory(DisplayName = "RoomValidator should have error when hotel ID is invalid")]
    [InlineData("")]
    public void RoomValidator_ShouldHaveError_WhenHotelIdIsInvalid(string hotelId)
    {
        RoomInsertDto dto = RoomBuilder.New().WithHotelId(hotelId).BuildAsInsertDto();
        TestValidationResult<RoomInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(r => r.HotelId);
    }

    [Theory(DisplayName = "RoomValidator should have error when image is invalid")]
    [InlineData("")]
    public void RoomValidator_ShouldHaveError_WhenImageIsInvalid(string image)
    {
        RoomInsertDto dto = RoomBuilder.New().WithImage(image).BuildAsInsertDto();
        TestValidationResult<RoomInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(r => r.Image);
    }

    [Theory(DisplayName = "RoomValidator should have error when capacity is invalid")]
    [InlineData(0)]
    [InlineData(-1)]
    public void RoomValidator_ShouldHaveError_WhenCapacityIsInvalid(int capacity)
    {
        RoomInsertDto dto = RoomBuilder.New().WithCapacity(capacity).BuildAsInsertDto();
        TestValidationResult<RoomInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(r => r.Capacity);
    }
}
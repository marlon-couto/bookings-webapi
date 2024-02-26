using BookingsWebApi.DTOs;
using BookingsWebApi.Test.Helpers.Builders;
using BookingsWebApi.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace BookingsWebApi.Test.Unit.Validators;

public class HotelValidatorTest
{
    private readonly HotelValidator _validator = new();

    [Fact(DisplayName = "HotelValidator should not have error when data is valid")]
    public void HotelValidator_ShouldNotHaveErrors_WhenDataIsValid()
    {
        HotelInsertDto dto = HotelBuilder.New().BuildAsInsertDto();
        TestValidationResult<HotelInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "HotelValidator should have error when name is invalid")]
    [InlineData("")]
    [InlineData("x")]
    [InlineData("Lorem ipsum dolor sit amet consectetur adipiscing elit.")]
    public void HotelValidator_ShouldHaveError_WhenNameIsInvalid(string name)
    {
        HotelInsertDto dto = HotelBuilder.New().WithName(name).BuildAsInsertDto();
        TestValidationResult<HotelInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(h => h.Name);
    }

    [Theory(DisplayName = "HotelValidator should have error when address is invalid")]
    [InlineData("")]
    [InlineData("x")]
    [InlineData(
        "Lorem ipsum dolor sit amet consectetur adipiscing elit. Lorem ipsum dolor sit amet consectetur adipiscing elit."
    )]
    public void HotelValidator_ShouldHaveError_WhenAddressIsInvalid(string address)
    {
        HotelInsertDto dto = HotelBuilder.New().WithAddress(address).BuildAsInsertDto();
        TestValidationResult<HotelInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(h => h.Address);
    }

    [Theory(DisplayName = "HotelValidator should have error when city ID is invalid")]
    [InlineData("")]
    public void HotelValidator_ShouldHaveError_WhenRoomIdIsInvalid(string cityId)
    {
        HotelInsertDto dto = HotelBuilder.New().WithCityId(cityId).BuildAsInsertDto();
        TestValidationResult<HotelInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(h => h.CityId);
    }
}

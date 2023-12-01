using BookingsWebApi.DTOs;
using BookingsWebApi.Test.Helpers.Builders;
using BookingsWebApi.Validators;

using FluentValidation.TestHelper;

using Xunit;

namespace BookingsWebApi.Test.Unit.Validators;

public class CityValidatorTest
{
    private readonly CityValidator _validator = new();

    [Fact(DisplayName = "CityValidator should not have any errors when data is valid")]
    public void CityValidator_ShouldNotHaveAnyErrors_WhenDataIsValid()
    {
        CityInsertDto dto = CityBuilder.New().BuildAsInsertDto();
        TestValidationResult<CityInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "CityValidator should have error when name is invalid")]
    [InlineData("")]
    [InlineData("x")]
    [InlineData("Lorem ipsum dolor sit amet consectetur adipiscing elit.")]
    public void CityValidator_ShouldHaveError_WhenNameIsInvalid(string name)
    {
        CityInsertDto dto = CityBuilder.New().WithName(name).BuildAsInsertDto();
        TestValidationResult<CityInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Theory(DisplayName = "CityValidator should have error when state is invalid")]
    [InlineData("")]
    [InlineData("x")]
    [InlineData("Lorem ipsum dolor sit amet consectetur adipiscing elit.")]
    public void CityValidator_ShouldHaveError_WhenStateIsInvalid(string state)
    {
        CityInsertDto dto = CityBuilder.New().WithState(state).BuildAsInsertDto();
        TestValidationResult<CityInsertDto>? result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(c => c.State);
    }
}
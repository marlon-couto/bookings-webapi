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
}
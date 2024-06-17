using System.Collections.Generic;
using System.Threading.Tasks;

using BookingsWebApi.Controllers;
using BookingsWebApi.DTOs;
using BookingsWebApi.Helpers;
using BookingsWebApi.Models;
using BookingsWebApi.Services;
using BookingsWebApi.Test.Helpers.Builders;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Moq;

using Xunit;

namespace BookingsWebApi.Test.Unit.Controllers;

public class LoginControllerTest
{
    private readonly LoginController _controller;
    private readonly Mock<IUserService> _serviceMock;
    private readonly Mock<IValidator<LoginInsertDto>> _validatorMock;

    public LoginControllerTest()
    {
        _validatorMock = new Mock<IValidator<LoginInsertDto>>();
        _serviceMock = new Mock<IUserService>();
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    { "Token:Secret", "6ce1a0e05b576372b1fc569425a1e0f5e72adad7b318bb6420a6c307b6f2ca41" },
                    { "Token:ExpiresDay", "1" }
                }
            )
            .Build();

        _controller = new LoginController(
            _serviceMock.Object,
            _validatorMock.Object,
            configuration
        );
    }

    [Fact(DisplayName = "Login should return OK with token")]
    public async Task Login_ShouldReturnOkWithToken()
    {
        LoginInsertDto dto = UserBuilder.New().BuildAsLoginDto();
        UserModel user = UserBuilder.New().WithEmail(dto.Email).WithPassword(dto.Password).Build();
        MockValidator();
        _serviceMock.Setup(s => s.GetUserByEmail(dto.Email)).ReturnsAsync(user);
        IActionResult result = await _controller.Login(dto);
        OkObjectResult? objResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objResult.StatusCode.Should().Be(200);
        objResult
            .Value.Should()
            .BeOfType<ControllerResponse<string>>()
            .Which.Data.Should()
            .NotBeNullOrEmpty();
    }

    private void MockValidator()
    {
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<LoginInsertDto>(), default))
            .ReturnsAsync(new ValidationResult());
    }
}
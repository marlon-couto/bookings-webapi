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
        var tokenModel = new TokenModel { ExpireDay = 7, Secret = "super_secret_key" };
        _controller = new LoginController(_serviceMock.Object, _validatorMock.Object, new TokenService(tokenModel));
    }

    [Fact(DisplayName = "Login should return OK with token")]
    public async Task Login_ShouldReturnOkWithToken()
    {
        var dto = UserBuilder.New().BuildAsLoginDto();
        var user = UserBuilder.New().WithEmail(dto.Email).WithPassword(dto.Password).Build();
        MockValidator();
        _serviceMock.Setup(s => s.GetUserByEmail(dto.Email)).ReturnsAsync(user);
        var result = await _controller.Login(dto);
        var objResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objResult.StatusCode.Should().Be(200);
        objResult.Value.Should()
            .BeOfType<ControllerResponse>()
            .Which.Data.Should()
            .NotBeNull();
    }

    private void MockValidator()
    {
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<LoginInsertDto>(), default))
            .ReturnsAsync(new ValidationResult());
    }
}
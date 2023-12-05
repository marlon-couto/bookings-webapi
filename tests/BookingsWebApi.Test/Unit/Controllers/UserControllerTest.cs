using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

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

public class UserControllerTest
{
    private readonly UserController _controller;
    private readonly Mock<IUserService> _serviceMock;

    public UserControllerTest()
    {
        Mock<IMapper> mapperMock = new();
        Mock<IValidator<UserInsertDto>> validatorMock = new();

        _serviceMock = new Mock<IUserService>();
        _controller = new UserController(_serviceMock.Object, mapperMock.Object, validatorMock.Object);

        mapperMock
            .Setup(mapper => mapper.Map<UserDto>(It.IsAny<User>()))
            .Returns((User source) =>
                new UserDto { Id = source.Id, Email = source.Email, Name = source.Name, Role = source.Role });

        validatorMock
            .Setup(validator => validator.ValidateAsync(It.IsAny<UserInsertDto>(), default))
            .ReturnsAsync(new ValidationResult());
    }

    [Fact(DisplayName = "GetAsync should return OK with users")]
    public async Task GetAsync_ShouldReturnOkWithUsers()
    {
        _serviceMock
            .Setup(service => service.GetUsers())
            .ReturnsAsync(new List<User> { UserBuilder.New().Build(), UserBuilder.New().Build() });

        IActionResult result = await _controller.GetAsync();

        OkObjectResult? objResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objResult.StatusCode.Should().Be(200);
        objResult.Value.Should().BeOfType<ControllerListResponse<UserDto>>().Which.Data.Should().HaveCount(2);
    }
}
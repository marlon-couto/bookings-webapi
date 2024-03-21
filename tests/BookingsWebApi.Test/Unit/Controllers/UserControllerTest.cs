using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using BookingsWebApi.Controllers;
using BookingsWebApi.DTOs;
using BookingsWebApi.Helpers;
using BookingsWebApi.Models;
using BookingsWebApi.Services;
using BookingsWebApi.Test.Helpers;
using BookingsWebApi.Test.Helpers.Builders;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace BookingsWebApi.Test.Unit.Controllers;

public class UserControllerTest
{
    private readonly Mock<IAuthHelper> _authHelperMock;
    private readonly UserController _controller;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserService> _serviceMock;
    private readonly Mock<IValidator<UserInsertDto>> _validatorMock;

    public UserControllerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<UserInsertDto>>();
        _serviceMock = new Mock<IUserService>();
        _authHelperMock = new Mock<IAuthHelper>();

        _controller = new UserController(
            _serviceMock.Object,
            _mapperMock.Object,
            _validatorMock.Object,
            _authHelperMock.Object
        );

        MockMapper();
    }

    [Fact(DisplayName = "GetAsync should return OK with users")]
    public async Task GetAsync_ShouldReturnOkWithUsers()
    {
        List<UserModel> users = new()
        {
            UserBuilder.New().Build(), UserBuilder.New().Build()
        };

        _serviceMock.Setup(s => s.GetUsers()).ReturnsAsync(users);

        IActionResult result = await _controller.GetAsync();

        OkObjectResult? objResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objResult.StatusCode.Should().Be(200);
        objResult
            .Value.Should()
            .BeOfType<ControllerResponse<List<UserDto>>>()
            .Which.Data.Should()
            .HaveCount(2);
    }

    [Fact(DisplayName = "PostAsync should return Created with user created")]
    public async Task PostAsync_ShouldReturnCreatedWithUserCreated()
    {
        UserModel user = UserBuilder.New().Build();
        UserInsertDto dto =
            new()
            {
                Email = user.Email, Name = user.Name, Password = user.Password
            };

        MockValidator();

        _serviceMock.Setup(s => s.AddUser(dto)).ReturnsAsync(user);

        IActionResult result = await _controller.PostAsync(dto);

        CreatedResult? objResult = result.Should().BeOfType<CreatedResult>().Subject;
        objResult.StatusCode.Should().Be(201);
        objResult
            .Value.Should()
            .BeOfType<ControllerResponse<UserDto>>()
            .Which.Data.Should()
            .BeEquivalentTo(user.AsDto());
    }

    [Fact(DisplayName = "PostAsync should return BadRequest when validation fails")]
    public async Task PostAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        UserInsertDto dto = UserBuilder.New().BuildAsInsertDto();

        MockValidatorFailure();

        IActionResult result = await _controller.PostAsync(dto);

        BadRequestObjectResult? objResult = result
            .Should()
            .BeOfType<BadRequestObjectResult>()
            .Subject;
        objResult.StatusCode.Should().Be(400);
        objResult
            .Value.Should()
            .BeOfType<ControllerErrorResponse>()
            .Which.Message.Should()
            .NotBeNull();
    }

    [Fact(DisplayName = "PostAsync should return Conflict when email exists")]
    public async Task PostAsync_ShouldReturnConflict_WhenEmailExists()
    {
        UserInsertDto dto = UserBuilder.New().BuildAsInsertDto();

        MockValidator();

        _serviceMock
            .Setup(s => s.EmailExists(dto.Email))
            .ThrowsAsync(
                new InvalidOperationException("The email provided is already registered.")
            );

        IActionResult result = await _controller.PostAsync(dto);

        ConflictObjectResult? objResult = result.Should().BeOfType<ConflictObjectResult>().Subject;
        objResult.StatusCode.Should().Be(409);
        objResult
            .Value.Should()
            .BeOfType<ControllerErrorResponse>()
            .Which.Message.Should()
            .Be("The email provided is already registered.");
    }

    [Fact(DisplayName = "PutAsync should return OK with updated user")]
    public async Task PutAsync_ShouldReturnOkWithUpdatedUser()
    {
        UserModel user = UserBuilder.New().Build();
        UserInsertDto dto = UserBuilder.New().BuildAsInsertDto();
        UserModel userUpdated =
            new()
            {
                Email = dto.Email,
                Name = dto.Name,
                Password = dto.Password,
                Role = user.Role,
                Id = user.Id
            };

        MockAuthHelper(user.Email);
        MockValidator();

        _serviceMock.Setup(s => s.GetUserByEmail(user.Email)).ReturnsAsync(user);
        _serviceMock.Setup(s => s.UpdateUser(dto, user)).ReturnsAsync(userUpdated);

        IActionResult result = await _controller.PutAsync(dto);

        OkObjectResult? objResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objResult.StatusCode.Should().Be(200);
        objResult
            .Value.Should()
            .BeOfType<ControllerResponse<UserDto>>()
            .Which.Data.Should()
            .BeEquivalentTo(userUpdated.AsDto());
    }

    [Fact(DisplayName = "PutAsync should return BadRequest when validation fails")]
    public async Task PutAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        UserInsertDto dto = UserBuilder.New().BuildAsInsertDto();

        MockAuthHelper(dto.Email);
        MockValidatorFailure();

        IActionResult result = await _controller.PutAsync(dto);

        BadRequestObjectResult? objResult = result
            .Should()
            .BeOfType<BadRequestObjectResult>()
            .Subject;
        objResult.StatusCode.Should().Be(400);
        objResult
            .Value.Should()
            .BeOfType<ControllerErrorResponse>()
            .Which.Message.Should()
            .NotBeNull();
    }

    [Fact(DisplayName = "PutAsync should return Unauthorized when user is not authorized")]
    public async Task PutAsync_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
    {
        UserModel user = UserBuilder.New().Build();
        UserInsertDto dto = UserBuilder.New().BuildAsInsertDto();

        MockAuthHelper(user.Email);
        MockValidator();

        _serviceMock
            .Setup(s => s.GetUserByEmail(user.Email))
            .ThrowsAsync(
                new UnauthorizedAccessException("The email or password provided is incorrect.")
            );

        IActionResult result = await _controller.PutAsync(dto);

        UnauthorizedObjectResult? objResult = result
            .Should()
            .BeOfType<UnauthorizedObjectResult>()
            .Subject;
        objResult.StatusCode.Should().Be(401);
        objResult
            .Value.Should()
            .BeOfType<ControllerErrorResponse>()
            .Which.Message.Should()
            .Be("The email or password provided is incorrect.");
    }

    [Fact(DisplayName = "DeleteAsync should return NoContent")]
    public async Task DeleteAsync_ShouldReturnNoContent()
    {
        UserModel user = UserBuilder.New().Build();

        MockAuthHelper(user.Email);

        _serviceMock.Setup(s => s.GetUserByEmail(user.Email)).ReturnsAsync(user);
        _serviceMock.Setup(s => s.DeleteUser(user));

        IActionResult result = await _controller.DeleteAsync();

        NoContentResult? objResult = result.Should().BeOfType<NoContentResult>().Subject;
        objResult.StatusCode.Should().Be(204);
    }

    [Fact(DisplayName = "DeleteAsync should return Unauthorized when user is not authorized")]
    public async Task DeleteAsync_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
    {
        UserModel user = UserBuilder.New().Build();

        MockAuthHelper(user.Email);

        _serviceMock
            .Setup(s => s.GetUserByEmail(user.Email))
            .ThrowsAsync(
                new UnauthorizedAccessException("The email or password provided is incorrect.")
            );

        IActionResult result = await _controller.DeleteAsync();

        UnauthorizedObjectResult? objResult = result
            .Should()
            .BeOfType<UnauthorizedObjectResult>()
            .Subject;
        objResult.StatusCode.Should().Be(401);
        objResult
            .Value.Should()
            .BeOfType<ControllerErrorResponse>()
            .Which.Message.Should()
            .Be("The email or password provided is incorrect.");
    }

    [Fact(DisplayName = "DeleteAsync should return NotFound when user not exists")]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenUserNotExists()
    {
        UserModel user = UserBuilder.New().Build();

        MockAuthHelper(user.Email);

        _serviceMock
            .Setup(s => s.GetUserByEmail(user.Email))
            .ThrowsAsync(new KeyNotFoundException("The email or password provided is incorrect."));

        IActionResult result = await _controller.DeleteAsync();

        NotFoundObjectResult? objResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        objResult.StatusCode.Should().Be(404);
        objResult
            .Value.Should()
            .BeOfType<ControllerErrorResponse>()
            .Which.Message.Should()
            .Be("The email or password provided is incorrect.");
    }

    private void MockMapper()
    {
        _mapperMock
            .Setup(m => m.Map<UserDto>(It.IsAny<UserModel>()))
            .Returns((UserModel source) => source.AsDto());
    }

    private void MockValidator()
    {
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<UserInsertDto>(), default))
            .ReturnsAsync(new ValidationResult());
    }

    private void MockValidatorFailure()
    {
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<UserInsertDto>(), default))
            .ReturnsAsync(
                new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new()
                        {
                            PropertyName = "PropertyName", ErrorMessage = "ErrorMessage"
                        }
                    }
                )
            );
    }

    private void MockAuthHelper(string userEmail)
    {
        Mock<ClaimsIdentity> identityMock = new();
        identityMock
            .Setup(i => i.Claims)
            .Returns(new List<Claim>
            {
                new(ClaimTypes.Email, userEmail)
            });

        Mock<HttpContext> httpContextMock = new();
        httpContextMock.Setup(h => h.User.Identity).Returns(identityMock.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        _authHelperMock
            .Setup(a => a.GetLoggedUserEmail(It.IsAny<ClaimsIdentity>()))
            .Returns(userEmail);
    }
}
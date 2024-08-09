using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BookingsWebApi.Controllers;
using BookingsWebApi.DTOs;
using BookingsWebApi.Exceptions;
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
        var users = new List<UserModel> { UserBuilder.New().Build(), UserBuilder.New().Build() };
        _serviceMock.Setup(s => s.GetUsers()).ReturnsAsync(users);
        var result = await _controller.GetAsync();
        var objResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objResult.StatusCode.Should().Be(200);
        objResult.Value.Should()
            .BeOfType<ControllerResponse>()
            .Which.Data.Should()
            .NotBeNull();
    }

    [Fact(DisplayName = "PostAsync should return Created with user created")]
    public async Task PostAsync_ShouldReturnCreatedWithUserCreated()
    {
        var user = UserBuilder.New().Build();
        var dto = new UserInsertDto { Email = user.Email, Name = user.Name, Password = user.Password };
        MockValidator();
        _serviceMock.Setup(s => s.AddUser(dto)).ReturnsAsync(user);
        var result = await _controller.PostAsync(dto);
        var objResult = result.Should().BeOfType<CreatedResult>().Subject;
        objResult.StatusCode.Should().Be(201);
        objResult.Value.Should()
            .BeOfType<ControllerResponse>()
            .Which.Data.Should()
            .BeEquivalentTo(user.AsDto());
    }

    [Fact(DisplayName = "PostAsync should return BadRequest when validation fails")]
    public async Task PostAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        var dto = UserBuilder.New().BuildAsInsertDto();
        MockValidatorFailure();
        var act = () => _controller.PostAsync(dto);
        await act.Should().ThrowAsync<InvalidInputDataException>();
    }

    [Fact(DisplayName = "PostAsync should return Conflict when email exists")]
    public async Task PostAsync_ShouldReturnConflict_WhenEmailExists()
    {
        var dto = UserBuilder.New().BuildAsInsertDto();
        MockValidator();
        _serviceMock
            .Setup(s => s.EmailExists(dto.Email))
            .ReturnsAsync(true);
        var act = () => _controller.PostAsync(dto);
        await act.Should()
            .ThrowAsync<InvalidEmailException>()
            .WithMessage("The email provided is already registered.");
    }

    [Fact(DisplayName = "PutAsync should return OK with updated user")]
    public async Task PutAsync_ShouldReturnOkWithUpdatedUser()
    {
        var user = UserBuilder.New().Build();
        var dto = UserBuilder.New().BuildAsInsertDto();
        var userUpdated = new UserModel
        {
            Email = dto.Email ?? string.Empty,
            Name = dto.Name ?? string.Empty,
            Password = dto.Password ?? string.Empty,
            Role = user.Role,
            Id = user.Id
        };
        MockAuthHelper(user.Email);
        MockValidator();
        _serviceMock.Setup(s => s.GetUserByEmail(user.Email)).ReturnsAsync(user);
        _serviceMock.Setup(s => s.UpdateUser(dto, user)).ReturnsAsync(userUpdated);
        var result = await _controller.PutAsync(dto);
        var objResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objResult.StatusCode.Should().Be(200);
        objResult.Value.Should()
            .BeOfType<ControllerResponse>()
            .Which.Data.Should()
            .BeEquivalentTo(userUpdated.AsDto());
    }

    [Fact(DisplayName = "PutAsync should return BadRequest when validation fails")]
    public async Task PutAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        var dto = UserBuilder.New().BuildAsInsertDto();
        MockAuthHelper(dto.Email);
        MockValidatorFailure();
        var act = () => _controller.PutAsync(dto);
        await act.Should().ThrowAsync<InvalidInputDataException>();
    }

    [Fact(DisplayName = "PutAsync should return Unauthorized when user is not authorized")]
    public async Task PutAsync_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
    {
        var user = UserBuilder.New().Build();
        var dto = UserBuilder.New().BuildAsInsertDto();
        MockAuthHelper(user.Email);
        MockValidator();
        _serviceMock
            .Setup(s => s.GetUserByEmail(user.Email))
            .ReturnsAsync((UserModel?)null);
        var act = () => _controller.PutAsync(dto);
        await act.Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("The email or password provided is incorrect.");
    }

    [Fact(DisplayName = "DeleteAsync should return NoContent")]
    public async Task DeleteAsync_ShouldReturnNoContent()
    {
        var user = UserBuilder.New().Build();
        MockAuthHelper(user.Email);
        _serviceMock.Setup(s => s.GetUserByEmail(user.Email)).ReturnsAsync(user);
        _serviceMock.Setup(s => s.DeleteUser(user));
        var result = await _controller.DeleteAsync();
        var objResult = result.Should().BeOfType<NoContentResult>().Subject;
        objResult.StatusCode.Should().Be(204);
    }

    [Fact(DisplayName = "DeleteAsync should return Unauthorized when user is not authorized")]
    public async Task DeleteAsync_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
    {
        var user = UserBuilder.New().Build();
        MockAuthHelper(user.Email);
        _serviceMock
            .Setup(s => s.GetUserByEmail(user.Email))
            .ReturnsAsync((UserModel?)null);
        var act = () => _controller.DeleteAsync();
        await act.Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("The email or password provided is incorrect.");
    }

    [Fact(DisplayName = "DeleteAsync should return NotFound when user not exists")]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenUserNotExists()
    {
        var user = UserBuilder.New().Build();
        MockAuthHelper(user.Email);
        _serviceMock
            .Setup(s => s.GetUserByEmail(user.Email))
            .ReturnsAsync((UserModel?)null);
        var act = () => _controller.DeleteAsync();
        await act.Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("The email or password provided is incorrect.");
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
                        new() { PropertyName = "PropertyName", ErrorMessage = "ErrorMessage" }
                    }
                )
            );
    }

    private void MockAuthHelper(string? userEmail)
    {
        Mock<ClaimsIdentity> identityMock = new();
        identityMock
            .Setup(i => i.Claims)
            .Returns(new List<Claim> { new(ClaimTypes.Email, userEmail ?? string.Empty) });
        Mock<HttpContext> httpContextMock = new();
        httpContextMock.Setup(h => h.User.Identity).Returns(identityMock.Object);
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContextMock.Object };
        _authHelperMock
            .Setup(a => a.GetLoggedUserEmail(It.IsAny<ClaimsIdentity>()))
            .Returns(userEmail ?? string.Empty);
    }
}
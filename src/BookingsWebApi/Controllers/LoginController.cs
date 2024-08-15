using BookingsWebApi.Controllers.Interfaces;
using BookingsWebApi.DTOs;
using BookingsWebApi.Exceptions;
using BookingsWebApi.Helpers;
using BookingsWebApi.Models;
using BookingsWebApi.Services;
using BookingsWebApi.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class LoginController : Controller, ILoginController
{
    private readonly IUserService _service;
    private readonly IValidator<LoginInsertDto> _validator;
    private readonly TokenService _tokenService;

    public LoginController(IUserService service,
        IValidator<LoginInsertDto> validator,
        TokenService tokenService
    )
    {
        _service = service;
        _validator = validator;
        _tokenService = tokenService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginInsertDto dto)
    {
        var errors = await GetInputDataErrors(dto);
        if (errors != null)
        {
            throw new InvalidInputDataException(string.Join(" ", errors));
        }

        var userFound = await _service.GetUserByEmail(dto.Email);
        if (userFound == null)
        {
            throw new UnauthorizedException("The email or password provided is incorrect.");
        }

        var isValidPassword = IsValidPassword(dto.Password, userFound);
        if (!isValidPassword)
        {
            throw new UnauthorizedException("The email or password provided is incorrect.");
        }

        var token = _tokenService.Generate(userFound);
        return Ok(new ControllerResponse { Data = token });
    }

    private async Task<IEnumerable<string>?> GetInputDataErrors(LoginInsertDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (validationResult.IsValid)
        {
            return null;
        }

        var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage);
        return errorMessages;
    }

    private static bool IsValidPassword(string? passwordTyped, UserModel user)
    {
        return HashPassword.VerifyPassword(passwordTyped, user.Password, user.Salt);
    }
}
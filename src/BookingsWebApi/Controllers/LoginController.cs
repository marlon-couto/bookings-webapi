using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
using BookingsWebApi.Services;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class LoginController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly UserService _service;
    private readonly IValidator<LoginInsertDto> _validator;

    public LoginController(
        UserService service,
        IValidator<LoginInsertDto> validator,
        IConfiguration configuration
    )
    {
        _service = service;
        _validator = validator;
        _configuration = configuration;
    }

    /// <summary>
    ///     Logs in a user using the provided data.
    /// </summary>
    /// <param name="dto">The data to validate the user.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     POST /api/login
    ///     {
    ///     "email": "email@mail.com",
    ///     "password": "Pass+12345"
    ///     }
    /// </remarks>
    /// <response code="201">Returns 201 and a JWT token.</response>
    /// <response code="401">
    ///     If the email or password is incorrect, returns 401 and an
    ///     error message.
    /// </response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error
    ///     message.
    /// </response>
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginInsertDto dto)
    {
        try
        {
            await ValidateInputData(dto);

            User userFound = await _service.GetUserByEmail(dto.Email);
            IsValidPassword(dto.Password, userFound.Password);

            string token = new TokenService(_configuration).Generate(userFound);
            return Ok(new { Data = token, Result = "Success" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message, Result = "Error" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { ex.Message, Result = "Error" });
        }
    }

    private async Task ValidateInputData(LoginInsertDto dto)
    {
        ValidationResult? validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            List<string> errorMessages = validationResult
                .Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ArgumentException(string.Join(" ", errorMessages));
        }
    }

    private static void IsValidPassword(string inputPassword, string dbPassword)
    {
        bool isValidPassword = inputPassword == dbPassword; // TODO: improve implementation for encrypted passwords.
        if (!isValidPassword)
        {
            throw new UnauthorizedAccessException("The email or password provided is incorrect.");
        }
    }
}
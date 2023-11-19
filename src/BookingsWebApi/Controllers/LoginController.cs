using BookingsWebApi.Dtos;
using BookingsWebApi.Models;
using BookingsWebApi.Repositories;
using BookingsWebApi.Services;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : Controller
{
    private readonly IUserRepository _repository;
    private readonly IValidator<LoginInsertDto> _validator;
    private readonly ITokenGenerator _tokenGenerator;
    public LoginController(IUserRepository repository, IValidator<LoginInsertDto> validator, ITokenGenerator tokenGenerator)
    {
        _repository = repository;
        _validator = validator;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginInsertDto inputData)
    {
        try
        {
            await ValidateInputData(inputData);

            User userFound = await _repository.GetUserByEmail(inputData.Email);
            IsValidPassword(inputData.Password, userFound.Password);

            var token = _tokenGenerator.Generate(userFound);
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

    private async Task ValidateInputData(LoginInsertDto inputData)
    {
        var validationResult = await _validator.ValidateAsync(inputData);
        if (!validationResult.IsValid)
        {
            List<string> errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ArgumentException(string.Join(" ", errorMessages));
        }
    }

    private static void IsValidPassword(string inputPassword, string dbPassword)
    {
        bool isValidPassword = inputPassword == dbPassword; // TODO: melhorar a implementação para criptografar e descriptografar as senhas.
        if (!isValidPassword)
        {
            throw new UnauthorizedAccessException("The email or password provided is incorrect");
        }
    }
}

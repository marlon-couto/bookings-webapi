using BookingsWebApi.Dtos;
using BookingsWebApi.Repositories;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserRepository _repository;
    private readonly IValidator<UserInsertDto> _validator;
    public UserController(IUserRepository repository, IValidator<UserInsertDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        List<UserDto> allUsers = await _repository.GetAllUsers();
        return Ok(new { Data = allUsers, Result = "Success" });
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] UserInsertDto inputData)
    {
        try
        {
            await ValidateInputData(inputData);
            await _repository.EmailExists(inputData.Email);

            UserDto createdUser = await _repository.AddUser(inputData);
            return Created("/api/login", new { Data = createdUser, Result = "Success" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message, Result = "Error" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { ex.Message, Result = "Error" });
        }
    }

    private async Task ValidateInputData(UserInsertDto inputData)
    {
        var validationResult = await _validator.ValidateAsync(inputData);
        if (!validationResult.IsValid)
        {
            List<string> errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ArgumentException(string.Join(" ", errorMessages));
        }
    }
}

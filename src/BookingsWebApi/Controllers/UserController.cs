using System.Security.Claims;
using AutoMapper;
using BookingsWebApi.DTOs;
using BookingsWebApi.Exceptions;
using BookingsWebApi.Helpers;
using BookingsWebApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UserController : Controller, IUserController
{
    private readonly IAuthHelper _authHelper;
    private readonly IMapper _mapper;
    private readonly IUserService _service;
    private readonly IValidator<UserInsertDto> _validator;

    public UserController(
        IUserService service,
        IMapper mapper,
        IValidator<UserInsertDto> validator,
        IAuthHelper authHelper
    )
    {
        _service = service;
        _mapper = mapper;
        _validator = validator;
        _authHelper = authHelper;
    }

    [HttpGet]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> GetAsync()
    {
        var users = await _service.GetUsers();
        var usersMapped = users.Select(u => _mapper.Map<UserDto>(u));
        return Ok(new ControllerResponse { Data = usersMapped });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> PostAsync([FromBody] UserInsertDto dto)
    {
        var errors = await GetInputDataErrors(dto);
        if (errors != null)
        {
            throw new InvalidInputDataException(string.Join(" ", errors));
        }

        var emailExists = await _service.EmailExists(dto.Email);
        if (emailExists)
        {
            throw new InvalidEmailException("The email provided is already registered.");
        }

        var userCreated = await _service.AddUser(dto);
        var userMapped = _mapper.Map<UserDto>(userCreated);
        return Created("/api/login", new ControllerResponse { Data = userMapped, StatusCode = 201 });
    }

    [HttpPut]
    [Authorize(Policy = "Client")]
    public async Task<IActionResult> PutAsync([FromBody] UserInsertDto dto)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userEmail = _authHelper.GetLoggedUserEmail(identity);
        var errors = await GetInputDataErrors(dto);
        if (errors != null)
        {
            throw new InvalidInputDataException(string.Join(" ", errors));
        }

        var userFound = await _service.GetUserByEmail(userEmail);
        if (userFound == null)
        {
            throw new UnauthorizedException("The email or password provided is incorrect.");
        }

        var userUpdated = await _service.UpdateUser(dto, userFound);
        var userMapped = _mapper.Map<UserDto>(userUpdated);
        return Ok(new ControllerResponse { Data = userMapped });
    }

    [HttpDelete]
    [Authorize(Policy = "Client")]
    public async Task<IActionResult> DeleteAsync()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userEmail = _authHelper.GetLoggedUserEmail(identity);
        var userFound = await _service.GetUserByEmail(userEmail);
        if (userFound == null)
        {
            throw new UnauthorizedException("The email or password provided is incorrect.");
        }

        await _service.DeleteUser(userFound);
        return NoContent();
    }

    private async Task<IEnumerable<string>?> GetInputDataErrors(UserInsertDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (validationResult.IsValid)
        {
            return null;
        }

        var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage);
        return errorMessages;
    }
}
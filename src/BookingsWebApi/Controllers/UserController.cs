using System.Security.Claims;
using AutoMapper;
using BookingsWebApi.DTOs;
using BookingsWebApi.Helpers;
using BookingsWebApi.Models;
using BookingsWebApi.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UserController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<UserInsertDto> _validator;

    public UserController(
        IUserRepository userRepository,
        IMapper mapper,
        IValidator<UserInsertDto> validator
    )
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _validator = validator;
    }

    /// <summary>
    ///     Retrieves user information from the database.
    /// </summary>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <response code="200">Returns 200 and the user data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    [HttpGet]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> GetAsync()
    {
        List<User> allUsers = await _userRepository.GetAllUsers();
        return Ok(
            new
            {
                Data = allUsers.Select(u => _mapper.Map<UserDto>(u)).ToList(),
                Result = "Success"
            }
        );
    }

    /// <summary>
    ///     Creates a new user for based on the provided data.
    /// </summary>
    /// <param name="inputData">The data for creating a new user.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     POST /api/user
    ///     {
    ///     "name": "New User",
    ///     "email": "email@mail.com",
    ///     "password": "Pass12345!"
    ///     }
    /// </remarks>
    /// <response code="201">Returns 201 and the newly created user data.</response>
    /// <response code="400">If the input data is invalid, returns 400 and an error message.</response>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> PostAsync([FromBody] UserInsertDto inputData)
    {
        try
        {
            await ValidateInputData(inputData);
            await _userRepository.EmailExists(inputData.Email);

            User createdUser = await _userRepository.AddUser(inputData);
            return Created(
                "/api/login",
                new { Data = _mapper.Map<UserDto>(createdUser), Result = "Success" }
            );
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

    /// <summary>
    ///     Updates the logged user based on the provided data.
    /// </summary>
    /// <param name="inputData">The data for updating the user retrieved.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     PUT /api/user
    ///     {
    ///     "name": "New User v2",
    ///     "email": "email@mail.com",
    ///     "password": "Password@123456"
    ///     }
    /// </remarks>
    /// <response code="200">Returns 200 and the updated user data.</response>
    /// <response code="401">If the user is unauthorized, returns 401 and a error message.</response>
    /// <response code="400">If the input data is invalid, returns 400 and an error message.</response>
    [HttpPut]
    [Authorize(Policy = "Client")]
    public async Task<IActionResult> PutAsync([FromBody] UserInsertDto inputData)
    {
        try
        {
            string userEmail = AuthHelper.GetLoggedUserEmail(
                HttpContext.User.Identity as ClaimsIdentity
            );
            await _userRepository.GetUserByEmail(userEmail);

            await ValidateInputData(inputData);

            User userFound = await _userRepository.GetUserByEmail(userEmail);

            User updatedUser = await _userRepository.UpdateUser(inputData, userFound);
            return Ok(new { Data = _mapper.Map<UserDto>(updatedUser), Result = "Success" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { ex.Message, Result = "Error" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Deletes the logged user from the database.
    /// </summary>
    /// <returns>A status code 204 and no content.</returns>
    /// <response code="204">Returns 204 with no content.</response>
    /// <response code="401">If the user is unauthorized, returns 401 and a error message.</response>
    [HttpDelete]
    [Authorize(Policy = "Client")]
    public async Task<IActionResult> DeleteAsync()
    {
        try
        {
            string userEmail = AuthHelper.GetLoggedUserEmail(
                HttpContext.User.Identity as ClaimsIdentity
            );
            await _userRepository.GetUserByEmail(userEmail);

            User userFound = await _userRepository.GetUserByEmail(userEmail);
            await _userRepository.DeleteUser(userFound);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { ex.Message, Result = "Error" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { ex.Message, Result = "Error" });
        }
    }

    private async Task ValidateInputData(UserInsertDto inputData)
    {
        ValidationResult? validationResult = await _validator.ValidateAsync(inputData);
        if (!validationResult.IsValid)
        {
            List<string> errorMessages = validationResult
                .Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ArgumentException(string.Join(" ", errorMessages));
        }
    }
}

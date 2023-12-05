using System.Security.Claims;

using AutoMapper;

using BookingsWebApi.DTOs;
using BookingsWebApi.Helpers;
using BookingsWebApi.Models;
using BookingsWebApi.Services;

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
    private readonly IUserService _service;
    private readonly IValidator<UserInsertDto> _validator;

    public UserController(IUserService service, IMapper mapper, IValidator<UserInsertDto> validator)
    {
        _service = service;
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
        List<User> users = await _service.GetUsers();
        List<UserDto> usersMapped = users.Select(u => _mapper.Map<UserDto>(u)).ToList();

        return Ok(new ControllerListResponse<UserDto> { Data = usersMapped, Result = "Success" });
    }

    /// <summary>
    ///     Creates a new user for based on the provided data.
    /// </summary>
    /// <param name="dto">The data for creating a new user.</param>
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
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> PostAsync([FromBody] UserInsertDto dto)
    {
        try
        {
            await ValidateInputData(dto);
            await _service.EmailExists(dto.Email);

            User userCreated = await _service.AddUser(dto);
            UserDto userMapped = _mapper.Map<UserDto>(userCreated);

            return Created("/api/login", new ControllerResponse<UserDto> { Data = userMapped, Result = "Success" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ControllerErrorResponse { Message = ex.Message, Result = "Error" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ControllerErrorResponse { Message = ex.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Updates the logged user based on the provided data.
    /// </summary>
    /// <param name="dto">The data for updating the user retrieved.</param>
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
    /// <response code="401">
    ///     If the user is unauthorized, returns 401 and a error message.
    /// </response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    [HttpPut]
    [Authorize(Policy = "Client")]
    public async Task<IActionResult> PutAsync([FromBody] UserInsertDto dto)
    {
        try
        {
            string userEmail = AuthHelper.GetLoggedUserEmail(HttpContext.User.Identity as ClaimsIdentity);
            await _service.GetUserByEmail(userEmail);

            await ValidateInputData(dto);

            User userFound = await _service.GetUserByEmail(userEmail);

            User userUpdated = await _service.UpdateUser(dto, userFound);
            UserDto userMapped = _mapper.Map<UserDto>(userUpdated);

            return Ok(new ControllerResponse<UserDto> { Data = userMapped, Result = "Success" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ControllerErrorResponse { Message = ex.Message, Result = "Error" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ControllerErrorResponse { Message = ex.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Deletes the logged user from the database.
    /// </summary>
    /// <returns>A status code 204 and no content.</returns>
    /// <response code="204">Returns 204 with no content.</response>
    /// <response code="401">
    ///     If the user is unauthorized, returns 401 and a error message.
    /// </response>
    [HttpDelete]
    [Authorize(Policy = "Client")]
    public async Task<IActionResult> DeleteAsync()
    {
        try
        {
            string userEmail = AuthHelper.GetLoggedUserEmail(HttpContext.User.Identity as ClaimsIdentity);
            await _service.GetUserByEmail(userEmail);

            User userFound = await _service.GetUserByEmail(userEmail);
            await _service.DeleteUser(userFound);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ControllerErrorResponse { Message = ex.Message, Result = "Error" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ControllerErrorResponse { Message = ex.Message, Result = "Error" });
        }
    }

    private async Task ValidateInputData(UserInsertDto dto)
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
}
using BookingsWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers.Interfaces;

public interface ILoginController
{
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
    ///     If the email or password is incorrect, returns 401 and an error message.
    /// </response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    public Task<IActionResult> Login([FromBody] LoginInsertDto dto);
}

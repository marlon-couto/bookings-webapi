using BookingsWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers.Interfaces;

public interface IUserController
{
    /// <summary>
    ///     Retrieves user information from the database.
    /// </summary>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <response code="200">Returns 200 and the user data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    public Task<IActionResult> GetAsync();

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
    public Task<IActionResult> PostAsync([FromBody] UserInsertDto dto);

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
    public Task<IActionResult> PutAsync([FromBody] UserInsertDto dto);

    /// <summary>
    ///     Deletes the logged user from the database.
    /// </summary>
    /// <returns>A status code 204 and no content.</returns>
    /// <response code="204">Returns 204 with no content.</response>
    /// <response code="401">
    ///     If the user is unauthorized, returns 401 and a error message.
    /// </response>
    public Task<IActionResult> DeleteAsync();
}
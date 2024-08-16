using BookingsWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers.Interfaces;

public interface ICityController
{
    /// <summary>
    ///     Retrieves all cities information.
    /// </summary>
    /// <returns>A JSON response representing the result of the operation</returns>
    /// <response code="200">Returns 200 and the city data.</response>
    public Task<IActionResult> GetAsync();

    /// <summary>
    ///     Creates a new city based on the provided data.
    /// </summary>
    /// <param name="dto">The data for creating a new city.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     POST /api/city
    ///     {
    ///     "name": "New City",
    ///     "state": "State 1"
    ///     }
    /// </remarks>
    /// <response code="201">Returns 201 and the newly created city data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    public Task<IActionResult> PostAsync(CityInsertDto dto);

    /// <summary>
    ///     Updates the city with the given ID based on the provided data.
    /// </summary>
    /// <param name="dto">The data for updating the city retrieved.</param>
    /// <param name="id">The ID of the city to update.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     PUT /api/city/1
    ///     {
    ///     "name": "New City v2",
    ///     "state": "State 1"
    ///     }
    /// </remarks>
    /// <response code="200">Returns 201 and the updated city data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    /// <response code="404">
    ///     If a city with the provided ID not exists, returns 404 and an error message.
    /// </response>
    public Task<IActionResult> PutAsync([FromBody] CityInsertDto dto, Guid id);

    /// <summary>
    ///     Deletes a city with the given ID.
    /// </summary>
    /// <param name="id">The ID of the city to delete.</param>
    /// <returns>A status code 204 and no content.</returns>
    /// <response code="204">Returns 204 with no content.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="404">
    ///     If the city is not found, returns 404 and an error message.
    /// </response>
    public Task<IActionResult> DeleteAsync(Guid id);
}

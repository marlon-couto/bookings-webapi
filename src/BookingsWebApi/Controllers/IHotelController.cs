using BookingsWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers;

public interface IHotelController
{
    /// <summary>
    ///     Retrieves hotel information by ID.
    /// </summary>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <response code="200">Returns 200 and the hotel data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    public Task<IActionResult> GetAsync();

    /// <summary>
    ///     Retrieves room information by associated hotel ID.
    /// </summary>
    /// <param name="id">The ID of the hotel that the rooms will be retrieved.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <response code="200">Returns 200 and the rooms data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    public Task<IActionResult> GetHotelRoomsAsync(Guid id);

    /// <summary>
    ///     Creates a new hotel for based on the provided data.
    /// </summary>
    /// <param name="dto">The data for creating a new hotel.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     POST /api/hotel
    ///     {
    ///     "name": "New Hotel",
    ///     "address": "Address 1",
    ///     "cityId": "1"
    ///     }
    /// </remarks>
    /// <response code="201">Returns 201 and the newly created hotel data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="404">
    ///     If the associated city is not found, returns 404 and a error message.
    /// </response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    public Task<IActionResult> PostAsync([FromBody] HotelInsertDto dto);

    /// <summary>
    ///     Updates the hotel with the given ID based on the provided data.
    /// </summary>
    /// <param name="dto">The data for updating the hotel retrieved.</param>
    /// <param name="id">The ID of the hotel to update.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     PUT /api/hotel/1
    ///     {
    ///     "name": "New Hotel v2",
    ///     "address": "Address 1",
    ///     "cityId": "1"
    ///     }
    /// </remarks>
    /// <response code="200">Returns 200 and the updated hotel data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    /// <response code="404">
    ///     If a hotel with the provided ID not exists or the associated city is not found, returns 404 and an error
    ///     message.
    /// </response>
    public Task<IActionResult> PutAsync([FromBody] HotelInsertDto dto, Guid id);

    /// <summary>
    ///     Deletes a hotel with the given ID.
    /// </summary>
    /// <param name="id">The ID of the hotel to delete.</param>
    /// <returns>A status code 204 and no content.</returns>
    /// <response code="204">Returns 204 with no content.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="404">
    ///     If the hotel is not found, returns 404 and an error message.
    /// </response>
    public Task<IActionResult> DeleteAsync(Guid id);
}
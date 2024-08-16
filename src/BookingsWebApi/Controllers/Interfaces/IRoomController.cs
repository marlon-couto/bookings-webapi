using BookingsWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers.Interfaces;

public interface IRoomController
{
    /// <summary>
    ///     Retrieves all rooms information from the database.
    /// </summary>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <response code="200">Returns 200 and the rooms data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    public Task<IActionResult> GetAsync();

    /// <summary>
    ///     Creates a new room for based on the provided data.
    /// </summary>
    /// <param name="dto">The data for creating a new room.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     POST /api/room
    ///     {
    ///     "name": "New Room",
    ///     "capacity": 1,
    ///     "image": "https://img.url",
    ///     "hotelId": "1"
    ///     }
    /// </remarks>
    /// <response code="201">Returns 201 and the newly created room data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="404">
    ///     If the associated hotel is not found, returns 404 and a error message.
    /// </response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    public Task<IActionResult> PostAsync([FromBody] RoomInsertDto dto);

    /// <summary>
    ///     Updates the room with the given ID based on the provided data.
    /// </summary>
    /// <param name="dto">The data for updating the room retrieved.</param>
    /// <param name="id">The ID of the room to update.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     PUT /api/room/1
    ///     {
    ///     "name": "New Room v2",
    ///     "capacity": 2,
    ///     "image": "https://img.url",
    ///     "hotelId": "1"
    ///     }
    /// </remarks>
    /// <response code="200">Returns 200 and the updated room data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    /// <response code="404">
    ///     If a room with the provided ID not exists or the associated hotel is not found, returns 404 and an error
    ///     message.
    /// </response>
    public Task<IActionResult> PutAsync([FromBody] RoomInsertDto dto, Guid id);

    /// <summary>
    ///     Deletes a room with the given ID.
    /// </summary>
    /// <param name="id">The ID of the room to delete.</param>
    /// <returns>A status code 204 and no content.</returns>
    /// <response code="204">Returns 204 with no content.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="404">
    ///     If the room is not found, returns 404 and an error message.
    /// </response>
    public Task<IActionResult> DeleteAsync(Guid id);
}

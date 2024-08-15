using BookingsWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers.Interfaces;

public interface IBookingController
{
    /// <summary>
    ///     Retrieves all bookings information for the logged user.
    /// </summary>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <response code="200">Returns 200 and the booking data.</response>
    /// <response code="401">
    ///     If the user is unauthorized, returns 401 and an error message.
    /// </response>
    public Task<IActionResult> GetAsync();

    /// <summary>
    ///     Retrieves booking information by ID for the logged user.
    /// </summary>
    /// <param name="id">The ID of the booking to retrieve.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <response code="200">Returns 200 and the booking data.</response>
    /// <response code="401">
    ///     If the user is unauthorized, returns 401 and an error message.
    /// </response>
    /// <response code="404">
    ///     If the booking is not found, returns 404 and an error message.
    /// </response>
    public Task<IActionResult> GetAsync(Guid id);

    /// <summary>
    ///     Creates a new booking for the logged user based on the provided data.
    /// </summary>
    /// <param name="dto">The data for creating a new booking.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     POST /api/booking
    ///     {
    ///     "checkIn": "08/11/23",
    ///     "checkOut": "09/11/23",
    ///     "guestQuantity": 1,
    ///     "roomId": "1"
    ///     }
    /// </remarks>
    /// <response code="201">Returns 201 and the newly created booking data.</response>
    /// <response code="401">
    ///     If the user is unauthorized, returns 401 and an error message.
    /// </response>
    /// <response code="404">
    ///     If the associated room is not found, returns 404 and an error message.
    /// </response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    public Task<IActionResult> PostAsync([FromBody] BookingInsertDto dto);

    /// <summary>
    ///     Updates the booking with the given ID based on the provided data.
    /// </summary>
    /// <param name="dto">The data for updating the booking retrieved.</param>
    /// <param name="id">The ID of the booking to update.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     PUT /api/booking/1
    ///     {
    ///     "checkIn": "20/11/23",
    ///     "checkOut": "21/11/23",
    ///     "guestQuantity": 1,
    ///     "roomId": "2"
    ///     }
    /// </remarks>
    /// <response code="200">Returns 200 and the updated booking data.</response>
    /// <response code="401">
    ///     If the user is unauthorized, returns 401 and a error message.
    /// </response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    /// <response code="404">
    ///     If a booking with the provided ID not exists or the associated room is not found, returns 404 and
    ///     an error message.
    /// </response>
    public Task<IActionResult> PutAsync([FromBody] BookingInsertDto dto, Guid id);

    /// <summary>
    ///     Deletes a booking with the given ID.
    /// </summary>
    /// <param name="id">The ID of the booking to delete.</param>
    /// <returns>A status code 204 and no content.</returns>
    /// <response code="204">Returns 204 with no content.</response>
    /// <response code="401">
    ///     If the user is unauthorized, returns 401 and a error message.
    /// </response>
    /// <response code="404">
    ///     If the booking is not found, returns 404 and an error message.
    /// </response>
    public Task<IActionResult> DeleteAsync(Guid id);
}
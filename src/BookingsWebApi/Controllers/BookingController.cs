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
[Authorize(Policy = "Client")]
public class BookingController : Controller
{
    private readonly IMapper _mapper;
    private readonly IBookingRepository _repository;
    private readonly IValidator<BookingInsertDto> _validator;

    public BookingController(IBookingRepository repository, IMapper mapper, IValidator<BookingInsertDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    /// <summary>
    ///     Retrieves all bookings information for the logged user.
    /// </summary>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <response code="200">Returns 200 and the booking data.</response>
    /// <response code="401">If the user is unauthorized, returns 401 and an error message.</response>
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            string userEmail = AuthHelper.GetLoggedUserEmail(HttpContext.User.Identity as ClaimsIdentity);
            List<Booking> allBookings = await _repository.GetAllBookings(userEmail);
            return Ok(new { Data = allBookings.Select(b => _mapper.Map<BookingDto>(b)), Result = "Success" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { ex.Message, Result = "Error" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { ex.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Retrieves booking information by ID for the logged user.
    /// </summary>
    /// <param name="id">The ID of the booking to retrieve.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <response code="200">Returns 200 and the booking data.</response>
    /// <response code="401">If the user is unauthorized, returns 401 and an error message.</response>
    /// <response code="404">If the booking is not found, returns 404 and an error message.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        try
        {
            string userEmail = AuthHelper.GetLoggedUserEmail(HttpContext.User.Identity as ClaimsIdentity);
            Booking bookingFound = await _repository.GetBookingById(id, userEmail);
            return Ok(new { Data = _mapper.Map<BookingDto>(bookingFound), Result = "Success" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { ex.Message, Result = "Error" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { ex.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Creates a new booking for the logged user based on the provided data.
    /// </summary>
    /// <param name="inputData">The data for creating a new booking.</param>
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
    /// <response code="401">If the user is unauthorized, returns 401 and an error message.</response>
    /// <response code="404">If the associated room is not found, returns 404 and an error message.</response>
    /// <response code="400">If the input data is invalid, returns 400 and an error message.</response>
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] BookingInsertDto inputData)
    {
        try
        {
            string userEmail = AuthHelper.GetLoggedUserEmail(HttpContext.User.Identity as ClaimsIdentity);
            User userFound = await _repository.GetUserByEmail(userEmail);

            await ValidateInputData(inputData);

            Room roomFound = await _repository.GetRoomById(inputData.RoomId);
            HasEnoughCapacity(inputData, roomFound);

            Booking createdBooking = await _repository.AddBooking(inputData, userFound, roomFound);
            return Created($"/api/booking/{createdBooking.BookingId}",
                new { Data = _mapper.Map<BookingDto>(createdBooking), Result = "Success" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { ex.Message, Result = "Error" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { ex.Message, Result = "Error" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Updates the booking with the given ID based on the provided data.
    /// </summary>
    /// <param name="inputData">The data for updating the booking retrieved.</param>
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
    /// <response code="401">If the user is unauthorized, returns 401 and a error message.</response>
    /// <response code="400">If the input data is invalid, returns 400 and an error message.</response>
    /// <response code="404">
    ///     If a booking with the provided ID not exists or the associated room is not found, returns 404 and
    ///     an error message.
    /// </response>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromBody] BookingInsertDto inputData, string id)
    {
        try
        {
            string userEmail = AuthHelper.GetLoggedUserEmail(HttpContext.User.Identity as ClaimsIdentity);
            await _repository.GetUserByEmail(userEmail);

            await ValidateInputData(inputData);

            Booking bookingFound = await _repository.GetBookingById(id, userEmail);

            Room roomFound = await _repository.GetRoomById(inputData.RoomId);
            HasEnoughCapacity(inputData, roomFound);

            Booking updatedBooking = _repository.UpdateBooking(inputData, bookingFound, roomFound);
            return Ok(new { Data = _mapper.Map<BookingDto>(updatedBooking), Result = "Success" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { ex.Message, Result = "Error" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { ex.Message, Result = "Error" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Deletes a booking with the given ID.
    /// </summary>
    /// <param name="id">The ID of the booking to delete.</param>
    /// <returns>A status code 204 and no content.</returns>
    /// <response code="204">Returns 204 with no content.</response>
    /// <response code="401">If the user is unauthorized, returns 401 and a error message.</response>
    /// <response code="404">If the booking is not found, returns 404 and an error message.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        try
        {
            string userEmail = AuthHelper.GetLoggedUserEmail(HttpContext.User.Identity as ClaimsIdentity);
            await _repository.GetUserByEmail(userEmail);

            Booking bookingFound = await _repository.GetBookingById(id, userEmail);
            _repository.DeleteBooking(bookingFound);
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

    // Validates input data. If they are not valid, it returns the associated error messages.
    private async Task ValidateInputData(BookingInsertDto inputData)
    {
        ValidationResult? validationResult = await _validator.ValidateAsync(inputData);
        if (!validationResult.IsValid)
        {
            List<string> errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ArgumentException(string.Join(" ", errorMessages));
        }
    }

    private static void HasEnoughCapacity(BookingInsertDto inputData, Room roomFound)
    {
        bool hasEnoughCapacity = roomFound.Capacity >= inputData.GuestQuantity;
        if (!hasEnoughCapacity)
        {
            throw new ArgumentException("The number of guests exceeds the maximum capacity");
        }
    }
}
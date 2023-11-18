using System.Security.Claims;

using BookingsWebApi.Dtos;
using BookingsWebApi.Helpers;
using BookingsWebApi.Models;
using BookingsWebApi.Repositories;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Policy = "Client")]
    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        private readonly IValidator<BookingInsertDto> _validator;
        public BookingController(IBookingRepository repository, IValidator<BookingInsertDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        /// <summary>
        /// Retrieves booking information by ID for the logged user.
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
                string userEmail = new AuthHelper().GetLoggedUserEmail(HttpContext.User.Identity as ClaimsIdentity);
                BookingDto bookingFound = await _repository.GetBookingById(id, userEmail);
                return Ok(new { Data = bookingFound, Result = "Success" });
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
        /// Creates a new booking for the logged user based on the provided data.
        /// </summary>
        /// <param name="inputData">The data for creating a new booking.</param>
        /// <returns>A JSON response representing the result of the operation.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Booking
        ///     {
        ///         "checkIn": "08/11/23",
        ///         "checkOut": "09/11/23",
        ///         "guestQuantity": 1,
        ///         "roomId": "1"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns 201 and the newly created booking data.</response>
        /// <response code="401">If the user is unauthorized, returns 401 and an error message.</response>
        /// <response code="404">If the room is not found, returns 404 and an error message.</response>
        /// <response code="400">If the input data is invalid, returns 400 and an error message.</response>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] BookingInsertDto inputData)
        {
            try
            {
                string userEmail = new AuthHelper().GetLoggedUserEmail(HttpContext.User.Identity as ClaimsIdentity);
                User userFound = await _repository.GetUserByEmail(userEmail);

                await ValidateInputData(inputData);

                Room roomFound = await _repository.GetRoomById(inputData.RoomId);
                HasEnoughCapacity(inputData, roomFound);

                BookingDto createdBooking = await _repository.AddBooking(inputData, userFound, roomFound);
                return Created($"/api/booking/{createdBooking.BookingId}", new
                {
                    Data = createdBooking,
                    Result = "Success"
                });
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

        private static void HasEnoughCapacity(BookingInsertDto inputData, Room roomFound)
        {
            bool hasEnoughCapacity = roomFound.Capacity >= inputData.GuestQuantity;
            if (!hasEnoughCapacity)
            {
                throw new ArgumentException("The number of guests exceeds the maximum capacity");
            }
        }

        private async Task ValidateInputData(BookingInsertDto inputData)
        {
            var validationResult = await _validator.ValidateAsync(inputData);
            if (!validationResult.IsValid)
            {
                List<string> errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ArgumentException(string.Join(" ", errorMessages));
            }
        }
    }
}

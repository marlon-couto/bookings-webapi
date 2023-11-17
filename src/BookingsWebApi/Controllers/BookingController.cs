using BookingsWebApi.Dtos;
using BookingsWebApi.Models;
using BookingsWebApi.Repositories;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers
{
    /// <summary>
    /// Controller for interact with bookings in the application.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        private readonly IValidator<BookingInsertDto> _validator;
        /// <summary>
        /// Initializes a new instance of the <see cref="BookingController"/> class.
        /// </summary>
        /// <param name="repository">The repository for the bookings.</param>
        /// <param name="validator">The Validator instance for request body validation.</param>
        public BookingController(IBookingRepository repository, IValidator<BookingInsertDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        /// <summary>
        /// Retrieves booking information by ID for the logged user.
        /// </summary>
        /// <param name="id">The ID of the booking to retrieve.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the operation.
        /// If successful, returns an OK result with booking data.
        /// If the user is unauthorized, returns an Unauthorized result with an error message.
        /// If the booking is not found, returns a NotFound result with an error message.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                string userEmail = "user1@mail.com";
                await _repository.GetUserByEmail(userEmail);

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
        /// Checks if the provided room has enough capacity for the specified number of guests.
        /// </summary>
        /// <param name="inputData">The data containing the guest quantity for the booking.</param>
        /// <param name="roomFound">The room for which the capacity check is performed.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if the number of guests in the booking exceeds the maximum capacity of the room.
        /// </exception>
        private static void HasEnoughCapacity(BookingInsertDto inputData, Room roomFound)
        {
            bool hasEnoughCapacity = roomFound.Capacity >= inputData.GuestQuantity;
            if (!hasEnoughCapacity)
            {
                throw new ArgumentException("The number of guests exceeds the maximum capacity");
            }
        }

        /// <summary>
        /// Creates a new booking for the logged user based on the provided data.
        /// </summary>
        /// <param name="inputData">The data for creating a new booking.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the operation.
        /// If successful, returns a Created result with the newly created booking data.
        /// If the user is unauthorized, returns an Unauthorized result with an error message.
        /// If the room is not found, returns a NotFound result with an error message.
        /// If the input data is invalid, returns a BadRequest result with an error message.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] BookingInsertDto inputData)
        {
            try
            {
                string userEmail = "user1@mail.com";
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

        /// <summary>
        /// Validates the input data for creating a new booking.
        /// </summary>
        /// <param name="inputData">The data to validate.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if the input data is not valid, and contains all the error messages.
        /// </exception>
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

using BookingsWebApi.Dtos;
using BookingsWebApi.Models;
using BookingsWebApi.Repositories;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        private readonly IValidator<BookingInsertDto> _validator;
        public BookingController(IBookingRepository repository, IValidator<BookingInsertDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                string userEmail = "user1@mail.com";
                await _repository.GetUserByEmail(userEmail);

                BookingDto bookingFound = await _repository.GetBookingById(id, userEmail);
                return Ok(bookingFound);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
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
                return Created($"/api/booking/{createdBooking.BookingId}", createdBooking);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        private async Task ValidateInputData(BookingInsertDto inputData)
        {
            var validationResult = await _validator.ValidateAsync(inputData);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.Errors[0].ErrorMessage);
            }
        }
    }
}

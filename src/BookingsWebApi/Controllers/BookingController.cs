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

        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetAsync(string bookingId)
        {
            string userEmail = "user1@mail.com";
            User? userFound = await _repository.GetUserByEmail(userEmail);
            if (userFound is null)
            {
                return Unauthorized(new { Message = "The user with the email provided does not exist" });
            }

            BookingDto? bookingFound = await _repository.GetBookingById(bookingId, userEmail);
            return bookingFound is null
                ? NotFound(new { Message = "The booking with the id provided does not exist" })
                : Ok(bookingFound);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] BookingInsertDto bookingInsert)
        {
            string userEmail = "user1@mail.com";
            User? userFound = await _repository.GetUserByEmail(userEmail);
            if (userFound is null)
            {
                return Unauthorized(new { Message = "The user with the email provided does not exist" });
            }

            var validationResult = await _validator.ValidateAsync(bookingInsert);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors[0].ErrorMessage });
            }

            Room? roomFound = await _repository.GetRoomById(bookingInsert.RoomId);
            if (roomFound is null)
            {
                return NotFound(new { Message = "The room with the id provided does not exist" });
            }

            bool hasEnoughRoom = roomFound.Capacity >= bookingInsert.GuestQuantity;
            if (!hasEnoughRoom)
            {
                return BadRequest(new { Message = "The number of guests exceeds the maximum capacity" });
            }

            BookingDto createdBooking = await _repository.AddBooking(bookingInsert, userFound, roomFound);
            return Created($"/api/booking/{createdBooking.BookingId}", createdBooking);
        }
    }
}

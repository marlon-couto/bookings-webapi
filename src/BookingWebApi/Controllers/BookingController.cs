using AutoMapper;

using BookingWebApi.Dtos;
using BookingWebApi.Models;
using BookingWebApi.Repositories;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBookingWebApiContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<BookingInsertDto> _validator;
        public BookingController(IBookingWebApiContext context, IMapper mapper, IValidator<BookingInsertDto> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetAsync(string bookingId)
        {
            string userEmail = "user1@mail.com";
            var userFound = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (userFound is null)
            {
                return Unauthorized(new { Message = "The user with the email provided does not exist" });
            }

            var bookingFound = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .Include(b => b.Room!.Hotel)
                .Include(b => b.Room!.Hotel!.City)
                .Where(b => b.User!.Email == userEmail && b.BookingId == bookingId)
                .Select(b => _mapper.Map<BookingDto>(b))
                .FirstOrDefaultAsync();

            return bookingFound is null
                ? NotFound(new { Message = "The booking with the id provided does not exist" })
                : Ok(bookingFound);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] BookingInsertDto bookingInsert)
        {
            string userEmail = "user1@mail.com";
            var userFound = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (userFound is null)
            {
                return Unauthorized(new { Message = "The user with the email provided does not exist" });
            }

            var validationResult = await _validator.ValidateAsync(bookingInsert);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors[0].ErrorMessage });
            }

            var roomFound = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == bookingInsert.RoomId);
            if (roomFound is null)
            {
                return NotFound(new { Message = "The room with the id provided does not exist" });
            }

            bool hasEnoughRoom = roomFound.Capacity >= bookingInsert.GuestQuantity;
            if (!hasEnoughRoom)
            {
                return BadRequest(new { Message = "The number of guests exceeds the maximum capacity" });
            }

            var newBooking = _mapper.Map<Booking>(bookingInsert);
            newBooking.BookingId = Guid.NewGuid().ToString();
            newBooking.UserId = userFound.UserId;
            await _context.Bookings.AddAsync(newBooking);
            _context.SaveChanges();

            newBooking.User = userFound;
            newBooking.Room = await _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.Hotel!.City)
                .FirstOrDefaultAsync();

            var createdBooking = _mapper.Map<BookingDto>(newBooking);
            return Created($"/api/booking/{createdBooking.BookingId}", createdBooking);
        }
    }
}

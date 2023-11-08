using AutoMapper;

using BookingWebApi.Repositories;
using BookingWebApi.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingWebApi.Controllers
{
    [ApiController]
    [Route("booking")]
    public class BookingController : Controller
    {
        private readonly IBookingWebApiContext _context;
        private readonly IMapper _mapper;
        public BookingController(IBookingWebApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult FindAllBookings()
        {
            var allBookings = _context.Bookings
                .Include(booking => booking.User)
                .Include(booking => booking.Room)
                .Include(booking => booking.Room!.Hotel)
                .Include(booking => booking.Room!.Hotel!.City)
                .Select(booking => _mapper.Map<BookingDto>(booking))
                .ToList();

            return Ok(allBookings);
        }

        [HttpPost]
        public IActionResult AddBooking([FromBody] BookingInsertDto bookingInput)
        {
            return Ok();
        }
    }
}

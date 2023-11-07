using AutoMapper;

using BookingWebApi.Data;
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
            var bookings = _context.Bookings
                .Include(booking => booking.User)
                .Include(booking => booking.Room);

            var mappedBookings = bookings
                .Select(booking => _mapper.Map<BookingDto>(booking));

            return Ok(mappedBookings);
        }
    }
}

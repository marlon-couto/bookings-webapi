using AutoMapper;

using BookingWebApi.Repositories;
using BookingWebApi.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingWebApi.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IBookingWebApiContext _context;
        private readonly IMapper _mapper;
        public UserController(IBookingWebApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult FindAllUsersBookings()
        {
            var allUsers = _context.Users
                .Include(user => user.Bookings)
                .Select(user => _mapper.Map<UserDto>(user))
                .ToList();

            return Ok(allUsers);
        }
    }
}

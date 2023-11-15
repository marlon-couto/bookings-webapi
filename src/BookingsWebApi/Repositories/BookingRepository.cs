using AutoMapper;

using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IBookingsDbContext _context;
        private readonly IMapper _mapper;
        public BookingRepository(IBookingsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookingDto?> GetBookingById(string bookingId, string userEmail)
        {
            return await _context.Bookings
                            .Include(b => b.User)
                            .Include(b => b.Room)
                            .Include(b => b.Room!.Hotel)
                            .Include(b => b.Room!.Hotel!.City)
                            .Where(b => b.User!.Email == userEmail && b.BookingId == bookingId)
                            .Select(b => _mapper.Map<BookingDto>(b))
                            .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByEmail(string userEmail)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        }

        public async Task<Room?> GetRoomById(string roomId)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
        }

        public async Task<BookingDto> AddBooking(BookingInsertDto bookingInsert, User userFound)
        {
            Booking newBooking = _mapper.Map<Booking>(bookingInsert);
            newBooking.BookingId = Guid.NewGuid().ToString();
            newBooking.UserId = userFound.UserId;

            await _context.Bookings.AddAsync(newBooking);
            _context.SaveChanges();

            newBooking.User = userFound;
            newBooking.Room = await _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.Hotel!.City)
                .FirstOrDefaultAsync();

            return _mapper.Map<BookingDto>(newBooking);
        }
    }
}

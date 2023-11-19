using AutoMapper;

using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly IBookingsDbContext _context;
    private readonly IMapper _mapper;
    public BookingRepository(IBookingsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BookingDto> AddBooking(BookingInsertDto inputData, User userFound, Room roomFound)
    {
        Booking newBooking = _mapper.Map<Booking>(inputData);
        newBooking.BookingId = Guid.NewGuid().ToString();
        newBooking.UserId = userFound.UserId;

        await _context.Bookings.AddAsync(newBooking);
        _context.SaveChanges();

        newBooking.User = userFound;
        newBooking.Room = roomFound;
        return _mapper.Map<BookingDto>(newBooking);
    }

    public async Task<BookingDto> GetBookingById(string id, string userEmail)
    {
        Booking? bookingFound = await _context.Bookings
                        .Include(b => b.User)
                        .Include(b => b.Room)
                        .Include(b => b.Room!.Hotel)
                        .Include(b => b.Room!.Hotel!.City)
                        .FirstOrDefaultAsync(b => b.User!.Email == userEmail && b.BookingId == id);

        return bookingFound is null
            ? throw new KeyNotFoundException("The booking with the id provided does not exist")
            : _mapper.Map<BookingDto>(bookingFound);
    }

    public async Task<Room> GetRoomById(string roomId)
    {
        return await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId)
            ?? throw new KeyNotFoundException("The room with the id provided does not exist");
    }

    public async Task<User> GetUserByEmail(string userEmail)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail)
            ?? throw new UnauthorizedAccessException("The user with the email provided does not exist");
    }
}

using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

public class BookingService : IBookingService
{
    private readonly IBookingsDbContext _context;

    public BookingService(IBookingsDbContext context)
    {
        _context = context;
    }

    public async Task<Booking> AddBooking(BookingInsertDto dto, User bookingUser, Room bookingRoom)
    {
        Booking bookingCreated =
            new()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = bookingUser.Id,
                CheckIn = DateTime.Parse(dto.CheckIn).ToUniversalTime(),
                CheckOut = DateTime.Parse(dto.CheckOut).ToUniversalTime(),
                RoomId = dto.RoomId,
                GuestQuantity = dto.GuestQuantity
            };

        await _context.Bookings.AddAsync(bookingCreated);
        await _context.SaveChangesAsync();

        bookingCreated.User = bookingUser;
        bookingCreated.Room = bookingRoom;
        return bookingCreated;
    }

    public async Task DeleteBooking(Booking booking)
    {
        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Booking>> GetBookings(string userEmail)
    {
        List<Booking> bookings = await _context
            .Bookings
            .AsNoTracking()
            .Where(b => b.User!.Email == userEmail)
            .Include(b => b.User)
            .Include(b => b.Room)
            .ThenInclude(r => r!.Hotel)
            .ThenInclude(h => h!.City)
            .ToListAsync();

        return bookings;
    }

    public async Task<Booking> GetBookingById(string id, string userEmail)
    {
        Booking? bookingFound = await _context
            .Bookings
            .Where(b => b.User!.Email == userEmail && b.Id == id)
            .Include(b => b.User)
            .Include(b => b.Room)
            .ThenInclude(r => r!.Hotel)
            .ThenInclude(h => h!.City)
            .FirstOrDefaultAsync();

        return bookingFound
               ?? throw new KeyNotFoundException("The booking with the id provided does not exist.");
    }

    public async Task<Room> GetRoomById(string roomId)
    {
        Room? roomFound = await _context
            .Rooms
            .Where(r => r.Id == roomId)
            .Include(r => r.Hotel)
            .ThenInclude(h => h!.City)
            .FirstOrDefaultAsync();

        return roomFound ?? throw new KeyNotFoundException("The room with the id provided does not exist.");
    }

    public async Task<User> GetUserByEmail(string userEmail)
    {
        User? userFound = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        return userFound ?? throw new UnauthorizedAccessException("The user with the email provided does not exist.");
    }

    public async Task<Booking> UpdateBooking(
        BookingInsertDto dto,
        Booking booking,
        Room bookingRoom
    )
    {
        booking.CheckIn = DateTime.Parse(dto.CheckIn).ToUniversalTime();
        booking.CheckOut = DateTime.Parse(dto.CheckOut).ToUniversalTime();
        booking.GuestQuantity = dto.GuestQuantity;
        booking.RoomId = dto.RoomId;
        await _context.SaveChangesAsync();

        booking.Room = bookingRoom;
        return booking;
    }
}
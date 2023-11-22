using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly IBookingsDbContext _context;

    public BookingRepository(IBookingsDbContext context)
    {
        _context = context;
    }

    public async Task<Booking> AddBooking(BookingInsertDto inputData, User loggedUser, Room bookingRoom)
    {
        Booking newBooking = new()
        {
            BookingId = Guid.NewGuid().ToString(),
            UserId = loggedUser.UserId,
            CheckIn = DateTime.Parse(inputData.CheckIn).ToUniversalTime(),
            CheckOut = DateTime.Parse(inputData.CheckOut).ToUniversalTime(),
            RoomId = inputData.RoomId,
            GuestQuantity = inputData.GuestQuantity
        };

        await _context.Bookings.AddAsync(newBooking);
        await _context.SaveChangesAsync();

        newBooking.User = loggedUser;
        newBooking.Room = bookingRoom;
        return newBooking;
    }

    public async Task DeleteBooking(Booking booking)
    {
        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Booking>> GetAllBookings(string userEmail)
    {
        return await _context.Bookings
            .Where(b => b.User!.Email == userEmail)
            .Include(b => b.User)
            .Include(b => b.Room)
            .Include(b => b.Room!.Hotel)
            .Include(b => b.Room!.Hotel!.City)
            .ToListAsync();
    }

    public async Task<Booking> GetBookingById(string id, string userEmail)
    {
        Booking? bookingFound = await _context.Bookings
            .Where(b => b.User!.Email == userEmail && b.BookingId == id)
            .Include(b => b.User)
            .Include(b => b.Room)
            .Include(b => b.Room!.Hotel)
            .Include(b => b.Room!.Hotel!.City)
            .FirstOrDefaultAsync();

        return bookingFound ?? throw new KeyNotFoundException("The booking with the id provided does not exist");
    }

    public async Task<Room> GetRoomById(string roomId)
    {
        return await _context.Rooms
                   .Where(r => r.RoomId == roomId)
                   .Include(r => r.Hotel)
                   .Include(r => r.Hotel!.City)
                   .FirstOrDefaultAsync()
               ?? throw new KeyNotFoundException("The room with the id provided does not exist");
    }

    public async Task<User> GetUserByEmail(string userEmail)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail)
               ?? throw new UnauthorizedAccessException("The user with the email provided does not exist");
    }

    public async Task<Booking> UpdateBooking(BookingInsertDto inputData, Booking booking, Room bookingRoom)
    {
        booking.CheckIn = DateTime.Parse(inputData.CheckIn).ToUniversalTime();
        booking.CheckOut = DateTime.Parse(inputData.CheckOut).ToUniversalTime();
        booking.GuestQuantity = inputData.GuestQuantity;
        booking.RoomId = inputData.RoomId;
        await _context.SaveChangesAsync();

        booking.Room = bookingRoom;
        return booking;
    }
}
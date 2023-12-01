using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

/// <summary>
///     Service for managing bookings in the application.
/// </summary>
public class BookingService
{
    private readonly IBookingsDbContext _context;

    public BookingService(IBookingsDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Adds a new booking to the database.
    /// </summary>
    /// <param name="dto">The data to create a new booking.</param>
    /// <param name="bookingUser">The user associated with the booking.</param>
    /// <param name="bookingRoom">The room associated with the booking.</param>
    /// <returns>A <see cref="Booking" /> representing the newly created booking.</returns>
    public async Task<Booking> AddBooking(BookingInsertDto dto, User bookingUser, Room bookingRoom)
    {
        Booking booking =
            new()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = bookingUser.Id,
                CheckIn = DateTime.Parse(dto.CheckIn).ToUniversalTime(),
                CheckOut = DateTime.Parse(dto.CheckOut).ToUniversalTime(),
                RoomId = dto.RoomId,
                GuestQuantity = dto.GuestQuantity
            };

        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();

        booking.User = bookingUser;
        booking.Room = bookingRoom;
        return booking;
    }

    /// <summary>
    ///     Deletes a booking with the given ID from the database.
    /// </summary>
    /// <param name="booking">The entity that will be removed from the database.</param>
    public async Task DeleteBooking(Booking booking)
    {
        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    ///     Retrieves all bookings for the logged user from the database.
    /// </summary>
    /// <param name="userEmail">The email from the user associated with the bookings.</param>
    /// <returns>A list of <see cref="Booking" /> representing the bookings found. </returns>
    public async Task<List<Booking>> GetBookings(string userEmail)
    {
        return await _context
            .Bookings
            .AsNoTracking()
            .Where(b => b.User!.Email == userEmail)
            .Include(b => b.User)
            .Include(b => b.Room)
            .ThenInclude(r => r!.Hotel)
            .ThenInclude(h => h!.City)
            .ToListAsync();
    }

    /// <summary>
    ///     Retrieves a booking with the given ID from the database.
    /// </summary>
    /// <param name="id">The booking ID to search the database.</param>
    /// <param name="userEmail">The email from the user associated with the booking.</param>
    /// <returns>A <see cref="Booking" /> representing the booking found. </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a booking with the given ID and email does not exist.
    /// </exception>
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

    /// <summary>
    ///     Retrieves a room with the given ID from the database.
    /// </summary>
    /// <param name="roomId">The room ID to search the database.</param>
    /// <returns>The <see cref="Room" /> found.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a room with the given ID does
    ///     not exist.
    /// </exception>
    public async Task<Room> GetRoomById(string roomId)
    {
        return await _context
                   .Rooms
                   .Where(r => r.Id == roomId)
                   .Include(r => r.Hotel)
                   .ThenInclude(h => h!.City)
                   .FirstOrDefaultAsync()
               ?? throw new KeyNotFoundException("The room with the id provided does not exist.");
    }

    /// <summary>
    ///     Retrieves a user with the given email from the database.
    /// </summary>
    /// <param name="userEmail">The email to search the database.</param>
    /// <returns>The <see cref="User" /> found.</returns>
    /// <exception cref="UnauthorizedAccessException">
    ///     Thrown if a user with the given email does not exist.
    /// </exception>
    public async Task<User> GetUserByEmail(string userEmail)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail)
               ?? throw new UnauthorizedAccessException(
                   "The user with the email provided does not exist."
               );
    }

    /// <summary>
    ///     Updates a booking in the database.
    /// </summary>
    /// <param name="dto">The data used to update the booking.</param>
    /// <param name="booking">The entity that will be updated in the database.</param>
    /// <param name="bookingRoom">The room associated with the booking.</param>
    /// <returns>A <see cref="Booking" /> representing the updated booking.</returns>
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
using System.Globalization;
using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Exceptions;
using BookingsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

public class BookingService : IBookingService
{
    private readonly IBookingsDbContext _ctx;

    public BookingService(IBookingsDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<BookingModel> AddBooking(BookingInsertDto dto, UserModel bookingUser, RoomModel bookingRoom)
    {
        if (
            !DateTime.TryParseExact(
                dto.CheckIn,
                "MM/dd/yyyy HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var checkInDate
            )
        )
        {
            throw new InvalidDateException($"Invalid date for CheckIn: {dto.CheckIn}");
        }

        if (
            !DateTime.TryParseExact(
                dto.CheckOut,
                "MM/dd/yyyy HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var checkOutDate
            )
        )
        {
            throw new InvalidDateException($"Invalid date for CheckOut: {dto.CheckOut}");
        }

        var bookingCreated = new BookingModel
        {
            Id = Guid.NewGuid().ToString(),
            UserId = bookingUser.Id,
            CheckIn = checkInDate.ToUniversalTime(),
            CheckOut = checkOutDate.ToUniversalTime(),
            RoomId = dto.RoomId ?? string.Empty,
            GuestQuantity = dto.GuestQuantity
        };
        await _ctx.Bookings.AddAsync(bookingCreated);
        await _ctx.SaveChangesAsync();
        bookingCreated.User = bookingUser;
        bookingCreated.Room = bookingRoom;
        return bookingCreated;
    }

    public async Task DeleteBooking(BookingModel booking)
    {
        _ctx.Bookings.Remove(booking);
        await _ctx.SaveChangesAsync();
    }

    public async Task<List<BookingModel>> GetBookings(string userEmail)
    {
        var bookings = await _ctx
            .Bookings.AsNoTracking()
            .Where(b => b.User!.Email == userEmail)
            .Include(b => b.User)
            .Include(b => b.Room)
            .ThenInclude(r => r!.Hotel)
            .ThenInclude(h => h!.City)
            .ToListAsync();
        return bookings;
    }

    public async Task<BookingModel?> GetBookingById(string id, string userEmail)
    {
        var bookingFound = await _ctx
            .Bookings.Where(b => b.User!.Email == userEmail && b.Id == id)
            .Include(b => b.User)
            .Include(b => b.Room)
            .ThenInclude(r => r!.Hotel)
            .ThenInclude(h => h!.City)
            .FirstOrDefaultAsync();
        return bookingFound ?? null;
    }

    public async Task<RoomModel?> GetRoomById(string? roomId)
    {
        var roomFound = await _ctx
            .Rooms.Where(r => r.Id == roomId)
            .Include(r => r.Hotel)
            .ThenInclude(h => h!.City)
            .FirstOrDefaultAsync();
        return roomFound ?? null;
    }

    public async Task<UserModel?> GetUserByEmail(string userEmail)
    {
        return await _ctx.Users.FirstOrDefaultAsync(u => u.Email == userEmail)
               ?? null;
    }

    public async Task<BookingModel> UpdateBooking(
        BookingInsertDto dto,
        BookingModel booking,
        RoomModel bookingRoom
    )
    {
        if (
            !DateTime.TryParseExact(
                dto.CheckIn,
                "MM/dd/yyyy HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var checkInDate
            )
        )
        {
            throw new InvalidDateException($"Invalid date for CheckIn: {dto.CheckIn}");
        }

        if (
            !DateTime.TryParseExact(
                dto.CheckOut,
                "MM/dd/yyyy HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var checkOutDate
            )
        )
        {
            throw new InvalidDateException($"Invalid date for CheckOut: {dto.CheckOut}");
        }

        booking.CheckIn = checkInDate.ToUniversalTime();
        booking.CheckOut = checkOutDate.ToUniversalTime();
        booking.GuestQuantity = dto.GuestQuantity;
        booking.RoomId = dto.RoomId ?? string.Empty;
        await _ctx.SaveChangesAsync();
        booking.Room = bookingRoom;
        return booking;
    }
}
using System.Globalization;
using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Exceptions;
using BookingsWebApi.Models;
using BookingsWebApi.Services.Interfaces;
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
                "dd/MM/yyyy HH:mm:ss",
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
                "dd/MM/yyyy HH:mm:ss",
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
            Id = Guid.NewGuid(),
            UserId = bookingUser.Id,
            CheckIn = DateTime.SpecifyKind(checkInDate, DateTimeKind.Utc),
            CheckOut = DateTime.SpecifyKind(checkOutDate, DateTimeKind.Utc),
            RoomId = dto.RoomId ?? Guid.Empty,
            GuestQuantity = (int)dto.GuestQuantity!
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

    public async Task<List<BookingModel>> GetBookings(string userEmail, bool isAdmin = false)
    {
        return isAdmin
            ? await _ctx.Bookings.AsNoTracking()
                .Include(b => b.User)
                .Include(b => b.Room)
                .ThenInclude(r => r!.Hotel)
                .ThenInclude(r => r!.City)
                .ToListAsync()
            : await _ctx.Bookings.AsNoTracking()
                .Where(b => b.User!.Email == userEmail)
                .Include(b => b.User)
                .Include(b => b.Room)
                .ThenInclude(r => r!.Hotel)
                .ThenInclude(h => h!.City)
                .ToListAsync();
    }

    public async Task<BookingModel?> GetBookingById(Guid id, string userEmail)
    {
        return await _ctx.Bookings.AsNoTracking()
            .Where(b => b.User!.Email == userEmail && b.Id == id)
            .Include(b => b.User)
            .Include(b => b.Room)
            .ThenInclude(r => r!.Hotel)
            .ThenInclude(h => h!.City)
            .FirstOrDefaultAsync();
    }

    public async Task<RoomModel?> GetRoomById(Guid? roomId)
    {
        return await _ctx.Rooms.AsNoTracking()
            .Where(r => r.Id == roomId)
            .Include(r => r.Hotel)
            .ThenInclude(h => h!.City)
            .FirstOrDefaultAsync();
    }

    public async Task<UserModel?> GetUserByEmail(string userEmail)
    {
        return await _ctx.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == userEmail);
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
                "dd/MM/yyyy HH:mm:ss",
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
                "dd/MM/yyyy HH:mm:ss",
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
        booking.GuestQuantity = (int)dto.GuestQuantity!;
        booking.RoomId = dto.RoomId ?? Guid.Empty;
        await _ctx.SaveChangesAsync();
        booking.Room = bookingRoom;
        return booking;
    }
}
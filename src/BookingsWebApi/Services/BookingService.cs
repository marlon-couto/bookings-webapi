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

    public async Task<BookingModel> AddBooking(
        BookingInsertDto dto,
        UserModel bookingUser,
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

        var bookingCreated = new BookingModel
        {
            Id = Guid.NewGuid(),
            UserId = bookingUser.Id,
            CheckIn = checkInDate.ToUniversalTime(),
            CheckOut = checkOutDate.ToUniversalTime(),
            RoomId = dto.RoomId ?? Guid.Empty,
            GuestQuantity = (int)dto.GuestQuantity!,
            CreatedAt = DateTime.Now.ToUniversalTime()
        };
        await _ctx.Bookings.AddAsync(bookingCreated);
        await _ctx.SaveChangesAsync();
        bookingCreated.User = bookingUser;
        bookingCreated.Room = bookingRoom;
        return bookingCreated;
    }

    public async Task DeleteBooking(BookingModel booking)
    {
        booking.IsDeleted = true;
        booking.UpdatedAt = DateTime.Now.ToUniversalTime();
        await _ctx.SaveChangesAsync();
    }

    public async Task<List<BookingModel>> GetBookings(string userEmail, bool isAdmin = false)
    {
        return isAdmin
            ? await _ctx
                .Bookings.AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Include(x => x.User)
                .Include(x => x.Room)
                .ThenInclude(y => y!.Hotel)
                .ThenInclude(z => z!.City)
                .ToListAsync()
            : await _ctx
                .Bookings.AsNoTracking()
                .Where(x => x.User!.Email == userEmail && !x.IsDeleted)
                .Include(x => x.User)
                .Include(x => x.Room)
                .ThenInclude(y => y!.Hotel)
                .ThenInclude(z => z!.City)
                .ToListAsync();
    }

    public async Task<BookingModel?> GetBookingById(Guid id, string userEmail, bool isAdmin = false)
    {
        return isAdmin
            ? await _ctx
                .Bookings.Where(x => !x.IsDeleted)
                .Include(x => x.User)
                .Include(x => x.Room)
                .ThenInclude(y => y!.Hotel)
                .ThenInclude(z => z!.City)
                .FirstOrDefaultAsync()
            : await _ctx
                .Bookings.Where(x => x.User!.Email == userEmail && x.Id == id && !x.IsDeleted)
                .Include(x => x.User)
                .Include(x => x.Room)
                .ThenInclude(y => y!.Hotel)
                .ThenInclude(z => z!.City)
                .FirstOrDefaultAsync();
    }

    public async Task<RoomModel?> GetRoomById(Guid? roomId)
    {
        return await _ctx
            .Rooms.AsNoTracking()
            .Where(x => x.Id == roomId && !x.IsDeleted)
            .Include(x => x.Hotel)
            .ThenInclude(y => y!.City)
            .FirstOrDefaultAsync();
    }

    public async Task<UserModel?> GetUserByEmail(string userEmail)
    {
        return await _ctx
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == userEmail && !x.IsDeleted);
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
        booking.UpdatedAt = DateTime.Now.ToUniversalTime();
        await _ctx.SaveChangesAsync();
        booking.Room = bookingRoom;
        return booking;
    }
}

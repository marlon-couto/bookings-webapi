using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Bogus;

using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
using BookingsWebApi.Services;
using BookingsWebApi.Test.Helpers;
using BookingsWebApi.Test.Helpers.Builders;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace BookingsWebApi.Test.Unit.Services;

public class BookingServiceTest : IClassFixture<TestFixture>, IDisposable
{
    private readonly TestDbContext _context;
    private readonly Faker _faker = new();
    private readonly BookingService _service;

    public BookingServiceTest(TestFixture fixture)
    {
        _context = fixture.Context;
        _service = new BookingService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }

    [Fact(DisplayName = "AddBooking should add booking")]
    public async Task AddBooking_ShouldAddBooking()
    {
        User bookingUser = UserBuilder.New().Build();
        Room bookingRoom = RoomBuilder.New().Build();
        BookingInsertDto dto =
            new()
            {
                CheckIn = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                CheckOut = DateTime.UtcNow.AddDays(1).ToString(CultureInfo.InvariantCulture),
                GuestQuantity = _faker.Random.Int(),
                RoomId = bookingRoom.Id
            };
        Booking bookingCreated = await _service.AddBooking(dto, bookingUser, bookingRoom);

        bookingCreated.Should().NotBeNull();
    }

    [Fact(DisplayName = "DeleteBooking should delete booking")]
    public async Task DeleteBooking_ShouldDeleteBooking()
    {
        Booking booking = BookingBuilder.New().Build();
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();

        await _service.DeleteBooking(booking);

        List<Booking> bookings = await _context.Bookings.AsNoTracking().ToListAsync();
        bookings.Count.Should().Be(0);
    }

    [Fact(DisplayName = "GetBookings should return all bookings")]
    public async Task GetBookings_ShouldReturnAllBookings()
    {
        User user = UserBuilder.New().Build();
        Booking booking1 = BookingBuilder.New().WithUser(user).Build();
        Booking booking2 = BookingBuilder.New().WithUser(user).Build();
        await _context.Bookings.AddAsync(booking1);
        await _context.Bookings.AddAsync(booking2);
        await _context.SaveChangesAsync();

        List<Booking> bookings = await _service.GetBookings(user.Email);

        bookings.Count.Should().Be(2);
    }

    [Fact(DisplayName = "GetBookingById should return booking found")]
    public async Task GetBookingById_ShouldReturnBookingFound()
    {
        Booking booking = BookingBuilder.New().Build();
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();

        Booking bookingFound = await _service.GetBookingById(booking.Id, booking.User!.Email);

        bookingFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetBookingById throw KeyNotFoundException if booking not exists")]
    public async Task GetBookingById_ThrowKeyNotFoundException_IfBookingNotExists()
    {
        Func<Task> act = async () =>
            await _service.GetBookingById(_faker.Random.Guid().ToString(), _faker.Internet.Email());

        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("The booking with the id provided does not exist.");
    }

    [Fact(DisplayName = "GetRoomById should return room found")]
    public async Task GetRoomById_ShouldReturnRoomFound()
    {
        Room room = RoomBuilder.New().Build();
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();

        Room roomFound = await _service.GetRoomById(room.Id);

        roomFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetRoomById throw KeyNotFoundException if room not exists")]
    public async Task GetRoomById_ThrowKeyNotFoundException_IfRoomNotExists()
    {
        Func<Task> act = async () => await _service.GetRoomById(_faker.Random.Guid().ToString());

        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("The room with the id provided does not exist.");
    }

    [Fact(DisplayName = "GetUserByEmail should return user found")]
    public async Task GetUserByEmail_ShouldReturnUserFound()
    {
        User user = UserBuilder.New().Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        User userFound = await _service.GetUserByEmail(user.Email);

        userFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetUserByEmail throw UnauthorizedAccessException if email not exists")]
    public async Task GetUserByEmail_ThrowUnauthorizedAccessException_IfEmailNotExists()
    {
        Func<Task<User>> act = async () => await _service.GetUserByEmail(_faker.Internet.Email());

        await act.Should()
            .ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("The user with the email provided does not exist.");
    }

    [Fact(DisplayName = "UpdateBooking should update booking")]
    public async Task UpdateBooking_ShouldUpdateBooking()
    {
        Booking booking = BookingBuilder.New().Build();
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();

        BookingInsertDto dto =
            new()
            {
                CheckIn = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                CheckOut = DateTime.UtcNow.AddDays(1).ToString(CultureInfo.InvariantCulture),
                GuestQuantity = _faker.Random.Int(),
                RoomId = booking.RoomId
            };
        Booking bookingUpdated = await _service.UpdateBooking(dto, booking, booking.Room!);

        bookingUpdated.Should().NotBeNull();
    }
}
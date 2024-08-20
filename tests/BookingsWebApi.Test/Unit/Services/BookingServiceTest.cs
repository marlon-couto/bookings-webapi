using System;
using System.Threading.Tasks;
using Bogus;
using BookingsWebApi.DTOs;
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
        var bookingUser = UserBuilder.New().Build();
        await _context.Users.AddAsync(bookingUser);
        var bookingRoom = RoomBuilder.New().Build();
        await _context.Rooms.AddAsync(bookingRoom);
        await _context.SaveChangesAsync();
        var dto = new BookingInsertDto
        {
            CheckIn = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
            CheckOut = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss"),
            GuestQuantity = _faker.Random.Int(),
            RoomId = bookingRoom.Id
        };
        var bookingCreated = await _service.AddBooking(dto, bookingUser, bookingRoom);
        bookingCreated.Should().NotBeNull();
    }

    [Fact(DisplayName = "DeleteBooking should delete booking")]
    public async Task DeleteBooking_ShouldDeleteBooking()
    {
        var bookingUser = UserBuilder.New().Build();
        var bookingRoom = RoomBuilder.New().Build();
        var booking = BookingBuilder.New().WithUser(bookingUser).WithRoomId(bookingRoom.Id).Build();
        await _context.Users.AddAsync(bookingUser);
        await _context.Rooms.AddAsync(bookingRoom);
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
        var bookingsBefore = await _context.Bookings.AsNoTracking().ToListAsync();
        await _service.DeleteBooking(booking);
        var bookingsAfter = await _context.Bookings.AsNoTracking().ToListAsync();
        bookingsAfter.Count.Should().NotBe(bookingsBefore.Count);
    }

    [Fact(DisplayName = "GetBookings should return all bookings")]
    public async Task GetBookings_ShouldReturnAllBookings()
    {
        var user = UserBuilder.New().Build();
        var booking1 = BookingBuilder.New().WithUser(user).Build();
        var booking2 = BookingBuilder.New().WithUser(user).Build();
        await _context.Bookings.AddAsync(booking1);
        await _context.Bookings.AddAsync(booking2);
        await _context.SaveChangesAsync();
        var bookings = await _service.GetBookings(user.Email);
        bookings.Count.Should().Be(2);
    }

    [Fact(DisplayName = "GetBookingById should return booking found")]
    public async Task GetBookingById_ShouldReturnBookingFound()
    {
        var bookingUser = UserBuilder.New().Build();
        var bookingRoom = RoomBuilder.New().Build();
        var booking = BookingBuilder.New().WithUser(bookingUser).WithRoomId(bookingRoom.Id).Build();
        await _context.Users.AddAsync(bookingUser);
        await _context.Rooms.AddAsync(bookingRoom);
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
        var bookingFound = await _service.GetBookingById(booking.Id, booking.User!.Email);
        bookingFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetBookingById return null if booking not exists")]
    public async Task GetBookingById_ReturnNull_IfBookingNotExists()
    {
        var bookingFound = await _service.GetBookingById(
            _faker.Random.Guid(),
            _faker.Internet.Email()
        );
        bookingFound.Should().BeNull();
    }

    [Fact(DisplayName = "GetRoomById should return room found")]
    public async Task GetRoomById_ShouldReturnRoomFound()
    {
        var roomHotel = HotelBuilder.New().Build();
        var room = RoomBuilder.New().WithHotelId(roomHotel.Id).Build();
        await _context.Hotels.AddAsync(roomHotel);
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
        var roomFound = await _service.GetRoomById(room.Id);
        roomFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetRoomById return null if room not exists")]
    public async Task GetRoomById_ReturnNull_IfRoomNotExists()
    {
        var roomFound = await _service.GetRoomById(_faker.Random.Guid());
        roomFound.Should().BeNull();
    }

    [Fact(DisplayName = "GetUserByEmail should return user found")]
    public async Task GetUserByEmail_ShouldReturnUserFound()
    {
        var user = UserBuilder.New().Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        var userFound = await _service.GetUserByEmail(user.Email);
        userFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetUserByEmail return null if email not exists")]
    public async Task GetUserByEmail_ReturnNull_IfEmailNotExists()
    {
        var userFound = await _service.GetUserByEmail(_faker.Internet.Email());
        userFound.Should().BeNull();
    }

    [Fact(DisplayName = "UpdateBooking should update booking")]
    public async Task UpdateBooking_ShouldUpdateBooking()
    {
        var bookingUser = UserBuilder.New().Build();
        var bookingRoom = RoomBuilder.New().Build();
        var booking = BookingBuilder.New().WithRoomId(bookingRoom.Id).WithUser(bookingUser).Build();
        await _context.Users.AddAsync(bookingUser);
        await _context.Rooms.AddAsync(bookingRoom);
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
        var dto = new BookingInsertDto
        {
            CheckIn = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
            CheckOut = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss"),
            GuestQuantity = _faker.Random.Int(),
            RoomId = booking.RoomId
        };
        var bookingUpdated = await _service.UpdateBooking(dto, booking, booking.Room!);
        bookingUpdated.Should().NotBeNull();
    }
}
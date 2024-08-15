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

public class RoomServiceTest : IClassFixture<TestFixture>, IDisposable
{
    private readonly TestDbContext _context;
    private readonly Faker _faker = new();
    private readonly RoomService _service;

    public RoomServiceTest(TestFixture fixture)
    {
        _context = fixture.Context;
        _service = new RoomService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }

    [Fact(DisplayName = "AddRoom should add room")]
    public async Task AddRoom_ShouldAddRoom()
    {
        var hotelRoom = HotelBuilder.New().Build();
        var dto = new RoomInsertDto
        {
            Name = _faker.Lorem.Sentence(),
            HotelId = hotelRoom.Id,
            Capacity = _faker.Random.Int(),
            Image = _faker.Image.PicsumUrl()
        };
        var roomCreated = await _service.AddRoom(dto, hotelRoom);
        roomCreated.Should().NotBeNull();
    }

    [Fact(DisplayName = "DeleteRoom should delete room")]
    public async Task DeleteRoom_ShouldDeleteRoom()
    {
        var room = RoomBuilder.New().Build();
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
        await _service.DeleteRoom(room);
        var rooms = await _context.Rooms.AsNoTracking().ToListAsync();
        rooms.Count.Should().Be(0);
    }

    [Fact(DisplayName = "GetRooms should return all rooms")]
    public async Task GetRooms_ShouldReturnAllUsers()
    {
        var room1 = RoomBuilder.New().Build();
        var room2 = RoomBuilder.New().Build();
        await _context.Rooms.AddAsync(room1);
        await _context.Rooms.AddAsync(room2);
        await _context.SaveChangesAsync();
        var rooms = await _service.GetRooms();
        rooms.Count.Should().Be(2);
    }

    [Fact(DisplayName = "GetHotelById should return hotel found")]
    public async Task GetHotelById_ShouldReturnHotelFound()
    {
        var hotel = HotelBuilder.New().Build();
        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();
        var hotelFound = await _service.GetHotelById(hotel.Id);
        hotelFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetHotelById return null if hotel not exists")]
    public async Task GetHotelById_ReturnNull_IfHotelNotExists()
    {
        var hotelFound = await _service.GetHotelById(_faker.Random.Guid());
        hotelFound.Should().BeNull();
    }

    [Fact(DisplayName = "GetRoomById should return room found")]
    public async Task GetRoomById_ShouldReturnRoomFound()
    {
        var room = RoomBuilder.New().Build();
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
        var roomFound = await _service.GetRoomById(room.Id);
        roomFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetRoomById return null if room not exists")]
    public async Task GetRoomById_ReturnNull_IfRoomNotExists()
    {
        var hotelFound = await _service.GetRoomById(_faker.Random.Guid());
        hotelFound.Should().BeNull();
    }

    [Fact(DisplayName = "UpdateRoom should update room")]
    public async Task UpdateRoom_ShouldUpdateRoom()
    {
        var room = RoomBuilder.New().Build();
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
        var dto = new RoomInsertDto
        {
            Name = _faker.Lorem.Sentence(),
            Capacity = _faker.Random.Int(),
            HotelId = room.HotelId,
            Image = _faker.Image.PicsumUrl()
        };
        var roomUpdated = await _service.UpdateRoom(dto, room, room.Hotel!);
        roomUpdated.Should().NotBeNull();
    }
}
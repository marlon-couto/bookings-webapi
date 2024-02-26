using System;
using System.Collections.Generic;
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
        Hotel hotelRoom = HotelBuilder.New().Build();
        RoomInsertDto dto =
            new()
            {
                Name = _faker.Lorem.Sentence(),
                HotelId = hotelRoom.Id,
                Capacity = _faker.Random.Int(),
                Image = _faker.Image.PicsumUrl()
            };
        Room roomCreated = await _service.AddRoom(dto, hotelRoom);

        roomCreated.Should().NotBeNull();
    }

    [Fact(DisplayName = "DeleteRoom should delete room")]
    public async Task DeleteRoom_ShouldDeleteRoom()
    {
        Room room = RoomBuilder.New().Build();
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();

        await _service.DeleteRoom(room);

        List<Room> rooms = await _context.Rooms.AsNoTracking().ToListAsync();
        rooms.Count.Should().Be(0);
    }

    [Fact(DisplayName = "GetRooms should return all rooms")]
    public async Task GetRooms_ShouldReturnAllUsers()
    {
        Room room1 = RoomBuilder.New().Build();
        Room room2 = RoomBuilder.New().Build();
        await _context.Rooms.AddAsync(room1);
        await _context.Rooms.AddAsync(room2);
        await _context.SaveChangesAsync();

        List<Room> rooms = await _service.GetRooms();

        rooms.Count.Should().Be(2);
    }

    [Fact(DisplayName = "GetHotelById should return hotel found")]
    public async Task GetHotelById_ShouldReturnHotelFound()
    {
        Hotel hotel = HotelBuilder.New().Build();
        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();

        Hotel hotelFound = await _service.GetHotelById(hotel.Id);

        hotelFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetHotelById throw KeyNotFoundException if hotel not exists")]
    public async Task GetHotelById_ThrowKeyNotFoundException_IfHotelNotExists()
    {
        Func<Task> act = async () => await _service.GetHotelById(_faker.Random.Guid().ToString());

        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("The hotel with the provided id does not exist.");
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
            .WithMessage("The room with the provided id does not exist.");
    }

    [Fact(DisplayName = "UpdateRoom should update room")]
    public async Task UpdateRoom_ShouldUpdateRoom()
    {
        Room room = RoomBuilder.New().Build();
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();

        RoomInsertDto dto =
            new()
            {
                Name = _faker.Lorem.Sentence(),
                Capacity = _faker.Random.Int(),
                HotelId = room.HotelId,
                Image = _faker.Image.PicsumUrl()
            };
        Room roomUpdated = await _service.UpdateRoom(dto, room, room.Hotel!);

        roomUpdated.Should().NotBeNull();
    }
}

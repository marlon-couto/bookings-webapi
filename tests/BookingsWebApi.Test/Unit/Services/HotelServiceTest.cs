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

public class HotelServiceTest : IClassFixture<TestFixture>, IDisposable
{
    private readonly TestDbContext _context;
    private readonly Faker _faker = new();
    private readonly HotelService _service;

    public HotelServiceTest(TestFixture fixture)
    {
        _context = fixture.Context;
        _service = new HotelService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }

    [Fact(DisplayName = "AddHotel should add hotel")]
    public async Task AddHotel_ShouldAddHotel()
    {
        City hotelCity = CityBuilder.New().Build();
        HotelInsertDto dto =
            new()
            {
                Name = _faker.Lorem.Sentence(), Address = _faker.Address.FullAddress(), CityId = hotelCity.Id
            };

        Hotel hotelCreated = await _service.AddHotel(dto, hotelCity);

        hotelCreated.Should().NotBeNull();
    }

    [Fact(DisplayName = "DeleteHotel should delete hotel")]
    public async Task DeleteHotel_ShouldDeleteHotel()
    {
        Hotel hotel = HotelBuilder.New().Build();
        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();

        await _service.DeleteHotel(hotel);

        List<Hotel> hotels = await _context.Hotels.AsNoTracking().ToListAsync();
        hotels.Count.Should().Be(0);
    }

    [Fact(DisplayName = "GetHotels should return all hotels")]
    public async Task GetHotels_ShouldReturnAllHotels()
    {
        Hotel hotel1 = HotelBuilder.New().Build();
        Hotel hotel2 = HotelBuilder.New().Build();
        await _context.Hotels.AddAsync(hotel1);
        await _context.Hotels.AddAsync(hotel2);
        await _context.SaveChangesAsync();

        List<Hotel> hotels = await _service.GetHotels();

        hotels.Count.Should().Be(2);
    }

    [Fact(DisplayName = "GetCityById should return city found")]
    public async Task GetCityById_ShouldReturnCityFound()
    {
        City city = CityBuilder.New().Build();
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();

        City cityFound = await _service.GetCityById(city.Id);

        cityFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetCityById throw KeyNotFoundException if city not exists")]
    public async Task GetCityById_ThrowKeyNotFoundException_IfCityNotExists()
    {
        Func<Task> act = async () => await _service.GetCityById(_faker.Random.Guid().ToString());

        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("The city with the id provided does not exist.");
    }

    [Fact(DisplayName = "GetHotelId should return hotel found")]
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
            .WithMessage("The hotel with the id provided does not exist.");
    }

    [Fact(DisplayName = "GetHotelRooms should return all hotel rooms")]
    public async Task GetHotelRooms_ShouldReturnAllHotelRooms()
    {
        Hotel hotel = HotelBuilder.New().Build();
        Room room1 = RoomBuilder.New().WithHotel(hotel).Build();
        Room room2 = RoomBuilder.New().WithHotel(hotel).Build();
        await _context.Rooms.AddAsync(room1);
        await _context.Rooms.AddAsync(room2);
        await _context.SaveChangesAsync();

        List<Room> hotelRooms = await _service.GetHotelRooms(hotel.Id);

        hotelRooms.Count.Should().Be(2);
    }

    [Fact(DisplayName = "UpdateHotel should update hotel")]
    public async Task UpdateHotel_ShouldUpdateHotel()
    {
        Hotel hotel = HotelBuilder.New().Build();
        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();

        HotelInsertDto dto =
            new()
            {
                Address = _faker.Address.FullAddress(), CityId = hotel.CityId, Name = _faker.Lorem.Sentence()
            };
        Hotel hotelUpdated = await _service.UpdateHotel(dto, hotel, hotel.City!);

        hotelUpdated.Should().NotBeNull();
    }
}
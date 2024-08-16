using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
using BookingsWebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

public class HotelService : IHotelService
{
    private readonly IBookingsDbContext _ctx;

    public HotelService(IBookingsDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<HotelModel> AddHotel(HotelInsertDto dto, CityModel hotelCity)
    {
        var hotelCreated = new HotelModel
        {
            Id = Guid.NewGuid(),
            Name = dto.Name ?? string.Empty,
            CityId = dto.CityId ?? Guid.Empty,
            Address = dto.Address ?? string.Empty
        };
        await _ctx.Hotels.AddAsync(hotelCreated);
        await _ctx.SaveChangesAsync();
        hotelCreated.City = hotelCity;
        return hotelCreated;
    }

    public async Task DeleteHotel(HotelModel hotel)
    {
        _ctx.Hotels.Remove(hotel);
        await _ctx.SaveChangesAsync();
    }

    public async Task<List<HotelModel>> GetHotels()
    {
        return await _ctx.Hotels.AsNoTracking().Include(x => x.City).ToListAsync();
    }

    public async Task<CityModel?> GetCityById(Guid? id)
    {
        return await _ctx.Cities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id) ?? null;
    }

    public async Task<HotelModel?> GetHotelById(Guid id)
    {
        return await _ctx
                .Hotels.AsNoTracking()
                .Where(x => x.Id == id)
                .Include(x => x.City)
                .FirstOrDefaultAsync() ?? null;
    }

    public async Task<HotelModel> UpdateHotel(
        HotelInsertDto dto,
        HotelModel hotel,
        CityModel? hotelCity
    )
    {
        hotel.Name = dto.Name ?? string.Empty;
        hotel.Address = dto.Address ?? string.Empty;
        hotel.CityId = dto.CityId ?? Guid.Empty;
        await _ctx.SaveChangesAsync();
        hotel.City = hotelCity;
        return hotel;
    }

    public async Task<List<RoomModel>> GetHotelRooms(HotelModel hotel)
    {
        var roomsFound = await _ctx
            .Rooms.AsNoTracking()
            .Where(x => x.HotelId == hotel.Id)
            .ToListAsync();
        foreach (var room in roomsFound)
        {
            room.Hotel = hotel;
        }

        return roomsFound;
    }
}

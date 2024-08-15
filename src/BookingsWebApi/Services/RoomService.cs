using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

public class RoomService : IRoomService
{
    private readonly IBookingsDbContext _ctx;

    public RoomService(IBookingsDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<RoomModel> AddRoom(RoomInsertDto dto, HotelModel roomHotel)
    {
        var roomCreated = new RoomModel
        {
            Id = Guid.NewGuid(),
            Name = dto.Name ?? string.Empty,
            Image = dto.Image ?? string.Empty,
            HotelId = dto.HotelId ?? Guid.Empty,
            Capacity = dto.Capacity
        };
        await _ctx.Rooms.AddAsync(roomCreated);
        await _ctx.SaveChangesAsync();
        roomCreated.Hotel = roomHotel;
        return roomCreated;
    }

    public async Task DeleteRoom(RoomModel room)
    {
        _ctx.Rooms.Remove(room);
        await _ctx.SaveChangesAsync();
    }

    public async Task<List<RoomModel>> GetRooms()
    {
        return await _ctx.Rooms.AsNoTracking()
            .Include(r => r.Hotel)
            .ThenInclude(h => h!.City)
            .ToListAsync();
    }

    public async Task<HotelModel?> GetHotelById(Guid? hotelId)
    {
        return await _ctx.Hotels.AsNoTracking()
            .Where(h => h.Id == hotelId)
            .Include(h => h.City)
            .FirstOrDefaultAsync();
    }

    public async Task<RoomModel?> GetRoomById(Guid? id)
    {
        return await _ctx.Rooms.AsNoTracking()
            .Where(r => r.Id == id)
            .Include(r => r.Hotel)
            .ThenInclude(h => h!.City)
            .FirstOrDefaultAsync();
    }

    public async Task<RoomModel> UpdateRoom(RoomInsertDto dto, RoomModel room, HotelModel roomHotel)
    {
        room.Capacity = dto.Capacity;
        room.HotelId = dto.HotelId ?? Guid.Empty;
        room.Image = dto.Image ?? string.Empty;
        room.Name = dto.Name ?? string.Empty;
        await _ctx.SaveChangesAsync();
        room.Hotel = roomHotel;
        return room;
    }
}
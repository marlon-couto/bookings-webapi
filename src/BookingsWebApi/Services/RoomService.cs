using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Exceptions;
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
            Id = Guid.NewGuid().ToString(),
            Name = dto.Name ?? string.Empty,
            Image = dto.Image ?? string.Empty,
            HotelId = dto.HotelId ?? string.Empty,
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
        var rooms = await _ctx
            .Rooms.AsNoTracking()
            .Include(r => r.Hotel)
            .ThenInclude(h => h!.City)
            .ToListAsync();
        return rooms;
    }

    public async Task<HotelModel?> GetHotelById(string? hotelId)
    {
        var hotelFound = await _ctx
            .Hotels.Where(h => h.Id == hotelId)
            .Include(h => h.City)
            .FirstOrDefaultAsync();
        return hotelFound ?? null;
    }

    public async Task<RoomModel?> GetRoomById(string? id)
    {
        var roomFound = await _ctx
            .Rooms.Where(r => r.Id == id)
            .Include(r => r.Hotel)
            .ThenInclude(h => h!.City)
            .FirstOrDefaultAsync();
        return roomFound ?? null;
    }

    public async Task<RoomModel> UpdateRoom(RoomInsertDto dto, RoomModel room, HotelModel roomHotel)
    {
        room.Capacity = dto.Capacity;
        room.HotelId = dto.HotelId ?? string.Empty;
        room.Image = dto.Image ?? string.Empty;
        room.Name = dto.Name ?? string.Empty;
        await _ctx.SaveChangesAsync();
        room.Hotel = roomHotel;
        return room;
    }
}
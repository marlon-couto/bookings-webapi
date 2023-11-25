using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly IBookingsDbContext _context;

    public RoomRepository(IBookingsDbContext context)
    {
        _context = context;
    }

    public async Task<Room> AddRoom(RoomInsertDto dto, Hotel roomHotel)
    {
        Room room =
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Image = dto.Image,
                HotelId = dto.HotelId,
                Capacity = dto.Capacity
            };

        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();

        room.Hotel = roomHotel;
        return room;
    }

    public async Task DeleteRoom(Room room)
    {
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Room>> GetAllRooms()
    {
        return await _context.Rooms.Include(r => r.Hotel)
            .ThenInclude(h => h!.City).ToListAsync();
    }

    public async Task<Hotel> GetHotelById(string hotelId)
    {
        return await _context
                   .Hotels
                   .Where(h => h.Id == hotelId)
                   .Include(h => h.City)
                   .FirstOrDefaultAsync()
               ?? throw new KeyNotFoundException(
                   "The hotel with the provided id does not exist");
    }

    public async Task<Room> GetRoomById(string id)
    {
        return await _context
                   .Rooms
                   .Where(r => r.Id == id)
                   .Include(r => r.Hotel)
                   .ThenInclude(h => h!.City)
                   .FirstOrDefaultAsync()
               ?? throw new KeyNotFoundException(
                   "The room with the provided id does not exist");
    }

    public async Task<Room> UpdateRoom(RoomInsertDto dto, Room room,
        Hotel roomHotel)
    {
        room.Capacity = dto.Capacity;
        room.HotelId = dto.HotelId;
        room.Image = dto.Image;
        room.Name = dto.Name;
        await _context.SaveChangesAsync();

        room.Hotel = roomHotel;
        return room;
    }
}
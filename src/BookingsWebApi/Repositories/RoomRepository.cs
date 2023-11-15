using AutoMapper;

using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IBookingsDbContext _context;
        private readonly IMapper _mapper;
        public RoomRepository(IBookingsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RoomDto> AddRoom(RoomInsertDto inputData, Hotel hotelFound)
        {
            Room newRoom = _mapper.Map<Room>(inputData);
            newRoom.RoomId = Guid.NewGuid().ToString();

            await _context.Rooms.AddAsync(newRoom);
            _context.SaveChanges();

            newRoom.Hotel = hotelFound;
            return _mapper.Map<RoomDto>(newRoom);
        }

        public void DeleteRoom(Room roomFound)
        {
            _context.Rooms.Remove(roomFound);
            _context.SaveChanges();
        }

        public async Task<Room?> GetRoomById(string id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == id);
        }

        public async Task<Hotel?> GetHotelById(string hotelId)
        {
            return await _context.Hotels.FirstOrDefaultAsync(h => h.HotelId == hotelId);
        }

        public async Task<List<RoomDto>> GetHotelRooms(string hotelId)
        {
            List<Room> hotelRooms = await _context.Rooms.Where(r => r.HotelId == hotelId).ToListAsync();
            return hotelRooms.Select(h => _mapper.Map<RoomDto>(h)).ToList();
        }
    }
}

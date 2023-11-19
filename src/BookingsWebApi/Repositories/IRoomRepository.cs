using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories;

public interface IRoomRepository
{
    public Task<RoomDto> AddRoom(RoomInsertDto inputData, Hotel hotelFound);
    public void DeleteRoom(Room roomFound); // TODO: alterar essa implementação para assíncrona.
    public Task<Hotel> GetHotelById(string hotelId);
    public Task<List<RoomDto>> GetHotelRooms(string hotelId);
    public Task<Room> GetRoomById(string id);
}

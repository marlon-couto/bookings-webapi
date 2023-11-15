using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories
{
    public interface IBookingRepository
    {
        Task<BookingDto?> GetBookingById(string id, string userEmail);
        Task<User?> GetUserByEmail(string userEmail);
        Task<Room?> GetRoomById(string id);
        Task<BookingDto> AddBooking(BookingInsertDto inputData, User userFound, Room roomFound);
    }
}

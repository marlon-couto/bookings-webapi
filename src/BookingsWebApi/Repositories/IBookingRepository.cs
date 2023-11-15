using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories
{
    public interface IBookingRepository
    {
        Task<BookingDto?> GetBookingById(string bookingId, string userEmail);
        Task<User?> GetUserByEmail(string userEmail);
        Task<Room?> GetRoomById(string roomId);
        Task<BookingDto> AddBooking(BookingInsertDto bookingInsert, User userFound);
    }
}

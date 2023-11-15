using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories
{
    public interface IBookingRepository
    {
        public Task<BookingDto?> GetBookingById(string id, string userEmail);
        public Task<User?> GetUserByEmail(string userEmail);
        public Task<Room?> GetRoomById(string id);
        public Task<BookingDto> AddBooking(BookingInsertDto inputData, User userFound, Room roomFound);
    }
}

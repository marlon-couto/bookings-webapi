using BookingWebApi.Dtos;

namespace BookingWebApi.Repositories
{
    public interface IUserRepository
    {
        Task<List<UserDto>> GetAllUsers();
        Task<bool> EmailExists(string userEmail);
        Task<UserDto> AddUser(UserInsertDto userInsert);
    }
}

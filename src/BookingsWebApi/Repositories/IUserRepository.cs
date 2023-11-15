using BookingsWebApi.Dtos;

namespace BookingsWebApi.Repositories
{
    public interface IUserRepository
    {
        Task<List<UserDto>> GetAllUsers();
        Task<bool> EmailExists(string userEmail);
        Task<UserDto> AddUser(UserInsertDto inputData);
    }
}

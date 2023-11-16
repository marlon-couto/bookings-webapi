using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories
{
    public interface IUserRepository
    {
        public Task<UserDto> AddUser(UserInsertDto inputData);
        public Task EmailExists(string userEmail);
        public Task<List<UserDto>> GetAllUsers();
        public Task<User> GetUserByEmail(string userEmail);
    }
}

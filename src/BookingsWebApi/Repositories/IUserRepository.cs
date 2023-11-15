using BookingsWebApi.Dtos;

namespace BookingsWebApi.Repositories
{
    public interface IUserRepository
    {
        public Task<List<UserDto>> GetAllUsers();
        public Task<bool> EmailExists(string userEmail);
        public Task<UserDto> AddUser(UserInsertDto inputData);
    }
}

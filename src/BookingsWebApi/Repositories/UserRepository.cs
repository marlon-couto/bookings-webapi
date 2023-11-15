using AutoMapper;

using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IBookingsDbContext _context;
        private readonly IMapper _mapper;
        public UserRepository(IBookingsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            return await _context.Users.Select(u => _mapper.Map<UserDto>(u)).ToListAsync();
        }

        public async Task<bool> EmailExists(string userEmail)
        {
            User? userFound = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            return userFound is not null;
        }

        public async Task<UserDto> AddUser(UserInsertDto inputData)
        {
            User newUser = _mapper.Map<User>(inputData);
            newUser.UserId = Guid.NewGuid().ToString();
            newUser.Role = "Client";

            await _context.Users.AddAsync(newUser);
            _context.SaveChanges();

            return _mapper.Map<UserDto>(newUser);
        }
    }
}

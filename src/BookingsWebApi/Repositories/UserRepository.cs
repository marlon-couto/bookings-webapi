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
            List<User> allUsers = await _context.Users.ToListAsync();
            return allUsers.Select(u => _mapper.Map<UserDto>(u)).ToList();
        }

        public async Task<bool> EmailExists(string userEmail)
        {
            User? userFound = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            return userFound is not null;
        }

        public async Task<UserDto> AddUser(UserInsertDto userInsert)
        {
            User newUser = _mapper.Map<User>(userInsert);
            newUser.UserId = Guid.NewGuid().ToString();
            newUser.Role = "Client";

            await _context.Users.AddAsync(newUser);
            _context.SaveChanges();

            return _mapper.Map<UserDto>(newUser);
        }
    }
}
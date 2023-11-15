using AutoMapper;

using BookingWebApi.Dtos;
using BookingWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingWebApi.Repositories
{
    public class UserRepository
    {
        private readonly IBookingWebApiContext _context;
        private readonly IMapper _mapper;
        public UserRepository(IBookingWebApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var allUsers = await _context.Users.ToListAsync();
            return allUsers.Select(u => _mapper.Map<UserDto>(u)).ToList();
        }

        public async Task<bool> EmailExists(string userEmail)
        {
            var userFound = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            return userFound is not null;
        }

        public async Task<UserDto> AddUser(UserInsertDto userInsert)
        {
            var newUser = _mapper.Map<User>(userInsert);
            newUser.UserId = Guid.NewGuid().ToString();
            newUser.Role = "Client";

            await _context.Users.AddAsync(newUser);
            _context.SaveChanges();

            return _mapper.Map<UserDto>(newUser);
        }
    }
}

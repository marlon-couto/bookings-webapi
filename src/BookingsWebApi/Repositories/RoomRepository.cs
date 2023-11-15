using AutoMapper;

namespace BookingsWebApi.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IBookingsDbContext _context;
        private readonly IMapper _mapper;
        public RoomRepository(IBookingsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}

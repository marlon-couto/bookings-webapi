using AutoMapper;

using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly IBookingsDbContext _context;
        private readonly IMapper _mapper;
        public HotelRepository(IBookingsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<HotelDto> AddHotel(HotelInsertDto inputData, City cityFound)
        {
            Hotel newHotel = _mapper.Map<Hotel>(inputData);
            newHotel.HotelId = Guid.NewGuid().ToString();

            await _context.Hotels.AddAsync(newHotel);
            _context.SaveChanges(); // TODO: mudar essa implementação para um método assíncrono.

            newHotel.City = cityFound;
            return _mapper.Map<HotelDto>(newHotel);
        }

        public async Task<List<HotelDto>> GetAllHotels()
        {
            List<Hotel> allHotels = await _context.Hotels.Include(h => h.City).ToListAsync();
            return allHotels.Select(h => _mapper.Map<HotelDto>(h)).ToList();
        }

        public async Task<City> GetCityById(string id)
        {
            return await _context.Cities.FirstOrDefaultAsync(c => c.CityId == id)
                ?? throw new KeyNotFoundException("The city with the id provided does not exist");
        }
    }
}

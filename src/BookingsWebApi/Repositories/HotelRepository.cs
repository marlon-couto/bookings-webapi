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

        public async Task<HotelDto> AddHotel(HotelInsertDto inputData, City city)
        {
            Hotel newHotel = _mapper.Map<Hotel>(inputData);
            newHotel.HotelId = Guid.NewGuid().ToString();

            await _context.Hotels.AddAsync(newHotel);
            _context.SaveChanges(); // TODO: mudar essa implementação para um método assíncrono.

            newHotel.City = city;
            return _mapper.Map<HotelDto>(newHotel);
        }

        public void DeleteHotel(Hotel hotel)
        {
            _context.Hotels.Remove(hotel);
            _context.SaveChanges();
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

        public async Task<Hotel> GetHotelById(string id)
        {
            return await _context.Hotels.FirstOrDefaultAsync(h => h.HotelId == id)
                ?? throw new KeyNotFoundException("The hotel with the id provided does not exist");
        }

        public HotelDto UpdateHotel(Hotel hotel, City city, HotelInsertDto inputData)
        {
            hotel.Name = inputData.Name;
            hotel.Address = inputData.Address;
            hotel.CityId = inputData.CityId;
            _context.SaveChanges();

            hotel.City = city;
            return _mapper.Map<HotelDto>(hotel);
        }
    }
}

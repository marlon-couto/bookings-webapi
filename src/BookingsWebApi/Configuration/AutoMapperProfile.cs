using AutoMapper;

using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserInsertDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<CityInsertDto, City>();
            CreateMap<City, CityDto>();

            CreateMap<HotelInsertDto, Hotel>();
            CreateMap<Hotel, HotelDto>()
                .ForMember(dest => dest.CityName,
                    opts => opts.MapFrom(src => src.City!.Name))
                .ForMember(dest => dest.CityState,
                    opts => opts.MapFrom(src => src.City!.State));

            CreateMap<RoomInsertDto, Room>();
            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.Hotel, opts => CreateMap<Hotel, HotelDto>());

            CreateMap<BookingInsertDto, Booking>()
                .ForMember(dest => dest.CheckIn,
                    opts => opts.MapFrom(src => DateTime.Parse(src.CheckIn).ToUniversalTime()))
                .ForMember(dest => dest.CheckOut,
                    opts => opts.MapFrom(src => DateTime.Parse(src.CheckOut).ToUniversalTime()));

            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.Room, opts => CreateMap<Room, RoomDto>());
        }
    }
}

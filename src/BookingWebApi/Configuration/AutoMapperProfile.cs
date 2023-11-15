using AutoMapper;

using BookingWebApi.Dtos;
using BookingWebApi.Models;

namespace BookingWebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserInsertDto, User>();
            CreateMap<City, CityDto>();
            CreateMap<Hotel, HotelDto>()
                .ForMember(dest => dest.CityName,
                    opts => opts.MapFrom(src => src.City!.Name))
                .ForMember(dest => dest.CityState,
                    opts => opts.MapFrom(src => src.City!.State));

            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.Hotel, opts => CreateMap<Hotel, HotelDto>());

            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.Room, opts => CreateMap<Room, RoomDto>());

            CreateMap<BookingInsertDto, Booking>()
                .ForMember(dest => dest.CheckIn,
                    opts => opts.MapFrom(src => DateTime.Parse(src.CheckIn).ToUniversalTime()))
                .ForMember(dest => dest.CheckOut,
                    opts => opts.MapFrom(src => DateTime.Parse(src.CheckOut).ToUniversalTime()));
        }
    }
}

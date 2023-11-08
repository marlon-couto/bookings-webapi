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
            CreateMap<City, CityDto>();
            CreateMap<Hotel, HotelDto>()
                .ForMember(dest => dest.City, opts => CreateMap<City, CityDto>());

            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.Hotel, opts => CreateMap<Hotel, HotelDto>());

            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.User, opts => CreateMap<User, UserDto>())
                .ForMember(dest => dest.Room, opts => CreateMap<Room, RoomDto>());
        }
    }
}

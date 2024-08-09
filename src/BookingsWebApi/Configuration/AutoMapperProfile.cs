using AutoMapper;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Configuration;

/// <inheritdoc />
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserModel, UserDto>();
        CreateMap<CityModel, CityDto>();
        CreateMap<HotelModel, HotelDto>()
            .ForMember(dest => dest.CityName, opts => opts.MapFrom(src => src.City!.Name))
            .ForMember(dest => dest.CityState, opts => opts.MapFrom(src => src.City!.State));
        CreateMap<RoomModel, RoomDto>()
            .ForMember(dest => dest.Hotel, _ => CreateMap<HotelModel, HotelDto>());
        CreateMap<BookingModel, BookingDto>()
            .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.User!.Id))
            .ForMember(dest => dest.Room, _ => CreateMap<RoomModel, RoomDto>());
    }
}
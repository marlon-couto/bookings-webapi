using AutoMapper;
using BookingsWebApi.DTOs;
using BookingsWebApi.Exceptions;
using BookingsWebApi.Helpers;
using BookingsWebApi.Models;
using BookingsWebApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize(Policy = "Admin")]
public class HotelController : Controller, IHotelController
{
    private readonly IMapper _mapper;
    private readonly IHotelService _service;
    private readonly IValidator<HotelInsertDto> _validator;

    public HotelController(
        IHotelService service,
        IMapper mapper,
        IValidator<HotelInsertDto> validator
    )
    {
        _service = service;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsync()
    {
        var hotels = await _service.GetHotels();
        var hotelsMapped = hotels.Select(h => _mapper.Map<HotelDto>(h));
        return Ok(new ControllerResponse { Data = hotelsMapped });
    }

    [HttpGet("{id}/room")]
    [AllowAnonymous]
    public async Task<IActionResult> GetHotelRoomsAsync(string id)
    {
        await _service.GetHotelById(id);
        var hotelRooms = await _service.GetHotelRooms(id);
        var roomsMapped = hotelRooms.Select(r => _mapper.Map<RoomDto>(r));
        return Ok(new ControllerResponse { Data = roomsMapped });
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] HotelInsertDto dto)
    {
        var errors = await GetInputDataErrors(dto);
        if (errors != null)
        {
            throw new InvalidInputDataException(string.Join(" ", errors));
        }

        var cityFound = await _service.GetCityById(dto.CityId);
        if (cityFound == null)
        {
            throw new NotFoundException("The city with the id provided does not exist.");
        }

        var hotelCreated = await _service.AddHotel(dto, cityFound);
        var hotelMapped = _mapper.Map<HotelDto>(hotelCreated);
        return Created("/api/hotel", new ControllerResponse { Data = hotelMapped });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromBody] HotelInsertDto dto, string id)
    {
        var errors = await GetInputDataErrors(dto);
        if (errors != null)
        {
            throw new InvalidInputDataException(string.Join(" ", errors));
        }

        var hotelFound = await _service.GetHotelById(id);
        if (hotelFound == null)
        {
            throw new NotFoundException("The hotel with the id provided does not exist.");
        }

        var cityFound = await _service.GetCityById(dto.CityId);
        if (cityFound == null)
        {
            throw new NotFoundException("The city with the id provided does not exist.");
        }

        var hotelUpdated = await _service.UpdateHotel(dto, hotelFound, cityFound);
        var hotelMapped = _mapper.Map<HotelDto>(hotelUpdated);
        return Ok(new ControllerResponse { Data = hotelMapped });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var hotelFound = await _service.GetHotelById(id);
        if (hotelFound == null)
        {
            throw new NotFoundException("The hotel with the id provided does not exist.");
        }

        await _service.DeleteHotel(hotelFound);
        return NoContent();
    }

    private async Task<IEnumerable<string>?> GetInputDataErrors(HotelInsertDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (validationResult.IsValid)
        {
            return null;
        }

        var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage);
        return errorMessages;
    }
}
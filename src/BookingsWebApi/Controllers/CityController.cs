using AutoMapper;
using BookingsWebApi.Controllers.Interfaces;
using BookingsWebApi.DTOs;
using BookingsWebApi.Exceptions;
using BookingsWebApi.Helpers;
using BookingsWebApi.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize(Policy = "Admin")]
public class CityController : Controller, ICityController
{
    private readonly IMapper _mapper;
    private readonly ICityService _service;
    private readonly IValidator<CityInsertDto> _validator;

    public CityController(ICityService service, IMapper mapper, IValidator<CityInsertDto> validator)
    {
        _service = service;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsync()
    {
        var cities = await _service.GetCities();
        var citiesMapped = cities.Select(c => _mapper.Map<CityDto>(c));
        return Ok(new ControllerResponse { Data = citiesMapped });
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CityInsertDto dto)
    {
        var errors = await GetInputDataErrors(dto);
        if (errors != null)
        {
            throw new InvalidInputDataException(string.Join(" ", errors));
        }

        var cityCreated = await _service.AddCity(dto);
        var cityMapped = _mapper.Map<CityDto>(cityCreated);
        return Created("/api/city", new ControllerResponse { Data = cityMapped, StatusCode = 201 });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync([FromBody] CityInsertDto dto, Guid id)
    {
        var errors = await GetInputDataErrors(dto);
        if (errors != null)
        {
            throw new InvalidInputDataException(string.Join(" ", errors));
        }

        var cityFound = await _service.GetCityById(id);
        if (cityFound == null)
        {
            throw new NotFoundException("The city with the id provided does not exist.");
        }

        var cityUpdated = await _service.UpdateCity(dto, cityFound);
        var cityMapped = _mapper.Map<CityDto>(cityUpdated);
        return Ok(new ControllerResponse { Data = cityMapped });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var cityFound = await _service.GetCityById(id);
        if (cityFound == null)
        {
            throw new NotFoundException("The city with the id provided does not exist.");
        }

        await _service.DeleteCity(cityFound);
        return NoContent();
    }

    private async Task<IEnumerable<string>?> GetInputDataErrors(CityInsertDto dto)
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
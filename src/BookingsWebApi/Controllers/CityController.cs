using AutoMapper;

using BookingsWebApi.DTOs;
using BookingsWebApi.Helpers;
using BookingsWebApi.Models;
using BookingsWebApi.Services;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize(Policy = "Admin")]
public class CityController : Controller
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

    /// <summary>
    ///     Retrieves all cities information.
    /// </summary>
    /// <returns>A JSON response representing the result of the operation</returns>
    /// <response code="200">Returns 200 and the city data.</response>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsync()
    {
        List<CityModel> cities = await _service.GetCities();
        List<CityDto> citiesMapped = cities.Select(c => _mapper.Map<CityDto>(c)).ToList();

        return Ok(
            new ControllerResponse<List<CityDto>> { Data = citiesMapped, Result = "Success" }
        );
    }

    /// <summary>
    ///     Creates a new city based on the provided data.
    /// </summary>
    /// <param name="dto">The data for creating a new city.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     POST /api/city
    ///     {
    ///     "name": "New City",
    ///     "state": "State 1"
    ///     }
    /// </remarks>
    /// <response code="201">Returns 201 and the newly created city data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    [HttpPost]
    public async Task<IActionResult> PostAsync(CityInsertDto dto)
    {
        try
        {
            await ValidateInputData(dto);
            CityModel cityCreated = await _service.AddCity(dto);
            CityDto cityMapped = _mapper.Map<CityDto>(cityCreated);

            return Created(
                "/api/city",
                new ControllerResponse<CityDto> { Data = cityMapped, Result = "Success" }
            );
        }
        catch (ArgumentException e)
        {
            return BadRequest(
                new ControllerErrorResponse { Message = e.Message, Result = "Error" }
            );
        }
    }

    /// <summary>
    ///     Updates the city with the given ID based on the provided data.
    /// </summary>
    /// <param name="dto">The data for updating the city retrieved.</param>
    /// <param name="id">The ID of the city to update.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     PUT /api/city/1
    ///     {
    ///     "name": "New City v2",
    ///     "state": "State 1"
    ///     }
    /// </remarks>
    /// <response code="200">Returns 201 and the updated city data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    /// <response code="404">
    ///     If a city with the provided ID not exists, returns 404 and an error message.
    /// </response>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromBody] CityInsertDto dto, string id)
    {
        try
        {
            await ValidateInputData(dto);
            CityModel cityFound = await _service.GetCityById(id);
            CityModel cityUpdated = await _service.UpdateCity(dto, cityFound);
            CityDto cityMapped = _mapper.Map<CityDto>(cityUpdated);

            return Ok(new ControllerResponse<CityDto> { Data = cityMapped, Result = "Success" });
        }
        catch (ArgumentException e)
        {
            return BadRequest(
                new ControllerErrorResponse { Message = e.Message, Result = "Error" }
            );
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new ControllerErrorResponse { Message = e.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Deletes a city with the given ID.
    /// </summary>
    /// <param name="id">The ID of the city to delete.</param>
    /// <returns>A status code 204 and no content.</returns>
    /// <response code="204">Returns 204 with no content.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="404">
    ///     If the city is not found, returns 404 and an error message.
    /// </response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        try
        {
            CityModel cityFound = await _service.GetCityById(id);
            await _service.DeleteCity(cityFound);

            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new ControllerErrorResponse { Message = e.Message, Result = "Error" });
        }
    }

    private async Task ValidateInputData(CityInsertDto dto)
    {
        ValidationResult? validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            List<string> errorMessages = validationResult
                .Errors.Select(e => e.ErrorMessage)
                .ToList();

            throw new ArgumentException(string.Join(" ", errorMessages));
        }
    }
}
using AutoMapper;

using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
using BookingsWebApi.Repositories;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize(Policy = "Admin")]
public class HotelController : Controller
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<HotelInsertDto> _validator;

    public HotelController(
        IHotelRepository hotelRepository,
        IMapper mapper,
        IValidator<HotelInsertDto> validator
    )
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
        _validator = validator;
    }

    /// <summary>
    ///     Retrieves hotel information by ID.
    /// </summary>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <response code="200">Returns 200 and the hotel data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsync()
    {
        List<Hotel> allHotels = await _hotelRepository.GetAllHotels();
        return Ok(
            new { Data = allHotels.Select(h => _mapper.Map<HotelDto>(h)).ToList(), Result = "Success" }
        );
    }

    /// <summary>
    ///     Retrieves room information by associated hotel ID.
    /// </summary>
    /// <param name="id">The ID of the hotel that the rooms will be retrieved.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <response code="200">Returns 200 and the rooms data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    [HttpGet("{id}/room")]
    [AllowAnonymous]
    public async Task<IActionResult> GetHotelRoomsAsync(string id)
    {
        try
        {
            await _hotelRepository.GetHotelById(id);

            List<Room> hotelRooms = await _hotelRepository.GetHotelRooms(id);
            return Ok(
                new { Data = hotelRooms.Select(r => _mapper.Map<RoomDto>(r)).ToList(), Result = "Success" }
            );
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { ex.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Creates a new hotel for based on the provided data.
    /// </summary>
    /// <param name="inputData">The data for creating a new hotel.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     POST /api/hotel
    ///     {
    ///     "name": "New Hotel",
    ///     "address": "Address 1",
    ///     "cityId": "1"
    ///     }
    /// </remarks>
    /// <response code="201">Returns 201 and the newly created hotel data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="404">If the associated city is not found, returns 404 and a error message.</response>
    /// <response code="400">If the input data is invalid, returns 400 and an error message.</response>
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] HotelInsertDto inputData)
    {
        try
        {
            await ValidateInputData(inputData);

            City cityFound = await _hotelRepository.GetCityById(inputData.CityId);

            Hotel createdHotel = await _hotelRepository.AddHotel(inputData, cityFound);
            return Created(
                "/api/hotel",
                new { Data = _mapper.Map<HotelDto>(createdHotel), Result = "Success" }
            );
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { ex.Message, Result = "Error" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Updates the hotel with the given ID based on the provided data.
    /// </summary>
    /// <param name="inputData">The data for updating the hotel retrieved.</param>
    /// <param name="id">The ID of the hotel to update.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     PUT /api/hotel/1
    ///     {
    ///     "name": "New Hotel v2",
    ///     "address": "Address 1",
    ///     "cityId": "1"
    ///     }
    /// </remarks>
    /// <response code="200">Returns 200 and the updated hotel data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="400">If the input data is invalid, returns 400 and an error message.</response>
    /// <response code="404">
    ///     If a hotel with the provided ID not exists or the associated city is not found, returns 404 and
    ///     an error message.
    /// </response>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromBody] HotelInsertDto inputData, string id)
    {
        try
        {
            await ValidateInputData(inputData);

            Hotel hotelFound = await _hotelRepository.GetHotelById(id);
            City cityFound = await _hotelRepository.GetCityById(inputData.CityId);

            Hotel updatedHotel = await _hotelRepository.UpdateHotel(
                inputData,
                hotelFound,
                cityFound
            );
            return Ok(new { Data = _mapper.Map<HotelDto>(updatedHotel), Result = "Success" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message, Result = "Error" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { ex.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Deletes a hotel with the given ID.
    /// </summary>
    /// <param name="id">The ID of the hotel to delete.</param>
    /// <returns>A status code 204 and no content.</returns>
    /// <response code="204">Returns 204 with no content.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="404">If the hotel is not found, returns 404 and an error message.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        try
        {
            Hotel hotelFound = await _hotelRepository.GetHotelById(id);
            await _hotelRepository.DeleteHotel(hotelFound);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { ex.Message, Result = "Error" });
        }
    }

    private async Task ValidateInputData(HotelInsertDto inputData)
    {
        ValidationResult? validationResult = await _validator.ValidateAsync(inputData);
        if (!validationResult.IsValid)
        {
            List<string> errorMessages = validationResult
                .Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ArgumentException(string.Join(" ", errorMessages));
        }
    }
}
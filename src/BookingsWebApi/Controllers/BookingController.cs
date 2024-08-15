using System.Security.Claims;
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
[Authorize(Policy = "Client")]
public class BookingController : Controller, IBookingController
{
    private readonly IAuthHelper _authHelper;
    private readonly IMapper _mapper;
    private readonly IBookingService _service;
    private readonly IValidator<BookingInsertDto> _validator;

    public BookingController(
        IBookingService service,
        IMapper mapper,
        IValidator<BookingInsertDto> validator,
        IAuthHelper authHelper
    )
    {
        _service = service;
        _mapper = mapper;
        _validator = validator;
        _authHelper = authHelper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userEmail = _authHelper.GetLoggedUserEmail(identity);
        var isAdmin = _authHelper.IsAdmin(identity);
        var bookings = await _service.GetBookings(userEmail, isAdmin);
        var bookingsMapped = bookings.Select(b => _mapper.Map<BookingDto>(b));
        return Ok(new ControllerResponse { Data = bookingsMapped });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userEmail = _authHelper.GetLoggedUserEmail(identity);
        var bookingFound = await _service.GetBookingById(id, userEmail);
        if (bookingFound == null)
        {
            throw new NotFoundException("The booking with the id provided does not exist.");
        }

        var bookingMapped = _mapper.Map<BookingDto>(bookingFound);
        return Ok(new ControllerResponse { Data = bookingMapped });
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] BookingInsertDto dto)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userEmail = _authHelper.GetLoggedUserEmail(identity);
        var userFound = await _service.GetUserByEmail(userEmail);
        if (userFound == null)
        {
            throw new NotFoundException("The user with the email provided does not exist.");
        }

        var errors = await GetInputDataErrors(dto);
        if (errors != null)
        {
            throw new InvalidInputDataException(string.Join(" ", errors));
        }

        var roomFound = await _service.GetRoomById(dto.RoomId);
        if (roomFound == null)
        {
            throw new NotFoundException("The room with the id provided does not exist.");
        }

        var hasEnoughCapacity = HasEnoughCapacity(dto, roomFound);
        if (!hasEnoughCapacity)
        {
            throw new MaximumCapacityException();
        }

        var bookingCreated = await _service.AddBooking(dto, userFound, roomFound);
        var bookingMapped = _mapper.Map<BookingDto>(bookingCreated);
        return Created($"/api/booking/{bookingCreated.Id}", new ControllerResponse { Data = bookingMapped, StatusCode = 201 });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync([FromBody] BookingInsertDto dto, Guid id)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userEmail = _authHelper.GetLoggedUserEmail(identity);
        var userFound = await _service.GetUserByEmail(userEmail);
        if (userFound == null)
        {
            throw new NotFoundException("The user with the email provided does not exist.");
        }

        var errors = await GetInputDataErrors(dto);
        if (errors != null)
        {
            throw new InvalidInputDataException(string.Join(" ", errors));
        }

        var bookingFound = await _service.GetBookingById(id, userEmail);
        if (bookingFound == null)
        {
            throw new NotFoundException("The booking with the id provided does not exist.");
        }

        var roomFound = await _service.GetRoomById(dto.RoomId);
        if (roomFound == null)
        {
            throw new NotFoundException("The room with the id provided does not exist.");
        }

        var hasEnoughCapacity = HasEnoughCapacity(dto, roomFound);
        if (!hasEnoughCapacity)
        {
            throw new MaximumCapacityException();
        }

        var bookingUpdated = await _service.UpdateBooking(dto, bookingFound, roomFound);
        var bookingMapped = _mapper.Map<BookingDto>(bookingUpdated);
        return Ok(new ControllerResponse { Data = bookingMapped });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userEmail = _authHelper.GetLoggedUserEmail(identity);
        var userFound = await _service.GetUserByEmail(userEmail);
        if (userFound == null)
        {
            throw new NotFoundException("The user with the email provided does not exist.");
        }

        var bookingFound = await _service.GetBookingById(id, userEmail);
        if (bookingFound == null)
        {
            throw new NotFoundException("The booking with the id provided does not exist.");
        }

        await _service.DeleteBooking(bookingFound);
        return NoContent();
    }

    // Validates input data. If they are not valid, it returns the associated error messages.
    private async Task<IEnumerable<string>?> GetInputDataErrors(BookingInsertDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (validationResult.IsValid)
        {
            return null;
        }

        var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage);
        return errorMessages;
    }

    private static bool HasEnoughCapacity(BookingInsertDto dto, RoomModel roomFound)
    {
        return roomFound.Capacity >= dto.GuestQuantity;
    }
}
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Services;

/// <summary>
///     Service for managing bookings in the application.
/// </summary>
public interface IBookingService
{
    /// <summary>
    ///     Adds a new booking to the database.
    /// </summary>
    /// <param name="dto">The data to create a new booking.</param>
    /// <param name="bookingUser">The user associated with the booking.</param>
    /// <param name="bookingRoom">The room associated with the booking.</param>
    /// <returns>A <see cref="Booking" /> representing the newly created booking.</returns>
    Task<Booking> AddBooking(BookingInsertDto dto, User bookingUser, Room bookingRoom);

    /// <summary>
    ///     Deletes a booking with the given ID from the database.
    /// </summary>
    /// <param name="booking">The entity that will be removed from the database.</param>
    Task DeleteBooking(Booking booking);

    /// <summary>
    ///     Retrieves all bookings for the logged user from the database.
    /// </summary>
    /// <param name="userEmail">The email from the user associated with the bookings.</param>
    /// <returns>A list of <see cref="Booking" /> representing the bookings found. </returns>
    Task<List<Booking>> GetBookings(string userEmail);

    /// <summary>
    ///     Retrieves a booking with the given ID from the database.
    /// </summary>
    /// <param name="id">The booking ID to search the database.</param>
    /// <param name="userEmail">The email from the user associated with the booking.</param>
    /// <returns>A <see cref="Booking" /> representing the booking found. </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a booking with the given ID and email does not exist.
    /// </exception>
    Task<Booking> GetBookingById(string id, string userEmail);

    /// <summary>
    ///     Retrieves a room with the given ID from the database.
    /// </summary>
    /// <param name="roomId">The room ID to search the database.</param>
    /// <returns>The <see cref="Room" /> found.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a room with the given ID does not exist.
    /// </exception>
    Task<Room> GetRoomById(string roomId);

    /// <summary>
    ///     Retrieves a user with the given email from the database.
    /// </summary>
    /// <param name="userEmail">The email to search the database.</param>
    /// <returns>The <see cref="User" /> found.</returns>
    /// <exception cref="UnauthorizedAccessException">
    ///     Thrown if a user with the given email does not exist.
    /// </exception>
    Task<User> GetUserByEmail(string userEmail);

    /// <summary>
    ///     Updates a booking in the database.
    /// </summary>
    /// <param name="dto">The data used to update the booking.</param>
    /// <param name="booking">The entity that will be updated in the database.</param>
    /// <param name="bookingRoom">The room associated with the booking.</param>
    /// <returns>A <see cref="Booking" /> representing the updated booking.</returns>
    Task<Booking> UpdateBooking(BookingInsertDto dto, Booking booking, Room bookingRoom);
}
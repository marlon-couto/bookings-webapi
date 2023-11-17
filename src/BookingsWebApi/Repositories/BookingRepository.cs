using AutoMapper;

using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories
{
    /// <summary>
    /// Repository for managing bookings in the application.
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly IBookingsDbContext _context;
        private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="BookingRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for the application.</param>
        /// <param name="mapper">The AutoMapper instance for object mapping.</param>
        public BookingRepository(IBookingsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new booking to the database.
        /// </summary>
        /// <param name="inputData">The data to create a new booking.</param>
        /// <param name="userFound">The user associated with the booking.</param>
        /// <param name="roomFound">The room associated with the booking.</param>
        /// <returns>A <see cref="BookingDto"/> representing the newly created booking.</returns>
        public async Task<BookingDto> AddBooking(BookingInsertDto inputData, User userFound, Room roomFound)
        {
            Booking newBooking = _mapper.Map<Booking>(inputData);
            newBooking.BookingId = Guid.NewGuid().ToString();
            newBooking.UserId = userFound.UserId;

            await _context.Bookings.AddAsync(newBooking);
            _context.SaveChanges();

            newBooking.User = userFound;
            newBooking.Room = roomFound;
            return _mapper.Map<BookingDto>(newBooking);
        }

        /// <summary>
        /// Retrieves a booking with the given ID from the database.
        /// </summary>
        /// <param name="id">The booking ID to search the database.</param>
        /// <param name="userEmail">The email from the user associated with the booking.</param>
        /// <returns>A <see cref="BookingDto"/> representing the booking found. </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if a booking with the given ID and email does not exist.
        /// </exception>
        public async Task<BookingDto> GetBookingById(string id, string userEmail)
        {
            Booking? bookingFound = await _context.Bookings
                            .Include(b => b.User)
                            .Include(b => b.Room)
                            .Include(b => b.Room!.Hotel)
                            .Include(b => b.Room!.Hotel!.City)
                            .FirstOrDefaultAsync(b => b.User!.Email == userEmail && b.BookingId == id);

            return bookingFound is null
                ? throw new KeyNotFoundException("The booking with the id provided does not exist")
                : _mapper.Map<BookingDto>(bookingFound);
        }

        /// <summary>
        /// Retrieves a room with the given ID from the database.
        /// </summary>
        /// <param name="roomId">The room ID to search the database.</param>
        /// <returns>The <see cref="Room"/> found.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if a room with the given ID does not exist.</exception>
        public async Task<Room> GetRoomById(string roomId)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId)
                ?? throw new KeyNotFoundException("The room with the id provided does not exist");
        }

        /// <summary>
        /// Retrieves a user with the given email from the database.
        /// </summary>
        /// <param name="userEmail">The email to search the database.</param>
        /// <returns>The <see cref="User"/> found.</returns>
        /// <exception cref="UnauthorizedAccessException">
        /// Thrown if a user with the given email does not exist.
        /// </exception>
        public async Task<User> GetUserByEmail(string userEmail)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail)
                ?? throw new UnauthorizedAccessException("The user with the email provided does not exist");
        }
    }
}

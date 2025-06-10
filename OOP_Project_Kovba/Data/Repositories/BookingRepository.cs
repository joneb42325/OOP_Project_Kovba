using OOP_Project_Kovba.Models;
using OOP_Project_Kovba.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace OOP_Project_Kovba.Data.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddBookingAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllUsersBookings(string userId)
        {
            return await _context.Bookings
            .Include(t => t.Trip)
            .Include(t => t.Trip.Driver)
            .Include(t => t.User)
            .Where(t => t.UserId == userId
                    && t.IsCancelled == false
                    && t.Trip.ArrivalDate.Date >= DateTime.Today)
            .OrderBy(t => t.Trip.DepartureTime)
            .ToListAsync();
        }

        public async Task DeleteBookingsByTripIdAsync(string tripId)
        {
            var bookingsToDelete = _context.Bookings.Where(b => b.TripId == tripId);
            _context.Bookings.RemoveRange(bookingsToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(string bookingId)
        {
            return await _context.Bookings
            .Include(t => t.Trip)
            .Include(t => t.User)
            .Where(t => t.IsCancelled == false)
            .SingleOrDefaultAsync(t => t.Id == bookingId);
        }
    }
}

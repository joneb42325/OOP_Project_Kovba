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

        public async Task<IEnumerable<Booking>> GetAllUsersBookings(string userId)
        {
            return await _context.Bookings
            .Include(t => t.User)
            .Where(t => t.UserId == userId
                    && t.IsCancelled == false)
            .OrderBy(t => t.Trip.DepartureTime)
            .ToListAsync();
        }

    }
}

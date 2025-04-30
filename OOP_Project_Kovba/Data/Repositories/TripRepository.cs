using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Microsoft.EntityFrameworkCore;
using OOP_Project_Kovba.Interfaces;
using OOP_Project_Kovba.Models;

namespace OOP_Project_Kovba.Data.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly ApplicationDbContext _context;

        public TripRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTripAsync(Trip trip)
        {
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTripAsync(Trip trip)
        {
            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Trip>> GetTripsAsync(string from, string to, DateTime date, int passengers)
        {
           return await _context.Trips
           .Include(t => t.Driver)
           .Where(t => t.IsCancelled == false
                 && t.FromCity == from
                 && t.ToCity == to
                 && t.DepartureTime.Date == date.Date
                 && t.MaxPassengers >= passengers
                 && t.IsCancelled == false
                 ) .ToListAsync();
        }

        public async Task<IEnumerable<Trip>> GetAllDriverTrips(string userId)
        {
            return await _context.Trips
            .Include(t => t.Driver)
            .Where(t => t.DriverId == userId
                    && t.IsCancelled == false)
            .OrderBy(t => t.DepartureTime)
            .ToListAsync();
        }

        public async Task<Trip?> GetTripByIdAsync(string id)
        {
            return await _context.Trips
            .Include(t => t.Bookings.Where(b => b.IsCancelled == false))
                .ThenInclude(b => b.User)
            .Include(t => t.Driver)
            .Where(t => t.IsCancelled == false)
            .SingleOrDefaultAsync(t => t.Id == id);
        }

    }
}

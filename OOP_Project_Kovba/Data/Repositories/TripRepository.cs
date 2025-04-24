using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using MyMVC.Controllers;
using OOP_Project_Kovba.Models;

namespace MyMVC.Data
{
    public class TripRepository : ITripRepository
    {
        private readonly ApplicationDbContext _context;
     //   private AccountController _accountController;

        public TripRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTripAsync(Trip trip)
        {
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();
        }





        //  public ApplicationUser? GetUsersTrips (int userId)


        //  public bool AddUserTrip (CreatedTrip trip)


        //  public ApplicationUser? GetTrips(string fromLocation, string toLocation, DateTime date, int passengersAmount)


        //   public CreatedTrip? GetTripById(int id)

    }
}

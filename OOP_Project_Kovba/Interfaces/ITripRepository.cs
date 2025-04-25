using OOP_Project_Kovba.Models;
namespace MyMVC.Data
{
    public interface ITripRepository
    {
        Task AddTripAsync(Trip trip);
        Task <IEnumerable<Trip>> GetTripsAsync(string from, string to, DateTime date, int passengers);
       // Task<List<BookedTrip>> GetBookedTripsAsync(string userId);
       // Task<List<CreatedTrip>> GetCreatedTripsAsync(string userId);
    }

}

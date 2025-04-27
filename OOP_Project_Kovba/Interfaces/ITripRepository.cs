using OOP_Project_Kovba.Models;
namespace OOP_Project_Kovba.Interfaces
{
    public interface ITripRepository
    {
        Task AddTripAsync(Trip trip);
        Task <IEnumerable<Trip>> GetTripsAsync(string from, string to, DateTime date, int passengers);
        public Task<Trip?> GetTripByIdAsync(string id);
        public Task UpdateTripAsync(Trip trip);
        public Task<IEnumerable<Trip>> GetAllDriverTrips(string userId);
    }
}

using OOP_Project_Kovba.Services;
using OOP_Project_Kovba.ViewModels;

namespace OOP_Project_Kovba.Interfaces
{
    public interface ITripService
    {
        Task<TripDetailsViewModel?> GetTripDetailsViewModelAsync(string id);

        public Task<IEnumerable<TripResultViewModel>> GetDriversPlannedTripsAsync(string userId);

        public Task<IEnumerable<BookingResultViewModel>> GetPassengerBookingsAsync(string userId);

        public Task<(bool Success, string Message, TripDetailsViewModel? ViewModel)> TryCreateBookingAsync(string tripId, string userId, int seatsBooked);
    }
}

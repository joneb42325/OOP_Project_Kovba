using OOP_Project_Kovba.Interfaces;
using OOP_Project_Kovba.ViewModels;

namespace OOP_Project_Kovba
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;

        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<TripDetailsViewModel?> GetTripDetailsViewModelAsync(string id)
        {
            var trip = await _tripRepository.GetTripByIdAsync(id);

            if (trip == null)
            {
                return null;
            }

            var viewModel = new TripDetailsViewModel
            {
                Id = trip.Id,
                FromCity = trip.FromCity,
                ToCity = trip.ToCity,
                FromStreetAndHouse = trip.FromStreetAndHouse,
                ToStreetAndHouse = trip.ToStreetAndHouse,
                ArrivalTime = trip.ArrivalDate,
                DepartureTime = trip.DepartureTime,
                DriverName = trip.Driver.FullName,
                DriverEmail = trip.Driver.Email,
                CarModel = trip.CarModel,
                MaxPassengers = trip.MaxPassengers,
                Price = trip.Price
            };

            return viewModel;
        }
    }
}

using OOP_Project_Kovba.Interfaces;
using OOP_Project_Kovba.Models;
using OOP_Project_Kovba.ViewModels;

namespace OOP_Project_Kovba
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IBookingRepository _bookingRepository;
        public TripService(ITripRepository tripRepository, IBookingRepository bookingRepository)
        {
            _tripRepository = tripRepository;
            _bookingRepository = bookingRepository;
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
                Comment = trip.Comment,
                MaxPassengers = trip.MaxPassengers,
                Price = trip.Price,
                Bookings = trip.Bookings,
            };

            return viewModel;
        }

        public async Task<(bool Success, string Message, TripDetailsViewModel? ViewModel)> TryCreateBookingAsync(string tripId, string userId, int seatsBooked)
        {
            var trip = await _tripRepository.GetTripByIdAsync(tripId);
            if (trip == null)
                return (false, "Поїздку не знайдено", null);

            if (userId == trip.DriverId)
                return (false, "Ви є водієм в цій поїздці.", await GetTripDetailsViewModelAsync(tripId));

            if (!trip.HasAvailableSeats())
                return (false, "В поїздці немає вільних місць!", await GetTripDetailsViewModelAsync(tripId));

            if (Booking.IsUserAlreadyBooked(userId, tripId, trip.Bookings))
                return (false, "Ви вже заброньовані в цій поїздці!", await GetTripDetailsViewModelAsync(tripId));

            var booking = new Booking
            {
                TripId = tripId,
                UserId = userId,
                SeatsBooked = seatsBooked
            };

            await _bookingRepository.AddBookingAsync(booking);
            trip.Bookings.Add(booking);
            trip.ReduceAvailableSeats(seatsBooked); 

            await _tripRepository.UpdateTripAsync(trip);

            var updatedViewModel = await GetTripDetailsViewModelAsync(tripId);

            return (true, "Бронювання успішно створено!", updatedViewModel);
        }

        public async Task<IEnumerable<TripResultViewModel>> GetDriversPlannedTripsAsync(string userId)
        {
            var trips = await _tripRepository.GetAllDriverTrips(userId);

            var driverTrips = trips.Select(trip => new TripResultViewModel
            {
                Id = trip.Id,
                FromCity = trip.FromCity,
                ToCity = trip.ToCity,
                DepartureTime = trip.DepartureTime,
                ArrivalTime = trip.ArrivalDate,
                DriverName = trip.Driver.FullName,
                CarModel = trip.CarModel,
                MaxPassengers = trip.MaxPassengers,
                Price = trip.Price
            }).ToList();

            return driverTrips;
        }

        public async Task<IEnumerable<TripResultViewModel>> GetPassengerBookingsAsync(string userId)
        {
            var bookings = await _bookingRepository.GetAllUsersBookings(userId);

            var passengerBookings = bookings.Select(booking => new TripResultViewModel
            {
                Id = booking.Trip.Id,
                FromCity = booking.Trip.FromCity,
                ToCity = booking.Trip.ToCity,
                DepartureTime = booking.Trip.DepartureTime,
                ArrivalTime = booking.Trip.ArrivalDate,
                DriverName = booking.Trip.Driver.FullName,
                CarModel = booking.Trip.CarModel,
                MaxPassengers = booking.Trip.MaxPassengers,
                Price = booking.Trip.Price
            }).ToList();

            return passengerBookings;
        }
    }
}

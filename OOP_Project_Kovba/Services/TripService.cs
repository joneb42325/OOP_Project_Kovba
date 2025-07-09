using OOP_Project_Kovba.Interfaces;
using OOP_Project_Kovba.Models;
using OOP_Project_Kovba.ViewModels;
using OOP_Project_Kovba.Services;
namespace OOP_Project_Kovba.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IBookingRepository _bookingRepository;
        public delegate void TripBookedEventHandler(object sender, TripBookedEventArgs e);
        public event TripBookedEventHandler? TripBooked;
        public TripService(ITripRepository tripRepository, IBookingRepository bookingRepository)
        {
            _tripRepository = tripRepository;
            _bookingRepository = bookingRepository;

            TripBooked += (sender, e) =>
            {
                Console.WriteLine($"User {e.UserId} booked {e.SeatsBooked} seats for trip {e.TripId}");
            };
        }


        public async Task<TripDetailsViewModel?> GetTripDetailsViewModelAsync(string id)
        {
            var trip = await _tripRepository.GetTripByIdAsync(id);

            if (trip == null)
            {
                throw new InvalidOperationException();
            }

            if(trip.Driver.Email == null)
            {
                throw new InvalidOperationException();
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

            if (seatsBooked < 1 || seatsBooked > trip.MaxPassengers)
            {
                return (false, "Некоректна кількість місць", await GetTripDetailsViewModelAsync(tripId));
            }

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

            OnTripBooked(tripId, userId, seatsBooked);

            return (true, "Бронювання успішно створено!", updatedViewModel);
        }

        public virtual void OnTripBooked(string tripId, string userId, int seatsBooked)
        {
            TripBooked?.Invoke(this, new TripBookedEventArgs
            {
                TripId = tripId,
                UserId = userId,
                SeatsBooked = seatsBooked
            });
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

        public async Task<IEnumerable<BookingResultViewModel>> GetPassengerBookingsAsync(string userId)
        {
            var bookings = await _bookingRepository.GetAllUsersBookings(userId);

            var passengerBookings = bookings.Select(booking => new BookingResultViewModel
            {
                BookingId = booking.Id,
                TripId = booking.Trip.Id,
                FromCity = booking.Trip.FromCity,
                ToCity = booking.Trip.ToCity,
                DepartureTime = booking.Trip.DepartureTime,
                ArrivalTime = booking.Trip.ArrivalDate,
                DriverName = booking.Trip.Driver.FullName,
                CarModel = booking.Trip.CarModel,
                MaxPassengers = booking.Trip.MaxPassengers,
                Price = booking.Trip.Price,
            }).ToList();

            return passengerBookings;
        }
    }
}

//Fix typo
// Additional fix

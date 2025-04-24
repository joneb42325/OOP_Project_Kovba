using System.ComponentModel.DataAnnotations;

namespace OOP_Project_Kovba.Models
{
    public class Booking : AuditableEntity
    {
        public string Id { get; set; }

        private int _seatsBooked;
        
        private string _userId = string.Empty;

        private string _tripId = string.Empty;

        private bool _isCancelled = false;

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public Trip Trip { get; set; }

        public string TripId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Має бути заброньовано принаймні одне місце")]
        public int SeatsBooked {
            get => _seatsBooked;
            set
            {
                if (value > Trip.MaxPassengers)
                    throw new ArgumentException($"Кількість місць не може перевищувати {Trip.MaxPassengers} для цієї поїздки.");

                if (value < 1)
                    throw new ArgumentException("Має бути заброньовано принаймні одне місце");
    
                _seatsBooked = value;
            }
        }

        [Required]
        [MinLength(1, ErrorMessage = "Ідентифікатор користувача не може бути порожнім")]
        public string UserId
        {
            get => _userId;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("UserId не може бути порожнім");
                _userId = value;
            }
        }

        /*
        [Required]
        [MinLength(1, ErrorMessage = "Ідентифікатор поїздки не може бути порожнім")]
        public string TripId
        {
            get => _tripId;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("TripId не може бути порожнім");
                _tripId = value;
            }
        }
        */
        public bool IsCancelled
        {
            get => _isCancelled;
            set { _isCancelled = value; }
        }

        public Booking() { }

        public Booking(int seatsBooked, string userId, ApplicationUser user, string tripId, Trip trip)
        {
            Trip = trip ?? throw new ArgumentNullException(nameof(trip));
            TripId = tripId;
            User = user ?? throw new ArgumentNullException(nameof(user));
            UserId = userId;
            SeatsBooked = seatsBooked;
        }

        public void ChangeSeats(int newSeats)
        {
            SeatsBooked = newSeats;
        }

        public override string GetInfo()
        {
            return $"Поїздка: {Trip.FromCity} → {Trip.ToCity}, " +
                   $"Кількість місць: {SeatsBooked}, " +
                   $"Модель авто: {Trip.CarModel}, " +
                   $"Ціна: {Trip.Price} грн";
        }

        public void CancelBooking()
        {
            if ((Trip.DepartureTime - DateTime.UtcNow).TotalHours < 24)
                throw new InvalidOperationException("Trip cannot be canceled less than 24 hours before departure.");
            IsCancelled = true;
        }

        public static bool IsUserAlreadyBooked(string userId, string tripId, ICollection<Booking> bookings)
        {
            return bookings.Any(booking => booking.UserId == userId && booking.TripId == tripId);
        }
    }
}

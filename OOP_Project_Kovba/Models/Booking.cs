using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Project_Kovba.Models
{
    public class Booking : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        private int _seatsBooked;
        
        private string _userId = string.Empty;

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

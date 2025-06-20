﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Project_Kovba.Models
{
    public class Booking : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;

        private int _seatsBooked;
        
        private string _userId = string.Empty;

        private bool _isCancelled = false;

        [Required]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public Trip Trip { get; set; } = null!;

        public string TripId { get; set; } = string.Empty;

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
            return
                $"Поїздка: {Trip.FromCity} → {Trip.ToCity}\n" +
                $"Кількість місць: {SeatsBooked}\n" +
                $"Модель авто: {Trip.CarModel}\n" +
                $"Ціна: {Trip.Price} грн";
        }


        public void CancelBooking()
        {
            var hoursToDeparture = (Trip.DepartureTime - DateTime.Now).TotalHours;
            if (hoursToDeparture < 24)
                throw new InvalidOperationException("Неможливо скасувати бронювання менше ніж за 24 години до відправлення.");
            IsCancelled = true;
        }

        public static bool IsUserAlreadyBooked(string userId, string tripId, ICollection<Booking> bookings)
        {
            return bookings.Any(booking => booking.UserId == userId && booking.TripId == tripId);
        }
    }
}

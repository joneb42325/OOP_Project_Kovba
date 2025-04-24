using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Project_Kovba.Models
{
    public class Trip : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        private string _fromCity = string.Empty;
        private string _fromStreetAndHouse = string.Empty;
        private string _toCity = string.Empty;
        private string _toStreetAndHouse = string.Empty;
        private string _carModel = string.Empty;
        private string _driverId = string.Empty;
        private int _maxPassengers;
        private decimal _price;
        private string _comment;
        private DateTime _departureTime;
        private DateTime _arrivalDate;
        private bool _isCancelled = false;
        
        [Required]
        [MinLength(2)]
        public string FromCity
        {
            get => _fromCity;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 50)
                    throw new ArgumentException("Назва міста відправлення має містити від 2 до 50 символів.");
                _fromCity = value;
            }
        }

        [Required]
        [MinLength(3)]
        public string FromStreetAndHouse
        {
            get => _fromStreetAndHouse;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || value.Length > 100)
                    throw new ArgumentException("Адреса відправлення має містити від 3 до 100 символів.");
                _fromStreetAndHouse = value;
            }
        }

        [Required]
        [MinLength(2)]
        public string ToCity
        {
            get => _toCity;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 50)
                    throw new ArgumentException("Назва міста прибуття має містити від 2 до 50 символів.");
                _toCity = value;
            }
        }

        [Required]
        [MinLength(3)]
        public string ToStreetAndHouse
        {
            get => _toStreetAndHouse;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || value.Length > 100)
                    throw new ArgumentException("Адреса прибуття має містити від 3 до 100 символів.");
                _toStreetAndHouse = value;
            }
        }

        [Required]
        public DateTime DepartureTime
        {
            get => _departureTime;
            set
            {
                 if (value < DateTime.UtcNow)
                    throw new ArgumentException("Неможливо створити поїздку на минулу дату");
                _departureTime = value;
            }
        }

        [Required]
        public DateTime ArrivalDate
        {
            get => _arrivalDate;
            set
            {
                if (value < DepartureTime)
                    throw new ArgumentException("Неможливо встановити дату прибуття раніше віправлення");
                if (!IsValidTripDuration(value, _departureTime))
                    throw new ArgumentException("Тривалість поїздки недопустима");
                _arrivalDate = value;
            }
        }

        [Required]
        [MinLength(3)]
        public string CarModel
        {
            get => _carModel;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 30)
                    throw new ArgumentException("Модель авто має містити від 2 до 30 символів.");
                _carModel = value;
            }
        }

        [Range(1, int.MaxValue, ErrorMessage = "Кількість пасажирів повинна бути більше 0")]
        public int MaxPassengers
        {
            get => _maxPassengers;
            set
            {
                if (!IsValidSeats(value) || value == 0)
                    throw new ArgumentException("Кількість місць недопустима");
                _maxPassengers = value;
            }
        }

        [Range(0, double.MaxValue, ErrorMessage = "Ціна має бути не менше 0")]
        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Ціна не може бути не меншою за 0");
                _price = value;
            }
        }

        public bool IsCancelled
        {
            get => _isCancelled;
            set { _isCancelled = value; }
        } 

        [Required]
        [MinLength(1)]
        public string DriverId
        {
            get => _driverId;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ID водія не може бути порожнім");
                _driverId = value;
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
            }
        }

        [Required]
        public ApplicationUser Driver { get; set; }
        public ICollection<ApplicationUser> Passengers { get; set; } = new List<ApplicationUser>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public static int MaxSeatsForTrip { get; } = 50;
        public static int MaxTripDurationInHours { get; } = 100;

        public Trip() { }

        public Trip(
            string fromCity, string fromAddress, string toCity, string toAddress,
            DateTime departureTime, DateTime arrivalDate, string carModel,
            int maxPassengers, decimal price, string driverId, ApplicationUser driver)
        {
            FromCity = fromCity;
            FromStreetAndHouse = fromAddress;
            ToCity = toCity;
            ToStreetAndHouse = toAddress;
            DepartureTime = departureTime;
            ArrivalDate = arrivalDate;
            CarModel = carModel;
            MaxPassengers = maxPassengers;
            Price = price;
            DriverId = driverId;
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
            Passengers = new List<ApplicationUser>();
        }

        public bool HasAvailableSeats()
        {
            return Passengers.Count < MaxPassengers;
        }

        public void ChangePrice(decimal newPrice)
        {
            Price = newPrice;
        }

        public override string GetInfo()
        {
            return $"Маршрут: {FromCity} → {ToCity}, " +
                   $"Відправлення: {DepartureTime}, " +
                   $"Прибуття: {ArrivalDate}, " +
                   $"Авто: {CarModel}, " +
                   $"Ціна: {Price} грн";
        }

        public void CancelTrip()
        {
            if ((DepartureTime - DateTime.UtcNow).TotalHours < 24)
                throw new InvalidOperationException("Trip cannot be canceled less than 24 hours before departure.");
            IsCancelled = true;
        }

        public bool IsTripModifiable(List<ApplicationUser> passengers)
        {
            return (passengers.Count == 0);
        }

        public static bool IsValidSeats(int seats)
        {
            return (seats < MaxSeatsForTrip);
        }

        public static bool IsValidTripDuration(DateTime departureTime, DateTime arrivalTime)
        {
            return ((arrivalTime - departureTime).TotalHours < MaxTripDurationInHours);     
        }
    }
}

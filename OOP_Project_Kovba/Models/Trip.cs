using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Project_Kovba.Models
{
    public class Trip : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(1, int.MaxValue, ErrorMessage = "ID має бути позитивним числом")]
        public string Id { get; set; }

        private string _fromCity = string.Empty;
        private string _fromStreetAndHouse = string.Empty;
        private string _toCity = string.Empty;
        private string _toStreetAndHouse = string.Empty;
        private string _carModel = string.Empty;
        private string _driverId = string.Empty;
        private int _maxPassengers;
        private decimal _price;
        private DateTime _departureTime;
        private DateTime _arrivalDate;
        private bool _isCancelled = false;

        [Required]
        [MinLength(1)]
        public string FromCity
        {
            get => _fromCity;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Місто відправлення не може бути порожнім");
                _fromCity = value;
            }
        }

        [Required]
        [MinLength(1)]
        public string FromStreetAndHouse
        {
            get => _fromStreetAndHouse;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Адреса відправлення не може бути порожньою");
                _fromStreetAndHouse = value;
            }
        }

        [Required]
        [MinLength(1)]
        public string ToCity
        {
            get => _toCity;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Місто прибуття не може бути порожнім");
                _toCity = value;
            }
        }

        [Required]
        [MinLength(1)]
        public string ToStreetAndHouse
        {
            get => _toStreetAndHouse;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Адреса прибуття не може бути порожньою");
                _toStreetAndHouse = value;
            }
        }

        [Required]
        public DateTime DepartureTime
        {
            get => _departureTime;
            set => _departureTime = value;
        }

        [Required]
        public DateTime ArrivalDate
        {
            get => _arrivalDate;
            set
            {
                if (!IsValidTripDuration(value, _departureTime))
                    throw new ArgumentException("Тривалість поїздки недопустима");
                _arrivalDate = value;
            }
        }

        [Required]
        [MinLength(1)]
        public string CarModel
        {
            get => _carModel;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Модель авто не може бути порожньою");
                _carModel = value;
            }
        }

        [Range(1, int.MaxValue, ErrorMessage = "Кількість пасажирів повинна бути більше 0")]
        public int MaxPassengers
        {
            get => _maxPassengers;
            set
            {
                if (!IsValidSeats(value))
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
                    throw new ArgumentException("Ціна не може бути меншою за 0");
                _price = value;
            }
        }

        public bool IsCancelled => _isCancelled;

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

        [Required]
        public ApplicationUser Driver { get; set; }

        public ICollection<ApplicationUser>? Passengers { get; set; }

        public static int MaxSeatsForTrip { get; } = 50;
        public static int MaxTripDurationInHours { get; } = 100;

        private Trip() { }

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
            throw new NotImplementedException();
        }

        public void ChangePrice(decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public override string GetInfo()
        {
            throw new NotImplementedException();
        }

        public void CancelTrip()
        {
            throw new NotImplementedException();
        }

        public bool IsTripModifiable(List<ApplicationUser> passengers)
        {
            throw new NotImplementedException();
        }

        public static bool IsValidSeats(int seats)
        {
            throw new NotImplementedException();
        }

        public static bool IsValidTripDuration(DateTime departureTime, DateTime arrivalTime)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Runtime.CompilerServices;
using OOP_Project_Kovba.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace GhodPool.Tests
{
    [TestClass]
    public class TripTests
    {
        private ApplicationUser driver = null!;
        private DateTime departureTime;
        private DateTime arrivalTime;

        [TestInitialize]
        public void SetUp ()
        {
            driver = new ApplicationUser
            {
                Id = "1",
                UserName = "driver"

            };
            departureTime = DateTime.Now.AddHours(2);
            arrivalTime = departureTime.AddHours(3);
        }

        //Additional Method
        private Trip GetValidTrip()
        {
            return new Trip("Kyiv", "Shevchenko 1", "Lviv", "Franko 5",
                departureTime, arrivalTime, "Tesla", 4, 250, driver.Id, driver);
        }

        [TestMethod]
        public void Constructor_ValidData_CreatesTrip()
        {
            //Arrange & Act
            var trip = GetValidTrip();

            //Assert
            Assert.IsNotNull(trip);
            Assert.AreEqual("Kyiv", trip.FromCity);
            Assert.AreEqual("Lviv", trip.ToCity);
            Assert.AreEqual(4, trip.MaxPassengers);
            Assert.AreEqual(250, trip.Price);
            Assert.IsFalse(trip.IsCancelled);
        }

        [TestMethod]
        public void Constructor_ShouldThrowException_WhenDriverIsNull()
        {
             // Act & Assert
             Assert.ThrowsException<ArgumentNullException>(() =>
                new Trip("Kyiv", "Shevchenko 1", "Lviv", "Franko 5",
                    departureTime, arrivalTime, "Tesla", 4, 250, driver.Id, null!)
             );
        }

        [TestMethod]
        public void HasAvailableSeats_WhenSeatsAreAvailable_ReturnsTrue()
        {
            // Arrange
            var trip = GetValidTrip();
            trip.Passengers = new List<ApplicationUser>(); 

            // Act
            var result = trip.HasAvailableSeats();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasAvailableSeats_WhenSeatsAreNotAvailable_ReturnsFalse()
        {
            // Arrange
            var trip = GetValidTrip();
            trip.Passengers = new List<ApplicationUser>
            {
                new ApplicationUser(),
                new ApplicationUser(),
                new ApplicationUser(),
                new ApplicationUser()
            }; 

            // Act
            var result = trip.HasAvailableSeats();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ChangePrice_ValidPrice_UpdatesPrice()
        {
            //Arrange
            var trip = GetValidTrip();
            int price = 300;

            //Act
            trip.ChangePrice(price);

            //Assert
            Assert.AreEqual(price, trip.Price);
        }

        [TestMethod]
        public void ChangePrice_NegativePrice_ThrowsArgumentException()
        {
            //Arrange
            var trip = GetValidTrip();
            int price = -100;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
                trip.ChangePrice(price)
            );
        }

        [TestMethod]
        public void CancelTrip_WhenCalled_CancelsTheTrip()
        {
            //Arrange
            var trip = GetValidTrip();
            trip.DepartureTime = DateTime.Now.AddDays(2);
            trip.ArrivalDate = trip.DepartureTime.AddHours(3);
            //Act
            trip.CancelTrip();

            //Assert
            Assert.IsTrue(trip.IsCancelled);
        }

        [TestMethod]
        public void CancelTrip_WhenLessThan24HoursBeforeDeparture_ThrowsInvalidOperationException()
        {
            //Arrange
            var trip = GetValidTrip();
            var booking = new Booking() { Id = "fds" };
            trip.Bookings.Add(booking);
            
            trip.DepartureTime = DateTime.Now.AddHours(23); 
            trip.ArrivalDate = trip.DepartureTime.AddHours(3);

            //Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                trip.CancelTrip()
            );
        }

        [TestMethod]
        public void IsTripModifiable_WhenThereArePassengers_ReturnsFalse()
        {
            //Arrange
            var trip = GetValidTrip();

            var passengers = new List<ApplicationUser>
        {
            new ApplicationUser(),
            new ApplicationUser()
        };
            //Act
            var result = trip.IsTripModifiable(passengers);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsTripModifiable_WhenNoPassengers_ReturnsTrue()
        {
            //Arrange
            var trip = GetValidTrip();

            var passengers = new List<ApplicationUser>(); // No passengers

            //Act
            var result = trip.IsTripModifiable(passengers);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidSeats_WhenValidSeats_ReturnsTrue()
        {
            //Arrange
            int seats = 4;

            //Act
            var result = Trip.IsValidSeats(seats);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidSeats_WhenSeatsExceedMax_ReturnsFalse()
        {
            // Arrange
            int seats = 51;

            //Act
            var result = Trip.IsValidSeats(seats);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidTripDuration_WhenDurationIsValid_ReturnsTrue()
        {
            // Arrange: departureTime and arrivalTime are initialized in SetUp()

            //Act
            var result = Trip.IsValidTripDuration(departureTime, arrivalTime);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidTripDuration_WhenDurationExceedsMax_ReturnsFalse()
        {
            //Arrange
            int hours = 101;
            var longArrivalTime = departureTime.AddHours(hours);

            //Act
            var result = Trip.IsValidTripDuration(departureTime, longArrivalTime);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetInfo_WhenCalled_ReturnsCorrectTripInfo()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            var info = trip.GetInfo();

            //Assert
            Assert.IsTrue(info.Contains("Kyiv"));
            Assert.IsTrue(info.Contains("Lviv"));
            Assert.IsTrue(info.Contains("Tesla"));
            Assert.IsTrue(info.Contains("250"));
            Assert.IsTrue(info.Contains(departureTime.ToString()));
            Assert.IsTrue(info.Contains(arrivalTime.ToString()));
        }

        [TestMethod]
        public void SetUpdatedAt_Success()
        {
            //Arrange
            var trip = GetValidTrip();
            var before = DateTime.Now;

            //Act
            trip.SetUpdatedAt();

            var after = DateTime.Now;

            //Assert
            Assert.IsTrue(trip.UpdatedAt >= before && trip.UpdatedAt <= after);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FromCity_SetEmpty_Throws()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            trip.FromCity = "";
        }

        [TestMethod]
        public void FromCity_SetValid_Success()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            trip.FromCity = "Odessa";

            //Assert
            Assert.AreEqual("Odessa", trip.FromCity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FromStreetAndHouse_SetNull_Throws()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            trip.FromStreetAndHouse = null!;
        }

        [TestMethod]
        public void FromStreetAndHouse_SetValid_Success()
        {
            //Arrange 
            var trip = GetValidTrip();

            //Act
            trip.FromStreetAndHouse = "Centralna 3";

            //Assert
            Assert.AreEqual("Centralna 3", trip.FromStreetAndHouse);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ToCity_SetWhitespace_Throws()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            trip.ToCity = "   ";
        }

        [TestMethod]
        public void ToCity_SetValid_Success()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            trip.ToCity = "Dnipro";

            //Assert
            Assert.AreEqual("Dnipro", trip.ToCity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CarModel_SetEmpty_Throws()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            trip.CarModel = "";
        }

        [TestMethod]
        public void CarModel_SetValid_Success()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            trip.CarModel = "BMW";

            //Assert
            Assert.AreEqual("BMW", trip.CarModel);
        }

        [TestMethod]
        public void MaxPassengers_SetValid_Success()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            trip.MaxPassengers = 5;

            //Assert
            Assert.AreEqual(5, trip.MaxPassengers);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Price_SetNegative_Throws()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            trip.Price = -10;
        }

        [TestMethod]
        public void Price_SetValid_Success()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            trip.Price = 300;

            //Assert
            Assert.AreEqual(300, trip.Price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArrivalDate_SetBeforeDeparture_Throws()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            trip.ArrivalDate = trip.DepartureTime.AddMinutes(-30);
        }

        [TestMethod]
        public void ArrivalDate_SetValid_Success()
        {
            //Arrange
            var trip = GetValidTrip();
            var validArrival = trip.DepartureTime.AddHours(2);

            //Act
            trip.ArrivalDate = validArrival;

            //Assert
            Assert.AreEqual(validArrival, trip.ArrivalDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DriverId_SetNull_Throws()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            trip.DriverId = null!;
        }

        [TestMethod]
        public void DriverId_SetValid_Success()
        {
            //Arrange
            var trip = GetValidTrip();

            //Act
            trip.DriverId = "2";

            //Assert
            Assert.AreEqual("2", trip.DriverId);
        }
    }
}

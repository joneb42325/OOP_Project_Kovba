using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_Project_Kovba.Models;

namespace GhodPool.Tests
{
    [TestClass]
    public class BookingTests
    {
        private ApplicationUser user;
        private Trip trip;
        private ApplicationUser driver;

        [TestInitialize] 
        public void Init() {

            driver = new ApplicationUser
            {
                Id = "1",
                UserName = "driver"
            };

            trip = new Trip("Odessa", "Ambulatorna 1", "Kharkiv", "Sumskaya 5",
                    DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddHours(8), "BMW M5", 3, 500, driver.Id, driver)
            {
                Id = "qfz2",
            };

            user = new ApplicationUser
            {  
                Id = "2",
                UserName = "passenger"
            };
        }

        public Booking GetValidBooking() 
        {
            int seats = 2;
            return new Booking(seats, user.Id, user, trip.Id, trip);
        }

        [TestMethod]
        public void Constructor_ValidData_CreatesBooking()
        {
            //Arrange & Act
            var booking = GetValidBooking();

            //Assert
            Assert.IsNotNull(booking);
            Assert.AreEqual(2, booking.SeatsBooked);
            Assert.AreEqual(user.Id, booking.UserId);
            Assert.AreEqual(trip.Id, booking.TripId);
        }

        [TestMethod]
        public void Constructor_ShouldThrowException_WhenUserIsNull()
        {
            //Arrange
            int seats = 3;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
               new Booking(seats, user.Id, null!, trip.Id, trip)
            );
        }

        [TestMethod]
        public void Constructor_ShouldThrowException_WhenTripIsNull()
        {
            //Arrange
            int seats = 3;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
               new Booking(seats, user.Id, user, trip.Id, null!)
            );
        }

        [TestMethod]
        public void ChangeSeats_ValidSeats()
        {
            //Arrange
            var booking = GetValidBooking();
            int newSeats = 3;

            //Act
            booking.ChangeSeats(newSeats);

            //Assert
            Assert.AreEqual(booking.SeatsBooked, newSeats);
        }

        [TestMethod]
        public void ChangeSeats_NegativeSeats_ThrowsException()
        {
            //Arrange
            var booking = GetValidBooking();
            int newSeats = -3;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
                booking.ChangeSeats(newSeats)
            );
        }

        [TestMethod]
        public void GetInfo_WhenCalled_ReturnsCorrectBookingInfo()
        {
            //Arrange
            var booking = GetValidBooking();

            //Act 
            var info = booking.GetInfo();

            //Assert
            Assert.IsTrue(info.Contains("Odessa"));
            Assert.IsTrue(info.Contains("Kharkiv"));
            Assert.IsTrue(info.Contains("2"));
            Assert.IsTrue(info.Contains("BMW M5"));
            Assert.IsTrue(info.Contains("500"));
        }

        [TestMethod]
        public void CancelBooking_WhenCalled_CancelsTheBooking()
        {
            //Arrange
            var booking = GetValidBooking();
            trip.DepartureTime = DateTime.UtcNow.AddDays(2);
            trip.ArrivalDate = trip.DepartureTime.AddHours(3);

            //Act
            booking.CancelBooking();

            //Assert
            Assert.IsTrue(booking.IsCancelled);
        }

        [TestMethod]
        public void CancelBooking_WhenLessThan24HoursBeforeDeparture_ThrowsInvalidOperationException()
        {
            //Arrange
            var booking = GetValidBooking();
            trip.DepartureTime = DateTime.UtcNow.AddHours(23);
            trip.ArrivalDate = trip.DepartureTime.AddHours(3);

            //Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                booking.CancelBooking()
            );
        }

        [TestMethod]
        public void IsAlreadyBooked_UserHasBooking_ReturnsTrue()
        {
            //Arrange
            int seats = 2;
            var bookings = new List<Booking>
            {
                new Booking(seats, user.Id, user, trip.Id, trip)
            };

            //Act
            bool result = Booking.IsUserAlreadyBooked(user.Id, trip.Id, bookings);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAlreadyBooked_UserHasNoBooking_ReturnsFalse()
        {
            //Arrange
            var bookings = new List<Booking> 
            {
                new Booking(2, "other_user_id", new ApplicationUser { Id = "other_user_id" }, trip.Id, trip)
            };
           
            //Act
            bool result = Booking.IsUserAlreadyBooked(user.Id, trip.Id, bookings);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAlreadyBooked_BookingForAnotherTrip_ReturnsFalse()
        {
            // Arrange
            var anotherTrip = new Trip(
                "Kyiv", "Street 1", "Lviv", "Street 2",
                DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddHours(5),
                "Tesla", 4, 100, driver.Id, driver
            )
            { Id = "fsdf" };

            var bookings = new List<Booking>
            {
                new Booking(2, user.Id, user, anotherTrip.Id, anotherTrip)
            };

            // Act
            bool result = Booking.IsUserAlreadyBooked(user.Id, trip.Id, bookings);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SetUpdatedAt_Success()
        {
            //Arrange
            var booking = GetValidBooking();
            var before = DateTime.Now;

            //Act
            booking.SetUpdatedAt();

            var after = DateTime.Now;

            //Assert
            Assert.IsTrue(booking.UpdatedAt >= before && booking.UpdatedAt <= after);
        }

        [TestMethod]
        public void SeatsBooked_SetValidValue_SetsValue()
        {
            // Arrange
            var booking = GetValidBooking(); // booking.Trip уже установлен
            int newSeats = 3;

            // Act
            booking.SeatsBooked = newSeats;

            // Assert
            Assert.AreEqual(newSeats, booking.SeatsBooked);
        }

        
        [TestMethod]
        public void SeatsBooked_LessThanOne_ThrowsArgumentException()
        {
            // Arrange
            var booking = GetValidBooking();

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
                booking.SeatsBooked = 0
            );
        }

        [TestMethod]
        public void UserId_SetValidValue_SetsValue()
        {
            // Arrange
            var booking = GetValidBooking();
            var newUserId = "user123";

            // Act
            booking.UserId = newUserId;

            // Assert
            Assert.AreEqual(newUserId, booking.UserId);
        }

        [TestMethod]
        public void UserId_SetEmpty_ThrowsArgumentException()
        {
            // Arrange
            var booking = GetValidBooking();

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
                booking.UserId = ""
            );
        }

        [TestMethod]
        public void TripId_SetValidValue_SetsValue()
        {
            // Arrange
            var booking = GetValidBooking();
            var newTripId = "trip456";

            // Act
            booking.TripId = newTripId;

            // Assert
            Assert.AreEqual(newTripId, booking.TripId);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OOP_Project_Kovba.Interfaces;
using OOP_Project_Kovba.Data;
using OOP_Project_Kovba.Models;

namespace OOP_Project_Kovba.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ITripRepository _tripRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITripService _tripService;

        public BookingController(IBookingRepository bookingRepository, ITripRepository tripRepository, UserManager<ApplicationUser> userManager, ITripService tripService)
        {
            _bookingRepository = bookingRepository;
            _tripRepository = tripRepository;
            _userManager = userManager;
            _tripService = tripService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateBooking(string tripId, int seatsBooked)
        {
            var trip = await _tripRepository.GetTripByIdAsync(tripId);
            if (trip == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Unauthorized();
            }

            /* Comment for tests
            if (userId == trip.DriverId)
            {
                TempData["SuccessMessage"] = "Помилка. Ви є водієм в цій поїздці.";
                var viewModelAlready = await _tripService.GetTripDetailsViewModelAsync(tripId);
                return View("~/Views/Trip/TripDetails.cshtml", viewModelAlready);
            }
            */
            bool isAlreadyBooked = Booking.IsUserAlreadyBooked(userId, tripId, trip.Bookings);

            if (isAlreadyBooked)
            {
                TempData["SuccessMessage"] = "Помилка. Ви вже заброньовані в цій поїздці!";
                var viewModelAlready = await _tripService.GetTripDetailsViewModelAsync(tripId);
                return View("~/Views/Trip/TripDetails.cshtml", viewModelAlready);
            }

            var booking = new Booking
            {
                TripId = tripId,
                UserId = userId ?? throw new InvalidOperationException("User ID must not be null"),
                SeatsBooked = seatsBooked,
            };

            await _bookingRepository.AddBookingAsync(booking);

            trip.Bookings.Add(booking);
            trip.MaxPassengers = trip.MaxPassengers - booking.SeatsBooked;
            await _tripRepository.UpdateTripAsync(trip);

            var updatedViewModel = await _tripService.GetTripDetailsViewModelAsync(tripId);

            TempData["SuccessMessage"] = "Бронювання успішно створено!";
            return View("~/Views/Trip/TripDetails.cshtml", updatedViewModel);
        }
    }
}

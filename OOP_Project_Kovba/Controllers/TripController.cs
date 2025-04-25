
using Microsoft.AspNetCore.Mvc;
using MyMVC.Data;
using OOP_Project_Kovba.ViewModels;
using OOP_Project_Kovba.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;


namespace MyMVC.Controllers
{
    public class TripController : Controller
    {
        private readonly ITripRepository _tripRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TripController(ITripRepository tripRepository, UserManager<ApplicationUser> userManager)
        {
            _tripRepository = tripRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        [Authorize]
        public IActionResult CreateTrip()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrip(CreateTripViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var trip = new Trip
            {
                FromCity = model.FromCity,
                FromStreetAndHouse = model.FromStreetAndHouse,
                ToCity = model.ToCity,
                ToStreetAndHouse = model.ToStreetAndHouse,
                DepartureTime = model.DepartureDate,
                ArrivalDate = model.ArrivalDate,
                MaxPassengers = model.MaxPassengers,
                CarModel = model.CarModel,
                Price = model.Price,
                Comment = model.Comment ?? "",
                Driver = await _userManager.GetUserAsync(User)
                    ?? throw new InvalidOperationException("User must be authenticated"),
                DriverId = _userManager.GetUserId(User)
                    ?? throw new InvalidOperationException("User ID must not be null")
            };
            await _tripRepository.AddTripAsync(trip);
            return RedirectToAction("Index", "Home");
          //  return RedirectToAction("PlannedTrips", "Trip");
        }

        [HttpGet("/search")]
        public async Task<IActionResult> SearchTrip(SearchTripViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var trips = await _tripRepository.GetTripsAsync(model.From, model.To, model.Date, model.Passengers);

            var resultViewModel = new SearchTripResultViewModel
            {
                Trips = trips.Select(trip => new TripResultViewModel
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
                }).ToList()
            };

            if (User?.Identity?.IsAuthenticated ?? false)
            {
                ViewData["Layout"] = "_Layout_Authorised";
            }
            else
            {
                ViewData["Layout"] = "_Layout";
            }

            return View("Result", resultViewModel);
        }
        

        [Authorize]
        public IActionResult PlannedTrips()
        {
            return View();
        }
        /*
        [HttpGet]
        public async Task<IActionResult> GetUserTrips()
        {
            var userId = await _userManager.GetUserAsync(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var bookedTrips = await _userManager.GetBookedTripsAsync(userId);
            var createdTrips = await _userManager.GetCreatedTripsAsync(userId);

            var model = new UserTripsViewModel
            {
                BookedTrips = bookedTrips,
                CreatedTrips = createdTrips
            };
            return RedirectToAction("PlannedTrips", model);
        }
        */
    }
}

﻿
using Microsoft.AspNetCore.Mvc;
using OOP_Project_Kovba.Interfaces;
using OOP_Project_Kovba.ViewModels;
using OOP_Project_Kovba.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OOP_Project_Kovba;
using OOP_Project_Kovba.Controllers;


namespace MyMVC.Controllers
{
    public class TripController : BaseController
    {
        private readonly ITripRepository _tripRepository;
        private readonly IBookingRepository _bookingRepository;
      // private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITripService _tripService;

        public TripController(ITripRepository tripRepository, IBookingRepository bookingRepository, UserManager<ApplicationUser> userManager, ITripService tripService) : base(userManager)
        {
            _tripRepository = tripRepository;
            _bookingRepository = bookingRepository;
         //   _userManager = userManager;
            _tripService = tripService;
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

        [Authorize]
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
        }

        [HttpGet("/search")]
        public async Task<IActionResult> SearchTrip(SearchTripViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var trips = await _tripRepository.GetTripsAsync(model.From, model.To, model.Date, model.Passengers);

            var searchTrips = trips.Select(trip => new TripResultViewModel
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


            var resultViewModel = new SearchTripResultViewModel
            {
                Trips = searchTrips
            };

            return View("Result", resultViewModel);
        }
        
        [HttpGet("trip-details/{id}")]
        public async Task<IActionResult> TripDetails(string id)
        {
            var viewModel = await _tripService.GetTripDetailsViewModelAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View("TripDetails", viewModel);
        }

        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> PlannedTrips()
        {
            var userId = _userManager.GetUserId(User);
            var driverTrips = await _tripService.GetDriversPlannedTripsAsync(userId);
            var passengerBookings = await _tripService.GetPassengerBookingsAsync(userId);

            var plannedTripsViewModel = new PlannedTripsViewModel
            {
                DriverTrips = driverTrips,
                PassengerBookings = passengerBookings
            };

            return View(plannedTripsViewModel);
        }

        [HttpGet("planned-trip-details/{id}")]
        public async Task<IActionResult> PlannedTripDetails(string id)
        {
            var viewModel = await _tripService.GetTripDetailsViewModelAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View("PlannedTripDetails", viewModel);
        }

        public async Task<IActionResult> CancelTrip(string tripId)
        {
            var trip = await _tripRepository.GetTripByIdAsync(tripId);

            if (trip == null)
            {
                return NotFound();
            }
            try
            {
                trip.CancelTrip();
            }
            catch (InvalidOperationException ex)
            {
                TempData["TripMessage"] = ex.Message;
                return RedirectToAction("PlannedTrips");
            }
            
            await _bookingRepository.DeleteBookingsByTripIdAsync(tripId);
            await _tripRepository.UpdateTripAsync(trip);
            TempData["TripMessage"] = "Поїздку успішно видалено.";
            return RedirectToAction("PlannedTrips");
        }
    }
}

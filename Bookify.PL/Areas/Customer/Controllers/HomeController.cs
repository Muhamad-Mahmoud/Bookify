using System.Diagnostics;
using Bookify.Models;
using Bookify.Models.ViewModels;
using Bookify.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.PL.Areas.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHotelService _hotelService;
        private readonly ICityService _cityService;

        public HomeController(
            ILogger<HomeController> logger,
            IHotelService hotelService,
            ICityService cityService)
        {
            _logger = logger;
            _hotelService = hotelService;
            _cityService = cityService;
        }

        public async Task<IActionResult> Index()
        {
            var homeVM = new HomeVM();

            // Get all approved hotels with city information
            var allHotels = await _hotelService.GetAllHotelsAsync(
                filter: h => h.Status == HotelStatus.Approved,
                includeProperties: "City"
            );

            // Featured Hotels 
            homeVM.FeaturedHotels = allHotels
                .Where(h => h.IsFeatured)
                .OrderByDescending(h => h.UserRating)
                .Take(3)
                .ToList();

            // If no featured hotels, get top rated hotels
            if (!homeVM.FeaturedHotels.Any())
            {
                homeVM.FeaturedHotels = allHotels
                    .OrderByDescending(h => h.UserRating)
                    .ThenByDescending(h => h.BookingCount)
                    .Take(3)
                    .ToList();
            }

            //Cities Gallery
            var allCities = await _cityService.GetAllCitiesAsync(includeProperties: "Country");
            homeVM.FeaturedCities = allCities
                .Where(c => !string.IsNullOrEmpty(c.Image))
                .Take(5)
                .ToList();

            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

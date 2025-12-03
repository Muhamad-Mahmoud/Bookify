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

            try
            {
                // Get all approved hotels with city information
                var allHotels = await _hotelService.GetAllHotelsAsync(
                    filter: h => h.Status == HotelStatus.Approved,
                    includeProperties: "City"
                );

                // Featured Hotels (top 10 featured hotels)
                homeVM.FeaturedHotels = allHotels
                    .Where(h => h.IsFeatured)
                    .OrderByDescending(h => h.UserRating)
                    .Take(10)
                    .ToList();

                // If no featured hotels, get top rated hotels
                if (!homeVM.FeaturedHotels.Any())
                {
                    homeVM.FeaturedHotels = allHotels
                        .OrderByDescending(h => h.UserRating)
                        .ThenByDescending(h => h.BookingCount)
                        .Take(10)
                        .ToList();
                }

                // Hotels in a Specific City (e.g., Giza)
                // Try to get hotels from Giza, Cairo, or the city with most hotels
                var gizaHotels = allHotels.Where(h => h.City?.Name.Contains("Giza") == true).ToList();
                
                if (gizaHotels.Any())
                {
                    homeVM.FeaturedCityName = "Giza";
                    homeVM.CityHotels = gizaHotels.OrderByDescending(h => h.UserRating).Take(10).ToList();
                }
                else
                {
                    // Fallback to city with most hotels
                    var cityWithMostHotels = allHotels
                        .GroupBy(h => h.City)
                        .OrderByDescending(g => g.Count())
                        .FirstOrDefault();

                    if (cityWithMostHotels != null)
                    {
                        homeVM.FeaturedCityName = cityWithMostHotels.Key?.Name;
                        homeVM.CityHotels = cityWithMostHotels
                            .OrderByDescending(h => h.UserRating)
                            .Take(10)
                            .ToList();
                    }
                }

                //Cities Gallery
                var allCities = await _cityService.GetAllCitiesAsync(includeProperties: "Country");
                homeVM.FeaturedCities = allCities
                    .Where(c => !string.IsNullOrEmpty(c.Image))
                    .Take(12)
                    .ToList();

                // If no cities have images, get all cities
                if (!homeVM.FeaturedCities.Any())
                {
                    homeVM.FeaturedCities = allCities.Take(12).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading home page data");
                // Return empty view model on error
            }

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

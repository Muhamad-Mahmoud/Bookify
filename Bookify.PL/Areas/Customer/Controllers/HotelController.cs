using Bookify.BL.Interfaces;
using Bookify.Models;
using Bookify.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Bookify.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IRoomService _roomService;
        private readonly IReservationService _reservationService;
        private readonly IReviewService _reviewService;
        private readonly UserManager<Bookify.Models.Customer> _userManager;

        public HotelController(
            IHotelService hotelService, 
            IRoomTypeService roomTypeService, 
            IRoomService roomService, 
            IReservationService reservationService,
            IReviewService reviewService,
            UserManager<Bookify.Models.Customer> userManager)
        {
            _hotelService = hotelService;
            _roomTypeService = roomTypeService;
            _roomService = roomService;
            _reservationService = reservationService;
            _reviewService = reviewService;
            _userManager = userManager;
        }

        public async Task<IActionResult> GetHotel( int id, DateTime? checkInDate, DateTime? checkOutDate, int guestCount)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id, includeProperties: "GalleryImages,City");

            var checkIn = checkInDate ?? DateTime.Now;
            var checkOut = checkOutDate ?? DateTime.Now.AddDays(1);

            var roomTypes = await _roomTypeService.GetRoomTypesWithAvailabilityAsync(id, checkIn, checkOut);
            
            // Filter to only show room types with available rooms 
            roomTypes = roomTypes
                .Where(rt => rt.AvailableRoomsCount > 0 && rt.MaxGuests >= guestCount)
                .ToList();

            // Get Reviews info
            var reviews = await _reviewService.GetHotelReviewsAsync(id);
            var canReview = false;
            
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    canReview = await _reviewService.CanUserReviewHotelAsync(user.Id, id) 
                             && await _reviewService.GetUserReviewForHotelAsync(user.Id, id) == null;
                }
            }

            var model = new HotelDetailsVM
            {
                Hotel = hotel,
                RoomTypes = roomTypes,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                GuestCount = guestCount,
                AvailableRoomsCount = roomTypes.ToDictionary(
                    rt => rt.Id,
                    rt => rt.AvailableRoomsCount
                ),
                Reviews = reviews,
                CanReview = canReview
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(int hotelId, int rating, string reviewText)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                var review = new Review
                {
                    HotelId = hotelId,
                    CustomerId = user.Id,
                    Rating = rating,
                    ReviewText = reviewText,
                    CreatedAt = DateTime.Now
                };

                await _reviewService.AddReviewAsync(review);
                TempData["SuccessMessage"] = "Review submitted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to submit review. You must have a completed stay.";
            }

            return Redirect(Url.Action(nameof(GetHotel), new { id = hotelId }) + "#reviews");
        }

        public async Task<IActionResult> Search(
            string location, 
            DateTime? checkInDate, 
            DateTime? checkOutDate, 
            int guestCount = 1,
            [FromQuery] List<int>? stars = null)
        {
            var checkIn = checkInDate ?? DateTime.Now;
            var checkOut = checkOutDate ?? DateTime.Now.AddDays(1);

            // Get Hotels by Location
            Expression<Func<Hotel, bool>> filter = null;
            if (!string.IsNullOrEmpty(location))
            {
                var lowerLocation = location.ToLower();
                filter = h => h.City.Name.ToLower().Contains(lowerLocation) 
                           || h.Name.ToLower().Contains(lowerLocation) 
                           || h.Address.ToLower().Contains(lowerLocation);
            }

            var hotels = await _hotelService.GetAllHotelsAsync(filter, includeProperties: "City,RoomTypes,GalleryImages");

            //  Filter by Availability and Guest Count
            var availableHotels = new List<Hotel>();

            foreach (var hotel in hotels)
            {
                var availableRoomTypes = await _roomTypeService.GetRoomTypesWithAvailabilityAsync(hotel.Id, checkIn, checkOut);

                var suitableRoomTypes = availableRoomTypes.Where(rt => rt.AvailableRoomsCount > 0 && rt.MaxGuests >= guestCount).ToList();

                if (suitableRoomTypes.Any())
                {
                    // Update the hotel room types 
                    hotel.RoomTypes = suitableRoomTypes;
                    availableHotels.Add(hotel);
                }
            }

            // --- Apply Filters ---

            // Filter by Stars (Based on User Reviews now)
            if (stars != null && stars.Any())
            {
                // Round UserRating to nearest int (e.g., 4.5 -> 5, 4.2 -> 4) to match filter checkboxes
                availableHotels = availableHotels.Where(h => stars.Contains((int)Math.Round(h.UserRating))).ToList();
            }

            var model = new HotelSearchVM
            {
                Hotels = availableHotels,
                Location = location,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                GuestCount = guestCount,
                SelectedStars = stars ?? new List<int>()
            };

            return View(model);
        }
    }
}

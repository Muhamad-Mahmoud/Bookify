using Bookify.BL.Interfaces;
using Bookify.Models;
using Bookify.Models.ViewModels;
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
        public HotelController(IHotelService hotelService, IRoomTypeService roomTypeService, IRoomService roomService, IReservationService reservationService)
        {
            _hotelService = hotelService;
            _roomTypeService = roomTypeService;
            _roomService = roomService;
            _reservationService = reservationService;
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
                )
            };

            return View(model);
        }

        public async Task<IActionResult> Search(string location, DateTime? checkInDate, DateTime? checkOutDate, int guestCount = 1)
        {
            var checkIn = checkInDate ?? DateTime.Now;
            var checkOut = checkOutDate ?? DateTime.Now.AddDays(1);

            // Get Hotels by Location
            Expression<Func<Hotel, bool>> filter = null;
            if (!string.IsNullOrEmpty(location))
            {
                filter = h => h.City.Name.Contains(location) || h.Name.Contains(location) || h.Address.Contains(location);
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

            var model = new HotelSearchVM
            {
                Hotels = availableHotels,
                Location = location,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                GuestCount = guestCount
            };

            return View(model);
        }
    }
}

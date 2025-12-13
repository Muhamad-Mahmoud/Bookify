using Bookify.Models;

namespace Bookify.Models.ViewModels
{
    public class HotelSearchVM
    {
        public IEnumerable<Hotel> Hotels { get; set; } = new List<Hotel>();
        public string Location { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int GuestCount { get; set; }

        // Filters
        public List<int> SelectedStars { get; set; } = new List<int>(); // Recommended, PriceLowHigh, PriceHighLow, RatingHighLow
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Models.ViewModels
{
    public class HotelDetailsVM
    {
        public Hotel Hotel { get; set; }
        public IEnumerable<RoomType> RoomTypes { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int GuestCount { get; set; }
        public Dictionary<int, int> AvailableRoomsCount { get; set; } = new Dictionary<int, int>();
    }
}

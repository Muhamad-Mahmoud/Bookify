using System;
using Bookify.Models;

namespace Bookify.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public RoomType RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int TotalNights { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

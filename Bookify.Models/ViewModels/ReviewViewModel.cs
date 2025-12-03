using System;

namespace Bookify.Models.ViewModels
{
    public class ReviewViewModel
    {
        public string ReviewerName { get; set; }
        public string ReviewerInitials { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public double Rating { get; set; }
        public string Text { get; set; }
    }
}

namespace Bookify.Models.ViewModels
{
    public class DashboardVM
    {
        // Statistics
        public int TotalHotels { get; set; }
        public int ActiveReservations { get; set; }
        public int AvailableRooms { get; set; }
        public decimal TotalRevenue { get; set; }
        
        // Recent data
        public IEnumerable<Reservation> RecentReservations { get; set; } = new List<Reservation>();
        
        // User role info
        public bool IsAdmin { get; set; }
        public string? OwnerEmail { get; set; }
    }
}

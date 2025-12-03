namespace Bookify.Models.ViewModels
{
    public class DashboardVM
    {
        // Statistics
        public int TotalHotels { get; set; }
        public int ActiveReservations { get; set; }
        public int AvailableRooms { get; set; }
        public decimal TotalRevenue { get; set; }
        
        // Month-over-month changes (for growth indicators)
        public decimal HotelsChange { get; set; }
        public decimal ReservationsChange { get; set; }
        public decimal RoomsChange { get; set; }
        public decimal RevenueChange { get; set; }
        
        // Recent data
        public IEnumerable<Reservation> RecentReservations { get; set; } = new List<Reservation>();
        public IEnumerable<Invoice> RecentInvoices { get; set; } = new List<Invoice>();
        
        // User role info
        public bool IsAdmin { get; set; }
        public string? OwnerEmail { get; set; }
    }
}

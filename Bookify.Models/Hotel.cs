using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookify.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hotel name is required")]
        [StringLength(100, ErrorMessage = "Hotel name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [ValidateNever]
        public HotelStatus Status { get; set; } = HotelStatus.Pending;

        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        [ValidateNever]
        public City? City { get; set; }

        [StringLength(500, ErrorMessage = "Main image URL cannot exceed 500 characters")]
        [Url(ErrorMessage = "Invalid URL format")]
        [Display(Name = "Main Image")]
        public string? MainImage { get; set; }

        [Range(1, 5, ErrorMessage = "Star rating must be between 1 and 5")]
        [Display(Name = "Star Rating")]
        public int StarRating { get; set; } = 3;

        [Range(0, 10, ErrorMessage = "User rating must be between 0 and 10")]
        [Display(Name = "User Rating")]
        public double UserRating { get; set; } = 0;

        [Display(Name = "Review Count")]
        public int ReviewCount { get; set; } = 0;

        [Display(Name = "Is Featured")]
        public bool IsFeatured { get; set; } = false;

        [Display(Name = "Booking Count")]
        public int BookingCount { get; set; } = 0;

        
        [ValidateNever]
        public ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();
        [ValidateNever]
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

        [ValidateNever]
        public ICollection<HotelImage> GalleryImages { get; set; } = new List<HotelImage>();

        public string? OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [ValidateNever]
        public Customer? Owner { get; set; }
    }
    public enum HotelStatus
    {
        Pending,
        Approved,
        Rejected
    }

}

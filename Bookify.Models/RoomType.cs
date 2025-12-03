using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookify.Models
{
    /// Represents a room category/type that is visible to users (e.g., Deluxe, Superior, Suite).
    /// This entity contains all user-facing information about the room type.
    public class RoomType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Room type name is required")]
        [StringLength(100, ErrorMessage = "Room type name cannot exceed 100 characters")]
        [Display(Name = "Room Type Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Room size is required")]
        [Range(1, 10000, ErrorMessage = "Size must be between 1 and 10,000 square meters")]
        [Display(Name = "Size (m²)")]
        public int Size { get; set; }

        [Required(ErrorMessage = "Maximum guests is required")]
        [Range(1, 10, ErrorMessage = "Maximum guests must be between 1 and 10")]
        [Display(Name = "Max Guests")]
        public int MaxGuests { get; set; }

        [Required(ErrorMessage = "Bed type is required")]
        [StringLength(50, ErrorMessage = "Bed type cannot exceed 50 characters")]
        [Display(Name = "Bed Type")]
        public string BedType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Base price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999,999.99")]
        [DataType(DataType.Currency)]
        [Display(Name = "Base Price per Night")]
        public decimal BasePrice { get; set; }

        [StringLength(1000, ErrorMessage = "Amenities cannot exceed 1000 characters")]
        [Display(Name = "Amenities")]
        public string? Amenities { get; set; }

        [StringLength(500, ErrorMessage = "Main image URL cannot exceed 500 characters")]
        [Url(ErrorMessage = "Invalid URL format")]
        [Display(Name = "Main Image")]
        public string? MainImage { get; set; }

        [Required(ErrorMessage = "Hotel is required")]
        [Display(Name = "Hotel")]
        public int HotelId { get; set; }

        [ForeignKey(nameof(HotelId))]
        [ValidateNever]
        public Hotel? Hotel { get; set; }

        [ValidateNever]
        public ICollection<Room> Rooms { get; set; } = new List<Room>();


        [ValidateNever]
        public ICollection<RoomImage> GalleryImages { get; set; } = new List<RoomImage>();

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [NotMapped]
        public int AvailableRoomsCount { get; set; }

    }
}

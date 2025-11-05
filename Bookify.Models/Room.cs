using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bookify.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Room name is required")]
        [StringLength(50, ErrorMessage = "Room name cannot exceed 50 characters")]
        [Display(Name = "Room Name")]
        public string RoomName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 999999.99, ErrorMessage = "Price must be between 0 and 999,999.99")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, 20, ErrorMessage = "Capacity must be between 1 and 20 persons")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Room size is required")]
        [Range(1, 1000, ErrorMessage = "Size must be between 1 and 1000 square meters")]
        [Display(Name = "Room Size (m²)")]
        public int Size { get; set; }

        [Required(ErrorMessage = "Services information is required")]
        [StringLength(500, ErrorMessage = "Services description cannot exceed 500 characters")]
        public string Services { get; set; } = string.Empty;

        [Url(ErrorMessage = "Invalid URL format")]
        [Display(Name = "Image URL")]
        public string? ImageURL { get; set; }

        [Required(ErrorMessage = "Room type is required")]
        [Display(Name = "Room Type")]
        public int RoomTypeId { get; set; }

        [ForeignKey(nameof(RoomTypeId))]
        [ValidateNever]
        public RoomType? RoomType { get; set; }
        
        [ValidateNever]
        public ICollection<ReservedRoom> RoomReserved { get; set; } = new List<ReservedRoom>();
    }
}

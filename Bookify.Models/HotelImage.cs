using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookify.Models
{
    public class HotelImage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hotel is required")]
        public int HotelId { get; set; }

        [ForeignKey(nameof(HotelId))]
        [ValidateNever]
        public Hotel? Hotel { get; set; }

        [Range(0, 999, ErrorMessage = "Display order must be between 0 and 999")]
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; } = 0;

    }
}

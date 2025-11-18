using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Models
{
    public class Customer : IdentityUser
    {
        [Required]
        public required string Name { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string? Address { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        [Display(Name = "Personal Image URL")]
        public string? PersonalImgUrl { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        [Display(Name = "Cover Image URL")]
        public string? CoverImgUrl { get; set; }

        public ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();
    }
}

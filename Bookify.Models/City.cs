using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookify.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "City name is required")]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(10, ErrorMessage = "Postal code cannot exceed 10 characters")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        [Url(ErrorMessage = "Invalid URL format")]
        [Display(Name = "City Image")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        [ValidateNever]
        public Country? Country { get; set; }
    }

}

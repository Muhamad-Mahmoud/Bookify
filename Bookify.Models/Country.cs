using System.ComponentModel.DataAnnotations;

namespace Bookify.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Country name is required")]
        [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters")]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; } = string.Empty;
    }
}

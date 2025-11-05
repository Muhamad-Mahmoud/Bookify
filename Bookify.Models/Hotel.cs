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

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Company is required")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public Company? Company { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        [ValidateNever]
        public City? City { get; set; }

        [ValidateNever]
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }

}

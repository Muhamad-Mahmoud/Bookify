using System.ComponentModel.DataAnnotations;

namespace Bookify.Models
{
    public class RoomType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Room type name is required")]
        [StringLength(50, ErrorMessage = "Room type name cannot exceed 50 characters")]
        [Display(Name = "Room Type")]
        public string TypeName { get; set; } = string.Empty;
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookify.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Room number is required")]
        [StringLength(20, ErrorMessage = "Room number cannot exceed 20 characters")]
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; } = string.Empty;

        [Range(0, 999, ErrorMessage = "Floor must be between 0 and 999")]
        [Display(Name = "Floor")]
        public int? Floor { get; set; }

        
        [Required(ErrorMessage = "Room type is required")]
        [Display(Name = "Room Type")]
        public int RoomTypeId { get; set; }

        [ForeignKey(nameof(RoomTypeId))]
        [ValidateNever]
        public RoomType? RoomType { get; set; }


        [ValidateNever]
        public RoomStatus Status { get; set; } = RoomStatus.Available;


        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }

        [ValidateNever]
        public ICollection<ReservedRoom> Reservations { get; set; } = new List<ReservedRoom>();
    }

    public enum RoomStatus
    {
        Available,
        Occupied,
        Maintenance
    }
}

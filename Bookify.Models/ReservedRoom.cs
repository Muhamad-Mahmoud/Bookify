using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bookify.Models
{
    public class ReservedRoom
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Reservation is required")]
        public int ReservationId { get; set; }

        [ForeignKey(nameof(ReservationId))]
        [ValidateNever]
        public Reservation? Reservation { get; set; }

        [Required(ErrorMessage = "Room is required")]
        public int RoomId { get; set; }

        [ForeignKey(nameof(RoomId))]
        [ValidateNever]
        public Room? Room { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 999999.99, ErrorMessage = "Price must be between 0 and 999,999.99")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
